using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Common
{
	public static class TimeZoneHelper
	{
		public static List<SelectListItem> GetTimeZones()
		{
			List<SelectListItem> timeZones = new List<SelectListItem>();

			var globalTimeZones = TimeZoneInfo.GetSystemTimeZones();

			foreach (var timeZone in globalTimeZones)
			{
				timeZones.Add(new SelectListItem() { Text = timeZone.DisplayName, Value = timeZone.Id });
			}

			return timeZones;
		}
	}
}