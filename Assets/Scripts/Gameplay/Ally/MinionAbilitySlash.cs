using System;
using UnityEngine;

namespace Gameplay
{
	public class MinionAbilitySlash : MinionHandler
	{
        private bool unLock;

        private float duration;

        private float cooldownTime;

        private string description;


        private float cooldownTimeTracking;
        public void Init(float duration, float cooldownTime, string description, bool isActiveAtStart)
		{
			unLock = true;
			this.duration = duration;
			this.cooldownTime = cooldownTime;
			this.description = description;
			cooldownTimeTracking = ((!isActiveAtStart) ? cooldownTime : 0f);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			unLock = false;
		}

		public override void Update()
		{
			base.Update();
			if (!unLock)
			{
				return;
			}
			if (!base.MinionEntity || !base.MinionEntity.IsAlive)
			{
				return;
			}
			cooldownTimeTracking = Mathf.MoveTowards(cooldownTimeTracking, 0f, Time.deltaTime);
		}

		private bool IsCooldownDone()
		{
			return cooldownTimeTracking == 0f;
		}

		public void TryToCastskill()
		{
			if (!unLock)
			{
				return;
			}
			if (!IsCooldownDone())
			{
				return;
			}
			if (base.MinionEntity.currentTarget && base.MinionEntity.IsInMeleeRange(base.MinionEntity.currentTarget))
			{
				CastSkill();
			}
		}

		private void CastSkill()
		{
			if (!IsEmptySpecialState())
			{
				return;
			}
			base.MinionEntity.SetSpecialStateDuration(duration);
			base.MinionEntity.SetSpecialStateAnimationName(MinionMotionHandler.animActiveSkill);
			base.MinionEntity.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				MinionMotionHandler.animActiveSkill
			});
			cooldownTimeTracking = cooldownTime;
		}

		private bool IsEmptySpecialState()
		{
			return !(base.MinionEntity.GetFsmController().GetCurrentState() is CharacterSpecialState);
		}

	}
}
