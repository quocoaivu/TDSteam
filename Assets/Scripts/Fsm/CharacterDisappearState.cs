using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Gameplay;
using UnityEngine;

public class CharacterDisappearState : CharacterState
{
    private SpriteRenderer spriteRenderer;

    private bool isDisappearedIntermediate;

    public CharacterDisappearState(CharacterEntity character, IFsmController fSMController, bool isDisappearedIntermediate = false) : base(character, fSMController)
	{
		this.isDisappearedIntermediate = isDisappearedIntermediate;
	}

	public override void OnStartState()
	{
		base.OnStartState();
		if (isDisappearedIntermediate)
		{
			OnFadeOutComplete();
		}
		else
		{
			spriteRenderer = character.GetComponent<SpriteRenderer>();
			DOTween.To(() => 255f, delegate(float x)
			{
				spriteRenderer.color = new Color(1f, 1f, 1f, x / 255f);
			}, 0f, 1f).OnComplete(new TweenCallback(OnFadeOutComplete));
		}
	}

	private void OnFadeOutComplete()
	{
		character.ReturnPool(0f);
	}
}
