﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Models
{
	public class AgentDetails : BaseModel
	{
		public string AgentFirstName { get; set; }

		public string AgentLastName { get; set; }

		public bool IsAgentState { get; set; }
	}
}