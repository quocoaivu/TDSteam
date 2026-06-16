using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero10Ability0 : HeroAbilityShared
	{
        public GameObject projectilePrefab;

        public GameObject explodePrefab;

        public Transform barrelPos;

        public float moveToTargetDuration;

        public float disToAttackPos;

        public float projFlyDuration = 0.3f;

        public float delayBtwShoot = 0.2f;

        private int heroID = 10;

        private int skillID;

        private int currentLevel;

        private int currentSkillLevel;

        private Hero10Ability0Specs skillParams;

        private float skillDuration;

        private float skillRange;
        private List<Wyrm10Ability0Projectile> projList = new List<Wyrm10Ability0Projectile>();

        private Vector3 targetPos3;


        private int magicDamagePerHit;
        private void Start()
		{
			HerosDirector.Instance.onCastHeroSkillToAssignedPosition += OnHeroCastActiveSkill;
		}

		private void OnDestroy()
		{
			HerosDirector.Instance.onCastHeroSkillToAssignedPosition -= OnHeroCastActiveSkill;
		}

		public override string GetUseType()
		{
			return skillParams.use_type;
		}

		public override float GetCooldownTime()
		{
			return (float)skillParams.cooldown_time * 0.001f;
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroAbilitySpec_10_0).listParam[currentSkillLevel - 1];
			skillDuration = (float)skillParams.duration * 0.001f;
			skillRange = (float)skillParams.skill_range / GameRecord.PIXEL_PER_UNIT;
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
			heroModel.SetSpecialStateDuration(skillDuration + moveToTargetDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animRun);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animRun
			});
			targetPos3 = new Vector3(targetPos.x, targetPos.y, 0f);
			if (heroModel.GetPet() != null)
			{
				(heroModel.GetPet().HeroSkillController.GetSkill(0) as Pet1010Ability).TriggerBabyDragonRage(targetPos3, skillDuration + moveToTargetDuration, skillRange, (float)skillParams.magic_damage);
			}
			float lookSide = (float)((targetPos.x - heroModel.transform.position.x <= 0f) ? -1 : 1);
			heroModel.transform.localScale = new Vector3(lookSide, 1f, 1f);
			Vector3 standPosToAttack = targetPos3 - new Vector3(lookSide * disToAttackPos, 0f, 0f);
			heroModel.transform.DOMove(standPosToAttack, moveToTargetDuration, false).SetEase(Ease.Linear);
			yield return new WaitForSeconds(moveToTargetDuration);
			heroModel.GetAnimationController().ToSpecialState(HeroMotionHandler.animActiveSkill, skillDuration);
			int numOfProjectiles = Mathf.FloorToInt((skillDuration - projFlyDuration * 0.5f) / delayBtwShoot);
			magicDamagePerHit = Mathf.CeilToInt((float)skillParams.magic_damage * 1f / (float)numOfProjectiles);
			for (int i = 0; i < numOfProjectiles; i++)
			{
				Vector2 offset = UnityEngine.Random.insideUnitCircle * skillRange;
				Vector3 projTargetPos = targetPos3 + new Vector3(offset.x, offset.y, 0f);
				projList.Add(new Wyrm10Ability0Projectile(projectilePrefab, explodePrefab, barrelPos.position, projTargetPos, 0.1f, projFlyDuration * UnityEngine.Random.Range(0.75f, 1.1f)));
				yield return new WaitForSeconds(delayBtwShoot);
			}
			yield break;
		}

		public override void Update()
		{
			base.Update();
			for (int i = projList.Count - 1; i >= 0; i--)
			{
				if (!projList[i].OnUpdate(Time.deltaTime))
				{
					projList.RemoveAt(i);
					List<EnemyData> listEnemiesInRange = GameKit.GetListEnemiesInRange(targetPos3, new SharedStrikeDamage(0, magicDamagePerHit, true, skillRange));
					for (int j = listEnemiesInRange.Count - 1; j >= 0; j--)
					{
						if (GameKit.IsValidEnemy(listEnemiesInRange[j]))
						{
							listEnemiesInRange[j].ProcessDamage(DamageKind.Magic, new SharedStrikeDamage(0, magicDamagePerHit, true, skillRange));
						}
					}
				}
			}
		}
	}
}
