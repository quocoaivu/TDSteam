using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class BuffHolder : MonoBehaviour
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event BuffValueChangedHandler OnBuffValueChanged;


        private Dictionary<string, BuffStatus> buffs = new Dictionary<string, BuffStatus>();

        private Dictionary<string, BuffStatus> currentBuffs = new Dictionary<string, BuffStatus>();

        private List<string> keysToRemove = new List<string>();


        private List<string> keysList = new List<string>();
        public bool ContainsBuffKey(string buffKey)
		{
			return buffs.ContainsKey(buffKey);
		}

		public bool TryGetBuffValue(string buffKey, out float buffValue)
		{
			BuffStatus buff;
			if (buffs.TryGetValue(buffKey, out buff))
			{
				buffValue = buff.Value;
				return true;
			}
			buffValue = 0f;
			return false;
		}

		public float GetBuffsValue(List<string> buffKeys)
		{
			float num = 0f;
			for (int i = 0; i < buffKeys.Count; i++)
			{
				BuffStatus buff;
				if (buffs.TryGetValue(buffKeys[i], out buff))
				{
					num += buff.Value;
				}
			}
			return num;
		}

		public void AddBuff(string buffKey, BuffStatus buff, BuffStackRule valueStackLogic, BuffStackRule timeStackLogic)
		{
			BuffStatus value;
			if (buffs.TryGetValue(buffKey, out value))
			{
				value.Value = BuffStackRuleAide.GetStackedValue(valueStackLogic, value.Value, buff.Value);
				value.Duration = BuffStackRuleAide.GetStackedValue(timeStackLogic, value.Duration, buff.Duration);
			}
			else
			{
				value = buff;
			}
			buffs[buffKey] = value;
			if (!keysList.Contains(buffKey))
			{
				keysList.Add(buffKey);
			}
			if (OnBuffValueChanged != null)
			{
				OnBuffValueChanged(buffKey, true);
			}
		}

		public void ResetBuffs()
		{
			buffs.Clear();
			keysToRemove.Clear();
			keysList.Clear();
		}

		public void RemoveBuffs(IEnumerable<string> buffKeys)
		{
			_RemoveBuffs(buffKeys);
		}

		public void RemoveBuffs(params string[] buffKeys)
		{
			_RemoveBuffs(buffKeys);
		}

		public void RemoveBuffs(bool isPositive)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, BuffStatus> keyValuePair in buffs)
			{
				if (keyValuePair.Value.IsPositive == isPositive)
				{
					list.Add(keyValuePair.Key);
				}
			}
			RemoveBuffs(list);
		}

		public void CopyBuff(BuffHolder targetBuffsHolder)
		{
			targetBuffsHolder.ResetBuffs();
			foreach (KeyValuePair<string, BuffStatus> keyValuePair in buffs)
			{
				targetBuffsHolder.AddBuff(keyValuePair.Key, keyValuePair.Value, BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
			}
		}

		public void Update()
		{
			UpdateBuffsTime();
			RemoveTimedOutBuffs();
		}

		private void _RemoveBuffs(IEnumerable<string> buffKeys)
		{
			foreach (string text in buffKeys)
			{
				buffs.Remove(text);
				keysList.Remove(text);
				if (OnBuffValueChanged != null)
				{
					OnBuffValueChanged(text, false);
				}
			}
		}

		private void UpdateBuffsTime()
		{
			if (buffs.Count == 0)
			{
				return;
			}
			for (int i = 0; i < keysList.Count; i++)
			{
				string text = keysList[i];
				BuffStatus value = buffs[text];
				if (value.Duration <= 0f)
				{
					keysToRemove.Add(text);
				}
				else
				{
					value.Duration -= Time.deltaTime;
				}
				buffs[text] = value;
			}
		}

		private void RemoveTimedOutBuffs()
		{
			if (keysToRemove.Count == 0)
			{
				return;
			}
			RemoveBuffs(keysToRemove);
			keysToRemove.Clear();
		}

	}
}
