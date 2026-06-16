using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

// Guards the "{id}_{level}" naming convention that couples tower prefabs, their TowerParameter
// data, and their bullet prefabs by NAME ONLY (see PoolNames / TowerPool / BulletPool.GetForTower).
// Copy-pasting a prefab without fixing its serialized id/level, or adding an attack tower without
// its bullet prefab, otherwise only surfaces at runtime (Debug.LogError + a null bullet mid-combat).
//
// TowerParameterManager is loaded at runtime from data, so it is empty in EditMode — the source of
// truth here is the prefabs in the project, enumerated via AssetDatabase.
public class TowerBulletNamingTest
{
    private const string TowerModelTypeName = "Gameplay.TowerModel";
    private const string CommonAttackControllerTypeName = "Gameplay.TowerAttackSingleTargetCommonController";

    private static readonly Regex TowerName = new Regex(@"^tower_(\d+)_(\d+)$");
    private static readonly Regex BulletName = new Regex(@"^bullet_(\d+)_(\d+)$");

    private struct PrefabId
    {
        public string path;
        public int id;
        public int level;
    }

    // The Linh-Lv2 drift class of bug: filename says (id,level) but the serialized TowerModel disagrees,
    // so the tower fires bullet_{wrongId}_{wrongLevel} and uses the wrong data row.
    [Test]
    public void TowerPrefabs_SerializedIdLevel_MatchFileName()
    {
        Type towerModelType = FindType(TowerModelTypeName);
        FieldInfo idField = towerModelType.GetField("id", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo levelField = towerModelType.GetField("level", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsNotNull(idField, "TowerModel.id field not found via reflection.");
        Assert.IsNotNull(levelField, "TowerModel.level field not found via reflection.");

        List<PrefabId> towers = FindPrefabs(TowerName);
        Assert.IsNotEmpty(towers, "No tower_{id}_{level} prefabs found in the project.");

        List<string> problems = new List<string>();
        foreach (PrefabId t in towers)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(t.path);
            Component model = (prefab != null) ? prefab.GetComponent(towerModelType) : null;
            if (model == null)
            {
                problems.Add($"{t.path}: no TowerModel component on root.");
                continue;
            }
            int serId = (int)idField.GetValue(model);
            int serLevel = (int)levelField.GetValue(model);
            if (serId != t.id || serLevel != t.level)
            {
                problems.Add($"{t.path}: name=({t.id},{t.level}) but TowerModel=({serId},{serLevel}).");
            }
        }
        Assert.IsEmpty(problems, "Tower prefab name vs serialized id/level drift:\n" + string.Join("\n", problems));
    }

    // Every tower that fires through the common bullet controller must have its bullet prefab, because
    // that controller calls BulletPool.GetForTower(id, level) => "bullet_{id}_{level}". Towers without
    // that controller (barracks, gold, custom-ultimate firing) intentionally have no required bullet.
    [Test]
    public void CommonAttackTowers_HaveMatchingBulletPrefab()
    {
        Type controllerType = FindType(CommonAttackControllerTypeName);

        HashSet<(int, int)> bullets = new HashSet<(int, int)>(
            FindPrefabs(BulletName).Select(b => (b.id, b.level)));

        List<PrefabId> towers = FindPrefabs(TowerName);
        Assert.IsNotEmpty(towers, "No tower_{id}_{level} prefabs found in the project.");

        List<string> missing = new List<string>();
        foreach (PrefabId t in towers)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(t.path);
            if (prefab == null)
            {
                continue;
            }
            bool firesCommonBullet = prefab.GetComponentInChildren(controllerType, true) != null;
            if (firesCommonBullet && !bullets.Contains((t.id, t.level)))
            {
                missing.Add($"{t.path} uses {CommonAttackControllerTypeName} but bullet_{t.id}_{t.level} is missing.");
            }
        }
        Assert.IsEmpty(missing, "Attack towers missing their bullet prefab:\n" + string.Join("\n", missing));
    }

    // A tower's levels must run 0..max with no hole (a missing middle level breaks the upgrade chain).
    [Test]
    public void TowerLevels_AreContiguousFromZero()
    {
        Dictionary<int, List<int>> levelsById = new Dictionary<int, List<int>>();
        foreach (PrefabId t in FindPrefabs(TowerName))
        {
            if (!levelsById.TryGetValue(t.id, out List<int> levels))
            {
                levels = new List<int>();
                levelsById[t.id] = levels;
            }
            levels.Add(t.level);
        }
        Assert.IsNotEmpty(levelsById, "No tower_{id}_{level} prefabs found in the project.");

        List<string> gaps = new List<string>();
        foreach (KeyValuePair<int, List<int>> kv in levelsById)
        {
            List<int> levels = kv.Value;
            for (int expected = 0; expected < levels.Count; expected++)
            {
                if (!levels.Contains(expected))
                {
                    gaps.Add($"tower id {kv.Key}: has levels [{string.Join(",", levels.OrderBy(x => x))}], missing level {expected}.");
                    break;
                }
            }
        }
        Assert.IsEmpty(gaps, "Tower level gaps:\n" + string.Join("\n", gaps));
    }

    // --- helpers ---

    // All prefabs in the project whose file name strictly matches the convention, with id/level parsed.
    private static List<PrefabId> FindPrefabs(Regex strict)
    {
        List<PrefabId> result = new List<PrefabId>();
        foreach (string guid in AssetDatabase.FindAssets("t:GameObject"))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Match m = strict.Match(Path.GetFileNameWithoutExtension(path));
            if (!m.Success)
            {
                continue;
            }
            result.Add(new PrefabId
            {
                path = path,
                id = int.Parse(m.Groups[1].Value),
                level = int.Parse(m.Groups[2].Value)
            });
        }
        return result;
    }

    private static Type FindType(string fullName)
    {
        Type type = AppDomain.CurrentDomain.GetAssemblies()
            .Select(asm => asm.GetType(fullName))
            .FirstOrDefault(t => t != null);
        Assert.IsNotNull(type, $"Type {fullName} not found in any loaded assembly.");
        return type;
    }
}
