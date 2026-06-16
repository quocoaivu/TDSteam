using System;
using UnityEngine;

namespace Gameplay
{
	public static class BuffStackRuleAide
	{
		public static float GetStackedValue(BuffStackRule stackLogic, float oldValue, float newValue)
		{
			switch (stackLogic)
			{
			case BuffStackRule.StackUp:
				return oldValue + newValue;
			case BuffStackRule.ChooseMin:
				return Mathf.Min(oldValue, newValue);
			case BuffStackRule.ChooseMax:
				return Mathf.Max(oldValue, newValue);
			case BuffStackRule.ChooseNew:
				return newValue;
			default:
				throw new NotSupportedException();
			}
		}
	}
}
