using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace EditorTools
{
	// Gộp nhiều file PNG nhỏ trong 1 thư mục thành 1 file PNG lớn (atlas).
	// - Cắt sprite native vào import settings (.meta) để Unity nhận từng sprite con.
	// - Xuất kèm 1 file JSON toạ độ (tuỳ chọn dùng cho tool/pipeline ngoài).
	// PNG output BẮT BUỘC nằm trong thư mục Assets của project.
	// Mở qua menu: Tools/Atlas/PNG Atlas Packer
	public class PngAtlasPacker : EditorWindow
	{
		private string _inputFolder = "";
		private string _outputPng = "";
		private int _padding = 2;
		private int _maxSize = 4096;
		private bool _exportJson = true;

		[MenuItem("Tools/Atlas/PNG Atlas Packer")]
		private static void Open()
		{
			GetWindow<PngAtlasPacker>("PNG Atlas Packer");
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField("PNG Atlas Packer", EditorStyles.boldLabel);
			EditorGUILayout.HelpBox(
				"Chọn thư mục chứa PNG nhỏ -> gộp thành 1 PNG lớn và cắt sprite native (Sprite Mode = Multiple).\n" +
				"Output PHẢI nằm trong thư mục Assets thì Unity mới nhận sprite.\n" +
				"Toạ độ có gốc (0,0) ở GÓC DƯỚI-TRÁI (quy ước texture Unity).",
				MessageType.Info);

			DrawFolderField();
			DrawOutputField();

			_padding = EditorGUILayout.IntField("Padding (px)", _padding);
			_maxSize = EditorGUILayout.IntField("Max Size (px)", _maxSize);
			_exportJson = EditorGUILayout.Toggle("Export JSON", _exportJson);

			EditorGUILayout.Space(10);

			using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_inputFolder) || string.IsNullOrEmpty(_outputPng)))
			{
				if (GUILayout.Button("Pack", GUILayout.Height(30)))
				{
					Pack();
				}
			}
		}

		private void DrawFolderField()
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Input Folder", _inputFolder, EditorStyles.textField);
			if (GUILayout.Button("Browse", GUILayout.Width(70)))
			{
				string picked = EditorUtility.OpenFolderPanel("Chọn thư mục chứa PNG", Application.dataPath, "");
				if (!string.IsNullOrEmpty(picked))
				{
					_inputFolder = picked;
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		private void DrawOutputField()
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Output PNG", _outputPng, EditorStyles.textField);
			if (GUILayout.Button("Browse", GUILayout.Width(70)))
			{
				string picked = EditorUtility.SaveFilePanel("Lưu file PNG atlas (trong Assets)", Application.dataPath, "atlas", "png");
				if (!string.IsNullOrEmpty(picked))
				{
					_outputPng = picked;
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		private void Pack()
		{
			if (!TryGetAssetPath(_outputPng, out string assetPath))
			{
				EditorUtility.DisplayDialog("PNG Atlas Packer",
					"Output PNG phải nằm trong thư mục Assets của project để Unity cắt sprite được.", "OK");
				return;
			}

			string[] pngPaths = Directory.GetFiles(_inputFolder, "*.png", SearchOption.TopDirectoryOnly);
			if (pngPaths.Length == 0)
			{
				EditorUtility.DisplayDialog("PNG Atlas Packer", "Không tìm thấy file PNG nào trong thư mục.", "OK");
				return;
			}

			Texture2D[] textures = LoadTextures(pngPaths, out string[] names);

			Texture2D atlas = new Texture2D(_maxSize, _maxSize, TextureFormat.RGBA32, false);
			Rect[] uvRects = atlas.PackTextures(textures, _padding, _maxSize);

			File.WriteAllBytes(_outputPng, atlas.EncodeToPNG());
			AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

			ApplySpriteSlices(assetPath, atlas, uvRects, names);

			if (_exportJson)
			{
				WriteJson(atlas, uvRects, names);
			}

			Cleanup(textures, atlas);

			AssetDatabase.Refresh();
			EditorUtility.DisplayDialog("PNG Atlas Packer",
				$"Đã gộp {textures.Length} PNG -> {atlas.width}x{atlas.height}.\nSprite đã cắt native trong:\n{assetPath}", "OK");
		}

		// Đọc PNG trực tiếp từ ổ đĩa để bảo đảm texture readable, không phụ thuộc import settings.
		private static Texture2D[] LoadTextures(string[] pngPaths, out string[] names)
		{
			Texture2D[] textures = new Texture2D[pngPaths.Length];
			names = new string[pngPaths.Length];
			for (int i = 0; i < pngPaths.Length; i++)
			{
				Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
				tex.LoadImage(File.ReadAllBytes(pngPaths[i]));
				textures[i] = tex;
				names[i] = Path.GetFileNameWithoutExtension(pngPaths[i]);
			}
			return textures;
		}

		// Ghi slice vào import settings để Unity nhận sprite native (không cần JSON).
		private static void ApplySpriteSlices(string assetPath, Texture2D atlas, Rect[] uvRects, string[] names)
		{
			TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
			if (importer == null)
			{
				Debug.LogError($"[PngAtlasPacker] Không lấy được TextureImporter cho {assetPath}");
				return;
			}

			importer.textureType = TextureImporterType.Sprite;
			importer.spriteImportMode = SpriteImportMode.Multiple;

			SpriteDataProviderFactories factory = new SpriteDataProviderFactories();
			factory.Init();
			ISpriteEditorDataProvider dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
			dataProvider.InitSpriteEditorDataProvider();

			SpriteRect[] rects = new SpriteRect[uvRects.Length];
			List<SpriteNameFileIdPair> nameIdPairs = new List<SpriteNameFileIdPair>(uvRects.Length);
			for (int i = 0; i < uvRects.Length; i++)
			{
				Rect uv = uvRects[i];
				GUID id = GUID.Generate();
				rects[i] = new SpriteRect
				{
					name = names[i],
					spriteID = id,
					rect = new Rect(
						Mathf.RoundToInt(uv.x * atlas.width),
						Mathf.RoundToInt(uv.y * atlas.height),
						Mathf.RoundToInt(uv.width * atlas.width),
						Mathf.RoundToInt(uv.height * atlas.height)),
					pivot = new Vector2(0.5f, 0.5f),
					alignment = SpriteAlignment.Center,
					border = Vector4.zero
				};
				nameIdPairs.Add(new SpriteNameFileIdPair(names[i], id));
			}

			dataProvider.SetSpriteRects(rects);

			// Gắn name<->fileId để tham chiếu sprite ổn định qua các lần reimport.
			ISpriteNameFileIdDataProvider nameIdProvider = dataProvider.GetDataProvider<ISpriteNameFileIdDataProvider>();
			nameIdProvider.SetNameFileIdPairs(nameIdPairs);

			dataProvider.Apply();
			importer.SaveAndReimport();
		}

		private void WriteJson(Texture2D atlas, Rect[] uvRects, string[] names)
		{
			AtlasMetadata meta = new AtlasMetadata
			{
				image = Path.GetFileName(_outputPng),
				width = atlas.width,
				height = atlas.height,
				origin = "bottom-left",
				sprites = new List<AtlasSpriteEntry>(uvRects.Length)
			};

			for (int i = 0; i < uvRects.Length; i++)
			{
				Rect uv = uvRects[i];
				meta.sprites.Add(new AtlasSpriteEntry
				{
					name = names[i],
					x = Mathf.RoundToInt(uv.x * atlas.width),
					y = Mathf.RoundToInt(uv.y * atlas.height),
					width = Mathf.RoundToInt(uv.width * atlas.width),
					height = Mathf.RoundToInt(uv.height * atlas.height)
				});
			}

			string jsonPath = Path.ChangeExtension(_outputPng, ".json");
			File.WriteAllText(jsonPath, JsonUtility.ToJson(meta, true));
		}

		// Đổi đường dẫn tuyệt đối -> đường dẫn asset "Assets/...". Trả về false nếu nằm ngoài Assets.
		private static bool TryGetAssetPath(string absolutePath, out string assetPath)
		{
			assetPath = null;
			string dataPath = Application.dataPath.Replace('\\', '/');
			string abs = absolutePath.Replace('\\', '/');
			if (!abs.StartsWith(dataPath, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			assetPath = "Assets" + abs.Substring(dataPath.Length);
			return true;
		}

		private static void Cleanup(Texture2D[] textures, Texture2D atlas)
		{
			foreach (Texture2D tex in textures)
			{
				DestroyImmediate(tex);
			}
			DestroyImmediate(atlas);
		}

		[Serializable]
		private class AtlasSpriteEntry
		{
			public string name;
			public int x;
			public int y;
			public int width;
			public int height;
		}

		[Serializable]
		private class AtlasMetadata
		{
			public string image;
			public int width;
			public int height;
			public string origin;
			public List<AtlasSpriteEntry> sprites;
		}
	}
}
