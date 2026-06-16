using System;
using Gameplay;
using UnityEngine;

public class PetStayAroundPhase : CharacterState
{
    private IEntityState curSubState;

    private IEntityState followState;

    private IEntityState idleState;

    private HeroEntity petModel;

    private float minDelayChangeSpot = 3.5f;

    private float maxDelayChangeSpot = 7f;

    private float disToOwner = 0.8f;

    private float countdown;

    private float countdownFindTarget;

    public Vector3 ownerToIdleSpotOffset;

    public PetStayAroundPhase(CharacterEntity character, IFsmController fSMController) : base(character, fSMController)
	{
		followState = new PetFollowPhase(character, fSMController, this);
		idleState = new PetIdlePhase(character, fSMController, this);
		petModel = (character as HeroEntity);
		ownerToIdleSpotOffset = GetRandomPosAroundOwner();
	}

	public override void OnStartState()
	{
		base.OnStartState();
		countdown = UnityEngine.Random.Range(minDelayChangeSpot, maxDelayChangeSpot);
		curSubState = null;
		SetSubState(idleState);
		character.AddTarget(null);
		if (petModel.PetConfigData.Atk_magic_min > 0 || petModel.PetConfigData.Atk_physics_min > 0)
		{
			countdownFindTarget = 0f;
		}
		else
		{
			countdownFindTarget = 1E+09f;
		}
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		countdown -= dt;
		if (countdown <= 0f)
		{
			countdown = UnityEngine.Random.Range(minDelayChangeSpot, maxDelayChangeSpot);
			ownerToIdleSpotOffset = GetRandomPosAroundOwner();
		}
		curSubState.Update(dt);
		countdownFindTarget -= dt;
		if (countdownFindTarget <= 0f)
		{
			countdownFindTarget = 1f;
			if (!IsOwnerMoving())
			{
				EnemyData enemyModel = base.FindTargetInRange();
				if (enemyModel != null)
				{
					OnInput(PhaseInputKind.MonsterInAtkRange, new object[]
					{
						enemyModel
					});
				}
			}
		}
	}

	public void SetSubState(IEntityState state)
	{
		if (curSubState == state)
		{
			return;
		}
		if (curSubState != null)
		{
			curSubState.OnExitState();
		}
		curSubState = state;
		curSubState.OnStartState();
	}

	public override void OnInput(PhaseInputKind inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType != PhaseInputKind.PetReachFollowSpot)
		{
			if (inputType != PhaseInputKind.PetFarFromOwner)
			{
				if (inputType == PhaseInputKind.MonsterInAtkRange)
				{
					if (!IsOwnerMoving())
					{
						EnemyData enemy = (EnemyData)args[0];
						if (character.IsInMeleeRange(enemy))
						{
							character.AddTarget(enemy);
							base.SetTransition(EntityPhaseEnum.CharacterMoveToTarget);
						}
						else if (character.IsInRangerRange(enemy))
						{
							character.AddTarget(enemy);
							base.SetTransition(EntityPhaseEnum.CharacterRangeAtk);
						}
					}
				}
			}
			else
			{
				SetSubState(followState);
			}
		}
		else
		{
			SetSubState(idleState);
		}
	}

	public Vector3 GetRandomPosAroundOwner()
	{
		Vector2 vector = UnityEngine.Random.insideUnitCircle;
		vector = vector / vector.magnitude * disToOwner;
		return vector;
	}

	public bool IsOwnerMoving()
	{
		return petModel.PetOwner.IsAlive && petModel.PetOwner.GetFsmController().GetCurrentState() is CharacterMoveState;
	}
}
