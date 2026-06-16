using System;

namespace Gameplay
{
	public class Pet1004Ability : HeroAbilityShared
	{
		public override void Init(HeroEntity heroModel)
		{
			PetSetupRecord petConfigData = heroModel.PetConfigData;
			atkbuffPercentage = (float)petConfigData.Skillvalues[0];
			HeroEntity petOwner = heroModel.PetOwner;
			petOwner.BuffsHolder.AddBuff("BuffAttackByPercentage", new BuffStatus(true, atkbuffPercentage, 999999f), BuffStackRule.StackUp, BuffStackRule.ChooseMax);
		}

		private float atkbuffPercentage;
	}
}
