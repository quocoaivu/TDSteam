using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Common.Editor
{
	public static class AddressablesMigrationTool
	{
		[MenuItem("Tools/Addressables/Migrate Selected Folder")]
		private static void MigrateSelectedFolder()
		{
			string folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (string.IsNullOrEmpty(folderPath) || !AssetDatabase.IsValidFolder(folderPath))
			{
				EditorUtility.DisplayDialog("Migration", "Hãy select 1 folder trong Project window trước.", "OK");
				return;
			}
			MigrateFolder(folderPath);
		}

		private static void MigrateFolder(string folderPath)
		{
			AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
			if (settings == null)
			{
				EditorUtility.DisplayDialog("Migration", "Chưa có Addressables Settings. Mở Window > Asset Management > Addressables > Groups và bấm Create Addressables Settings.", "OK");
				return;
			}

			// Address prefix = tên folder được chọn (last segment).
			// Asset paths relative tới parent của folder này => address = "<folderName>/<subpath>/<file>".
			// Hoạt động bất kể folder nằm ở Resources/, AddressableContent/, hay đâu khác.
			string folderName = Path.GetFileName(folderPath);
			string parentPath = folderPath.Substring(0, folderPath.Length - folderName.Length);

			AddressableAssetGroup group = GetOrCreateGroup(settings, folderName);

			string[] assetGuids = AssetDatabase.FindAssets("", new[] { folderPath });
			List<string> assetPaths = new List<string>();
			foreach (string guid in assetGuids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				if (AssetDatabase.IsValidFolder(path))
				{
					continue;
				}
				assetPaths.Add(path);
			}

			int processed = 0;
			foreach (string assetPath in assetPaths)
			{
				string guid = AssetDatabase.AssetPathToGUID(assetPath);
				AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group, false, false);

				// Address = path relative tới parent (giữ tên folder ở đầu), no extension, lowercase.
				string addressRelative = assetPath.Substring(parentPath.Length);
				string addressNoExt = Path.ChangeExtension(addressRelative, null);
				entry.address = addressNoExt.ToLowerInvariant();
				processed++;
			}

			settings.SetDirty(AddressableAssetSettings.ModificationEvent.BatchModification, null, true, true);
			AssetDatabase.SaveAssets();

			EditorUtility.DisplayDialog(
				"Migration Done",
				string.Format(
					"Đã mark {0} assets là Addressable trong group '{1}'.\n\nAddress pattern: '{2}/...'\n\nBước tiếp theo:\n1. Nếu folder còn trong Resources/, move ra Assets/AddressableContent/{2}.\n2. Thêm dòng vào AssetLoaderBootstrap:\n   composite.RegisterAddressablesPrefix(\"{2}/\");",
					processed, group.Name, folderName),
				"OK");
		}

		private static AddressableAssetGroup GetOrCreateGroup(AddressableAssetSettings settings, string groupName)
		{
			AddressableAssetGroup existing = settings.FindGroup(groupName);
			if (existing != null)
			{
				return existing;
			}
			return settings.CreateGroup(
				groupName,
				false,
				false,
				false,
				null,
				typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema),
				typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.ContentUpdateGroupSchema));
		}
	}
}
