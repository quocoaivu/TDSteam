using System;
using DG.Tweening;
using GameCore;
using TMPro;
using UnityEngine;

namespace Gameplay
{
	public class BountyHandler : BaseMonoBehaviour
	{
		public void SetMoneyMessage()
		{
			int money = MonoSingleton<GameRecord>.Instance.Money;
			if (money != lastMoney)
			{
				moneyMessage.SetText("{0}", money);
				AnimationAddMoney();
				lastMoney = money;
			}
		}

		private void AnimationAddMoney()
		{
			tween.Restart(true);
			tween = moneyMessage.transform.DOScale(scaleVector, 0.2f).OnComplete(new TweenCallback(LateAnimation));
		}

		private void LateAnimation()
		{
			tween = moneyMessage.transform.DOScale(Vector3.one, 0.1f);
		}

		[SerializeField]
		private TextMeshProUGUI moneyMessage;

		private int lastMoney = -1;

		private Vector3 scaleVector = new Vector3(1.2f, 1.2f, 1.2f);

		private Tween tween;
	}
}
