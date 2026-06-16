using System;

public class TappedObjectRecord
{
    public TappedObjectKind clickedObjType;

    public int id;

    public TappedObjectRecord(TappedObjectKind clickedObjType)
	{
		this.clickedObjType = clickedObjType;
		id = -1;
	}

	public TappedObjectRecord(TappedObjectKind clickedObjType, int id)
	{
		this.clickedObjType = clickedObjType;
		this.id = id;
	}
}
