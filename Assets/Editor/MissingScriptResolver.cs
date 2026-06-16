#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

// One-shot migration helper for prefabs/scenes that reference deleted plugin DLLs
// (old TextMeshPro DLL, old Unity UI DLL, old DOTween Pro DLL, etc.).
//
// Walks every .prefab and .unity file under Assets/, finds m_Script entries whose
// guid is unresolved by AssetDatabase, sniffs the serialized field names that follow,
// and rebinds the reference to a concrete MonoBehaviour type whose declared
// SerializeField set matches. Original files backed up to <file>.bak.
//
// Menu items:
//   Tools/Missing Script Resolver/1. Scan (no writes)
//   Tools/Missing Script Resolver/2. Repair (writes .bak then rewrites)
public static class MissingScriptResolver
{
	private const string Prefix = "[MSR] ";

	[MenuItem("Tools/Missing Script Resolver/1. Scan (no writes)")]
	public static void Scan() { Run(applyChanges: false); }

	[MenuItem("Tools/Missing Script Resolver/2. Repair (writes .bak then rewrites)")]
	public static void Repair() { Run(applyChanges: true); }

	private static void Run(bool applyChanges)
	{
		Debug.Log(Prefix + (applyChanges ? "REPAIR mode" : "SCAN mode"));

		var index = BuildTypeIndex();
		Debug.Log(Prefix + "Indexed " + index.Count + " concrete MonoBehaviour types with their canonical (guid, fileID).");

		var files = new List<string>();
		files.AddRange(Directory.GetFiles("Assets", "*.prefab", SearchOption.AllDirectories));
		files.AddRange(Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories));
		Debug.Log(Prefix + "Will scan " + files.Count + " files.");

		int filesChanged = 0;
		int refsFixed = 0;
		var unresolvedByCombo = new Dictionary<string, UnresolvedInfo>();

		foreach (var path in files)
		{
			string text = File.ReadAllText(path);
			string newText;
			int fixedInThisFile = TryRewrite(path, text, index, unresolvedByCombo, out newText);
			if (fixedInThisFile > 0)
			{
				if (applyChanges)
				{
					File.WriteAllText(path + ".bak", text);
					File.WriteAllText(path, newText);
				}
				filesChanged++;
				refsFixed += fixedInThisFile;
			}
		}

		Debug.Log(Prefix + "Resolved " + refsFixed + " broken refs across " + filesChanged + " files.");
		if (unresolvedByCombo.Count > 0)
		{
			var sb = new StringBuilder();
			sb.Append(Prefix).Append("Unresolved combos (").Append(unresolvedByCombo.Count).Append("):\n");
			foreach (var kv in unresolvedByCombo.OrderByDescending(p => p.Value.count))
			{
				sb.Append("  ").Append(kv.Key).Append("  x").Append(kv.Value.count).Append("  fields=[").Append(string.Join(",", kv.Value.sampleFields)).Append("]  e.g. ").Append(kv.Value.sampleFile).Append('\n');
			}
			Debug.LogWarning(sb.ToString());
		}

		if (applyChanges)
		{
			AssetDatabase.Refresh();
			Debug.Log(Prefix + "AssetDatabase refreshed.");
		}
	}

	// ------- Type index -------

	private struct TypeRef
	{
		public Type type;
		public string guid;
		public long fileID;
		public HashSet<string> fieldNames; // serialized field names declared on this type and its base chain
	}

	private static List<TypeRef> BuildTypeIndex()
	{
		var result = new List<TypeRef>();
		var allTypes = TypeCache.GetTypesDerivedFrom<MonoBehaviour>();
		foreach (var t in allTypes)
		{
			if (t == null) continue;
			if (t.IsAbstract || t.IsGenericTypeDefinition) continue;
			string guid;
			long fid;
			if (!TryGetMonoScriptId(t, out guid, out fid)) continue;
			var fields = CollectSerializedFieldNames(t);
			if (fields.Count == 0) continue;
			result.Add(new TypeRef { type = t, guid = guid, fileID = fid, fieldNames = fields });
		}
		return result;
	}

	private static bool TryGetMonoScriptId(Type t, out string guid, out long fileID)
	{
		guid = null; fileID = 0;
		// Resolve via AssetDatabase to avoid AddComponent side-effects (Awake/OnEnable/Reset
		// can throw on a bare probe GO and spam the Console).
		var candidates = AssetDatabase.FindAssets("t:MonoScript " + t.Name);
		foreach (var g in candidates)
		{
			var path = AssetDatabase.GUIDToAssetPath(g);
			var script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
			if (script != null && script.GetClass() == t)
			{
				return AssetDatabase.TryGetGUIDAndLocalFileIdentifier(script, out guid, out fileID);
			}
		}
		return false;
	}

	private static HashSet<string> CollectSerializedFieldNames(Type t)
	{
		var names = new HashSet<string>();
		var cur = t;
		while (cur != null && cur != typeof(MonoBehaviour) && cur != typeof(Behaviour) && cur != typeof(Component) && cur != typeof(UnityEngine.Object))
		{
			var fields = cur.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
			foreach (var f in fields)
			{
				if (f.IsStatic) continue;
				if (f.IsNotSerialized) continue;
				bool isPublic = f.IsPublic;
				bool hasSerField = f.GetCustomAttribute<SerializeField>() != null;
				if (!isPublic && !hasSerField) continue;
				names.Add(f.Name);
			}
			cur = cur.BaseType;
		}
		return names;
	}

	// ------- YAML rewrite -------

	private class UnresolvedInfo
	{
		public int count;
		public string sampleFile;
		public List<string> sampleFields = new List<string>();
	}

	private static readonly Regex ScriptRefRegex = new Regex(@"^(?<indent>\s*)m_Script:\s*\{fileID:\s*(?<fid>-?\d+),\s*guid:\s*(?<guid>[a-f0-9]{32}),\s*type:\s*3\}\s*$", RegexOptions.Multiline | RegexOptions.Compiled);

	// Fields that appear in every MonoBehaviour YAML block but aren't part of the
	// subclass's serialized signature. Filtering them lets the field-set match.
	private static readonly HashSet<string> UniversalYamlFields = new HashSet<string>
	{
		"m_Name", "m_EditorClassIdentifier",
		"m_ObjectHideFlags", "m_CorrespondingSourceObject", "m_PrefabInstance",
		"m_PrefabAsset", "m_GameObject", "m_Enabled", "m_EditorHideFlags",
	};

	// When the OBSERVED field set matches more than one concrete type with the same
	// declared field count, fall back to this preferred-namespace ordering keyed by
	// the old guid that referenced the missing script. The first candidate whose
	// FullName starts with one of these namespaces wins.
	private static readonly Dictionary<string, string[]> NamespaceHintByOldGuid = new Dictionary<string, string[]>
	{
		// Old TMP DLL (TextMeshPro-2017.3-Runtime.dll)
		{ "0904eccab3b1ee9ab22971135755e058", new[] { "TMPro" } },
		// Old Unity UI DLLs (two variants, identical class set)
		{ "f70555f144d8491a825f0804e09c671c", new[] { "UnityEngine.UI", "UnityEngine.EventSystems" } },
		{ "f5f67c52d1564df4a8936ccd202a3bd8", new[] { "UnityEngine.UI", "UnityEngine.EventSystems" } },
	};

	// Hardcoded overrides for truly indistinguishable signatures. Each maps
	// "<oldGuid>:<oldFileID>" -> assembly-qualified type name. These are best-effort
	// guesses based on common UI patterns; if you find one wrong, swap and re-run.
	// Lines commented out are placeholders for refs the script left unresolved on the
	// previous scan -- fill in once you confirm the type in a sample prefab.
	private static readonly Dictionary<string, string> ManualTypeOverrides = new Dictionary<string, string>
	{
		// HorizontalLayoutGroup vs VerticalLayoutGroup — same SerializeField set.
		// Defaulting 1297475563 = Vertical (CreditPanel-style stacked text) and
		// -405508275 = Horizontal (NewTowerPanel-style icon rows). Swap if wrong.
		{ "f70555f144d8491a825f0804e09c671c:1297475563", "UnityEngine.UI.VerticalLayoutGroup" },
		{ "f70555f144d8491a825f0804e09c671c:-405508275", "UnityEngine.UI.HorizontalLayoutGroup" },
		{ "f5f67c52d1564df4a8936ccd202a3bd8:1297475563", "UnityEngine.UI.VerticalLayoutGroup" },
		{ "f5f67c52d1564df4a8936ccd202a3bd8:-405508275", "UnityEngine.UI.HorizontalLayoutGroup" },
		// Outline vs Shadow — same field set. Outline is the far more common choice
		// in this project's UI. Swap to Shadow if you see thicker-than-expected text.
		{ "f70555f144d8491a825f0804e09c671c:-900027084", "UnityEngine.UI.Outline" },
		// TMP_SubMeshUI (the lowercase m_fontAsset/m_spriteAsset signature)
		{ "0904eccab3b1ee9ab22971135755e058:1908110080", "TMPro.TMP_SubMeshUI" },
	};

	private static int TryRewrite(string path, string text, List<TypeRef> index, Dictionary<string, UnresolvedInfo> unresolved, out string newText)
	{
		newText = text;
		var lines = text.Replace("\r\n", "\n").Split('\n');
		int changes = 0;
		bool changedAny = false;

		for (int i = 0; i < lines.Length; i++)
		{
			var line = lines[i];
			var m = ScriptRefRegex.Match(line);
			if (!m.Success) continue;
			string oldGuid = m.Groups["guid"].Value;
			string oldFid = m.Groups["fid"].Value;
			string indent = m.Groups["indent"].Value;

			// Check if the guid is resolvable; if so leave the line alone.
			string maybePath = AssetDatabase.GUIDToAssetPath(oldGuid);
			if (!string.IsNullOrEmpty(maybePath) && File.Exists(maybePath))
			{
				continue;
			}

			// Collect field names from subsequent lines until next document marker or
			// the indent decreases. Only count lines at EXACTLY the m_Script indent
			// (i.e. siblings, not nested) and skip universal Unity fields that every
			// MonoBehaviour has — those would poison the match against the type's
			// own SerializeField set.
			var fieldNames = new List<string>();
			for (int j = i + 1; j < lines.Length; j++)
			{
				var ln = lines[j];
				if (ln.StartsWith("---")) break;
				if (string.IsNullOrEmpty(ln)) break;
				if (!ln.StartsWith(indent)) break;
				// Reject deeper-indented (nested struct/event) lines.
				if (ln.Length > indent.Length)
				{
					char next = ln[indent.Length];
					if (next == ' ' || next == '\t') continue;
				}
				var afterIndent = ln.Substring(indent.Length);
				var fm = Regex.Match(afterIndent, @"^(?<name>m_[A-Za-z0-9_]+):");
				if (!fm.Success) continue;
				var name = fm.Groups["name"].Value;
				if (UniversalYamlFields.Contains(name)) continue;
				fieldNames.Add(name);
				if (fieldNames.Count >= 20) break;
			}

			string comboKey = oldGuid + ":" + oldFid;
			TypeRef? best = null;
			string forcedTypeName;
			if (ManualTypeOverrides.TryGetValue(comboKey, out forcedTypeName))
			{
				best = LookupByFullName(index, forcedTypeName);
			}
			if (best == null)
			{
				string[] namespaceHint;
				NamespaceHintByOldGuid.TryGetValue(oldGuid, out namespaceHint);
				best = ScoreMatch(index, fieldNames, namespaceHint);
			}
			if (best == null)
			{
				UnresolvedInfo info;
				if (!unresolved.TryGetValue(comboKey, out info))
				{
					info = new UnresolvedInfo { sampleFile = path, sampleFields = fieldNames.Take(8).ToList() };
					unresolved[comboKey] = info;
				}
				info.count++;
				continue;
			}

			var newLine = indent + "m_Script: {fileID: " + best.Value.fileID + ", guid: " + best.Value.guid + ", type: 3}";
			lines[i] = newLine;
			changes++;
			changedAny = true;
		}

		if (changedAny)
		{
			newText = string.Join("\n", lines);
			// Preserve trailing newline if original had one
			if (text.EndsWith("\n") && !newText.EndsWith("\n")) newText += "\n";
		}
		return changes;
	}

	private static TypeRef? ScoreMatch(List<TypeRef> index, List<string> fieldNames, string[] namespaceHint)
	{
		if (fieldNames.Count == 0) return null;
		var observed = new HashSet<string>(fieldNames);

		// 1. Collect every candidate whose declared field set is a superset of observed.
		var passing = new List<TypeRef>();
		foreach (var tr in index)
		{
			if (observed.IsSubsetOf(tr.fieldNames)) passing.Add(tr);
		}
		if (passing.Count == 0) return null;

		// 2. If a namespace hint is supplied, prefer types whose FullName starts with
		//    one of the hinted prefixes. Use the hint order as priority — earlier
		//    hints win over later ones.
		if (namespaceHint != null && namespaceHint.Length > 0)
		{
			for (int p = 0; p < namespaceHint.Length; p++)
			{
				var hint = namespaceHint[p];
				var hinted = passing.Where(tr => tr.type.FullName != null && tr.type.FullName.StartsWith(hint + ".")).ToList();
				if (hinted.Count > 0)
				{
					passing = hinted;
					break;
				}
			}
		}

		// 3. Tie-break by smallest declared field set (most specific match).
		int bestSize = int.MaxValue;
		TypeRef? bestExact = null;
		int tieCount = 0;
		foreach (var tr in passing)
		{
			int size = tr.fieldNames.Count;
			if (size < bestSize)
			{
				bestSize = size;
				bestExact = tr;
				tieCount = 1;
			}
			else if (size == bestSize)
			{
				tieCount++;
			}
		}
		if (bestExact == null || tieCount > 1) return null;
		return bestExact;
	}

	private static TypeRef? LookupByFullName(List<TypeRef> index, string fullName)
	{
		foreach (var tr in index)
		{
			if (tr.type.FullName == fullName) return tr;
		}
		return null;
	}
}
#endif
