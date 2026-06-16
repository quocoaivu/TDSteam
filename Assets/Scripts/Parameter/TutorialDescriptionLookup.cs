using System;
using System.Collections.Generic;

namespace Parameter
{
	public class TutorialDescriptionLookup : Singleton<TutorialDescriptionLookup>
	{
		public void ClearData()
		{
			listTutorialDescription.Clear();
		}

		public void SetTutParameter(TutorialDescriptionEntry tut)
		{
			if (!listTutorialDescription.ContainsKey(tut.id))
			{
				listTutorialDescription.Add(tut.id, tut.description);
			}
		}

		public string GetDescription(string tutID)
		{
			string empty = string.Empty;
			if (!listTutorialDescription.ContainsKey(tutID) || listTutorialDescription.TryGetValue(tutID, out empty))
			{
			}
			return empty;
		}

		private Dictionary<string, string> listTutorialDescription = new Dictionary<string, string>();
	}
}
