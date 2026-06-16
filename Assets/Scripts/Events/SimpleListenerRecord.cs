using System;

public class SimpleListenerRecord : IListenerRecord
{
    private GameSignalCenter.SimpleSubscribeMethod method;

    public SimpleListenerRecord(int subscriberId, GameSignalCenter.SimpleSubscribeMethod method)
	{
		this.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method();
	}
}
