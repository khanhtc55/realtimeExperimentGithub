using System;
namespace nFury.Utils.Diagnostics
{
	public abstract class BaseLogAppender : ILogAppender
	{
		protected LogEntry Entry;
		protected abstract void Trace(string formattedMessage);
		public void AddLogMessage(LogEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.Entry = entry;
			this.Trace(string.Concat(new object[]
			{
				entry.Timestamp,
				" ",
				Enum.GetName(typeof(LogLevel), entry.Level),
				": ",
				entry.Message
			}));
		}
	}
}
