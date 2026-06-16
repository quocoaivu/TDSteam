using System;
using System.Collections.Generic;

namespace Common
{
	public abstract class MultiPrototypesPool<T> : IMultiPrototypesPool<T>, IPoolBase<T>
	{

        private Dictionary<int, T> prototypesDictionary = new Dictionary<int, T>();

        private Dictionary<int, List<T>> instancesDictionary = new Dictionary<int, List<T>>();

        public bool ContainsPrototype(int prototypeId)
		{
			return prototypesDictionary.ContainsKey(prototypeId);
		}

		public void PushInstance<U>(U memberInstance) where U : IMultiPrototypesPoolMember<T>, T
		{
			int prototypeId = memberInstance.PrototypeId;
			memberInstance.Pool = this;
			memberInstance.InPool = true;
			instancesDictionary[prototypeId].Add((T)((object)memberInstance));
		}

		public void PushPrototype<U>(U memberPrototype) where U : IMultiPrototypesPoolMember<T>, T
		{
			int prototypeId = memberPrototype.PrototypeId;
			prototypesDictionary.Add(prototypeId, (T)((object)memberPrototype));
			instancesDictionary.Add(prototypeId, new List<T>());
		}

		public T TakeInstance(int prototypeId, bool forceCloning)
		{
			if (instancesDictionary[prototypeId].Count > 0)
			{
				return TakeInstanceAvailableInDictionary(prototypeId);
			}
			if (forceCloning)
			{
				return TakeInstanceByClonning(prototypeId);
			}
			throw new Exception("Pool memebers are not available to take");
		}

		public T TakeInstanceByPrototype<U>(U prototype) where U : IMultiPrototypesPoolMember<T>, T
		{
			int prototypeId = prototype.PrototypeId;
			if (!ContainsPrototype(prototypeId))
			{
				PushPrototype<U>(prototype);
			}
			return TakeInstance(prototypeId, true);
		}

		private T TakeInstanceAvailableInDictionary(int prototypeId)
		{
			List<T> list = instancesDictionary[prototypeId];
			int index = list.Count - 1;
			T t = list[index];
			(t as IMultiPrototypesPoolMember<T>).InPool = false;
			list.RemoveAt(index);
			return t;
		}

		private T TakeInstanceByClonning(int prototypeId)
		{
			T t = (prototypesDictionary[prototypeId] as ICloneable<T>).Clone();
			(t as IMultiPrototypesPoolMember<T>).Pool = this;
			(t as IMultiPrototypesPoolMember<T>).InPool = false;
			return t;
		}
	}
}
