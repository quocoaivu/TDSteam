using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common
{
	public static class BinarySerializationAide
	{
		public static T Deserialize<T>(byte[] bytes)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			MemoryStream serializationStream = new MemoryStream(bytes);
			return (T)((object)binaryFormatter.Deserialize(serializationStream));
		}

		public static byte[] Serialize<T>(T dataObject)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter { Binder = new Common.SaveTypeCompatBinder() };
			MemoryStream memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, dataObject);
			return memoryStream.ToArray();
		}

		public static T Clone<T>(T originalObject)
		{
			return BinarySerializationAide.Deserialize<T>(BinarySerializationAide.Serialize<T>(originalObject));
		}
	}
}
