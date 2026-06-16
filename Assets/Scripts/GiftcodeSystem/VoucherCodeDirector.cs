using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace GiftcodeSystem
{
	public class VoucherCodeDirector : MonoBehaviour
	{
        private const string GIFTCODE_ENABLE = "giftcode_enable";

        public static bool isGiftCodeEnable;

        private const string urlAPI = "http://api.zonmob.com/v1/giftcode";

        private const string product_uuid = "510e1472-7cde-47dd-8b16-64c8aa4c645d";

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event Action<ReceivedVoucherCodeMessage> onGiftCodeSubmited;

		private void Awake()
		{
		}

		public void SubmitGiftCode(string giftCode, string uuid)
		{
			string text = JsonUtility.ToJson(new SubmitVoucherCode
			{
				product_uuid = "510e1472-7cde-47dd-8b16-64c8aa4c645d",
				giftcode = giftCode,
				user_id = uuid
			});
			UnityEngine.Debug.Log(text);
			base.StartCoroutine(DoSubmitGiftCode(text));
		}

		private IEnumerator DoSubmitGiftCode(string data)
		{
			ReceivedVoucherCodeMessage receivedData = new ReceivedVoucherCodeMessage();
			using (UnityWebRequest www = new UnityWebRequest("http://api.zonmob.com/v1/giftcode", "POST"))
			{
				www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
				www.downloadHandler = new DownloadHandlerBuffer();
				www.SetRequestHeader("Content-Type", "application/json");
				yield return www.SendWebRequest();
				if (www.result != UnityWebRequest.Result.Success)
				{
					receivedData.status = 0;
					receivedData.message = www.error;
				}
				else
				{
					receivedData = JsonUtility.FromJson<ReceivedVoucherCodeMessage>(www.downloadHandler.text);
				}
			}
			if (onGiftCodeSubmited != null)
			{
				onGiftCodeSubmited(receivedData);
			}
			yield break;
		}
	}
}
