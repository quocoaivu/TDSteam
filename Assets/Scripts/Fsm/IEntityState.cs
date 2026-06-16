using System;

public interface IEntityState
{
	void InitTransition(EntityPhaseEnum tostateEnum, IEntityState tostate);

	void Update(float dt);

	void SetTransition(EntityPhaseEnum stateEnum);

	void OnInput(PhaseInputKind inputType, params object[] args);

	void OnStartState();

	void OnExitState();

	EntityPhaseEnum entityStateEnum { get; set; }
}
