
using System;
namespace nFury.Externals.EnvironmentManager
{
	public class IOSEnvironmentManager : IEnvironmentManager
	{
		public IOSEnvironmentManager ()
		{
		}

		public void Init ()
		{

		}
		public string GetDeviceId ()
		{
			return "-1";
		}
		public string GetLocale ()
		{
			return "-1";
		}
		public string GetCurrencyCode ()
		{
			return "-1";
		}
		public bool IsAutoRotationEnabled ()
		{
			return true;
		}
		public bool IsRestrictedProfile ()
		{
			return true;
		}
		public bool IsMusicPlaying ()
		{
			return true;
		}
		public bool AreHeadphonesConnected ()
		{
			return true;
		}
		public string GetMachine ()
		{
			return "-1";
		}
		public string GetModel ()
		{
			return "-1";
		}
		public void GainAudioFocus ()
		{

		}
		public void ShowAlert (string titleText, string messageText, string yesButtonText, string noButtonText)
		{

		}
	}
}

