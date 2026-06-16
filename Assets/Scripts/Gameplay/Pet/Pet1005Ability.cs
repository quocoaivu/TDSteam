using System;
using UnityEngine;

namespace Gameplay
{
	public class Pet1005Ability : HeroAbilityShared
	{
		public override void Init(HeroEntity heroModel)
		{
			this.heroModel = heroModel;
			unLock = true;
			PetSetupRecord petConfigData = heroModel.PetConfigData;
			chanceToStun = petConfigData.Skillvalues[0];
			duration = (float)petConfigData.Skillvalues[1] / 1000f;
			armorBuffPercentage = (float)petConfigData.Skillvalues[2];
			armorBonus = (float)petConfigData.Skillvalues[3] / 100f;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
			InitFXs();
			CastBuffToOwner();
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_STUN);
		}

		private void CastBuffToOwner()
		{
			HeroEntity petOwner = heroModel.PetOwner;
			petOwner.BuffsHolder.AddBuff("IncreaseArmorPhysics", new BuffStatus(true, armorBonus, 999999f), BuffStackRule.StackUp, BuffStackRule.ChooseMax);
			petOwner.BuffsHolder.AddBuff("IncreaseArmorMagic", new BuffStatus(true, armorBonus, 999999f), BuffStackRule.StackUp, BuffStackRule.ChooseMax);
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (unLock && heroModel.currentTarget && UnityEngine.Random.Range(0, 100) < chanceToStun)
			{
				heroModel.currentTarget.ProcessEffect(buffKey, 100, duration, DamageVfxType.Stun);
			}
		}
		private bool unLock;

		private int chanceToStun;

		private float duration;

		private string buffKey = "Slow";

		private float armorBuffPercentage;

		private float armorBonus;
	}
}
