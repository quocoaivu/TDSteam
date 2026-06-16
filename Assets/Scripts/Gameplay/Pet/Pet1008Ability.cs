using System;
using UnityEngine;

namespace Gameplay
{
	public class Pet1008Ability : HeroAbilityShared
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
				TryToCastSkill();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unLock = true;
			duration = (float)heroModel.PetConfigData.Skillvalues[0];
			cooldownTime = (float)heroModel.PetConfigData.Skillvalues[1];
			hpBuffPercentage = (float)heroModel.PetConfigData.Skillvalues[2];
			timeTracking = cooldownTime;
			ownerModel = heroModel.PetOwner;
			ownerModel.BuffsHolder.AddBuff("BuffHpByPercentage", new BuffStatus(true, hpBuffPercentage, 999999f), BuffStackRule.StackUp, BuffStackRule.ChooseMax);
			InitFXs();
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void TryToCastSkill()
		{
			target = heroModel.GetCurrentTarget();
			if (GameKit.IsValidEnemy(target))
			{
				heroModel.SetSpecialStateDuration(animDuration);
				heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animActiveSkill);
				heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
				{
					HeroMotionHandler.animActiveSkill
				});
				target.ProcessEffect(buffKey, 100, duration, DamageVfxType.Stun);
			}
			timeTracking = cooldownTime;
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_STUN);
		}
		private HeroEntity ownerModel;

		private bool unLock;

		private float duration;

		private float cooldownTime;

		private float hpBuffPercentage;

		private float timeTracking;

		private string buffKey = "Stun";

		private EnemyData target;

		[SerializeField]
		private float animDuration;
	}
}
