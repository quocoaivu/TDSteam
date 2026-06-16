using System;
using UnityEngine;

namespace Gameplay
{
	public class EnemyTargetAcquisition : EnemyBrain
	{
        public bool activeFindTarget;


        public CharacterEntity Target;
        public override void Initialize()
		{
			base.Initialize();
		}

		public override void OnAppear()
		{
			base.OnAppear();
			SetParameter();
		}

		private void SetParameter()
		{
			Target = null;
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			Target = null;
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(base.transform.position, (float)base.EnemyModel.OriginalParameter.attack_range_average / GameRecord.PIXEL_PER_UNIT);
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, (float)base.EnemyModel.OriginalParameter.attack_range_max / GameRecord.PIXEL_PER_UNIT);
		}

		public void AddTarget(CharacterEntity newtarget)
		{
			if (activeFindTarget)
			{
				return;
			}
			if (Target == null)
			{
				Target = newtarget;
			}
		}

	}
}
