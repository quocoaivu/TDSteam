using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class RuleEnemyAbilityClearBoon : EnemyBrain
	{
		public override void Initialize()
		{
			base.Initialize();
			base.EnemyModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (ShouldCastSkill() && IsCooldownSkillDone())
			{
				base.CustomCancelInvoke(new Action(CastSkillClearBuffs));
				base.CustomInvoke(new Action(CastSkillClearBuffs), delayTimeToClearBuffs / 1000f);
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			coolDownTime = coolDownTimeMillisecond / 1000f;
			coolDownTimeTracking = ((!activeAtStart) ? coolDownTime : 0f);
			skillReady = true;
		}

		public override void Update()
		{
			base.Update();
			if (!skillReady)
			{
				return;
			}
			if (!base.IsEnemyAlive())
			{
				return;
			}
			if (MonoSingleton<GameRecord>.Instance.IsGameOver)
			{
				return;
			}
			coolDownTimeTracking = Mathf.MoveTowards(coolDownTimeTracking, 0f, Time.deltaTime);
		}

		private bool ShouldCastSkill()
		{
			bool result = false;
			foreach (string text in ignoreBuffKeys)
			{
				float buffsValue = base.EnemyModel.BuffsHolder.GetBuffsValue(ignoreBuffKeys);
				if (buffsValue > 0f)
				{
					result = true;
				}
			}
			return result;
		}

		private bool IsCooldownSkillDone()
		{
			return coolDownTimeTracking == 0f;
		}

		private void CastSkillClearBuffs()
		{
			base.EnemyModel.BuffsHolder.RemoveBuffs(ignoreBuffKeys);
			base.EnemyModel.EnemyEffectController.RemoveAllFXs();
			base.EnemyModel.EnemyEffectController.SetNormalColor();
			UnityEngine.Debug.Log("clear all buffs");
			coolDownTimeTracking = coolDownTime;
		}

		[Space]
		[Header("Parameters")]
		[SerializeField]
		private float coolDownTimeMillisecond;

		[SerializeField]
		private float delayTimeToClearBuffs;

		[SerializeField]
		private bool activeAtStart;

		private float coolDownTime;

		private float coolDownTimeTracking;

		private bool skillReady;

		private List<string> ignoreBuffKeys = new List<string>
		{
			"Slow",
			"Burning"
		};
	}
}
