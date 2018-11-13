// -----------------------------------------------------------------------
// <copyright file="NexxoBizEvent.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Biz.Events.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NexxoBizEvent
    {
        public string Name { get; set; }

        public NexxoBizEvent(string name) {
            this.Name = name;
        }
    }
}
