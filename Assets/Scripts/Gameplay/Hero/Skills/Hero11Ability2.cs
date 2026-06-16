using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero11Ability2 : HeroAbilityShared
	{
        public GameObject minionPrefab;

        public GameObject spawnFxPrefab;

        public GameObject explodeFxPrefab;

        public float disTriggerAttack = 1f;

        public float delaySpawnMinion;

        public float jumpDuration = 0.3f;

        public float jumpHeight = 0.3f;

        private int heroID = 11;

        private int skillID = 2;

        private int petId = 1011;

        private int currentLevel;

        private int currentSkillLevel;
        private Hero11Ability2Specs skillParams;

        private float cooldownDuration;

        private float cooldownCountdown;

        private float sqDisTriggerAttack;

        private float explodeRange;

        private float minionLifetime;

        private int maxMinionQuantity;

        private int minionCountdown;

        private List<FirebirdLackeyRecord> minionList = new List<FirebirdLackeyRecord>();

        private float countdownDetectEnemy;

        private bool unlocked;


        private int gameEventSubscriberId;
        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unlocked = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroAbilitySpec_11_2).listParam[currentSkillLevel - 1];
			cooldownDuration = (float)skillParams.cooldown_time * 0.001f;
			cooldownCountdown = cooldownDuration * 0.35f;
			sqDisTriggerAttack = disTriggerAttack * disTriggerAttack;
			explodeRange = (float)skillParams.explode_range / GameRecord.PIXEL_PER_UNIT;
			minionLifetime = (float)skillParams.minion_lifetime * 0.001f;
			maxMinionQuantity = skillParams.minion_quantity;
			if (GameKit.IsUltimateHero(heroID))
			{
				maxMinionQuantity++;
			}
			minionCountdown = maxMinionQuantity;
			gameEventSubscriberId = GameKit.GetUniqueId();
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnAfterCalculateMagicDamage, new DamageDetailListenerRecord(gameEventSubscriberId, new GameSignalCenter.DamageInfoMethod(OnAfterCalculateDamage)));
		}

		private void OnDestroy()
		{
			GameSignalCenter.Instance.Unsubscribe(gameEventSubscriberId, GameSignalKind.OnAfterCalculateMagicDamage);
		}

		public void OnAfterCalculateDamage(SharedStrikeDamage damageInfo)
		{
			if (cooldownCountdown > 0f)
			{
				return;
			}
			if (damageInfo.sourceId != heroModel.HeroID && damageInfo.sourceId != petId)
			{
				return;
			}
			if (damageInfo.targetEnemyModel.EnemyHealthController.CurrentHealth > damageInfo.magicDamage)
			{
				return;
			}
			base.StartCoroutine(SpawnMinion(damageInfo.targetEnemyModel.transform.position));
		}

		private IEnumerator SpawnMinion(Vector3 targetPos)
		{
			minionCountdown--;
			if (minionCountdown <= 0)
			{
				cooldownCountdown = cooldownDuration;
			}
			ObjectCache.Spawn(spawnFxPrefab, targetPos);
			yield return new WaitForSeconds(delaySpawnMinion);
			GameObject minion = ObjectCache.Spawn(minionPrefab, targetPos);
			minion.transform.localScale = new Vector3((float)((UnityEngine.Random.Range(0, 100) >= 50) ? -1 : 1), 1f, 1f);
			yield return new WaitForSeconds(2f);
			minionList.Add(new FirebirdLackeyRecord(minion, minionLifetime));
			yield break;
		}

		public override void Update()
		{
			base.Update();
			if (!unlocked)
			{
				return;
			}
			cooldownCountdown -= Time.deltaTime;
			for (int i = minionList.Count - 1; i >= 0; i--)
			{
				if (!minionList[i].isAttacking)
				{
					minionList[i].lifetimeCountdown -= Time.deltaTime;
					if (minionList[i].lifetimeCountdown <= 0f)
					{
						minionList[i].isDestroyed = true;
					}
				}
				if (minionList[i].isDestroyed)
				{
					ObjectCache.Spawn(explodeFxPrefab, minionList[i].minion.transform.position);
					List<EnemyData> listEnemiesInRange = GameKit.GetListEnemiesInRange(minionList[i].minion.transform.position, new SharedStrikeDamage(0, 0, explodeRange));
					for (int j = 0; j < listEnemiesInRange.Count; j++)
					{
						listEnemiesInRange[j].ProcessDamage(DamageKind.Magic, new SharedStrikeDamage(0, skillParams.magic_damage, 0f));
					}
					minionList[i].minion.Recycle();
					minionList.RemoveAt(i);
				}
			}
			countdownDetectEnemy -= Time.deltaTime;
			if (countdownDetectEnemy <= 0f)
			{
				countdownDetectEnemy = 0.5f;
				if (minionList.Count > 0)
				{
					List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
					for (int k = listActiveEnemy.Count - 1; k >= 0; k--)
					{
						if (GameKit.IsValidEnemy(listActiveEnemy[k]) && !listActiveEnemy[k].IsInTunnel && !listActiveEnemy[k].IsUnderground)
						{
							for (int l = minionList.Count - 1; l >= 0; l--)
							{
								if (!minionList[l].isAttacking && MonoSingleton<GameRecord>.Instance.SqrDistance(minionList[l].minion.transform.position, listActiveEnemy[k].transform.position) < sqDisTriggerAttack)
								{
									FirebirdLackeyRecord m = minionList[l];
									m.isAttacking = true;
									m.minion.transform.localScale = new Vector3((float)((listActiveEnemy[k].transform.position.x <= minionList[l].minion.transform.position.x) ? -1 : 1), 1f, 1f);
									float magnitude = (listActiveEnemy[k].transform.position - minionList[l].minion.transform.position).magnitude;
									float duration = Mathf.Max(jumpDuration * magnitude / disTriggerAttack, 0.4f) + 0.1f;
									m.PlayJump();
									m.minion.transform.DOJump(listActiveEnemy[k].transform.position, jumpHeight, 1, duration, false).OnComplete(delegate
									{
										m.isDestroyed = true;
									});
									break;
								}
							}
						}
					}
				}
			}
		}
	}
}
