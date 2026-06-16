using System;

public class SignalTriggerListenerRecord : IListenerRecord
{
    private GameSignalCenter.EventTriggerMethod method;

    public SignalTriggerListenerRecord(int subscriberId, GameSignalCenter.EventTriggerMethod method)
	{
		this.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method((SignalTriggerRecord)gameEventData);
	}
}
