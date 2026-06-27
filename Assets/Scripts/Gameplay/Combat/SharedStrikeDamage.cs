using System;
using Gameplay;

public class SharedStrikeDamage
{
	public SharedStrikeDamage()
	{
	}

	public SharedStrikeDamage(MarkKind targetType, float aoeRange = 0f)
	{
		this.targetType = targetType;
		this.aoeRange = aoeRange;
	}

	public SharedStrikeDamage(int physicsDamage, int magicDamage, float aoeRange = 0f)
	{
		this.physicsDamage = physicsDamage;
		this.magicDamage = magicDamage;
		this.aoeRange = aoeRange;
	}

	public SharedStrikeDamage(int physicsDamage, int magicDamage, float aoeRange, bool isIgnoreArmor, bool isNotPlayIgnoreArmorEffect)
	{
		this.physicsDamage = physicsDamage;
		this.magicDamage = magicDamage;
		this.aoeRange = aoeRange;
		this.isIgnoreArmor = isIgnoreArmor;
		this.isNotPlayIgnoreArmorEffect = isNotPlayIgnoreArmorEffect;
	}

	public SharedStrikeDamage(int physicsDamage, int magicDamage, bool isTargetAir, float aoeRange = 0f)
	{
		this.physicsDamage = physicsDamage;
		this.magicDamage = magicDamage;
		targetType.isAir = isTargetAir;
		this.aoeRange = aoeRange;
	}

	public SharedStrikeDamage(int physicsDamage, int magicDamage, float aoeRange, int criticalStrikeChance, int ignoreArmorChance) : this(physicsDamage, magicDamage, aoeRange)
	{
		this.criticalStrikeChance = criticalStrikeChance;
		this.ignoreArmorChance = ignoreArmorChance;
		isCrit = false;
		isIgnoreArmor = false;
	}

	public SharedStrikeDamage(CharacterKind damageSource, int sourceId, int physicsDamage, int magicDamage, float aoeRange, int criticalStrikeChance, int ignoreArmorChance) : this(physicsDamage, magicDamage, aoeRange, criticalStrikeChance, ignoreArmorChance)
	{
		this.damageSource = damageSource;
		this.sourceId = sourceId;
	}

	public MarkKind targetType;

	public int physicsDamage;

	public int magicDamage;

	public int criticalStrikeChance;

	public float critMultiplier = 2f; // applied when isCrit is true; default 2x matches old behaviour

	public float projectileSpeed; // world units/s; 0 = use prefab default

	public int pierceCount; // number of enemies the projectile passes through (0 = single target)

	public bool isCrit;

	public int instantKillChance;

	public bool isInstantKill;

	public int ignoreArmorChance;

	public bool isIgnoreArmor;

	public int magicPenetration; // % of enemy magic resistance ignored on this hit

	public bool isNotPlayIgnoreArmorEffect;

	public float cooldown;

	public float aoeRange;

	public int aoeDamageFalloff; // % damage reduction from blast center to edge (0 = flat)

	public float maxRange;

	public int bulletDirection;

	public CharacterKind damageSource;

	public int sourceId;

	public int targetInstanceId;

	public EnemyData targetEnemyModel;
}
