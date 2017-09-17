﻿using System;
using System.ServiceModel.Configuration;

namespace TCF.Zeo.Common.Behaviour
{
    public class TraceOutputBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(TraceOutputBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new TraceOutputBehavior();
        }
    }
}
