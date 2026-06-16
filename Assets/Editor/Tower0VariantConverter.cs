using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Common.Editor
{
	// Scaffolds tower_0_1..tower_0_4 as Prefab Variants of tower_0_0 (the base) to kill per-level
	// config drift at the source (see TowerBulletNamingTest / the Linh-Lv2 drift bug). Tower prefabs
	// are coupled to code/Addressables BY NAME, and their GUIDs are referenced only by the Addressables
	// "towers" group, so adopting variants while keeping each .meta GUID leaves code + addresses intact.
	//
	// WHY a script and not hand-edited YAML: Prefab Variant relationships must be authored by Unity.
	//
	// WHY this does NOT blindly auto-copy every component value: these prefabs hold nested INTERNAL
	// references (bulletParametersInOneTurn -> "Gun Barrel 0"; the StartAttack / OnCreateBullet
	// UnityEvents -> "Tower Animation Controller"). Copying those with ComponentUtility would remap
	// them wrong (cross-prefab / null) and break the wiring — wiring that is identical across levels
	// and should simply be INHERITED from the base. So the script only auto-applies the one fully-safe
	// override (the `level` field) and prints a precise per-property diff report for everything else,
	// which you apply in the Inspector (Unity remaps internal refs correctly there).
	//
	// Workflow:
	//   1. Commit / branch first (no IDE undo for prefab edits).
	//   2. Tools > Tower0 Variants > 1. Generate Preview + Report
	//      -> writes tower_0_{L}_variant.prefab (variant of tower_0_0) and logs the override checklist.
	//   3. Open each *_variant, apply the listed overrides (sprites, animator, sounds, attack params,
	//      ultimate components) and fix anything flagged REVIEW (e.g. shadow named "Shadow" vs "bong").
	//   4. Adopt in the Editor: re-point the Addressable address of each level to its verified *_variant
	//      (or right-click the original > recreate as variant). Keep tower_0_0 as the base.
	public static class Tower0VariantConverter
	{
		private const string TowersDir = "Assets/AddressableContent/towers";
		private const string BaseName = "tower_0_0";
		private static readonly int[] Levels = { 1, 2, 3, 4 };

		[MenuItem("Tools/Tower0 Variants/1. Generate Preview + Report")]
		private static void GeneratePreview()
		{
			GameObject basePrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{TowersDir}/{BaseName}.prefab");
			if (basePrefab == null)
			{
				EditorUtility.DisplayDialog("Tower0 Variants", $"Base prefab not found: {TowersDir}/{BaseName}.prefab", "OK");
				return;
			}

			StringBuilder report = new StringBuilder();
			report.AppendLine($"Tower0 Variant report — base = {BaseName}");
			report.AppendLine("Apply the listed overrides on each *_variant in the Inspector. INTERNAL refs are inherited from base (do not re-wire unless flagged).");

			int made = 0;
			foreach (int level in Levels)
			{
				if (GenerateOneVariant(basePrefab, level, report))
				{
					made++;
				}
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			Debug.Log(report.ToString());
			EditorUtility.DisplayDialog("Tower0 Variants",
				$"Generated {made} preview variant(s) in {TowersDir}.\nSee Console for the override checklist.", "OK");
		}

		// Create tower_0_{level}_variant as a variant of the base, apply the safe `level` override,
		// and append a diff of every other property that differs from the base.
		private static bool GenerateOneVariant(GameObject basePrefab, int level, StringBuilder report)
		{
			string origPath = $"{TowersDir}/tower_0_{level}.prefab";
			GameObject origPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(origPath);
			if (origPrefab == null)
			{
				report.AppendLine($"\n[L{level}] SKIP — {origPath} not found.");
				return false;
			}

			report.AppendLine($"\n[L{level}] tower_0_{level}  (overrides to apply on the variant):");

			GameObject inst = (GameObject)PrefabUtility.InstantiatePrefab(basePrefab);
			try
			{
				// Safe auto-override: the level field on the root TowerModel.
				ApplyLevelOverride(inst, level, report);

				// Diff report: compare every matching GameObject/component, base vs original.
				Dictionary<string, Transform> baseMap = MapByPath(inst.transform);
				Dictionary<string, Transform> origMap = MapByPath(origPrefab.transform);
				foreach (KeyValuePair<string, Transform> kv in origMap)
				{
					if (baseMap.TryGetValue(kv.Key, out Transform baseT))
					{
						DiffComponents(baseT, kv.Value, kv.Key, report);
					}
					else
					{
						report.AppendLine($"    + ADD GameObject '{kv.Key}' (exists in original, not in base — e.g. ultimate objects).");
					}
				}
				foreach (KeyValuePair<string, Transform> kv in baseMap)
				{
					if (kv.Key.Length > 0 && !origMap.ContainsKey(kv.Key))
					{
						report.AppendLine($"    ! REVIEW base GameObject '{kv.Key}' has no counterpart in original (e.g. shadow naming).");
					}
				}

				string outPath = $"{TowersDir}/tower_0_{level}_variant.prefab";
				PrefabUtility.SaveAsPrefabAsset(inst, outPath);
				report.AppendLine($"    -> wrote {outPath}");
				return true;
			}
			finally
			{
				Object.DestroyImmediate(inst);
			}
		}

		private static void ApplyLevelOverride(GameObject inst, int level, StringBuilder report)
		{
			Transform root = inst.transform;
			Component towerModel = FindComponentByTypeName(root, "TowerModel");
			if (towerModel == null)
			{
				report.AppendLine("    ! REVIEW no TowerModel on root — could not set level.");
				return;
			}
			SerializedObject so = new SerializedObject(towerModel);
			SerializedProperty levelProp = so.FindProperty("level");
			if (levelProp != null)
			{
				levelProp.intValue = level;
				so.ApplyModifiedProperties();
				report.AppendLine($"    * level = {level} (applied automatically).");
			}
			else
			{
				report.AppendLine("    ! REVIEW TowerModel has no serialized 'level' field.");
			}
		}

		// Report every top-level serialized property that differs between the base and original
		// component, classified so the checklist stays low-noise and accurate.
		private static void DiffComponents(Transform baseT, Transform origT, string path, StringBuilder report)
		{
			Component[] baseComps = baseT.GetComponents<Component>();
			Component[] origComps = origT.GetComponents<Component>();
			HashSet<Component> used = new HashSet<Component>();

			foreach (Component oc in origComps)
			{
				if (oc == null)
				{
					report.AppendLine($"    ! REVIEW '{path}' has a missing/null script.");
					continue;
				}
				Component bc = FirstUnused(baseComps, oc.GetType(), used);
				if (bc == null)
				{
					report.AppendLine($"    + ADD component {oc.GetType().Name} on '{path}'.");
					continue;
				}
				used.Add(bc);
				DiffComponentProps(bc, oc, path, report);
			}
		}

		private static void DiffComponentProps(Component baseComp, Component origComp, string path, StringBuilder report)
		{
			SerializedObject soBase = new SerializedObject(baseComp);
			SerializedObject soOrig = new SerializedObject(origComp);
			SerializedProperty it = soOrig.GetIterator();
			bool enter = true;
			while (it.NextVisible(enter))
			{
				enter = false; // top-level properties only
				if (it.propertyPath == "m_Script")
				{
					continue;
				}
				SerializedProperty baseProp = soBase.FindProperty(it.propertyPath);
				if (baseProp == null)
				{
					continue;
				}
				string a = PropToString(baseProp);
				string b = PropToString(it);
				if (a != b)
				{
					report.AppendLine($"    - {path} / {origComp.GetType().Name}.{it.propertyPath}: base={a}  ->  orig={b}");
				}
			}
		}

		// Human-comparable string for a property. Asset refs become their path (stable to compare);
		// internal object refs are marked [internal] so they read as "inherited"; complex containers
		// (arrays / UnityEvents / structs) are marked so they're verified by hand, not blindly copied.
		private static string PropToString(SerializedProperty p)
		{
			switch (p.propertyType)
			{
				case SerializedPropertyType.Integer: return p.intValue.ToString();
				case SerializedPropertyType.Boolean: return p.boolValue.ToString();
				case SerializedPropertyType.Float: return p.floatValue.ToString("0.####");
				case SerializedPropertyType.String: return "\"" + p.stringValue + "\"";
				case SerializedPropertyType.Enum: return p.enumValueIndex.ToString();
				case SerializedPropertyType.Color: return p.colorValue.ToString();
				case SerializedPropertyType.Vector2: return p.vector2Value.ToString();
				case SerializedPropertyType.Vector3: return p.vector3Value.ToString();
				case SerializedPropertyType.ObjectReference:
				{
					Object o = p.objectReferenceValue;
					if (o == null) return "null";
					if (EditorUtility.IsPersistent(o)) return "asset:" + AssetDatabase.GetAssetPath(o);
					return "[internal]";
				}
				default:
					return $"<complex:{p.propertyType}>"; // arrays / UnityEvents / managed structs — verify in Inspector
			}
		}

		// --- helpers ---

		private static Dictionary<string, Transform> MapByPath(Transform root)
		{
			Dictionary<string, Transform> map = new Dictionary<string, Transform>();
			Walk(root, "", map);
			return map;
		}

		private static void Walk(Transform t, string path, Dictionary<string, Transform> map)
		{
			map[path] = t;
			foreach (Transform child in t)
			{
				string childPath = path.Length == 0 ? child.name : path + "/" + child.name;
				if (!map.ContainsKey(childPath))
				{
					Walk(child, childPath, map);
				}
			}
		}

		private static Component FirstUnused(Component[] comps, System.Type type, HashSet<Component> used)
		{
			foreach (Component c in comps)
			{
				if (c != null && c.GetType() == type && !used.Contains(c))
				{
					return c;
				}
			}
			return null;
		}

		private static Component FindComponentByTypeName(Transform root, string typeName)
		{
			foreach (Component c in root.GetComponents<Component>())
			{
				if (c != null && c.GetType().Name == typeName)
				{
					return c;
				}
			}
			return null;
		}
	}
}
