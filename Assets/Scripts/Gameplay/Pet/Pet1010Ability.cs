using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class Pet1010Ability : HeroAbilityShared
	{
		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			PetSetupRecord petConfigData = heroModel.PetConfigData;
			if (petConfigData == null)
			{
				UnityEngine.Debug.LogError(" pet config data is nulllllll ");
			}
			atkbuffPercentage = (float)petConfigData.Skillvalues[0];
			hpBuffPercentage = (float)petConfigData.Skillvalues[1];
			houndTotalMangicDam = (float)petConfigData.Skillvalues[2];
			numOfHoundProjectiles = petConfigData.Skillvalues[3];
			houndDamageRange = (float)petConfigData.Skillvalues[4] / GameRecord.PIXEL_PER_UNIT;
			houndCooldownDuration = (float)petConfigData.Skillvalues[5] / 1000f;
			houndCooldownCountdown = houndCooldownDuration;
			HeroEntity petOwner = heroModel.PetOwner;
			if (petOwner == null)
			{
				UnityEngine.Debug.LogError(" ower model is null");
			}
			petOwner.BuffsHolder.AddBuff("BuffAttackByPercentage", new BuffStatus(true, atkbuffPercentage, 999999f), BuffStackRule.StackUp, BuffStackRule.ChooseMax);
			petOwner.BuffsHolder.AddBuff("BuffHpByPercentage", new BuffStatus(true, hpBuffPercentage, 999999f), BuffStackRule.StackUp, BuffStackRule.ChooseMax);
			MonoSingleton<ZoneHandler>.Instance.OnEnemyReachGate += OnEnemyReachGate;
		}

		private void OnDestroy()
		{
			ZoneHandler mc = MonoSingleton<ZoneHandler>.InstanceIfExists;
			if (mc != null)
			{
				mc.OnEnemyReachGate -= OnEnemyReachGate;
			}
		}

		public override void Update()
		{
			base.Update();
			for (int i = projList.Count - 1; i >= 0; i--)
			{
				if (!projList[i].OnUpdate(Time.deltaTime))
				{
					projList.RemoveAt(i);
					List<EnemyData> listEnemiesInRange = GameKit.GetListEnemiesInRange(targetPos, new SharedStrikeDamage(0, magicDamagePerHit, true, skillRange));
					for (int j = listEnemiesInRange.Count - 1; j >= 0; j--)
					{
						if (GameKit.IsValidEnemy(listEnemiesInRange[j]))
						{
							listEnemiesInRange[j].ProcessDamage(DamageKind.Magic, new SharedStrikeDamage(0, magicDamagePerHit, true, skillRange));
						}
					}
				}
			}
			houndCooldownCountdown -= Time.deltaTime;
		}

		private void OnEnemyReachGate(Vector2 targetPosition)
		{
			if (houndCooldownCountdown > 0f)
			{
				return;
			}
			if (!heroModel.IsAlive)
			{
				return;
			}
			houndCooldownCountdown = houndCooldownDuration;
			base.StartCoroutine(CastSkill(new Vector3(targetPosition.x, targetPosition.y, 0f), (float)numOfHoundProjectiles * delayBtwShoot, houndDamageRange, houndTotalMangicDam, false));
		}

		public void TriggerBabyDragonRage(Vector3 targetPos, float skillDuration, float skillRange, float magicDamage)
		{
			if (!heroModel.IsAlive)
			{
				return;
			}
			base.StartCoroutine(CastSkill(targetPos, skillDuration, skillRange, magicDamage, true));
		}

		private IEnumerator CastSkill(Vector3 targetPos, float skillDuration, float skillRange, float magicDamage, bool moveNearTarget = true)
		{
			this.targetPos = targetPos;
			this.skillRange = skillRange;
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			float disToTarget = (targetPos - heroModel.transform.position).magnitude;
			float moveToTargetDuration = disToTarget / moveToTargetSpeed;
			int numOfJump = Mathf.CeilToInt(disToTarget / 2f);
			heroModel.SetSpecialStateDuration(skillDuration + ((!moveNearTarget) ? 0f : moveToTargetDuration));
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animRun);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animRun
			});
			float lookSide = (float)((targetPos.x - heroModel.transform.position.x <= 0f) ? -1 : 1);
			heroModel.transform.localScale = new Vector3(lookSide, 1f, 1f);
			if (moveNearTarget)
			{
				Vector3 standPosToAttack = targetPos - new Vector3(lookSide * disToAttackPos, 0f, 0f);
				heroModel.transform.DOJump(standPosToAttack, 0.7f, numOfJump, moveToTargetDuration, false).SetEase(Ease.Linear);
				yield return new WaitForSeconds(moveToTargetDuration);
			}
			heroModel.GetAnimationController().ToSpecialState(HeroMotionHandler.animActiveSkill, skillDuration);
			int numOfProjectiles = Mathf.FloorToInt((skillDuration - projFlyDuration * 0.5f) / delayBtwShoot);
			magicDamagePerHit = Mathf.CeilToInt(magicDamage * 1f / (float)numOfProjectiles);
			for (int i = 0; i < numOfProjectiles; i++)
			{
				Vector2 offset = UnityEngine.Random.insideUnitCircle * skillRange;
				Vector3 projTargetPos = targetPos + new Vector3(offset.x, offset.y, 0f);
				projList.Add(new Wyrm10Ability0Projectile(projectileDraRagePrefab, explodeDraRagePrefab, barrelPos.position, projTargetPos, 2f, projFlyDuration * UnityEngine.Random.Range(0.75f, 1.1f)));
				yield return new WaitForSeconds(delayBtwShoot);
			}
			yield break;
		}

		private float atkbuffPercentage;

		private float hpBuffPercentage;

		private float houndTotalMangicDam;

		private int numOfHoundProjectiles;

		private float houndDamageRange;

		private float houndCooldownDuration;

		public GameObject projectileDraRagePrefab;

		public GameObject explodeDraRagePrefab;

		public Transform barrelPos;

		public float moveToTargetSpeed;

		public float disToAttackPos;

		public float projFlyDuration = 0.3f;

		public float delayBtwShoot = 0.4f;

		private Vector3 targetPos;

		private int magicDamagePerHit;

		private float skillRange;

		private float houndCooldownCountdown;
		private List<Wyrm10Ability0Projectile> projList = new List<Wyrm10Ability0Projectile>();
	}
}
