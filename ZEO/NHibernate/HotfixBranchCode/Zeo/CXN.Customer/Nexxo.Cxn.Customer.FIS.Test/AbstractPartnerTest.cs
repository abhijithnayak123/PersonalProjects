// -----------------------------------------------------------------------
// <copyright file="AbstractPartnerTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Cxn.Customer.FIS.Test
{
using Spring.Testing.NUnit;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AbstractPartnerTest : AbstractTransactionalDbProviderSpringContextTests
    {

        protected override string[] ConfigLocations
        {
            get {
                return new string[] { "assembly://MGI.Cxn.Customer.FIS.Test/MGI.Cxn.Customer.FIS.Test/Cxn.Customer.FIS.Test.Spring.xml" }; 
            }
        }

    }
}
