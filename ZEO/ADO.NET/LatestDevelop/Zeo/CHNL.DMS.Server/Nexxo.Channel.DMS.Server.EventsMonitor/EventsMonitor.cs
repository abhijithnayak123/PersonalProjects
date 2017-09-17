using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace Nexxo.Channel.DMS.Server.EventsMonitor
{
	public class EventsMonitor
	{
		private MessageQueue messageQueue;
		private string eventLoggerUrl;

		public EventsMonitor(MessageQueue messageQueue, string eventLoggerUrl)
		{
			this.messageQueue = messageQueue;
			this.eventLoggerUrl = eventLoggerUrl;
		}

		public void Run()
		{
			string clientResponse;
			Message message = null;

			while (true)
			{
				waitForMessage();

				message = messageQueue.Receive();

				Trace.WriteLine(string.Format("Received new message: {0}", (string)message.Body));
				Trace.WriteLine(string.Format("Sending to {0}...", eventLoggerUrl));

				using (WebClient client = new WebClient())
				{
					client.Headers.Add("Content-Type", "application/json");
					clientResponse = client.UploadString(eventLoggerUrl, "POST", (string)message.Body);

					Trace.WriteLine(string.Format("Response:\n{0}", clientResponse));
					foreach (string s in client.ResponseHeaders)
						Trace.WriteLine(string.Format("   {0}: {1}", s, client.ResponseHeaders[s]));
				}
			}
		}
	
		private void waitForMessage()
		{
			Trace.Write("Waiting for message....");
			Message message = messageQueue.Peek();
			Trace.WriteLine(string.Format("New message arrived: {0}", (string)message.Body));
		}
	}
}
