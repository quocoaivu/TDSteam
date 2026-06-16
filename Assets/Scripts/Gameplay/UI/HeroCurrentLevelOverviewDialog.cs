using System;
using Data;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class HeroCurrentLevelOverviewDialog : BaseMonoBehaviour
	{
        [SerializeField]
        private Text currentHealth;

        [SerializeField]
        private Text currentDamage;

        [SerializeField]
        private Text currentArmor;

        [SerializeField]
        private Text currentAttackRange;

        [SerializeField]
        private Text currentAttackSpeed;

        private int heroID;

        private int heroLevel;

        [Space]
        [SerializeField]
        private Image iconDamage;

        [SerializeField]
        private Sprite physicsDamageIcon;

        [SerializeField]

        private Sprite magicDamageIcon;
        public void Init(int heroID, int heroLevel)
		{
			Open();
			this.heroID = heroID;
			this.heroLevel = heroLevel;
			UpdateDamage();
			UpdateUnitHealth();
			UpdateUnitArmor();
			UpdateAttackRange();
			UpdateAttackSpeed();
		}

		private void UpdateAttackSpeed()
		{
			currentAttackSpeed.text = AbilityRankDescriber.Instance.GetAttackSpeedDescriptionByValue(HeroParameterManager.Instance.GetHeroAttackCooldown(heroID, heroLevel));
		}

		private void UpdateAttackRange()
		{
			currentAttackRange.text = AbilityRankDescriber.Instance.GetAttackRangeDescriptionByValue(HeroParameterManager.Instance.GetHeroAttackRange(heroID, heroLevel));
		}

		private void UpdateUnitHealth()
		{
			HeroEntity hero = HerosDirector.Instance.GetHero(heroID);
			currentHealth.text = hero.HeroHealthController.OriginHealth.ToString();
		}

		private void UpdateUnitArmor()
		{
			HeroEntity hero = HerosDirector.Instance.GetHero(heroID);
			currentArmor.text = AbilityRankDescriber.Instance.GetArmorDescriptionByValue((int)(hero.HeroHealthController.CurrentPhysicsArmor * 100f));
		}

		private void UpdateDamage()
		{
			HeroEntity hero = HerosDirector.Instance.GetHero(heroID);
			if (HeroParameterManager.Instance.IsPhysicsAttack(heroID))
			{
				iconDamage.sprite = physicsDamageIcon;
				currentDamage.text = string.Format("{0} - {1}", hero.HeroAttackController.GetAtkPhysicsMin(), hero.HeroAttackController.GetAtkPhysicsMax());
			}
			else
			{
				iconDamage.sprite = magicDamageIcon;
				currentDamage.text = string.Format("{0} - {1}", hero.HeroAttackController.GetAtkMagicMin(), hero.HeroAttackController.GetAtkMagicMax());
			}
		}

		public void Open()
		{
			base.gameObject.SetActive(true);
		}

		public void Close()
		{
			base.gameObject.SetActive(false);
		}
	}
}
