using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Net;

namespace TCF.Channel.Zeo.Web.Common
{
	public class ZeoViewEngine : RazorViewEngine
	{
		protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
		{
			string path = string.Empty;
			string fullPath = string.Empty;

			if (HttpContext.Current.Session != null && HttpContext.Current.Session["ChannelPartnerName"] != null)
			{
				string channelPartner = HttpContext.Current.Session["ChannelPartnerName"].ToString();

				if (!string.IsNullOrWhiteSpace(channelPartner))
				{
					path = partialPath.Replace("Shared", channelPartner + "/Shared");
					fullPath = HttpContext.Current.Server.MapPath(path);

					if (System.IO.File.Exists(fullPath))
					{
						return base.CreatePartialView(controllerContext, path);
					}
					
				}
			}
			return base.CreatePartialView(controllerContext, partialPath);
		}
        
	}
}