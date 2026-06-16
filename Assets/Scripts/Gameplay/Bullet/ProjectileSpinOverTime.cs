using System;
using UnityEngine;

namespace Gameplay
{
	public class ProjectileSpinOverTime : MonoBehaviour
	{
        [SerializeField]
        private float rotationSpeed;

        private EnemyData targetEnemy;

        private Vector3 targetPosition;

        private ProjectileEntity bulletModel;

        public void Awake()
		{
			bulletModel = base.GetComponent<ProjectileEntity>();
			bulletModel.OnInitialized += BulletModel_OnInitialized;
		}

		private void BulletModel_OnInitialized()
		{
			targetEnemy = bulletModel.target;
		}

		private void Update()
		{
			Rotation();
		}

		private int GetRelativePositionX()
		{
			int result;
			if (targetEnemy && targetEnemy.transform.position.x - base.gameObject.transform.position.x > 0f)
			{
				result = 1;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		private void Rotation()
		{
			if (GetRelativePositionX() > 0)
			{
				base.transform.Rotate(rotationSpeed * Vector3.back);
			}
			else
			{
				base.transform.Rotate(rotationSpeed * Vector3.forward);
			}
		}
	}
}
