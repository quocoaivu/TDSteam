using System;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Pet1009Ability : HeroAbilityShared
	{
		public override void Update()
		{
			base.Update();
			if (!unLock)
			{
				return;
			}
			if (IsCooldownDone())
			{
				base.StartCoroutine(CastSkill());
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unLock = true;
			PetSetupRecord petConfigData = heroModel.PetConfigData;
			defenseBuffPercentage = (float)petConfigData.Skillvalues[0];
			aoeDamage = petConfigData.Skillvalues[1];
			aoeRange = (float)petConfigData.Skillvalues[2] / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)petConfigData.Skillvalues[3];
			timeTracking = cooldownTime;
			BuffToOwner();
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<BulletPool>.Instance.InitBulletsFromHeroes(heroModel.HeroID, 1);
		}

		private void BuffToOwner()
		{
			HeroEntity petOwner = heroModel.PetOwner;
			petOwner.BuffsHolder.AddBuff("BuffDeffenseByPercentage", new BuffStatus(true, defenseBuffPercentage, 999999f), BuffStackRule.StackUp, BuffStackRule.ChooseMax);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill()
		{
			target = heroModel.GetCurrentTarget();
			timeTracking = cooldownTime;
			if (GameKit.IsValidEnemy(target))
			{
				heroModel.SetSpecialStateDuration(animDuration);
				heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animActiveSkill);
				heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
				{
					HeroMotionHandler.animActiveSkill
				});
				yield return new WaitForSeconds(delayTimeCastSkill);
				ProjectileEntity bullet = MonoSingleton<BulletPool>.Instance.GetForHero(heroModel.HeroID, 1);
				bullet.transform.position = gunPos.position;
				bullet.gameObject.SetActive(true);
				bullet.InitFromHero(heroModel, new SharedStrikeDamage(aoeDamage, 0, aoeRange), target);
			}
			yield break;
		}
		private bool unLock;

		private EnemyData target;

		private float defenseBuffPercentage;

		private int aoeDamage;

		private float aoeRange;

		private float cooldownTime;

		private float timeTracking;

		[SerializeField]
		private Transform gunPos;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeCastSkill;
	}
}
