using System;
using Data;
using MetaGame;
using UnityEngine;
using UnityEngine.Serialization;

namespace MapLevel
{
	public class FormatSelectSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			CrossSceneData.Instance.BattleDifficulty = battleDifficulty;
			gameModeSelectGroupController.ShowSelectedImage(battleDifficulty);
			MapProgressStore.Instance.SaveLastMapModeChoose((int)(battleDifficulty + 1));
		}

		[SerializeField]
		private GameFormatSelectClusterHandler gameModeSelectGroupController;

		[FormerlySerializedAs("battleLevel")]
		[SerializeField]
		private BattleDifficulty battleDifficulty;
	}
}
