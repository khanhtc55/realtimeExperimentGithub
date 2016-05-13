using System;
using UnityEngine;
namespace nFury.Utils
{
	public class DateUtils
	{
		public const int SECONDS_IN_HOUR = 3600;
		private static readonly DateTime UnixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		public static DateTime GetDefaultDate()
		{
			return default(DateTime);
		}
		public static bool IsDefaultDate(DateTime date)
		{
			return DateUtils.GetDefaultDate().Equals(date);
		}
		public static DateTime DateFromMillis(long millis)
		{
			return new DateTime(DateUtils.UnixStart.Ticks + millis * 10000L);
		}
		public static float GetRealTimeSinceStartUpInMilliseconds()
		{
			return Mathf.Round(UnityUtils.GetRealTimeSinceStartUp() * 1000f);
		}
		public static int GetSecondsFromEpoch(DateTime date)
		{
			return (int)(date - DateUtils.UnixStart).TotalSeconds;
		}
	}
}
