using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Messaging;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace Nexxo.Channel.DMS.Server.EventsMonitor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

			string eventLoggerUrl = ConfigurationManager.AppSettings["EventLoggerUrl"];

			MessageQueue messageQueue;
			try
			{
				messageQueue = new MessageQueue(ConfigurationManager.AppSettings["EventsQueue"]);
				messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

				Trace.WriteLine(string.Format("Queue name: {0}", messageQueue.Label));
			}
			catch (MessageQueueException mex)
			{
				Console.WriteLine("Could not connect to event queue {0}: {1}", ConfigurationManager.AppSettings["EventsQueue"], mex.Message);
				return;
			}

			try
			{
				WebClient client = new WebClient();
				string s = client.DownloadString(ConfigurationManager.AppSettings["EventViewerUrl"]);
			}
			catch (WebException wex)
			{
				Console.WriteLine("Could not reach event logger: {0} ({1})", wex.Message, wex.Status);
				return;
			}

			try
			{
				EventsMonitor monitor = new EventsMonitor(messageQueue, eventLoggerUrl);
				monitor.Run();
			}
			catch (MessageQueueException mex)
			{
				Trace.WriteLine(string.Format("Message Queue EXCEPTION receiving message from queue: {0} ({1})", mex.Message, mex.MessageQueueErrorCode));
				Trace.WriteLine(mex);
			}
			catch (WebException wex)
			{
				Trace.WriteLine(string.Format("Web EXCEPTION sending queue message to event logger: {0} ({1})", wex.Message, wex.Status));

				if (wex.Response != null)
				{
					var responseStream = wex.Response.GetResponseStream();

					if (responseStream != null)
						using (var reader = new StreamReader(responseStream))
							Trace.WriteLine(string.Format("Response:\n{0}", reader.ReadToEnd()));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("EXCEPTION while processing queue: {0}", ex.Message);
			}
		}
	}
}
