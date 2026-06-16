# Tower Defense Game - Unity Project

# Coding style
- Write simple, readable code;
- Avoid complex design patterns when not necessary
- Keep functions short; each function should do one thing
- Use clear, meaningful names for variables and functions
- Add brief comments for non-obvious logic
- Do not add features beyond what was requested
- Avoid premature abstraction


## Project Overview
  2D Tower Defense game inspired by Kingdom Rush.
- **Engine**: Unity 6.4 (60000.4.1f1)
- **Platform**: PC + Mobile (responsive)
- **Style**: Fantasy medieval
- **Target**: 60 FPS PC, 30 FPS mobile (low-end)

## Code Conventions

### Naming
- Classes: PascalCase (`TowerController`, `EnemySpawner`)
- Private fields: camelCase with `_` prefix (`_currentHealth`)
- Serialized fields: camelCase (`[SerializeField] private float moveSpeed`)
- Constants: UPPER_SNAKE_CASE (`MAX_WAVE_COUNT`)
- Methods: PascalCase (`SpawnEnemy`, `OnButtonClick`)

### Architecture
- Use **ScriptableObjects** for data (TowerData, EnemyData, WaveData)
- Use **Object Pooling** for enemies, projectiles, VFX
- Use **Singleton pattern** for managers (GameManager, AudioManager, UIManager)
- Prefer **Events/UnityEvents** over direct references for loose coupling
- Use **DOTween** for all UI animations (no Coroutines for tweens)

### Animation
- Gameplay characters/towers drive state via `animator.Play(name)` directly — **Animator Controllers are used as clip storage**, not transition graphs. Don't author state-machine transitions or rely on parameter-driven blends.
- Clip names are shared conventions, not per-prefab data: heroes/pets use hardcoded `static string` fields on `HeroAnimationController` (e.g. "Run", "Idle", "MeleeAttack", "ActiveSkill", "Passive0"), and enemies use `const string` on `EnemyAnimationController`. Every prefab's Animator must name its clips to match these exact strings.
- Hero/pet state names are NOT defined per prefab: the 10 states (`Idle`, `Die`, `Run`, `MeleeAttack`, `RangeAttack`, `Play`, `ActiveSkill`, `Passive0/1/2`) live once in the base `Assets/AddressableContent/animatorcontrollers/HeroAnimatorCommon.controller`. Each of the ~22 hero/pet prefabs uses an `AnimatorOverrideController` built on it, swapping only the *clips* per state — the states always exist. So the 10 `static string` fields in `HeroAnimationController` are a hard mirror of `HeroAnimatorCommon`'s state names: rename a state there (or base a new unit on a different controller) and the matching `animator.Play(...)` fails silently (warning only, no compile error). Keep the two in sync.
- For hot `Animator.Play(...)` paths (per-frame or per-bullet), cache `Animator.StringToHash("Clip")` into a `static readonly int` and call `Play(hash)` instead of the string overload.
- For skill / one-shot clips, auto-return to Idle via the controller's `SetupAutoReturn(duration)` helper (Hero + Ally). `Die` is terminal — never auto-return.
- Animation Events on `.anim` assets invoke methods by name (e.g. `UnitSoundController.PlaySelect/PlayAttack/PlayDie`). **Never rename these methods** — anim event refs are name-based, not GUID-based, and silently break.
- UI animation is **DOTween-only** (see DON'T #6). Don't mix Mecanim and DOTween on the same UI element.

## Folder Structure

```
Assets/

```

## UI Performance Rules

1. **Canvas splitting**: Static UI on one Canvas, dynamic (health bars, damage numbers) on another
2. **No Layout Groups on frequently-updated UI** - they trigger full rebuild
3. **Disable Raycast Target** on non-interactive Images/Text
4. **Use TMP Pro** everywhere (never legacy Text)
5. **Pool damage numbers and floating text** - never Instantiate in gameplay
6. **Use sprite atlases** for UI sprites (Window > 2D > Sprite Atlas)
7. Event-driven updates - subscribe to GameEvents, don't poll

## Key Dependencies
- DOTween (Demigiant) - animation
- TextMeshPro - text rendering
- Input System - input handling
- Unity UI (uGUI) - main UI system

## Current Focus
Main Menu UI - optimization and polish phase.

##Important Documentation

##Docs/ui_optimization_guide.md - UI optimization rules. ALWAYS reference this when reviewing or writing UI code.

##Docs/UI_AUDIT_*.md - Periodic audit reports

##DO's

1. Use ScriptableObjects for tower/enemy/wave data
2. Subscribe events in OnEnable, unsubscribe in OnDisable
3. Use [SerializeField] private instead of public for inspector fields
4. Cache GetComponent results in Awake/Start
5. Use tmpText.SetText("Gold: {0}", gold) for TMP updates
6. Profile on low-end mobile device before optimizing

##DON'Ts

1. NO FindObjectOfType or GameObject.Find in runtime code
2. NO GetComponent in Update/FixedUpdate
3. NO string concatenation in Update ("Gold: " + gold)
4. NO Instantiate/Destroy in gameplay loops (use pools)
5. NO Layout Groups on dynamic content
6. NO Coroutines for simple tweens (use DOTween)
7. NO public fields exposed in Inspector (use [SerializeField] private)