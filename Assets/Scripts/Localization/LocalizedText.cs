using System;
using TMPro;
using UnityEngine;

//[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    public TMP_Text localText;

    public string localizeKey;

    [Space]
    [Header("Attribute")]
    [SerializeField]
    private bool setOnUpdate;

    private void Start()
	{
		SetLocalizeContent();
	}

	private void Update()
	{
		if (setOnUpdate)
		{
			SetLocalizeContent();
		}
	}

	private void SetLocalizeContent()
	{
		localText.text = GameKit.GetLocalization(localizeKey);
	}
}
