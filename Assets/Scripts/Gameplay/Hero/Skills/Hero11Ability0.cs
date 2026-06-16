using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero11Ability0 : HeroAbilityShared
	{
        public GameObject explodePrefab;

        public GameObject behitOnEnemyFx;

        public float flyL2RDuration;

        public float outsideX;

        public float delayCreateExplode = 0.3f;

        private int heroID = 11;

        private int skillID;

        private int currentLevel;

        private int currentSkillLevel;

        private Hero11Ability0Specs skillParams;

        private float skillHalfHeight;

        private int fireRoadDamage;

        private float fireRoadDuration;
        private float moveOutDur = 0.5f;

        private float countdownCreateExplode;


        private int gameEventSubscriberId;
        private void Start()
		{
			HerosDirector.Instance.onCastHeroSkillToAssignedPosition += OnHeroCastActiveSkill;
		}

		private void OnDestroy()
		{
			if (HerosDirector.Instance != null)
			{
				HerosDirector.Instance.onCastHeroSkillToAssignedPosition -= OnHeroCastActiveSkill;
			}
			GameSignalCenter.Instance.Unsubscribe(gameEventSubscriberId, GameSignalKind.OnAfterCalculateMagicDamage);
		}

		public override float GetCooldownTime()
		{
			return (float)skillParams.cooldown_time * 0.001f;
		}

		public override string GetUseType()
		{
			return skillParams.use_type;
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroAbilitySpec_11_0).listParam[currentSkillLevel - 1];
			skillHalfHeight = (float)skillParams.skill_range / GameRecord.PIXEL_PER_UNIT;
			fireRoadDamage = skillParams.magic_damage;
			fireRoadDuration = (float)skillParams.fire_road_duration * 0.001f;
			gameEventSubscriberId = GameKit.GetUniqueId();
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnAfterCalculateMagicDamage, new DamageDetailListenerRecord(gameEventSubscriberId, new GameSignalCenter.DamageInfoMethod(OnAfterCalculateDamage)));
		}

		public void OnAfterCalculateDamage(SharedStrikeDamage damageInfo)
		{
			if (damageInfo.sourceId != heroModel.HeroID)
			{
				return;
			}
			if (damageInfo.targetEnemyModel.EnemyHealthController.CurrentHealth <= damageInfo.magicDamage)
			{
				ObjectCache.Spawn(behitOnEnemyFx, damageInfo.targetEnemyModel.transform.position);
			}
			else
			{
				ObjectCache.Spawn(behitOnEnemyFx, damageInfo.targetEnemyModel.transform, new Vector3(UnityEngine.Random.Range(-0.06f, 0.06f), UnityEngine.Random.Range(0.03f, 0.1f), 0f));
			}
		}

		public void OnHeroCastActiveSkill(int heroId, Vector2 targetPos)
		{
			if (heroID != heroId)
			{
				return;
			}
			base.StartCoroutine(CastSkill(targetPos));
		}

		private IEnumerator CastSkill(Vector2 targetPos)
		{
			MonoSingleton<GameplayUIHeroDirector>.Instance.listSelectHeroSkillButton[heroID].DoCooldown();
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(flyL2RDuration + moveOutDur * 2f);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animActiveSkill);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animActiveSkill
			});
			Vector3 oldPos = heroModel.transform.position;
			heroModel.transform.localScale = new Vector3(1f, 1f, 1f);
			Vector3 outsidePos = new Vector3(outsideX, heroModel.transform.position.y, 0f);
			heroModel.transform.DOMoveX(outsideX, moveOutDur, false);
			yield return new WaitForSeconds(moveOutDur);
			heroModel.transform.position = new Vector3(-outsideX, targetPos.y, 0f);
			heroModel.transform.DOMove(new Vector3(outsideX, targetPos.y, 0f), flyL2RDuration, false).SetEase(Ease.Linear);
			float flyL2RCountdown = flyL2RDuration;
			while (flyL2RCountdown > 0f)
			{
				yield return null;
				flyL2RCountdown -= Time.deltaTime;
				countdownCreateExplode -= Time.deltaTime;
				if (countdownCreateExplode <= 0f)
				{
					countdownCreateExplode = delayCreateExplode;
					MonoSingleton<LensHandler>.Instance.ShakeNormal();
					ObjectCache.Spawn(explodePrefab, heroModel.transform.position);
					List<EnemyData> listEnemiesInRange = GameKit.GetListEnemiesInRange(heroModel.transform.position, new SharedStrikeDamage(0, 0, true, skillHalfHeight));
					for (int i = 0; i < listEnemiesInRange.Count; i++)
					{
						listEnemiesInRange[i].ProcessDamage(DamageKind.Magic, new SharedStrikeDamage(0, fireRoadDamage, 0f));
					}
				}
			}
			heroModel.transform.position = new Vector3(-outsideX, oldPos.y, 0f);
			heroModel.transform.DOMove(oldPos, moveOutDur, false).SetEase(Ease.Linear);
			yield break;
		}
	}
}
