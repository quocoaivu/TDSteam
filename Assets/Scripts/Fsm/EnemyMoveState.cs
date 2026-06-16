using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public class EnemyMoveState : EnemyState
{
    private bool haveRestOnTheWay;

    private float delayRest;

    private float countdownToRest;

    private bool isRanger;

    private float delayFindTarget = 0.12f;

    private float countdownFindTarget;

    private float rangerRange;

    private float sqRangerRange;

    private Vector3 invertXVector = new Vector3(-1f, 1f, 1f);

    public EnemyMoveState(EnemyData enemyModel, IFsmController fSMController) : base(enemyModel, fSMController)
	{
		haveRestOnTheWay = enemyModel.EnemyMovementController.HaveRestOnTheWay;
		delayRest = enemyModel.EnemyMovementController.DelayToRest;
		countdownToRest = delayRest;
		if (enemyModel.enemyAttackController)
		{
			isRanger = enemyModel.enemyAttackController.rangeAttack;
		}
		if (isRanger)
		{
			countdownFindTarget = delayFindTarget;
			rangerRange = enemyModel.enemyAttackController.GetRangerAtkRange();
			sqRangerRange = rangerRange * rangerRange;
		}
		LineRecord line = LineDirector.Current.GetLine(enemyModel.Gate, 0);
		enemyModel.transform.position = line.Position;
	}

	public override void OnStartState()
	{
		base.OnStartState();
		if (enemyModel.monsterPathData == null)
		{
			int moveLine = enemyModel.moveLine;
			int lineIndex;
			if (enemyModel.OriginalParameter.isBoss)
			{
				lineIndex = LineDirector.Current.GetLineIndex(enemyModel.Gate, 0);
				enemyModel.EnemyMovementController.currentLine = 0;
			}
			else
			{
				lineIndex = LineDirector.Current.GetLineIndex(enemyModel.Gate, moveLine);
				enemyModel.EnemyMovementController.currentLine = moveLine;
			}
			enemyModel.monsterPathData = new CreepPathRecord(lineIndex, new Action(OnMoveToEndPoint));
		}
		if (GameKit.IsUnderTargetOfAnyHero(enemyModel, false, -1))
		{
			base.SetTransition(EntityPhaseEnum.EnemyWaitForAtk);
			return;
		}
		countdownFindTarget = 0f;
		enemyModel.EnemyAnimationController.ToRunState(EnemyAnimation.animRunRight);
	}

	public override void OnInput(PhaseInputKind inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		switch (inputType)
		{
		case PhaseInputKind.SetEnemyIdleWaitForMeleeAtk:
		{
			CharacterEntity target = (CharacterEntity)args[0];
			if (enemyModel.EnemyFindTargetController != null)
			{
				enemyModel.EnemyFindTargetController.Target = target;
			}
			base.SetTransition(EntityPhaseEnum.EnemyWaitForAtk);
			break;
		}
		case PhaseInputKind.Die:
			enemyModel.monsterPathData = null;
			break;
		}
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		if (haveRestOnTheWay)
		{
			countdownToRest -= dt;
			if (countdownToRest <= 0f)
			{
				countdownToRest += delayRest;
				base.SetTransition(EntityPhaseEnum.EnemyRest);
			}
		}
		if (isRanger && !enemyModel.IsInTunnel)
		{
			countdownFindTarget -= dt;
			if (countdownFindTarget <= 0f)
			{
				countdownFindTarget = delayFindTarget;
				CharacterEntity characterModel = FindTarget();
				if (characterModel != null)
				{
					enemyModel.EnemyFindTargetController.Target = characterModel;
					base.SetTransition(EntityPhaseEnum.EnemyRangeAtk);
				}
			}
		}
		LineDirector.Current.RequestMove(enemyModel, enemyModel.monsterPathData, enemyModel.EnemyMovementController.Speed * dt, false, 0f);
		ChangeAnimationRun();
	}

	public void OnMoveToEndPoint()
	{
		GameSignalCenter.Instance.Trigger(GameSignalKind.OnEnemyMoveToEndPoint, enemyModel);
	}

	public CharacterEntity FindTarget()
	{
		List<CharacterEntity> listActiveAlly = MonoSingleton<GameRecord>.Instance.ListActiveAlly;
		for (int i = listActiveAlly.Count - 1; i >= 0; i--)
		{
			if (GameKit.IsValidCharacter(listActiveAlly[i]) && (listActiveAlly[i].transform.position - enemyModel.transform.position).sqrMagnitude <= sqRangerRange && GameKit.IsCharacterVisible(listActiveAlly[i]))
			{
				return listActiveAlly[i];
			}
		}
		return null;
	}

	private void ChangeAnimationRun()
	{
		Vector3 from = enemyModel.transform.position - enemyModel.CachedPosition;
		if (enemyModel.IsUnderground)
		{
			enemyModel.EnemyAnimationController.ToRunState(EnemyAnimation.animRunUnderground);
			return;
		}
		float num = Vector3.Angle(from, Vector3.right);
		if ((num > 0f && num < 60f) || (num > 300f && num < 360f))
		{
			enemyModel.EnemyAnimationController.ToRunState(EnemyAnimation.animRunRight);
			enemyModel.transform.localScale = Vector3.one;
		}
		else if (num > 120f && num < 240f)
		{
			enemyModel.EnemyAnimationController.ToRunState(EnemyAnimation.animRunRight);
			enemyModel.transform.localScale = invertXVector;
		}
		else if (num > 60f && num < 120f && from.y > 0f)
		{
			enemyModel.EnemyAnimationController.ToRunState(EnemyAnimation.animRunUp);
		}
		else if (num > 60f && num < 120f && from.y < 0f)
		{
			enemyModel.EnemyAnimationController.ToRunState(EnemyAnimation.animRunDown);
		}
	}
}
