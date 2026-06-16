using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class RuleEnemyAbility23 : EnemyBrain
	{
		public override void Initialize()
		{
			base.Initialize();
			countdown = 0f;
			subscribeId = GameKit.GetUniqueId();
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnAfterCalculatePhysicsDamage, new DamageDetailListenerRecord(subscribeId, new GameSignalCenter.DamageInfoMethod(OnAfterCalculatePhysicsDamage)));
		}

		public void OnAfterCalculatePhysicsDamage(SharedStrikeDamage damageInfo)
		{
			if (damageInfo.targetInstanceId != base.EnemyModel.gameObject.GetEntityId())
			{
				return;
			}
			if (damageInfo.damageSource == CharacterKind.Tower)
			{
				int num = damageInfo.sourceId % 10;
				int num2 = damageInfo.sourceId / 10;
				if (num == 0 && num2 == 4)
				{
					damageInfo.physicsDamage *= 3;
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
			GameSignalCenter.Instance.Unsubscribe(subscribeId, GameSignalKind.OnAfterCalculatePhysicsDamage);
			base.OnReturnPool();
		}

		public float CustomScoreCharacter(CharacterEntity hero, EnemyData enemy)
		{
			if (hero is MinionEntity)
			{
				return 7f;
			}
			return 0f;
		}

		public IEnumerator AttackSequence(CharacterEntity targetAlly)
		{
			base.EnemyModel.SetSpecialStateDuration(0.55f);
			base.EnemyModel.SetSpecialStateAnimationName(EnemyAnimation.animSpecialAttack);
			base.EnemyModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				EnemyAnimation.animSpecialAttack
			});
			base.EnemyModel.EnemyAnimationController.ToSpecialAttackState();
			base.EnemyModel.transform.localScale = new Vector3((float)((targetAlly.transform.position.x <= base.EnemyModel.transform.position.x) ? -1 : 1), 1f, 1f);
			yield return new WaitForSeconds(0.4f);
			GameObject projectile = ObjectCache.Spawn(projPrefab);
			projectile.transform.position = base.transform.position;
			projectile.transform.right = (targetAlly.transform.position - projectile.transform.position).normalized;
			yield return null;
			Vector3 targetPos = targetAlly.transform.position;
			float shootingDur = 0.15f;
			if (GameKit.IsValidEnemy(base.EnemyModel))
			{
				projectile.transform.DOMove(targetPos, shootingDur, false);
			}
			yield return new WaitForSeconds(shootingDur);
			projectile.transform.DOKill(false);
			projectile.Recycle();
			ObjectCache.Spawn(explodeEffectPrefab, targetPos);
			int minDamage = base.EnemyModel.OriginalParameter.attack_magic_min;
			int maxDamage = base.EnemyModel.OriginalParameter.attack_magic_max;
			int magicDamage = UnityEngine.Random.Range(minDamage, maxDamage);
			if (GameKit.IsValidCharacter(targetAlly))
			{
				if (targetAlly is HeroEntity)
				{
					targetAlly.ProcessDamage(DamageKind.Magic, new SharedStrikeDamage(0, magicDamage, 0f));
				}
				else
				{
					targetAlly.Dead();
					targetAlly.ReturnPool(0f);
				}
			}
			yield break;
		}

		public float delayAttack;

		public float detectRange;

		public GameObject projPrefab;

		public GameObject explodeEffectPrefab;

		private float countdown;

		private int subscribeId;
	}
}
