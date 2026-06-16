using System;
using GameCore;

namespace Gameplay
{
	public class HeroAbilityShared : BaseMonoBehaviour
	{
        protected HeroEntity heroModel;

        public virtual void Update()
		{
		}

		public virtual void Init(HeroEntity heroModel)
		{
			this.heroModel = heroModel;
		}

		public virtual void CastDamage()
		{
		}

		public virtual void CastDamage(EnemyData enemy)
		{
		}

		public virtual float GetCooldownTime()
		{
			return 0f;
		}

		public virtual string GetUseType()
		{
			return string.Empty;
		}

		public virtual void OnHeroReturnPool()
		{
		}

		public virtual bool IsEmptySpecialState()
		{
			return !(heroModel.GetFsmController().GetCurrentState() is CharacterSpecialState);
		}
	}
}
