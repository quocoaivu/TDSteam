using System;
using UnityEngine;

namespace Gameplay
{
	public class RuleEnemyAbilityImmuneWhileMoving : EnemyBrain
	{
		public override void Initialize()
		{
			base.Initialize();
			subscribeId = GameKit.GetUniqueId();
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnBeforeCalculatePhysicsDamage, new DamageDetailListenerRecord(subscribeId, new GameSignalCenter.DamageInfoMethod(OnBeforeCalculateDamage)));
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnAfterCalculatePhysicsDamage, new DamageDetailListenerRecord(subscribeId, new GameSignalCenter.DamageInfoMethod(OnAfterCalculateDamage)));
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnAfterCalculateMagicDamage, new DamageDetailListenerRecord(subscribeId, new GameSignalCenter.DamageInfoMethod(OnAfterCalculateDamage)));
		}

		public override void Update()
		{
			base.Update();
			isMoving = (base.EnemyModel.curState == EntityPhaseEnum.EnemyMove);
			countdownShowEffect -= Time.deltaTime;
			countdownIncreaseIgnore -= Time.deltaTime;
			if (countdownShowEffect > 0f)
			{
				effect.transform.position = base.EnemyModel.transform.position + new Vector3(0f, 0.25f, 0f);
			}
		}

		public void OnBeforeCalculateDamage(SharedStrikeDamage damageInfo)
		{
			if (damageInfo.targetInstanceId != base.EnemyModel.gameObject.GetEntityId())
			{
				return;
			}
			if (countdownIncreaseIgnore > 0f)
			{
				return;
			}
			if (damageInfo.damageSource == CharacterKind.Ally && damageInfo.sourceId / 1000 == 4)
			{
				int num = damageInfo.sourceId % 10;
				int num2 = damageInfo.sourceId % 1000 / 10;
				if (num == 1 && num2 == 4)
				{
					countdownIncreaseIgnore = 1f;
					damageInfo.physicsDamage = (int)((float)damageInfo.physicsDamage * 2f);
					if (!damageInfo.isIgnoreArmor && UnityEngine.Random.Range(0f, 1f) < 0.75f)
					{
						damageInfo.isIgnoreArmor = true;
					}
				}
			}
		}

		public void OnAfterCalculateDamage(SharedStrikeDamage damageInfo)
		{
			if (damageInfo.targetInstanceId != base.EnemyModel.gameObject.GetEntityId())
			{
				return;
			}
			if (!isMoving)
			{
				return;
			}
			if (damageInfo.damageSource == CharacterKind.Tower)
			{
				int num = damageInfo.sourceId % 10;
				if (num == 3 || num == 0)
				{
					damageInfo.magicDamage = 0;
					damageInfo.physicsDamage = 0;
					if (countdownShowEffect <= 0f)
					{
						countdownShowEffect = 1f;
						effect = ObjectCache.Spawn(immuneEffectPrefab, base.EnemyModel.transform.position + new Vector3(0f, 0.25f, 0f));
					}
				}
			}
		}

		public override void OnReturnPool()
		{
			GameSignalCenter.Instance.Unsubscribe(subscribeId, GameSignalKind.OnAfterCalculateMagicDamage);
			GameSignalCenter.Instance.Unsubscribe(subscribeId, GameSignalKind.OnAfterCalculatePhysicsDamage);
			GameSignalCenter.Instance.Unsubscribe(subscribeId, GameSignalKind.OnBeforeCalculatePhysicsDamage);
			base.OnReturnPool();
		}

		public GameObject immuneEffectPrefab;

		private bool isMoving;

		private float countdownShowEffect;

		private float countdownIncreaseIgnore;

		private GameObject effect;

		private int subscribeId;
	}
}
