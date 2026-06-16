using System;

public abstract class IListenerRecord
{
	public abstract void OnEventTrigger(object gameEventData);

	public int subscriberId;
}
