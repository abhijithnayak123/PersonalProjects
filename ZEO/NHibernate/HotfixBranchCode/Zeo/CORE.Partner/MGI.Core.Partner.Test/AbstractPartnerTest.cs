// -----------------------------------------------------------------------
// <copyright file="AbstractPartnerTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Core.Partner.Test
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
                return new string[] { "assembly://MGI.Core.Partner.Test/MGI.Core.Partner.Test/MGI.Core.Partner.Test.Spring.xml" }; 
            }
        }

    }
}
