using System;
using Parameter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertSynopsisSetter : MonoBehaviour
{
	private void Start()
	{
		SetDescription();
	}

	private void Update()
	{
		if (setOnUpdate)
		{
			SetDescription();
		}
	}

	public void SetDescription()
	{
		if (notiDescription)
		{
			notiDescription.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(notiID).Replace('@', '\n').Replace('#', '-');
		}
		if (textMesh)
		{
			textMesh.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(notiID).Replace('@', '\n').Replace('#', '-');
		}
		else if (titleTMPro != null)
		{
			titleTMPro.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(notiID).Replace('@', '\n').Replace('#', '-');
		}
	}

	[SerializeField]
	private int notiID;

	[SerializeField]
	private Text notiDescription;

	[SerializeField]
	private TextMesh textMesh;

	public TextMeshProUGUI titleTMPro;

	[Space]
	[Header("Attribute")]
	[SerializeField]
	private bool setOnUpdate;
}
