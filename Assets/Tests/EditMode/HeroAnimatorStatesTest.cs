using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Animations;

// Guards the hard mirror between HeroMotionHandler's animation-name strings
// and the state names in the shared base controller HeroAnimatorCommon.
// Renaming a state in the controller (or a string in code) otherwise only fails
// silently at runtime (animator.Play warning) with no compile error.
public class HeroAnimatorStatesTest
{
    private const string ControllerName = "HeroAnimatorCommon";
    private const string TypeFullName = "Gameplay.HeroMotionHandler";

    [Test]
    public void HeroMotionHandler_StateStrings_AllExistIn_HeroAnimatorCommon()
    {
        Dictionary<string, string> codeNames = GetAnimationNameFields();
        Assert.IsNotEmpty(codeNames, $"Could not read public static string fields from {TypeFullName}.");

        HashSet<string> states = GetControllerStateNames();
        Assert.IsNotEmpty(states, $"Could not read states from {ControllerName}.controller.");

        // Critical direction: every name the code calls Play(...) with must exist as a state.
        List<string> missing = codeNames
            .Where(kv => !states.Contains(kv.Value))
            .Select(kv => $"{kv.Key} = \"{kv.Value}\"")
            .ToList();

        Assert.IsEmpty(missing,
            $"These HeroMotionHandler strings have no matching state in {ControllerName}.controller " +
            $"(animator.Play would fail silently): {string.Join(", ", missing)}");

        // Informational: states present in the controller but not referenced by code.
        // Not a failure (may be reserved for future use), just surfaces drift.
        IEnumerable<string> codeValues = codeNames.Values;
        List<string> unreferenced = states.Where(s => !codeValues.Contains(s)).ToList();
        if (unreferenced.Count > 0)
        {
            UnityEngine.Debug.Log(
                $"[HeroAnimatorStatesTest] States in {ControllerName} not referenced by HeroMotionHandler: " +
                string.Join(", ", unreferenced));
        }
    }

    // Reads the 10 clip-name strings declared on HeroMotionHandler via reflection,
    // because the game code lives in Assembly-CSharp which an asmdef test cannot reference directly.
    private static Dictionary<string, string> GetAnimationNameFields()
    {
        Type type = AppDomain.CurrentDomain.GetAssemblies()
            .Select(asm => asm.GetType(TypeFullName))
            .FirstOrDefault(t => t != null);
        Assert.IsNotNull(type, $"Type {TypeFullName} not found in any loaded assembly.");

        return type
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(f => f.FieldType == typeof(string))
            .ToDictionary(f => f.Name, f => (string)f.GetValue(null));
    }

    private static HashSet<string> GetControllerStateNames()
    {
        string[] guids = AssetDatabase.FindAssets($"{ControllerName} t:AnimatorController");
        Assert.IsNotEmpty(guids, $"{ControllerName}.controller not found in the project.");

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
        Assert.IsNotNull(controller, $"Failed to load AnimatorController at {path}.");

        var names = new HashSet<string>();
        foreach (AnimatorControllerLayer layer in controller.layers)
        {
            CollectStateNames(layer.stateMachine, names);
        }
        return names;
    }

    private static void CollectStateNames(AnimatorStateMachine stateMachine, HashSet<string> names)
    {
        foreach (ChildAnimatorState child in stateMachine.states)
        {
            names.Add(child.state.name);
        }
        foreach (ChildAnimatorStateMachine sub in stateMachine.stateMachines)
        {
            CollectStateNames(sub.stateMachine, names);
        }
    }
}
