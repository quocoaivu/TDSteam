using System;

public class TapSwitchListenerRecord : IListenerRecord
{
    private GameSignalCenter.ClickButtonMethod method;


    public TapSwitchListenerRecord(int subscriberId, GameSignalCenter.ClickButtonMethod method)
	{
		this.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method((TappedObjectRecord)gameEventData);
	}
}
