using System;
using Gameplay;

public class EnemyEventData : IListenerRecord
{
    private GameSignalCenter.EnemySubscribeMethod method;

    public EnemyEventData(int subscriberId, GameSignalCenter.EnemySubscribeMethod method)
	{
		this.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method((EnemyData)gameEventData);
	}
}
