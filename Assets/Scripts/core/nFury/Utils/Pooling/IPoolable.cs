using System;
namespace nFury.Utils.Pooling
{
	public interface IPoolable
	{
		void Construct(IObjectPool objectPool);
		void Deactivate();
	}
}
