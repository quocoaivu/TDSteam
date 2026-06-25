using System;
using Parameter;
using Common;
using UnityEngine;

namespace Gameplay
{
	public class TurretAbilityProduceBullion : TurretHandler
	{
		public override void OnAppear()
		{
			base.OnAppear();
			SetParameter();
			producedGoldController.ResetParameter();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			skillReady = false;
			producedGoldController.ResetParameter();
		}

		private void SetParameter()
		{
			// Roguelite stats = base (level 0) + unlocked skill-tree nodes, same source the combat
			// towers read. The Supporter tree buffs goldProduce / reload / autoCollectTime here.
			TurretSpec spec = base.TowerModel.OriginalParameter;
			goldProduce = spec.goldProduce;
			float prodSpd = spec.attackSpeed;
			cooldownTimeProduce = prodSpd > 0 ? 1f / prodSpd : 999f;
			cooldownTimeProduceTracking = cooldownTimeProduce;
			cooldownTimeAutoCollect = (float)spec.autoCollectTime / 1000f;
			cooldownTimeAutoCollectTracking = cooldownTimeAutoCollect;
			skillReady = true;
		}

		public override void Update()
		{
			base.Update();
			if (!MonoSingleton<GameRecord>.Instance.IsGameStart)
			{
				return;
			}
			if (!skillReady)
			{
				return;
			}
			if (IsCooldownProduceDone())
			{
				ProduceGold();
			}
			cooldownTimeProduceTracking = Mathf.MoveTowards(cooldownTimeProduceTracking, 0f, Time.deltaTime);
			if (IsCooldownAutoCollectDone())
			{
				AutoCollect();
			}
			if (isProducingGold)
			{
				cooldownTimeAutoCollectTracking = Mathf.MoveTowards(cooldownTimeAutoCollectTracking, 0f, Time.deltaTime);
			}
		}

		private bool IsCooldownProduceDone()
		{
			return cooldownTimeProduceTracking == 0f;
		}

		private void ResetCooldownProduce()
		{
			cooldownTimeProduceTracking = cooldownTimeProduce;
		}

		private void ProduceGold()
		{
			producedGoldController.Init(goldProduce);
			ResetCooldownProduce();
			onProduceGold.Dispatch();
			isProducingGold = true;
		}

		private bool IsCooldownAutoCollectDone()
		{
			return cooldownTimeAutoCollectTracking == 0f;
		}

		private void ResetCooldownAutoCollect()
		{
			cooldownTimeAutoCollectTracking = cooldownTimeAutoCollect;
		}

		private void AutoCollect()
		{
			producedGoldController.TapOnGold();
			ResetCooldownAutoCollect();
			isProducingGold = false;
		}

		[SerializeField]
		private OrderedUnityEvent onProduceGold;

		[SerializeField]
		private ProducedBullionHandler producedGoldController;

		private int goldProduce;

		private float cooldownTimeProduce;

		private float cooldownTimeProduceTracking;

		private float cooldownTimeAutoCollect;

		private float cooldownTimeAutoCollectTracking;

		private bool skillReady;

		private bool isProducingGold;
	}
}
