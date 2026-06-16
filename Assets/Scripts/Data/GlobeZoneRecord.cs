using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
	public class GlobeZoneRecord : MonoBehaviour
	{
        [SerializeField]
        [FormerlySerializedAs("readDataMapRule")]
        private MapRuleDataLoader mapRuleLoader;

        public MapRuleDataLoader MapRuleLoader
		{
			get
			{
				return mapRuleLoader;
			}
			set
			{
				mapRuleLoader = value;
			}
		}

		public static GlobeZoneRecord Instance { get; set; }

		private void Awake()
		{
			if (GlobeZoneRecord.Instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			GlobeZoneRecord.Instance = this;
		}
	}
}
