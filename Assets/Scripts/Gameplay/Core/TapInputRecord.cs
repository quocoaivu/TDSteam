using System;
using UnityEngine;

namespace Gameplay
{
	public class TapInputRecord
	{
        public TapInputPhase clickInputPhase;

        public RaycastHit2D mapHit;


        public RaycastHit2D entityHit;
        public TapInputRecord(TapInputPhase clickInputPhase, RaycastHit2D mapHit, RaycastHit2D entityHit)
		{
			this.clickInputPhase = clickInputPhase;
			this.mapHit = mapHit;
			this.entityHit = entityHit;
		}

		public bool CompareTag(RaycastHit2D hit, string pTag)
		{
			return !(hit.collider == null) && hit.collider.CompareTag(pTag);
		}
	}
}
