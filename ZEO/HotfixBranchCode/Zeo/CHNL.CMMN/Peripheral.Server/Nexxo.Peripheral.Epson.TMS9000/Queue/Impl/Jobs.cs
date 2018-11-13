using System;
using System.Collections.Generic;

namespace MGI.Peripheral.Queue.Impl
{
	public static class Queue
	{
		public static List<Object> Jobs = new List<Object>();
		public static List<String> JobStatus = new List<String>();
		public static List<String> JobType = new List<String>();
	}
}
