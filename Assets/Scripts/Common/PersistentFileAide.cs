using System;
using System.IO;
using UnityEngine;

namespace Common
{
	public static class PersistentFileAide
	{
		public static void SaveFile(byte[] bytes, string fileName)
		{
			BinaryFileAide.SaveFile(bytes, PersistentFileAide.GetPath(fileName));
		}

		public static byte[] LoadFile(string fileName)
		{
			return BinaryFileAide.LoadFile(PersistentFileAide.GetPath(fileName));
		}

		public static bool FileExist(string fileName)
		{
			return File.Exists(PersistentFileAide.GetPath(fileName));
		}

		private static string GetPath(string fileName)
		{
			return Application.persistentDataPath + "/" + fileName;
		}
	}
}
