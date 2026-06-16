using System;
using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class EnemyOverviewHandler : MonoBehaviour
	{
		public void Init(int _enemyID)
		{
			SetBasicInformation(_enemyID);
			SetAbilityInformation(_enemyID);
		}

		private void SetFullAvatar(int _enemyID)
		{
			fullAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("Preview/Enemies/FullAvatars/fa_enemy_{0}", _enemyID));
		}

		private void SetBasicInformation(int _enemyID)
		{
			enemyName.text = Singleton<EnemyBioConfig>.Instance.GetEnemyName(_enemyID);
			description.text = Singleton<EnemyBioConfig>.Instance.GetEnemyDescription(_enemyID).Replace('@', '\n').Replace('#', '-');
			specialAbilityDescription.text = Singleton<EnemyBioConfig>.Instance.GetEnemySpecialAbility(_enemyID).Replace('@', '\n').Replace('#', '-');
		}

		private void SetAbilityInformation(int _enemyID)
		{
			health.text = EnemyDatabase.Instance.GetHealth(_enemyID, 1).ToString();
			damage.text = EnemyDatabase.Instance.GetMinDamage(_enemyID, 1) + "-" + EnemyDatabase.Instance.GetMaxDamage(_enemyID, 1);
			if (EnemyDatabase.Instance.isPhysicsAttack(_enemyID))
			{
				iconDamage.sprite = physicsDamageIcon;
			}
			else
			{
				iconDamage.sprite = magicDamageIcon;
			}
			physicsArmor.text = AbilityRankDescriber.Instance.GetArmorDescriptionByValue(EnemyDatabase.Instance.GetPhysicsArmor(_enemyID, 1));
			magicArmor.text = AbilityRankDescriber.Instance.GetArmorDescriptionByValue(EnemyDatabase.Instance.GetMagicArmor(_enemyID, 1));
			movementSpeed.text = AbilityRankDescriber.Instance.GetMoveSpeedDescriptionByValue(EnemyDatabase.Instance.GetSpeed(_enemyID, 1));
			lifeTaken.text = EnemyDatabase.Instance.GetLifeTaken(_enemyID, 1).ToString();
		}

		[SerializeField]
		private Image fullAvatar;

		[SerializeField]
		private Text enemyName;

		[SerializeField]
		private Text description;

		[SerializeField]
		private Text specialAbilityDescription;

		[SerializeField]
		private Text health;

		[SerializeField]
		private Text damage;

		[SerializeField]
		private Text physicsArmor;

		[SerializeField]
		private Text magicArmor;

		[SerializeField]
		private Text movementSpeed;

		[SerializeField]
		private Text lifeTaken;

		[Space]
		[SerializeField]
		private Image iconDamage;

		[SerializeField]
		private Sprite physicsDamageIcon;

		[SerializeField]
		private Sprite magicDamageIcon;
	}
}
