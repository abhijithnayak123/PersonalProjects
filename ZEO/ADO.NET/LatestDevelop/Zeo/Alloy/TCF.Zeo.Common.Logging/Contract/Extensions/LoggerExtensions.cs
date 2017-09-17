using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MGI.Common.Logging.Contract;
using MGI.Common.Logging.Data;

namespace MGI.Common.Logging.Contract
{
	public static class LoggerExtensions
	{
		public static void Log( this ILogger logger, string message )
		{
			Log( logger, message, null );
		}

		public static void Log( this ILogger logger, string message, params object[] parameters )
		{
			logger.Log( new LogEntry { Level = LogEntryLevel.Info, Message = message, Parameters = parameters } );
		}

		public static void Log( this ILogger logger, Exception ex )
		{
			logger.Log( new LogEntry { Level = LogEntryLevel.Error, Message=ex.Message, Exception = ex } );
		}

		public static void Log( this ILogger logger, Exception ex, string message )
		{
			logger.Log( new LogEntry { Level = LogEntryLevel.Error, Message = message, Exception = ex } );
		}

		public static void Debug( this ILogger logger, string message, params object[] parameters )
		{
			logger.Log( new LogEntry { Level = LogEntryLevel.Debug, Message = message, Parameters = parameters } );
		}

		public static void Warn( this ILogger logger, string message )
		{
			logger.Log( new LogEntry { Level = LogEntryLevel.Warn, Message = message } );
		}

		public static void Error( this ILogger logger, string message, params object[] parameters )
		{
			logger.Log( new LogEntry { Level = LogEntryLevel.Error, Message = message, Parameters = parameters } );
		}
	}
}
