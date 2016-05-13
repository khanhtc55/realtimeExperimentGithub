using System;
using UnityEngine;
namespace nFury.Main.Configs
{
	public static class PlayerSettings
	{
        private const string PREF_MUSIC_VOLUME = "prefMusicVolume";
        private const string PREF_SFX_VOLUME = "prefSfxVolume";
        private const string PREF_NOTIFICATIONS_LEVEL = "prefNotificationsLevel2";
        private const string PREF_LOCALE_COPY = "prefLocaleCopy";

		public const float DEFAULT_AUDIO_VOLUME = 1f;
		public const int DEFAULT_NOTIFICATIONS_LEVEL = 100;
		public static float GetMusicVolume()
		{
			return (!PlayerPrefs.HasKey(PREF_MUSIC_VOLUME)) ? 1f : PlayerPrefs.GetFloat(PREF_MUSIC_VOLUME);
		}
		public static void SetMusicVolume(float volume)
		{
			PlayerPrefs.SetFloat(PREF_MUSIC_VOLUME, volume);
		}
		public static float GetSfxVolume()
		{
			return (!PlayerPrefs.HasKey(PREF_SFX_VOLUME)) ? 1f : PlayerPrefs.GetFloat(PREF_SFX_VOLUME);
		}
		public static void SetSfxVolume(float volume)
		{
			PlayerPrefs.SetFloat(PREF_SFX_VOLUME, volume);
		}
		public static int GetNotificationsLevel()
		{
			return (!PlayerPrefs.HasKey(PREF_NOTIFICATIONS_LEVEL)) ? 0 : PlayerPrefs.GetInt(PREF_NOTIFICATIONS_LEVEL);
		}
		public static void SetNotificationsLevel(int level)
		{
			PlayerPrefs.SetInt(PREF_NOTIFICATIONS_LEVEL, level);
		}
		public static string GetLocaleCopy()
		{
			return (!PlayerPrefs.HasKey(PREF_LOCALE_COPY)) ? null : PlayerPrefs.GetString(PREF_LOCALE_COPY);
		}
		public static void SetLocaleCopy(string locale)
		{
			PlayerPrefs.SetString(PREF_LOCALE_COPY, locale);
		}
	}
}
