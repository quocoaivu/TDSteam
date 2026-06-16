using System;
using Common;
using UnityEngine;

namespace Common
{
	public abstract class Trailer : OutsiteMarkTransform
	{
        [Header("Follower")]
        [SerializeField]
        private PositionSource destination;

        [SerializeField]
        private bool lookAtDestination;

        [SerializeField]
        private Vector3 offset;

        [SerializeField]
        private bool followX = true;

        [SerializeField]
        private bool followY = true;

        [SerializeField]
        private bool followZ = true;


        private IPositionSource internalDestination;
        public IPositionSource Destination
		{
			get
			{
				if (internalDestination == null)
				{
					return destination;
				}
				return internalDestination;
			}
			set
			{
				internalDestination = value;
				destination = null;
			}
		}

		public Vector3 Offset
		{
			get
			{
				return offset;
			}
			set
			{
				offset = value;
			}
		}

		public bool FollowX
		{
			get
			{
				return followX;
			}
			set
			{
				followX = value;
			}
		}

		public bool FollowY
		{
			get
			{
				return followY;
			}
			set
			{
				followY = value;
			}
		}

		public bool FollowZ
		{
			get
			{
				return followZ;
			}
			set
			{
				followZ = value;
			}
		}

		public void FollowImmediate()
		{
			base.TargetPosition = Destination.Position + Offset;
		}

		public void LateUpdate()
		{
			UpdateFollower();
		}

		private void UpdateFollower()
		{
			OnUpdatePositionFollow();
			if (lookAtDestination)
			{
				LookAtDestination();
			}
		}

		protected virtual void LookAtDestination()
		{
			Vector3 a = base.transform.position - Destination.Position;
			Quaternion rotation = Quaternion.LookRotation(-a);
			base.transform.rotation = rotation;
			base.transform.eulerAngles = new Vector3(0f, 0f, -base.transform.eulerAngles.x * Mathf.Sign(base.transform.localScale.x));
		}

		protected virtual void OnUpdatePositionFollow()
		{
		}


	}
}
