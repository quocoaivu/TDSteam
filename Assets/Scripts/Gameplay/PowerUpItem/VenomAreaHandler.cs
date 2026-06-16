using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class VenomAreaHandler : BaseMonoBehaviour
	{
		private void Update()
		{
			if (!isReady)
			{
				return;
			}
			if (timeTracking == 0f)
			{
				TryToCastBuff();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			if (isReady)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(base.transform.position, aoeRange);
			}
		}

		public void Init(float _aoeRange, float _activationTime, int _burnDamage)
		{
			aoeRange = _aoeRange;
			activationTime = _activationTime;
			burnDamage = _burnDamage;
			base.CustomInvoke(new Action(GetReady), appearAnimDuration);
			base.CustomInvoke(new Action(EndOfLifeTime), activationTime + appearAnimDuration);
		}

		private void TryToCastBuff()
		{
			MarkKind targetType;
			targetType.isAir = true;
			targetType.isGround = true;
			targetType.isUnderGround = false;
			targetType.isTunnel = false;
			List<EnemyData> listEnemiesInRange = GameKit.GetListEnemiesInRange(base.gameObject, new SharedStrikeDamage(targetType, aoeRange));
			if (listEnemiesInRange.Count > 0)
			{
				foreach (EnemyData enemyModel in listEnemiesInRange)
				{
					if (GameKit.IsValidEnemy(enemyModel) && !enemyModel.IsInTunnel)
					{
						DamageEnemy(enemyModel);
					}
				}
			}
			timeTracking = trackingDuration;
		}

		private void DamageEnemy(EnemyData enemyModel)
		{
			enemyModel.ProcessEffect(buffKey, burnDamage, trackingDuration + Time.deltaTime, DamageVfxType.Poison1);
		}

		private void GetReady()
		{
			isReady = true;
			base.StartCoroutine(ShowDecorations());
		}

		private void EndOfLifeTime()
		{
			isReady = false;
			HideDecoration();
			animator.Play("Disappear");
			base.CustomInvoke(new Action(ReturnPool), disAppearAnimDuration);
		}

		private void ReturnPool()
		{
			MonoSingleton<TowerPool>.Instance.Despawn(base.gameObject);
		}

		private IEnumerator ShowDecorations()
		{
			for (int i = 0; i < decorations.Length; i++)
			{
				decorations[i].SetActive(true);
				yield return new WaitForSeconds(delayTimeCreateDecoration);
			}
			yield break;
		}

		private void HideDecoration()
		{
			for (int i = 0; i < decorations.Length; i++)
			{
				decorations[i].SetActive(false);
			}
		}

		private float aoeRange;

		private float activationTime;

		private string buffKey = "Burning";

		private int burnDamage;

		[SerializeField]
		private float appearAnimDuration;

		[SerializeField]
		private float disAppearAnimDuration;

		private bool isReady;

		private float timeTracking;

		private float trackingDuration = 1f;

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private GameObject[] decorations;

		[SerializeField]
		private float delayTimeCreateDecoration;
	}
}
