using System;

public class SignalTriggerRecord
{
    public SignalTriggerKind triggerType;

    public int triggerValue;

    public int addedQuantity;

    public bool forceSaveProgress;

    public SignalTriggerRecord(SignalTriggerKind triggerType, int addedQuantity, bool forceSaveProgress = false)
	{
		this.triggerType = triggerType;
		this.addedQuantity = addedQuantity;
		this.forceSaveProgress = forceSaveProgress;
	}

	public SignalTriggerRecord(SignalTriggerKind triggerType, int triggerValue, int addedQuantity, bool forceSaveProgress = false) : this(triggerType, addedQuantity, forceSaveProgress)
	{
		this.triggerValue = triggerValue;
	}
}
