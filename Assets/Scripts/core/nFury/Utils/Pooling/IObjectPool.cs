using System;
namespace nFury.Utils.Pooling
{
	public interface IObjectPool
	{
		int Count
		{
			get;
		}
		int CreationCount
		{
			get;
		}
	}
}
