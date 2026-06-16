using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace EditorTools
{
	// One-shot utility: forces every TMP_FontAsset through TMP's data upgrade,
	// then writes it back to disk so the "Upgrading font asset ... to version X"
	// log stops repeating. Run once, then delete this file.
	// Menu: Tools/TMP/Re-save Font Assets (Upgrade)
	public static class ReSaveTMPFontAssets
	{
		[MenuItem("Tools/TMP/Re-save Font Assets (Upgrade)")]
		private static void ReSaveAll()
		{
			string[] guids = AssetDatabase.FindAssets("t:TMP_FontAsset");
			if (guids.Length == 0)
			{
				Debug.Log("[ReSaveTMPFontAssets] No TMP_FontAsset found.");
				return;
			}

			var log = new StringBuilder();
			int saved = 0;

			foreach (string guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
				if (fontAsset == null)
				{
					continue;
				}

				// Runs UpgradeFontAsset() internally when the on-disk schema is old.
				fontAsset.ReadFontAssetDefinition();
				EditorUtility.SetDirty(fontAsset);
				saved++;
				log.AppendLine($"  {path}");
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			Debug.Log($"[ReSaveTMPFontAssets] Re-saved {saved} font asset(s):\n{log}");
		}
	}
}
