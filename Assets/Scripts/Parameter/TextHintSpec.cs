using System;
using System.Collections.Generic;
using UnityEngine;

namespace Parameter
{
	public class TextHintSpec : Singleton<TextHintSpec>
	{
		public void ClearData()
		{
			listTextTip.Clear();
		}

		public void SetTextTipParameter(TextHint textTip)
		{
			int count = listTextTip.Count;
			if (count <= textTip.level)
			{
				List<TextHint> list = new List<TextHint>();
				list.Insert(textTip.id, textTip);
				listTextTip.Insert(textTip.level, list);
			}
			else
			{
				List<TextHint> list2 = listTextTip[textTip.level];
				list2.Insert(textTip.id, textTip);
			}
		}

		public TextHint GetTextTipParameter(int level, int id)
		{
			if (CheckParameter(level, id))
			{
				return listTextTip[level][id];
			}
			return default(TextHint);
		}

		private bool CheckParameter(int level, int id)
		{
			return level <= GetNumberOfLevel() && id < GetNumberOfLevel();
		}

		public int GetNumberOfLevel()
		{
			return listTextTip.Count;
		}

		public int GetNumberTextTipByLevel(int level)
		{
			if (GetNumberOfLevel() > 0)
			{
				return listTextTip[level].Count;
			}
			return 0;
		}

		public string GetRandomTextTipContent(int level)
		{
			string empty = string.Empty;
			int index = UnityEngine.Random.Range(0, GetNumberTextTipByLevel(level));
			return listTextTip[level][index].textTipContent;
		}

		public List<List<TextHint>> listTextTip = new List<List<TextHint>>();
	}
}
