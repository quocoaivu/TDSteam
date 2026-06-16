using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class MinionOverviewDialog : MonoBehaviour
	{

        [SerializeField]
        private Text physicsArmorText;

        [SerializeField]
        private Text magicArmorText;

        [SerializeField]
        private Text damageText;

        [SerializeField]
        private Text healthText;


        private int gameEventSubscriberId;
        public void Init()
		{
			gameEventSubscriberId = GameKit.GetUniqueId();
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnSelectAlly, new MinionListenerRecord(gameEventSubscriberId, new GameSignalCenter.AllySubscribeMethod(SetInformation)));
		}

		private void OnDestroy()
		{
			GameSignalCenter.Instance.Unsubscribe(gameEventSubscriberId, GameSignalKind.OnSelectAlly);
		}

		public void SetInformation(MinionEntity allyModel)
		{
			if (allyModel == null)
			{
				base.gameObject.SetActive(false);
				return;
			}
			base.gameObject.SetActive(true);
			damageText.text = allyModel.PhysicsDamage_min + "-" + allyModel.PhysicsDamage_max;
			healthText.text = allyModel.Health.ToString();
			physicsArmorText.text = AbilityRankDescriber.Instance.GetArmorDescriptionByValue(allyModel.PhysicsArmor);
			magicArmorText.text = AbilityRankDescriber.Instance.GetArmorDescriptionByValue(allyModel.MagicArmor);
		}
	}
}
