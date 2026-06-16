using System;
using UnityEngine;

namespace Gameplay
{
	public class MinionAbilityInvisible : MinionHandler
	{

        [SerializeField]
        private bool changeAlphaWhenInvisible;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private Color tmpColor;


        private bool isInvisible;
        public override void Update()
		{
			base.Update();
			isInvisible = (base.MinionEntity.curState == EntityPhaseEnum.CharacterIdle);
			base.MinionEntity.IsInvisible = isInvisible;
			if (changeAlphaWhenInvisible)
			{
				SetAlpha(isInvisible);
			}
		}

		private void SetAlpha(bool isInvisible)
		{
			tmpColor = spriteRenderer.color;
			if (isInvisible)
			{
				tmpColor.a = 0.5f;
			}
			else
			{
				tmpColor.a = 1f;
			}
			spriteRenderer.color = tmpColor;
		}

	}
}
