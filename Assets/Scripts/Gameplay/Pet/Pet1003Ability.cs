using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class Pet1003Ability : HeroAbilityShared
	{
		public override void Init(HeroEntity heroModel)
		{
			minPhysicsDam = heroModel.PetConfigData.Skillvalues[0];
			maxPhysicsDam = heroModel.PetConfigData.Skillvalues[1];
			damageToHpRatio = (float)(heroModel.PetConfigData.Skillvalues[2] / 100);
			buffHpPercent = (float)heroModel.PetConfigData.Skillvalues[3];
			delayAttack = (float)heroModel.PetConfigData.Skillvalues[4] / 1000f;
			detectRange = (float)heroModel.PetConfigData.Skillvalues[5] / GameRecord.PIXEL_PER_UNIT;
			petModel = heroModel;
			ownerModel = petModel.PetOwner;
			ownerModel.BuffsHolder.AddBuff("BuffHpByPercentage", new BuffStatus(true, buffHpPercent, 999999f), BuffStackRule.StackUp, BuffStackRule.ChooseMax);
			Common.GameObjectPool.InitPool(GameKit.pet1003BulletPath, 0);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_HEAL_0);
			MonoSingleton<FXPool>.Instance.InitFX(explodeFx);
		}

		public override void Update()
		{
			base.Update();
			Pet1003Ability.NightwingState nightwingState = curState;
			if (nightwingState != Pet1003Ability.NightwingState.cooldown)
			{
				if (nightwingState != Pet1003Ability.NightwingState.findTarget)
				{
					if (nightwingState != Pet1003Ability.NightwingState.attack)
					{
					}
				}
				else
				{
					countdown -= Time.deltaTime;
					if (countdown <= 0f)
					{
						countdown = 0.3f;
						curTarget = FindTarget(-1f);
						if (curTarget != null)
						{
							base.StartCoroutine(CastSkill());
						}
					}
				}
			}
			else
			{
				countdown -= Time.deltaTime;
				if (countdown <= 0f)
				{
					curState = Pet1003Ability.NightwingState.findTarget;
				}
			}
		}

		public EnemyData FindTarget(float customDetectRange = -1f)
		{
			if (customDetectRange < 0f)
			{
				customDetectRange = detectRange;
			}
			float num = customDetectRange * customDetectRange;
			List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
			for (int i = listActiveEnemy.Count - 1; i >= 0; i--)
			{
				if (!listActiveEnemy[i].IsUnderground && MonoSingleton<GameRecord>.Instance.SqrDistance(petModel.transform.position, listActiveEnemy[i].transform.position) < num)
				{
					return listActiveEnemy[i];
				}
			}
			return null;
		}

		public IEnumerator CastSkill()
		{
			curState = Pet1003Ability.NightwingState.attack;
			Vector3 attackPos;
			float movingTime;
			GameKit.CalculateAttackPosition(petModel, curTarget, petModel.GetSpeed() * 2f, out attackPos, out movingTime);
			Vector3 startPos = petModel.transform.position;
			petModel.SetSpecialStateDuration(movingTime * 2f);
			petModel.SetSpecialStateAnimationName(HeroMotionHandler.animRun);
			petModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[0]);
			curTarget.SetSpecialStateDuration(movingTime + atkDuration + 0.1f);
			curTarget.SetSpecialStateAnimationName(EnemyAnimation.animIdle);
			curTarget.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[0]);
			LookTowardTargetPos(attackPos);
			petModel.transform.DOJump(attackPos, flyHeight, 1, movingTime, false);
			yield return new WaitForSeconds(movingTime);
			if (FireProjectile())
			{
				petModel.GetAnimationController().ToSpecialState(HeroMotionHandler.animActiveSkill, atkDuration);
				yield return new WaitForSeconds(atkDuration);
				if (projectile != null)
				{
					Common.GameObjectPool.Despawn(projectile);
				}
				DealDamageToTarget();
			}
			yield return new WaitForSeconds(0.1f);
			petModel.GetAnimationController().ToRunState();
			LookTowardTargetPos(startPos);
			petModel.transform.DOJump(startPos, -flyHeight, 1, movingTime, false);
			yield return new WaitForSeconds(movingTime);
			countdown = delayAttack;
			curState = Pet1003Ability.NightwingState.cooldown;
			yield break;
		}

		private void LookTowardTargetPos(Vector3 targetPos)
		{
			if (targetPos.x > petModel.transform.position.x)
			{
				petModel.transform.localScale = Vector3.one;
			}
			else
			{
				petModel.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}

		public bool FireProjectile()
		{
			if (!GameKit.IsValidEnemy(curTarget))
			{
				curTarget = FindTarget(detectRange * 0.5f);
			}
			if (!GameKit.IsValidEnemy(curTarget))
			{
				return false;
			}
			LookTowardTargetPos(curTarget.transform.position);
			projectile = Common.GameObjectPool.Spawn(GameKit.pet1003BulletName, gunBarrel.transform.position, default(Quaternion));
			projectile.transform.up = (gunBarrel.transform.position - curTarget.transform.position).normalized;
			projectile.transform.DOMove(curTarget.transform.position, atkDuration, false);
			return true;
		}

		private void DealDamageToTarget()
		{
			commonAttackDamageSender = new SharedStrikeDamage();
			commonAttackDamageSender.physicsDamage = UnityEngine.Random.Range(minPhysicsDam, maxPhysicsDam + 1);
			commonAttackDamageSender.magicDamage = 0;
			commonAttackDamageSender.criticalStrikeChance = 0;
			commonAttackDamageSender.isIgnoreArmor = false;
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(explodeFx);
			effect.transform.position = curTarget.transform.position;
			effect.Init(1f);
			int currentHealth = curTarget.EnemyHealthController.CurrentHealth;
			curTarget.ProcessDamage(DamageKind.Range, commonAttackDamageSender);
			int currentHealth2 = curTarget.EnemyHealthController.CurrentHealth;
			if (currentHealth2 < currentHealth && ownerModel.IsAlive)
			{
				int hpAmount = Mathf.RoundToInt((float)(currentHealth - currentHealth2) * damageToHpRatio);
				ownerModel.IncreaseHealth(hpAmount);
				VisualEffectInstance effect2 = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_HEAL_0);
				effect2.transform.position = ownerModel.transform.position;
				effect2.Init(1.5f, ownerModel.transform, ownerModel.GetComponent<SpriteRenderer>().sprite.rect.width);
			}
		}

		public Transform gunBarrel;

		private int minPhysicsDam;

		private int maxPhysicsDam;

		private float damageToHpRatio;

		private float buffHpPercent;

		private float delayAttack;

		private float detectRange;

		private HeroEntity petModel;

		private HeroEntity ownerModel;

		private EnemyData curTarget;

		private GameObject projectile;

		private float countdown;

		private float atkDuration = 0.3f;

		private float flyHeight = 0.3f;

		private Pet1003Ability.NightwingState curState;

		private SharedStrikeDamage commonAttackDamageSender;

		private string explodeFx = "FireBall";

		private enum NightwingState
		{
			cooldown,
			findTarget,
			attack
		}
	}
}
