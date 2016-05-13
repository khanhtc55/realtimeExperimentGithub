using System;
using UnityEngine;
namespace nFury.Main.Configs
{
	public static class HardwareProfile
	{
		public static bool IsLowEndDevice()
		{
			return true;
		}
		public static string GetDeviceModel()
		{
			return SystemInfo.deviceModel;
		}
	}
}
