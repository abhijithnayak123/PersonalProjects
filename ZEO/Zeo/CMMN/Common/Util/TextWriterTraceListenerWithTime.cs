using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MGI.Common.Util
{

	public class TextWriterTraceListenerWithTime : TextWriterTraceListener
	{
		string Msg = string.Empty;

		public TextWriterTraceListenerWithTime(Stream LogFile)
			: base(LogFile)
		{
		}

		public TextWriterTraceListenerWithTime(TextWriter LogFile)
			: base(LogFile)
		{
		}

		public override void Write(string Message)
		{
			if (Msg.Length == 0)
			{
				string Indent = string.Empty;
				if (IndentLevel > 0)
					Indent = new string('\t', this.IndentLevel);
				base.Write(DateTime.Now.ToString("MMM d HH:mm:ss:ffff") + Indent + "\t" + Message);
			}
			else
				base.Write(Message);
			Msg += Message;
		}

		public override void WriteLine(string Message)
		{
			if (Msg.Length == 0)
			{
				string Indent = string.Empty;
				if (IndentLevel > 0)
					Indent = new string('\t', this.IndentLevel);
				base.WriteLine(DateTime.Now.ToString("MMM d HH:mm:ss:ffff") + Indent + "\t" + Message);
			}
			else
				base.WriteLine(Message);
			Msg = string.Empty;
		}

		protected override void WriteIndent()
		{
			NeedIndent = false;
		}

		
	}
}
