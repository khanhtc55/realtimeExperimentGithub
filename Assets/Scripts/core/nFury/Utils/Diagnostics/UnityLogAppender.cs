using System;
using UnityEngine;
namespace nFury.Utils.Diagnostics
{
	public class UnityLogAppender : BaseLogAppender
	{
		protected override void Trace(string message)
		{
			LogLevel level = this.Entry.Level;
			if (level != LogLevel.Error)
			{
				if (level != LogLevel.Warn)
				{
					Debug.Log(message);
				}
				else
				{
					Debug.LogWarning(message);
				}
			}
			else
			{
				Debug.LogError(message);
			}
		}
	}
}
