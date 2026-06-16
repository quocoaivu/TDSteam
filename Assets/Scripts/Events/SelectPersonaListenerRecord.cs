using System;

public class SelectPersonaListenerRecord : IListenerRecord
{
    private GameSignalCenter.SelectCharacterMethod method;

    public SelectPersonaListenerRecord(int subscriberId, GameSignalCenter.SelectCharacterMethod method)
	{
		this.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method((int)gameEventData);
	}
}
