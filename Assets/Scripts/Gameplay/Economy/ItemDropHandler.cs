using System;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	// Floating "got an item" indicator dropped at an enemy's death position. Mirrors CrystalHandler /
	// DroppedBullionHandler: the item itself is auto-collected into the inventory (ItemDropRoller); this
	// is only the visual cue, which despawns after lifeTime.
	public class ItemDropHandler : BaseMonoBehaviour
	{
		[SerializeField]
		private float lifeTime;

		[SerializeField]
		private TextMesh textMesh;

		public void Init(string itemName)
		{
			textMesh.text = itemName;
			base.CustomInvoke(new Action(LateAnimationOpen), lifeTime);
		}

		private void LateAnimationOpen()
		{
			MonoSingleton<FXPool>.Instance.Despawn(base.gameObject);
		}
	}
}
