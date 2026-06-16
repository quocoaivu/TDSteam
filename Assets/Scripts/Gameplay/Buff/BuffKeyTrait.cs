using System;
using UnityEngine;

namespace Gameplay
{
	public class BuffKeyTrait : PropertyAttribute
	{
		public BuffKeyTrait(BuffKeyTrait.KeyType keyType)
		{
			this.keyType = keyType;
		}

		public BuffKeyTrait.KeyType GetKeyType()
		{
			return keyType;
		}

		private BuffKeyTrait.KeyType keyType;

		public enum KeyType
		{
			ToEnemy,
			ToTower
		}
	}
}
