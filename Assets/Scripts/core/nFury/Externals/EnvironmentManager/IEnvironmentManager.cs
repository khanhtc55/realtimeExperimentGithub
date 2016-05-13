using System;
namespace nFury.Externals.EnvironmentManager
{
	public interface IEnvironmentManager
	{
		void Init();
		string GetDeviceId();
		string GetLocale();
		string GetCurrencyCode();
		bool IsAutoRotationEnabled();
		bool IsRestrictedProfile();
		bool IsMusicPlaying();
		bool AreHeadphonesConnected();
		string GetMachine();
		string GetModel();
		void GainAudioFocus();
		void ShowAlert(string titleText, string messageText, string yesButtonText, string noButtonText);
	}
}
