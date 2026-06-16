using System;
using System.Collections.Generic;

namespace Gameplay
{
	public class ConstructSectorDirector : MonoSingleton<ConstructSectorDirector>
	{

        public List<ConstructSectorHandler> listRegions = new List<ConstructSectorHandler>();
        public void InvokeClickk(int regionID)
		{
			listRegions[regionID].TryToClick();
		}


	}
}
