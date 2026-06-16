using System;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

namespace Gameplay
{
	public class HeroTravelHandler : HeroHandler
	{
        public Vector3 assignedPosition;

        private float speed;


        public bool isMoving
		{
			get
			{
				return base.HeroModel.GetFsmController().GetCurrentState() is CharacterMoveState;
			}
		}

		public float GetSpeed()
		{
			return speed;
		}

		public override void OnAppear()
		{
			base.OnAppear();
			base.HeroModel.SetAssignedPosition(base.transform.position);
			SetParameter();
			base.HeroModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		private void SetParameter()
		{
			speed = (float)base.HeroModel.OriginalParameter.speed;
		}

		private void Start()
		{
			HerosDirector.Instance.onHeroMoveToAssignedPosition += Instance_onHeroMoveToAssignedPosition;
		}

		private void OnDestroy()
		{
			if (HerosDirector.Instance != null)
			{
				HerosDirector.Instance.onHeroMoveToAssignedPosition -= Instance_onHeroMoveToAssignedPosition;
			}
		}

		public override void Update()
		{
			base.Update();
			if (HerosDirector.Instance.HeroIDChoosing == base.HeroModel.HeroID && GameplayTutorialDirector.Instance.IsTutorialMap())
			{
				GameplayTutorialDirector.Instance.TutorialMoveHero.TryMoveToStep(1);
			}
		}

		private void Instance_onHeroMoveToAssignedPosition(int heroID, Vector2 targetPosition)
		{
			if (base.HeroModel.HeroID == heroID)
			{
				if (!base.HeroModel.IsAlive)
				{
					HerosDirector.Instance.UnChooseHero(base.HeroModel.HeroID);
					return;
				}
				base.HeroModel.UnitSoundController.PlayStartMove();
			}
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (increaseMovementSpeedBuffKeys.Contains(buffKey))
			{
				ApplyIncreaseMovementSpeed();
			}
		}

		private void ApplyIncreaseMovementSpeed()
		{
			float buffsValue = base.HeroModel.BuffsHolder.GetBuffsValue(increaseMovementSpeedBuffKeys);
			speed = (float)(base.HeroModel.OriginalParameter.speed + (int)((float)base.HeroModel.OriginalParameter.speed * buffsValue / 100f));
		}

		private List<string> increaseMovementSpeedBuffKeys = new List<string>
		{
			"IncreaseMovementSpeed"
		};
	}
}
