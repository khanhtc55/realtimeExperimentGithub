//using nFury.Main.Controllers;
using nFury.Utils.Core;
using System;
using UnityEngine;
namespace nFury.Externals.EnvironmentManager
{
	public class EnvironmentController
	{
		private IEnvironmentManager environmentManager;
		public EnvironmentController()
		{
			Service.Set<EnvironmentController>(this);
#if UNITY_ANDROID
			this.environmentManager = new AndroidEnvironmentManager();
#else
			this.environmentManager = new IOSEnvironmentManager();
#endif
			this.environmentManager.Init();
		}
		public string GetLocale()
		{
			string text = this.environmentManager.GetLocale();
			if (string.IsNullOrEmpty(text))
			{
				text = "en_US";
			}
			return this.GetBIAppropriateLocale(text);
		}
		public string GetCurrencyCode()
		{
			return this.environmentManager.GetCurrencyCode();
		}
		public string GetDeviceCountryCode()
		{
			string locale = this.environmentManager.GetLocale();
			string[] array = locale.Split(new char[]
			{
				'_'
			});
			string result = "US";
			if (array.Length > 1)
			{
				result = array[1];
			}
			return result;
		}
		private string GetBIAppropriateLocale(string deviceLocale)
		{
			string text = deviceLocale.Substring(0, 2);
			string text2 = text;
			string result;
			switch (text2)
			{
			case "de":
				result = "de_DE";
				return result;
			case "en":
				result = "en_US";
				return result;
			case "es":
				result = "es_LA";
				return result;
			case "fr":
				result = "fr_FR";
				return result;
			case "it":
				result = "it_IT";
				return result;
			case "ja":
				result = "ja_JP";
				return result;
			case "ko":
				result = "ko_KR";
				return result;
			case "pt":
				result = "pt_BR";
				return result;
			case "ru":
				result = "ru_RU";
				return result;
			case "zh":
				if (deviceLocale.Equals("zh_CN") || deviceLocale.Equals("zh_SG"))
				{
					result = "zh_CN";
				}
				else
				{
					result = "zh_TW";
				}
				return result;
			}
			result = "en_US";
			return result;
		}
		public string GetDeviceId()
		{
			return this.environmentManager.GetDeviceId();
		}
		public string GetMachine()
		{
			return this.environmentManager.GetMachine();
		}
		public string GetModel()
		{
			return this.environmentManager.GetModel();
		}
		public bool IsMusicPlaying()
		{
			return this.environmentManager.IsMusicPlaying();
		}
		public bool AreHeadphonesConnected()
		{
			return this.environmentManager.AreHeadphonesConnected();
		}
		public bool IsRestrictedProfile()
		{
			return this.environmentManager.IsRestrictedProfile();
		}
		public double GetTimezoneOffset()
		{
			TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
			return currentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
		}
		public int GetTimezoneOffsetSeconds()
		{
			return (int)(this.GetTimezoneOffset() * 3600.0);
		}
		public void GainAudioFocus()
		{
			this.environmentManager.GainAudioFocus();
		}
		public void SetupAutoRotation()
		{
			if (!this.environmentManager.IsAutoRotationEnabled())
			{
				Screen.orientation = ScreenOrientation.LandscapeLeft;
			}
			else
			{
				Screen.orientation = ScreenOrientation.AutoRotation;
			}
		}
		public void ShowAlert(string titleText, string messageText, string yesButtonText)
		{
			this.ShowAlert(titleText, messageText, yesButtonText, string.Empty);
		}
		public void ShowAlert(string titleText, string messageText, string yesButtonText, string noButtonText)
		{
//			if (Service.IsSet<GameIdleController>())
//			{
//				Service.Get<GameIdleController>().enabled = false;
//			}
			this.environmentManager.ShowAlert(titleText, messageText, yesButtonText, noButtonText);
		}
	}
}
