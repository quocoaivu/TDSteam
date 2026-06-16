using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero11Ability3 : HeroAbilityShared
	{
        public GameObject healFxPrefab;

        public float delayBtwHeal = 0.3f;

        private int heroID = 11;

        private int skillID = 3;

        private int currentLevel;

        private int currentSkillLevel;
        private Hero11Ability3Specs skillParams;

        private float cooldownDuration;

        private float cooldownCountdown;

        private float sqDisHeal;

        private GameObject healObj;

        private int healPerTime;

        private int numOfHealTimes;

        private bool unlocked;

        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unlocked = true;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroAbilitySpec_11_3).listParam[currentSkillLevel - 1];
			cooldownDuration = (float)skillParams.cooldown_time * 0.001f;
			cooldownCountdown = cooldownDuration * 0.7f;
			float num = (float)skillParams.heal_range / GameRecord.PIXEL_PER_UNIT;
			sqDisHeal = num * num;
			float num2 = (float)skillParams.heal_duration * 0.001f;
			numOfHealTimes = Mathf.RoundToInt(num2 / delayBtwHeal);
			healPerTime = skillParams.total_heal / numOfHealTimes;
		}

		public override void Update()
		{
			base.Update();
			if (!unlocked)
			{
				return;
			}
			if (cooldownCountdown > 0f)
			{
				cooldownCountdown -= Time.deltaTime;
			}
			else
			{
				cooldownCountdown = cooldownDuration;
				base.StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			healObj = ObjectCache.Spawn(healFxPrefab, heroModel.transform, Vector3.zero);
			UnityEngine.Debug.LogFormat("heal {0} times in {1}s, each time heals {2}hp", new object[]
			{
				numOfHealTimes,
				skillParams.heal_duration,
				healPerTime
			});
			for (int i = 0; i < numOfHealTimes; i++)
			{
				yield return new WaitForSeconds(delayBtwHeal);
				List<CharacterEntity> listAlly = MonoSingleton<GameRecord>.Instance.ListActiveAlly;
				for (int j = listAlly.Count - 1; j >= 0; j--)
				{
					if (MonoSingleton<GameRecord>.Instance.SqrDistance(heroModel.transform.position, listAlly[j].transform.position) < sqDisHeal)
					{
						listAlly[j].IncreaseHealth(healPerTime);
					}
				}
			}
			healObj.Recycle();
			yield break;
		}
	}
}
