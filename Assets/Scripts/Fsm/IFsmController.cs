using System;

// Minimal contract a state needs to inspect and switch the FSM's current state.
// Kept intentionally small (interface segregation): states depend only on this,
// not on the concrete EntityFsmController where AddState / CreateTransition /
// OnUpdate live. Don't widen this interface to expose graph-building methods.
public interface IFsmController
{
	IEntityState GetCurrentState();

	void SetCurrentState(IEntityState state);
}
