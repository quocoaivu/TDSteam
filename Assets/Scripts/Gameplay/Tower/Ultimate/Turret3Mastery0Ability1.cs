using System;
using System.Collections.Generic;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Turret3Mastery0Ability1 : TurretMasteryShared
	{
		private void Update()
		{
			if (!unlock)
			{
				return;
			}
			if (timeTracking == 0f)
			{
				GetAlliesInRange();
				HealingAlly();
				AddBuffIncreaseAttackSpeed();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public override void InitTowerModel(TurretEntity towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			ReadParameter(ultiLevel);
			TryToCreateMagicAura();
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_HEAL_1);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			magicCircle.SetActive(false);
		}

		private void ReadParameter(int currentSkillLevel)
		{
			hpGenerate = TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			reloadDecrease = TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			skillRange = (float)TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2) / GameRecord.PIXEL_PER_UNIT;
			duration = (float)TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 3);
			timeTracking = duration;
		}

		public void TryToCreateMagicAura()
		{
			if (!unlock)
			{
				return;
			}
			magicCircle.SetActive(true);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void GetAlliesInRange()
		{
			allyInRange.Clear();
			List<CharacterEntity> listActiveAlly = MonoSingleton<GameRecord>.Instance.ListActiveAlly;
			for (int i = 0; i < listActiveAlly.Count; i++)
			{
				CharacterEntity characterModel = listActiveAlly[i];
				float num = MonoSingleton<GameRecord>.Instance.SqrDistance(base.gameObject, characterModel.gameObject);
				if (num <= skillRange * skillRange)
				{
					allyInRange.Add(characterModel);
				}
			}
			timeTracking = duration;
			UnityEngine.Debug.Log("ally in range " + allyInRange.Count);
		}

		private void HealingAlly()
		{
			foreach (CharacterEntity characterModel in allyInRange)
			{
				if (characterModel.IsAlive)
				{
					characterModel.IncreaseHealth(hpGenerate);
					VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_HEAL_1);
					effect.transform.position = characterModel.transform.position;
					if (characterModel.GetComponent<SpriteRenderer>())
					{
						effect.Init(timeTracking + 0.5f, characterModel.BuffsHolder.transform, characterModel.GetComponent<SpriteRenderer>().sprite.rect.width);
					}
					else
					{
						effect.Init(timeTracking + 0.5f, characterModel.BuffsHolder.transform, characterModel.GetComponentInChildren<SpriteRenderer>().sprite.rect.width);
					}
				}
			}
		}

		private void AddBuffIncreaseAttackSpeed()
		{
			foreach (CharacterEntity characterModel in allyInRange)
			{
				characterModel.BuffsHolder.AddBuff(buffKey, new BuffStatus(true, (float)reloadDecrease, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
			}
		}

		private int towerID = 3;

		private int ultimateBranch;

		private int skillID = 1;

		private int hpGenerate;

		private int reloadDecrease;

		private float duration;

		private float skillRange;

		private TurretEntity towerModel;

		private string buffKey = "IncreaseAttackSpeed";

		private List<CharacterEntity> allyInRange = new List<CharacterEntity>();

		private float timeTracking;

		[SerializeField]
		private GameObject magicCircle;
	}
}
