using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class VisualEffectInstance : BaseMonoBehaviour
	{

        [Space]
        [Header("Smart size for frost effect")]
        [SerializeField]
        private float sampleEnemysize = 10f;

        [SerializeField]
        private float sampleFrostEffectSize = 0.25f;

        [SerializeField]
        private float sampleTargetSize = 64f;

        private float lifeTime;

        private Transform target;


        private Vector3 localScale;
        public float LifeTime
		{
			get
			{
				return lifeTime;
			}
			private set
			{
				lifeTime = value;
			}
		}

		public Transform Target
		{
			get
			{
				return target;
			}
			private set
			{
				target = value;
			}
		}

		public void Init(Transform _target)
		{
			Target = _target;
			base.gameObject.SetActive(true);
		}

		public void Init(float _lifeTime, Transform _target)
		{
			LifeTime = _lifeTime;
			Target = _target;
			base.gameObject.SetActive(true);
			base.CustomInvoke(new Action(ReturnPool), LifeTime);
		}

		public void Init(float _lifeTime, Transform _target, float targetSize)
		{
			LifeTime = _lifeTime;
			Target = _target;
			base.gameObject.SetActive(true);
			base.CustomInvoke(new Action(ReturnPool), LifeTime);
			localScale.Set(targetSize / sampleTargetSize, targetSize / sampleTargetSize, targetSize / sampleTargetSize);
			base.transform.localScale = localScale;
		}

		public void Init(float _lifeTime, Vector2 _target)
		{
			LifeTime = _lifeTime;
			base.transform.position = _target;
			base.gameObject.SetActive(true);
			base.CustomInvoke(new Action(ReturnPool), LifeTime);
		}

		public void Init(float _lifeTime)
		{
			LifeTime = _lifeTime;
			base.gameObject.SetActive(true);
			base.CustomInvoke(new Action(ReturnPool), LifeTime);
		}

		public void DoFadeIn(float _lifeTime, float targetValue)
		{
			SpriteRenderer spriteRender = base.GetComponent<SpriteRenderer>();
			Color color = spriteRender.color;
			DOTween.To(() => 0f, delegate(float x)
			{
				color.a = x;
				spriteRender.color = color;
			}, targetValue, _lifeTime).SetEase(Ease.Linear);
		}

		public void DoFadeOut(float _lifeTime, float originValue)
		{
			SpriteRenderer spriteRender = base.GetComponent<SpriteRenderer>();
			Color color = spriteRender.color;
			DOTween.To(() => originValue, delegate(float x)
			{
				color.a = x;
				spriteRender.color = color;
			}, 0f, _lifeTime).SetEase(Ease.Linear);
		}

		public void SetBiggerOverTime(float _duration)
		{
			base.transform.localScale = Vector3.zero;
			base.transform.DOScale(Vector3.one, _duration);
		}

		public void SetSize(int targetSize)
		{
			float num = (float)targetSize / sampleEnemysize;
			localScale.Set(num * sampleFrostEffectSize, num * sampleFrostEffectSize, num * sampleFrostEffectSize);
			base.transform.localScale = localScale;
		}

		public void SetLayerOverTarget(Transform target)
		{
			int sortingOrder = target.GetComponentInChildren<SpriteRenderer>().sortingOrder;
			base.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder + 1;
		}

		private void Update()
		{
			if (Target)
			{
				base.transform.position = Target.position;
			}
		}

		public void ReturnPool()
		{
			Target = null;
			base.gameObject.SetActive(false);
			Common.GameObjectPool.Despawn(base.gameObject);
		}
	}
}
