using System;
using System.Collections.Generic;
namespace nFury.Utils
{
	public class GenericObjectPool<T> where T : new()
	{
		private Stack<T> pool;
		public int Count
		{
			get
			{
				return this.pool.Count;
			}
		}
		public int CreationCount
		{
			get;
			protected set;
		}
		public GenericObjectPool()
		{
			this.pool = new Stack<T>();
			this.CreationCount = 0;
		}
		public void WarmUp(int n)
		{
			for (int i = 0; i < n; i++)
			{
				this.ReturnToPool(this.CreateNew());
			}
		}
		protected virtual T CreateNew()
		{
			this.CreationCount++;
			return (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
		}
		public virtual void ReturnToPool(T obj)
		{
			this.pool.Push(obj);
		}
		public virtual T GetFromPool(bool allowNewConstruction)
		{
			if (this.pool.Count != 0)
			{
				return this.pool.Pop();
			}
			if (allowNewConstruction)
			{
				return this.CreateNew();
			}
			return default(T);
		}
		public virtual T GetFromPool()
		{
			return this.GetFromPool(true);
		}
	}
}
