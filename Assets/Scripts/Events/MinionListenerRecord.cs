using System;
using Gameplay;

public class MinionListenerRecord : IListenerRecord
{
    private GameSignalCenter.AllySubscribeMethod method;

    public MinionListenerRecord(int subscriberId, GameSignalCenter.AllySubscribeMethod method)
	{
		this.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method((MinionEntity)gameEventData);
	}

}
