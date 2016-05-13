using System;
namespace nFury.Utils.Pooling
{
	public class ObjectPool<T> : GenericObjectPool<T>, IObjectPool where T : IPoolable, new()
	{
		protected override T CreateNew()
		{
			T result = base.CreateNew();
			result.Construct(this);
			return result;
		}
		public override void ReturnToPool(T obj)
		{
			obj.Deactivate();
			base.ReturnToPool(obj);
		}

	}
}
