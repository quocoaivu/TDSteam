using System;

public class DamageDetailListenerRecord : IListenerRecord
{
    private GameSignalCenter.DamageInfoMethod method;

    public DamageDetailListenerRecord(int subscriberId, GameSignalCenter.DamageInfoMethod method)
	{
		this.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method((SharedStrikeDamage)gameEventData);
	}
}
