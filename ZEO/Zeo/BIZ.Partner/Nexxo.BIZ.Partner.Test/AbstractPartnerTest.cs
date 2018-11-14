// -----------------------------------------------------------------------
// <copyright file="AbstractPartnerTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Biz.Partner.Test
{
using Spring.Testing.NUnit;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AbstractPartnerTest : AbstractTransactionalSpringContextTests
    {

        protected override string[] ConfigLocations
        {
            get {
                return new string[] { "assembly://MGI.Biz.Partner.Test/MGI.Biz.Partner.Test/Biz.Partner.Test.Spring.xml" }; 
            }
        }

    }
}
