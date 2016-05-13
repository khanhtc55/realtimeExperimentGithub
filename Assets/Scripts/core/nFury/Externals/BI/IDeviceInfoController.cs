using System;
namespace nFury.Externals.BI
{
	public interface IDeviceInfoController
	{
		void AddDeviceSpecificInfo(BILog log);
		string GetDeviceId();
	}
}
