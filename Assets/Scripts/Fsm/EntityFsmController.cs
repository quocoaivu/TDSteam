using System;
using System.Collections.Generic;

public class EntityFsmController : IFsmController
{
    private readonly List<IEntityState> backgroundStates = new List<IEntityState>();

    private IEntityState currentState;

    private readonly Dictionary<EntityPhaseEnum, IEntityState> stateDictionary = new Dictionary<EntityPhaseEnum, IEntityState>();

    public void AddBackgroundState(IEntityState state)
	{
		backgroundStates.Add(state);
	}

	public void AddState(EntityPhaseEnum stateEnum, IEntityState state)
	{
		stateDictionary.Add(stateEnum, state);
		state.entityStateEnum = stateEnum;
	}

	public void OnUpdate(float dt)
	{
		GetCurrentState().Update(dt);
		if (backgroundStates.Count > 0)
		{
			for (int i = backgroundStates.Count - 1; i >= 0; i--)
			{
				backgroundStates[i].Update(dt);
			}
		}
	}

	public void CreateTransition(EntityPhaseEnum fromState, EntityPhaseEnum toState)
	{
		stateDictionary[fromState].InitTransition(toState, stateDictionary[toState]);
	}

	public void CreateTransitionFromAllState(EntityPhaseEnum toState, params EntityPhaseEnum[] exceptionStates)
	{
		foreach (KeyValuePair<EntityPhaseEnum, IEntityState> keyValuePair in stateDictionary)
		{
			if (keyValuePair.Key != toState && IsNotExceptionState(keyValuePair.Key, exceptionStates))
			{
				CreateTransition(keyValuePair.Key, toState);
			}
		}
	}

	private bool IsNotExceptionState(EntityPhaseEnum state, EntityPhaseEnum[] exceptionStates)
	{
		for (int i = 0; i < exceptionStates.Length; i++)
		{
			if (exceptionStates[i] == state)
			{
				return false;
			}
		}
		return true;
	}

	public IEntityState GetCurrentState()
	{
		return currentState;
	}

	public void SetCurrentState(IEntityState state)
	{
		// Exit the outgoing state here so every state change is symmetric,
		// regardless of whether it comes from SetTransition or initial setup.
		if (currentState != null)
		{
			currentState.OnExitState();
		}
		currentState = state;
		state.OnStartState();
	}

	protected void SetInitialState(EntityPhaseEnum stateEnum)
	{
		SetCurrentState(stateDictionary[stateEnum]);
	}
}
