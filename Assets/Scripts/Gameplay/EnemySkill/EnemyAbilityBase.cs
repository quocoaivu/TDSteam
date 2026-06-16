using System;

namespace Gameplay
{
	public abstract class EnemyAbilityBase : EnemyBrain
	{
		public EnemyAbilityCaster EnemySkillActivation { get; set; }

		public abstract float CoolDownDuration { get; }

		public abstract float ActiveDuration { get; }

		public abstract bool OnActive();

		public abstract void OnInactive();
	}
}
