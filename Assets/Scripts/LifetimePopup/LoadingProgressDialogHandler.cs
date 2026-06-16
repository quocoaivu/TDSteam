using System;
using System.Collections;
using GeneralVariable;
using Parameter;
using UnityEngine;

namespace LifetimePopup
{
	public class LoadingProgressDialogHandler : MonoBehaviour
	{
		public void Open()
		{
			base.gameObject.SetActive(true);
			base.StartCoroutine(CheckConnectionTimeOut());
		}

		private IEnumerator CheckConnectionTimeOut()
		{
			yield return new WaitForSeconds((float)GeneralVariable.GeneralDefine.CONNECTION_TIMEOUT);
			if (IsGameObjectActive())
			{
				Close();
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(150);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
			}
			yield break;
		}

		public bool IsGameObjectActive()
		{
			return base.gameObject.activeSelf;
		}

		public void Close()
		{
			base.gameObject.SetActive(false);
			base.StopCoroutine(CheckConnectionTimeOut());
		}
	}
}
