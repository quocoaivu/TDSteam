using System;
using System.Collections.Generic;

public class EntityState : IEntityState
{
    private IFsmController fsmController;

    private Dictionary<EntityPhaseEnum, IEntityState> targetEnumToTargetState = new Dictionary<EntityPhaseEnum, IEntityState>();

    public EntityState(IFsmController fsmController)
	{
		this.fsmController = fsmController;
	}

	public EntityPhaseEnum entityStateEnum { get; set; }

	public void InitTransition(EntityPhaseEnum stateEnum, IEntityState state)
	{
		if (targetEnumToTargetState.ContainsKey(stateEnum))
		{
			return;
		}
		targetEnumToTargetState.Add(stateEnum, state);
	}

	public void SetTransition(EntityPhaseEnum stateEnum)
	{
		if (!targetEnumToTargetState.ContainsKey(stateEnum))
		{
#if UNITY_EDITOR
			UnityEngine.Debug.LogWarning($"FSM: no transition from {entityStateEnum} to {stateEnum} (missing CreateTransition?)");
#endif
			return;
		}
		// Re-entrancy guard: OnStartState (run inside SetCurrentState) may trigger another
		// SetTransition synchronously, so transitions can nest. Only the state that is
		// currently active may drive a transition; a state already switched away from must
		// not move the FSM. Do not remove this check.
		if (fsmController.GetCurrentState() != this)
		{
			return;
		}
		fsmController.SetCurrentState(targetEnumToTargetState[stateEnum]);
	}

	public virtual void Update(float dt)
	{
	}

	public virtual void OnInput(PhaseInputKind inputType, params object[] args)
	{
	}

	public virtual void OnStartState()
	{
	}

	public virtual void OnExitState()
	{
	}
}
