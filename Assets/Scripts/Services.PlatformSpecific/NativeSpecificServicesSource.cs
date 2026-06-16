using System;
using UnityEngine;

namespace Services.PlatformSpecific
{
	public class NativeSpecificServicesSource : MonoBehaviour
	{
		public static INativeSpecificServices Services
		{
			get
			{
				return NativeSpecificServicesSource.services;
			}
			set
			{
				if (NativeSpecificServicesSource.services != null)
				{
					throw new InvalidOperationException("Services can not be set twice");
				}
				NativeSpecificServicesSource.services = value;
			}
		}

		private static INativeSpecificServices services;
	}
}
