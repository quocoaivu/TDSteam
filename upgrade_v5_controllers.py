"""
Upgrade Unity AnimatorController files from v5 (older Unity) to v6/v7 format
that Unity 6 expects. Preserves all state wiring (m_ChildStates) intact.

Format diff applied:
- AnimatorController: serializedVersion 5 -> 6
    - Replace m_PrefabParentObject -> m_CorrespondingSourceObject
    - Replace m_PrefabInternal -> m_PrefabInstance + m_PrefabAsset
    - Add m_EvaluateTransitionsOnStart: 0 at end
- AnimatorState: serializedVersion 5 -> 6
    - Same prefab field replacement
    - Add m_TimeParameterActive after m_CycleOffsetParameterActive
    - Add m_TimeParameter after m_CycleOffsetParameter
- AnimatorStateMachine: serializedVersion 5 -> 7
    - Same prefab field replacement
    - Change m_StateMachineTransitions: [] -> m_StateMachineTransitions: {}
- AnimatorStateTransition: serializedVersion stays 3
    - Same prefab field replacement
"""

import os
import re
import sys

CONTROLLER_DIR = r"D:\2026TD\TDCore\Assets\AnimatorController"


def upgrade_content(content: str) -> tuple[str, bool]:
    """Return (new_content, was_changed)."""
    lines = content.split("\n")
    out = []
    block_type = None  # 'controller' | 'state' | 'statemachine' | 'transition' | None
    block_indent_prefab = False  # whether we already saw prefab field replacement for current block
    changed = False

    i = 0
    while i < len(lines):
        line = lines[i]

        # Detect block start
        if line.startswith("--- !u!91 &"):
            block_type = "controller_outer"  # next line is "AnimatorController:"
        elif line.startswith("--- !u!1102 &"):
            block_type = "state_outer"
        elif line.startswith("--- !u!1107 &"):
            block_type = "statemachine_outer"
        elif line.startswith("--- !u!1101 &"):
            block_type = "transition_outer"
        elif line.startswith("--- "):
            block_type = None
        elif block_type == "controller_outer" and line == "AnimatorController:":
            block_type = "controller"
        elif block_type == "state_outer" and line == "AnimatorState:":
            block_type = "state"
        elif block_type == "statemachine_outer" and line == "AnimatorStateMachine:":
            block_type = "statemachine"
        elif block_type == "transition_outer" and line == "AnimatorStateTransition:":
            block_type = "transition"

        # Replace prefab fields (works for any block containing these)
        if line == "  m_PrefabParentObject: {fileID: 0}":
            out.append("  m_CorrespondingSourceObject: {fileID: 0}")
            changed = True
            i += 1
            continue
        if line == "  m_PrefabInternal: {fileID: 0}":
            out.append("  m_PrefabInstance: {fileID: 0}")
            out.append("  m_PrefabAsset: {fileID: 0}")
            changed = True
            i += 1
            continue

        # AnimatorController: bump serializedVersion 5 -> 6
        if block_type == "controller" and line == "  serializedVersion: 5":
            out.append("  serializedVersion: 6")
            changed = True
            i += 1
            continue

        # AnimatorState: bump serializedVersion 5 -> 6
        if block_type == "state" and line == "  serializedVersion: 5":
            out.append("  serializedVersion: 6")
            changed = True
            i += 1
            continue

        # AnimatorStateMachine: bump serializedVersion 5 -> 7
        if block_type == "statemachine" and line == "  serializedVersion: 5":
            out.append("  serializedVersion: 7")
            changed = True
            i += 1
            continue

        # AnimatorStateMachine: change m_StateMachineTransitions: [] -> {}
        if block_type == "statemachine" and line == "  m_StateMachineTransitions: []":
            out.append("  m_StateMachineTransitions: {}")
            changed = True
            i += 1
            continue

        # AnimatorState: add m_TimeParameterActive after m_CycleOffsetParameterActive
        if block_type == "state" and line == "  m_CycleOffsetParameterActive: 0":
            out.append(line)
            # Check next line is not already m_TimeParameterActive
            if i + 1 < len(lines) and lines[i + 1].lstrip().startswith("m_TimeParameterActive"):
                pass  # already there
            else:
                out.append("  m_TimeParameterActive: 0")
                changed = True
            i += 1
            continue

        # AnimatorState: add m_TimeParameter after m_CycleOffsetParameter
        if block_type == "state" and line.startswith("  m_CycleOffsetParameter:"):
            out.append(line)
            if i + 1 < len(lines) and lines[i + 1].lstrip().startswith("m_TimeParameter"):
                pass
            else:
                out.append("  m_TimeParameter: ")
                changed = True
            i += 1
            continue

        out.append(line)
        i += 1

    # AnimatorController: add m_EvaluateTransitionsOnStart at very end before any trailing blank lines
    # The v6 format has this as last field of AnimatorController block
    # We need to find the end of the AnimatorController block (which is the last block in file usually)
    # Strategy: scan from end; if we find AnimatorController block and no m_EvaluateTransitionsOnStart in it, add it
    new_lines = out
    # Find last AnimatorController block
    ctrl_start_idx = None
    for idx in range(len(new_lines) - 1, -1, -1):
        if new_lines[idx] == "AnimatorController:":
            ctrl_start_idx = idx
            break
    if ctrl_start_idx is not None:
        # Find end of this block (next "---" or end of file)
        ctrl_end_idx = len(new_lines)
        for idx in range(ctrl_start_idx + 1, len(new_lines)):
            if new_lines[idx].startswith("---"):
                ctrl_end_idx = idx
                break
        # Check if m_EvaluateTransitionsOnStart already present in this block
        block_lines = new_lines[ctrl_start_idx:ctrl_end_idx]
        if not any("m_EvaluateTransitionsOnStart" in bl for bl in block_lines):
            # Insert before ctrl_end_idx, but trim trailing empty lines
            insert_idx = ctrl_end_idx
            while insert_idx > ctrl_start_idx and new_lines[insert_idx - 1].strip() == "":
                insert_idx -= 1
            new_lines.insert(insert_idx, "  m_EvaluateTransitionsOnStart: 0")
            changed = True

    return "\n".join(new_lines), changed


def is_v5(content: str) -> bool:
    """Check if file is still in v5 format."""
    # v5 marker: presence of m_PrefabParentObject in the AnimatorController block
    return "m_PrefabParentObject" in content


def main():
    files = [f for f in os.listdir(CONTROLLER_DIR) if f.endswith(".controller")]
    files.sort()

    upgraded = []
    skipped = []
    errored = []

    for fname in files:
        path = os.path.join(CONTROLLER_DIR, fname)
        try:
            with open(path, "r", encoding="utf-8") as f:
                content = f.read()
        except Exception as e:
            errored.append((fname, str(e)))
            continue

        if not is_v5(content):
            skipped.append(fname)
            continue

        new_content, changed = upgrade_content(content)
        if not changed:
            skipped.append(fname)
            continue

        try:
            with open(path, "w", encoding="utf-8", newline="\n") as f:
                f.write(new_content)
            upgraded.append(fname)
        except Exception as e:
            errored.append((fname, str(e)))

    print(f"Upgraded: {len(upgraded)}")
    for f in upgraded:
        print(f"  {f}")
    print(f"\nSkipped (already v6 or no changes): {len(skipped)}")
    if errored:
        print(f"\nErrored: {len(errored)}")
        for f, e in errored:
            print(f"  {f}: {e}")


if __name__ == "__main__":
    main()
