using System;

namespace Gameplay
{
	public class Pet1007Ability : HeroAbilityShared
	{
		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unLock = true;
			heroMaster = heroModel.PetOwner;
			heroMaster.OnBeHitEvent += HeroModel_OnBeHitEvent;
		}

		private void HeroModel_OnBeHitEvent()
		{
			if (heroMaster.currentTarget)
			{
				heroModel.AddTarget(heroMaster.currentTarget);
				if (heroMaster.currentTarget.EnemyFindTargetController)
				{
					heroMaster.currentTarget.EnemyFindTargetController.AddTarget(heroModel);
				}
			}
		}
		private bool unLock;

		private HeroEntity heroMaster;
	}
}
