using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class Pet1011Ability : HeroAbilityShared
	{
		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			base.Init(heroModel);
			this.heroModel = heroModel;
			PetSetupRecord petConfigData = heroModel.PetConfigData;
			atkbuffPercentage = (float)petConfigData.Skillvalues[3];
			hpBuffPercentage = (float)petConfigData.Skillvalues[4];
			healProportion = (float)petConfigData.Skillvalues[5] * 0.01f;
			cooldownDuration = (float)petConfigData.Skillvalues[6] * 0.001f;
			cooldownCountdown = cooldownDuration * 0.2f;
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_HEAL_0);
			HeroEntity petOwner = heroModel.PetOwner;
			if (petOwner == null)
			{
				UnityEngine.Debug.LogError(" ower model is null");
			}
			petOwner.BuffsHolder.AddBuff("BuffAttackByPercentage", new BuffStatus(true, atkbuffPercentage, 999999f), BuffStackRule.StackUp, BuffStackRule.ChooseMax);
			petOwner.BuffsHolder.AddBuff("BuffHpByPercentage", new BuffStatus(true, hpBuffPercentage, 999999f), BuffStackRule.StackUp, BuffStackRule.ChooseMax);
		}

		public override void Update()
		{
			base.Update();
			cooldownCountdown -= Time.deltaTime;
			if (cooldownCountdown <= 0f && !isCastingSkill)
			{
				cooldownCountdown = cooldownDuration;
				List<CharacterEntity> listActiveAlly = MonoSingleton<GameRecord>.Instance.ListActiveAlly;
				CharacterEntity characterModel = null;
				float num = 2f;
				for (int i = listActiveAlly.Count - 1; i >= 0; i--)
				{
					float num2 = (float)listActiveAlly[i].GetCurHp() * 1f / (float)listActiveAlly[i].GetMaxHp();
					if (GameKit.IsValidCharacter(listActiveAlly[i]) && num > num2)
					{
						characterModel = listActiveAlly[i];
						num = num2;
					}
				}
				if (characterModel != null && num < 1f)
				{
					base.StartCoroutine(CastSkill(characterModel));
				}
				else
				{
					cooldownCountdown = cooldownDuration * 0.1f;
				}
			}
		}

		private IEnumerator CastSkill(CharacterEntity pickedAlly)
		{
			isCastingSkill = true;
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			if (!GameKit.IsValidCharacter(pickedAlly))
			{
				isCastingSkill = false;
			}
			else
			{
				Vector3 healPos = pickedAlly.transform.position + new Vector3(-disToWoundedAlly, 0.3f, 0f);
				float flyTowardWoundedAllyDur = (healPos - heroModel.transform.position).magnitude / flyToWoundedAllySpd;
				heroModel.SetSpecialStateDuration(healDuration + flyTowardWoundedAllyDur);
				heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animRun);
				heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
				{
					HeroMotionHandler.animRun
				});
				heroModel.transform.localScale = new Vector3((float)((healPos.x <= heroModel.transform.position.x) ? -1 : 1), 1f, 1f);
				heroModel.transform.DOMove(healPos, flyTowardWoundedAllyDur, false);
				yield return new WaitForSeconds(flyTowardWoundedAllyDur);
				if (GameKit.IsValidCharacter(pickedAlly))
				{
					heroModel.GetAnimationController().ToSpecialState(HeroMotionHandler.animPassiveSkill_0, healDuration);
					heroModel.transform.localScale = new Vector3((float)((pickedAlly.transform.position.x <= heroModel.transform.position.x) ? -1 : 1), 1f, 1f);
					ObjectCache.Spawn(activateHealFx, heroModel.transform.position);
					int hpAmount = Mathf.RoundToInt((float)pickedAlly.GetMaxHp() * healProportion);
					pickedAlly.IncreaseHealth(hpAmount);
					VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_HEAL_0);
					effect.transform.position = pickedAlly.transform.position;
					effect.Init(1.5f, pickedAlly.BuffsHolder.transform, pickedAlly.GetComponent<SpriteRenderer>().sprite.rect.width);
				}
				yield return new WaitForSeconds(healDuration);
				isCastingSkill = false;
			}
			yield break;
		}

		public float flyToWoundedAllySpd = 3f;

		public float disToWoundedAlly = 1.5f;

		public float healDuration = 1f;

		public GameObject activateHealFx;

		private float atkbuffPercentage;

		private float hpBuffPercentage;

		private float cooldownDuration;

		private float healProportion;
		private float cooldownCountdown;

		private bool isCastingSkill;
	}
}
