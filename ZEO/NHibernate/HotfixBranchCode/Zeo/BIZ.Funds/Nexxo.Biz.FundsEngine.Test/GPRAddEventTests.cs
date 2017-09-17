// -----------------------------------------------------------------------
// <copyright file="GPRAddEventTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Biz.FundsEngine.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using NUnit.Framework;
    using Spring.Testing.NUnit;
    using MGI.Biz.FundsEngine;
    using MGI.Biz.FundsEngine.Contract;
    using MGI.Biz.FundsEngine.Data;
    
    

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GPRAddEventTests : AbstractTransactionalSpringContextTests
    {
        public IFundsEngine FundsEngine { get; set; }


        protected override string[] ConfigLocations
        {
            get { return new string[] { "assembly://MGI.Biz.FundsEngine.Test/MGI.Biz.FundsEngine.Test/Biz.Fund.Test.xml" }; }
        }

        public void TestGPRAddEvent() {
            FundsEngine.AnnounceGprAdd("Synovus", null);
        }

        
    }

    static class ObjectExtension {
        public static void AnnounceGprAdd(this IFundsEngine FundsEngine, 
                string channelPartner, GPRAddEvent gprEvent)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = FundsEngine.GetType();
            MethodInfo method = type.GetMethod("PublishEvent", flags);
            method.Invoke(FundsEngine, new Object [] {channelPartner, gprEvent});
        }
    }
}
