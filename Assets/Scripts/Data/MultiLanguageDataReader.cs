using System.Collections.Generic;
using GameCore;
using UnityEngine;

namespace Data
{
	public class MultiLanguageDataReader : BaseMonoBehaviour
	{
        [SerializeField]
        private List<CommonDescriptionLoader> listDescriptionReader = new List<CommonDescriptionLoader>();

        private void Awake()
		{
			ReadParameters();
		}

		private void ReadParameters()
		{
			foreach (CommonDescriptionLoader readCommonDescription in listDescriptionReader)
			{
				if (readCommonDescription != null)
				{
					readCommonDescription.ReadParameter();
				}
			}
		}

		public void ReloadParameters()
		{
			ReadParameters();
		}
	}
}
