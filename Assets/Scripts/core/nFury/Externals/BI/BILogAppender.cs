using nFury.Utils.Core;
using nFury.Utils.Diagnostics;

using System;
namespace nFury.Externals.BI
{
	public class BILogAppender : ILogAppender
	{
		public void AddLogMessage(LogEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			if (entry.Level == LogLevel.Debug)
			{
				return;
			}
			if (Service.IsSet<BILoggingController>())
			{
				Service.Get<BILoggingController>().TrackError(entry.Level, entry.Message);
			}
		}
	}

    public class BILoggingController
    {
        public void TrackError(LogLevel level, string message)
        {
            


        }

        public void TrackPerformance(float biFps, float memoryUsed)
        {
            throw new NotImplementedException();
        }
    }
}
