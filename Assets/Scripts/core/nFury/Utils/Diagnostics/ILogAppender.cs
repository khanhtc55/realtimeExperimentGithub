using System;
namespace nFury.Utils.Diagnostics
{
	public interface ILogAppender
	{
		void AddLogMessage(LogEntry logEntry);
	}
}
