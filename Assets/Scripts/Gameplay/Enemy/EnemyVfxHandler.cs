using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Gameplay
{
	public class EnemyVfxHandler : EnemyBrain
	{
        private static Color freezeColor = new Color(0.5372549f, 0.9411765f, 1f, 1f);

        private static Color slowColor = new Color(0.5764706f, 0.8901961f, 0.772549033f, 1f);

        private SpriteRenderer spriteRenderer;

        private Color colorOrigin = new Color(1f, 1f, 1f, 1f);

        private Color currentColor = new Color(1f, 1f, 1f, 1f);

        [Space]
        [Header("General")]
        [SerializeField]
        private Transform pointEffectPosition;

        [SerializeField]
        private Transform centerPosition;

        [Space]
        [Header("Effect má» dáº§n khi cháº¿t")]
        [SerializeField]
        private bool effectFadeOnDieAnim;

        [SerializeField]
        private float totalDieAnimTime;

        [SerializeField]
        private float dieAnimTime;

        private float fadeTime;

        private float alphaValue;

        private Tweener tween;

        [Space]
        [Header("Äá»•i hÃ¬nh áº£nh khi bá»‹ Ä‘Ã³ng bÄƒng")]
        [SerializeField]
        private bool changeSpriteWhenFreezing;

        [SerializeField]
        private Sprite freezingSprite;

        [SerializeField]
        private Sprite normalSprite;

        private List<VisualEffectInstance> listEffectsOnEnemy = new List<VisualEffectInstance>();

        private void Awake()
		{
			spriteRenderer = base.transform.parent.GetComponent<SpriteRenderer>();
			base.EnemyModel.OnEnemyDied += EnemyModel_OnEnemyDied;
			fadeTime = totalDieAnimTime / 2f - dieAnimTime / 2f;
		}

		private void EnemyModel_OnEnemyDied()
		{
			base.CustomInvoke(new Action(FadeOut), dieAnimTime);
		}

		private void OnDestroy()
		{
			base.EnemyModel.OnEnemyDied -= EnemyModel_OnEnemyDied;
		}

		public override void OnAppear()
		{
			base.OnAppear();
			SetNormalColor();
			RemoveAllFXs();
			Show();
			if (changeSpriteWhenFreezing)
			{
				SetNormalSprite();
			}
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			SetNormalColor();
			RemoveAllFXs();
		}

		public void PlayDamageFX(DamageVfxType damageFxType, float activationTime = 1f)
		{
			switch (damageFxType)
			{
			case DamageVfxType.Critical:
				PlayFXCritical(2f);
				break;
			case DamageVfxType.Slow:
				PlayFXSlow(activationTime);
				break;
			case DamageVfxType.Stun:
				PlayFXStun(activationTime);
				break;
			case DamageVfxType.Freezing:
				PlayFXFreezing(activationTime);
				break;
			case DamageVfxType.Root:
				PlayFXRoot(activationTime);
				break;
			case DamageVfxType.Electric:
				PlayFXElectric(activationTime);
				break;
			case DamageVfxType.Thunder:
				PlayFXThunder();
				break;
			case DamageVfxType.Bleed:
				PlayFXBleed(activationTime);
				break;
			case DamageVfxType.DefDown:
				PlayFXDefdown(activationTime);
				break;
			case DamageVfxType.Poison1:
				PlayFXPoison1(activationTime);
				break;
			}
		}

		public void Show()
		{
			spriteRenderer.enabled = true;
		}

		public void Hide()
		{
			spriteRenderer.enabled = false;
		}

		public void PlayFXCritical(float activationTime = 2f)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_CRITICAL);
			effect.transform.position = pointEffectPosition.position;
			effect.Init(activationTime, effect.transform);
		}

		public void PlayFXStun(float activationTime = 2f)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_STUN);
			effect.transform.position = pointEffectPosition.position;
			effect.Init(activationTime, pointEffectPosition);
		}

		public void PlayFXRoot(float activationTime = 2f)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_ROOT);
			effect.transform.position = base.transform.position;
			effect.Init(activationTime, base.transform);
		}

		public void PlayFXElectric(float activationTime = 2f)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_ELECTRIC);
			effect.transform.position = base.transform.position;
			effect.Init(activationTime, base.transform.position);
		}

		public void PlayFXThunder()
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_THUNDER);
			effect.transform.position = pointEffectPosition.position;
			effect.Init(1f, base.transform.position);
		}

		public void PlayFXMiss(float activationTime)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_MISS);
			effect.transform.position = pointEffectPosition.position;
			effect.Init(activationTime, base.transform);
		}

		public void PlayFXBurning(float activationTime)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_BURNING);
			effect.transform.position = base.transform.position;
			effect.Init(base.transform);
			listEffectsOnEnemy.Add(effect);
			base.StartCoroutine(RemoveFX(effect, activationTime));
		}

		public void PlayFXDefdown(float activationTime)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_DEF_DOWN);
			effect.transform.position = base.transform.position;
			effect.Init(base.transform);
			listEffectsOnEnemy.Add(effect);
			base.StartCoroutine(RemoveFX(effect, activationTime));
		}

		private void PlayFXBleed(float activationTime)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_BLEED);
			effect.transform.position = pointEffectPosition.position;
			effect.Init(activationTime, pointEffectPosition);
		}

		private void PlayFXPoison1(float activationTime)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_POISON1);
			effect.transform.position = centerPosition.transform.position;
			effect.Init(activationTime, centerPosition);
			effect.SetSize(base.EnemyModel.OriginalParameter.size);
		}

		private void PlayFXSlow(float activationTime)
		{
			spriteRenderer.color = EnemyVfxHandler.slowColor;
			base.CancelInvoke("SetNormalColor");
			base.CustomInvoke(new Action(SetNormalColor), activationTime);
		}

		public void PlayFXFreezing(float activationTime)
		{
			if (changeSpriteWhenFreezing)
			{
				ChangeFreezingSprite(activationTime);
			}
			else
			{
				ChangeFreezingColor(activationTime);
				FreezeEnemyFoot(activationTime);
			}
		}

		private void FreezeEnemyFoot(float activationTime)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_ITEM_FREEZE);
			effect.transform.position = base.transform.position;
			effect.Init(base.transform);
			effect.SetSize(base.EnemyModel.OriginalParameter.size);
			listEffectsOnEnemy.Add(effect);
			base.StartCoroutine(RemoveFX(effect, activationTime));
		}

		private void ChangeFreezingColor(float activationTime)
		{
			base.CancelInvoke("SetNormalColor");
			spriteRenderer.color = EnemyVfxHandler.freezeColor;
			base.CustomInvoke(new Action(SetNormalColor), activationTime);
		}

		private void ChangeFreezingSprite(float activationTime)
		{
			base.EnemyModel.EnemyAnimationController.TurnOffAnimator();
			base.CancelInvoke("SetNormalSprite");
			spriteRenderer.sprite = freezingSprite;
			base.CustomInvoke(new Action(SetNormalSprite), activationTime);
		}

		private void FadeOut()
		{
			alphaValue = 0f;
			if (effectFadeOnDieAnim)
			{
				tween = DOTween.To(() => 255f, delegate(float x)
				{
					alphaValue = x;
					currentColor = new Color(1f, 1f, 1f, alphaValue / 255f);
					spriteRenderer.color = currentColor;
				}, 0f, fadeTime).SetEase(Ease.Linear);
			}
		}

		public void SetNormalColor()
		{
			tween.Kill(false);
			spriteRenderer.color = colorOrigin;
		}

		public void SetNormalSprite()
		{
			base.EnemyModel.EnemyAnimationController.TurnOnAnimator();
			spriteRenderer.sprite = normalSprite;
		}

		public void RemoveAllFXs()
		{
			base.StopAllCoroutines();
			foreach (VisualEffectInstance effect in listEffectsOnEnemy)
			{
				base.StartCoroutine(RemoveFX(effect, 0f));
			}
		}

		private IEnumerator RemoveFX(VisualEffectInstance effect, float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			listEffectsOnEnemy.Remove(effect);
			effect.ReturnPool();
			yield break;
		}
	}
}
