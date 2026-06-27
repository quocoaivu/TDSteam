using TMPro;
using UnityEngine;

namespace Gameplay
{
	// One label/value line in the tower stat list. Cloned from a template by
	// CurrentLevelOverviewDialog, one per stat shown.
	public class StatRowView : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text labelText;

		[SerializeField]
		private TMP_Text valueText;

		public void Set(string label, string value)
		{
			if (labelText != null)
			{
				labelText.text = label;
			}
			if (valueText != null)
			{
				valueText.text = value;
			}
		}
	}
}
