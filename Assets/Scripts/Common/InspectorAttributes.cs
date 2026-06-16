using System;

namespace Common
{
	[AttributeUsage(AttributeTargets.Field)]
	public class InspectorCommandAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Field)]
	public class ReadOnlyTrait : Attribute
	{
	}
}
