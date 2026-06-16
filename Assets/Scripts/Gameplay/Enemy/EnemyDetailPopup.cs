using System;
using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDetailPopup : MonoBehaviour
{
	public void Init()
	{
		GameSignalCenter.Instance.Subscribe(GameSignalKind.OnSelectEnemy, new SelectPersonaListenerRecord(GameKit.GetUniqueId(), new GameSignalCenter.SelectCharacterMethod(SetInformation)));
	}

	public void SetInformation(int enemyId)
	{
		if (enemyId < 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		enemyAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("Preview/Enemies/p_enemy_{0}", enemyId));
		enemyAvatar.SetNativeSize();
		bool flag = EnemyDatabase.Instance.isPhysicsAttack(enemyId);
		physicsDamage.SetActive(flag);
		magicDamage.SetActive(!flag);
		damageText.text = EnemyDatabase.Instance.GetMinDamage(enemyId, 1) + "-" + EnemyDatabase.Instance.GetMaxDamage(enemyId, 1);
		healthText.text = EnemyDatabase.Instance.GetHealth(enemyId, 1).ToString();
		bool flag2 = EnemyDatabase.Instance.GetMagicArmor(enemyId, 1) != 0;
		physicsArmor.SetActive(!flag2);
		magicArmor.SetActive(flag2);
		armorText.text = AbilityRankDescriber.Instance.GetArmorDescriptionByValue(flag2 ? EnemyDatabase.Instance.GetMagicArmor(enemyId, 1) : EnemyDatabase.Instance.GetPhysicsArmor(enemyId, 1));
		lifeTakenText.text = EnemyDatabase.Instance.GetLifeTaken(enemyId, 1).ToString();
	}

	public Image enemyAvatar;

	public Text armorText;

	public Text damageText;

	public Text healthText;

	public Text lifeTakenText;

	public GameObject physicsDamage;

	public GameObject magicDamage;

	public GameObject physicsArmor;

	public GameObject magicArmor;

	private bool isInited;
}
