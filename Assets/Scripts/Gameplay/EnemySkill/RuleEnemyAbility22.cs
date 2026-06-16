using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class RuleEnemyAbility22 : EnemyBrain
	{
		public override void Initialize()
		{
			minDamage = 50;
			maxDamage = 65;
			base.Initialize();
			countdown = 0f;
			subscribeId = GameKit.GetUniqueId();
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnAfterCalculateMagicDamage, new DamageDetailListenerRecord(subscribeId, new GameSignalCenter.DamageInfoMethod(OnAfterCalculateMagicDamage)));
		}

		public void OnAfterCalculateMagicDamage(SharedStrikeDamage damageInfo)
		{
			if (damageInfo.targetInstanceId != base.EnemyModel.gameObject.GetEntityId())
			{
				return;
			}
			if (damageInfo.damageSource == CharacterKind.Tower)
			{
				int num = damageInfo.sourceId % 10;
				int num2 = damageInfo.sourceId / 10;
				if (num == 3 && num2 == 4)
				{
					damageInfo.magicDamage = (int)((float)damageInfo.magicDamage * 2.1f);
				}
			}
		}

		public override void Update()
		{
			base.Update();
			countdown -= Time.deltaTime;
			if (countdown <= 0f)
			{
				countdown = 0.4f;
				CharacterEntity allyWithHighestScore = GameKit.GetAllyWithHighestScore(base.EnemyModel, (CharacterEntity characterModel) => true, detectRange, new GameKit.CustomScoreModifier(CustomScoreCharacter));
				if (allyWithHighestScore != null && base.EnemyModel.IsAlive)
				{
					countdown = delayAttack;
					base.StartCoroutine(AttackSequence(allyWithHighestScore));
				}
			}
		}

		public override void OnReturnPool()
		{
			GameSignalCenter.Instance.Unsubscribe(subscribeId, GameSignalKind.OnAfterCalculateMagicDamage);
			base.OnReturnPool();
		}

		public float CustomScoreCharacter(CharacterEntity hero, EnemyData enemy)
		{
			if (hero is MinionEntity)
			{
				return 5f;
			}
			return 0f;
		}

		public IEnumerator AttackSequence(CharacterEntity targetAlly)
		{
			minDamage = base.EnemyModel.OriginalParameter.attack_physics_min;
			maxDamage = base.EnemyModel.OriginalParameter.attack_physics_max;
			base.EnemyModel.SetSpecialStateDuration(0.9f);
			base.EnemyModel.SetSpecialStateAnimationName(EnemyAnimation.animSpecialAttack);
			base.EnemyModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				EnemyAnimation.animSpecialAttack
			});
			base.EnemyModel.EnemyAnimationController.ToSpecialAttackState();
			base.EnemyModel.transform.localScale = new Vector3((float)((targetAlly.transform.position.x <= base.EnemyModel.transform.position.x) ? -1 : 1), 1f, 1f);
			yield return new WaitForSeconds(0.4f);
			GameObject bomb = ObjectCache.Spawn(bombPrefab);
			bomb.transform.position = base.transform.position;
			yield return null;
			Vector3 targetPos = targetAlly.transform.position;
			float throwBombDur = 0.6f;
			bomb.transform.DOJump(targetPos, 0.9f, 1, throwBombDur, false);
			yield return new WaitForSeconds(throwBombDur);
			bomb.transform.DOKill(false);
			bomb.Recycle();
			ObjectCache.Spawn(explodeEffectPrefab, targetPos);
			List<CharacterEntity> alliesInRange = GameKit.GetAllyInRange(targetPos, (CharacterEntity characterModel) => true, explodeRange);
			for (int i = alliesInRange.Count - 1; i >= 0; i--)
			{
				alliesInRange[i].ProcessDamage(DamageKind.Magic, new SharedStrikeDamage(UnityEngine.Random.Range(minDamage, maxDamage), 0, 0f));
			}
			yield break;
		}

		public float delayAttack;

		public float detectRange;

		public float explodeRange;

		private int minDamage;

		private int maxDamage;

		public GameObject bombPrefab;

		public GameObject explodeEffectPrefab;

		private float countdown;

		private int subscribeId;
	}
}
