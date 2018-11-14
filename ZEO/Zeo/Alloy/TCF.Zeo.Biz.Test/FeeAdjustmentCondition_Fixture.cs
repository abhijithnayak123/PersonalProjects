using TCF.Zeo.Biz.Common.Contract;
using TCF.Zeo.Biz.Common.Impl;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using NUnit.Framework;
using System.Collections.Generic;

namespace TCF.Zeo.Biz.Partner.Test
{
    [TestFixture]
    public class FeeAdjustmentCondition_Fixture
    {
        IFeeAdjustmentConditionValidation feeAdjCondition = new FeeAdjustmentConditionValidation();
        ZeoContext alloycontext = new ZeoContext();

        [TestCase]
        public void Can_ValidateCheckType()
        {
            List<FeeAdjustment> feeAdjustments = new List<FeeAdjustment>();
            FeeAdjustment feeAdjustment = new FeeAdjustment() { Conditions = new List<FeeAdjustmentConditions>() { new FeeAdjustmentConditions() { ConditionValue = "1, 2, 3", CompareType = 3 } } };

            feeAdjCondition.CheckTypeCondition(1, Zeo.Common.Util.Helper.CompareTypes.Equal, "1", alloycontext);

            Assert.AreEqual(1, feeAdjustments.Count);
        }

        [TestCase]
        public void Can_Validate_Promo_Code()
        {
            List<FeeAdjustment> feeAdjustments = new List<FeeAdjustment>();
            FeeAdjustment feeAdjustment = new FeeAdjustment() { Conditions = new List<FeeAdjustmentConditions>() { new FeeAdjustmentConditions() { ConditionValue = "1, 2, 3", CompareType = 3 } } };

            feeAdjCondition.CodeCondition("Promo", Zeo.Common.Util.Helper.CompareTypes.Equal, "Promo", alloycontext);

            Assert.AreEqual(1, feeAdjustments.Count);
        }

        [TestCase]
        public void Can_Validate_Group()
        {
            List<FeeAdjustment> feeAdjustments = new List<FeeAdjustment>();
            FeeAdjustment feeAdjustment = new FeeAdjustment() { Conditions = new List<FeeAdjustmentConditions>() { new FeeAdjustmentConditions() { ConditionValue = "1, 2, 3", CompareType = 3 } } };
            ZeoContext context = new ZeoContext();

            feeAdjCondition.GroupCondition(Zeo.Common.Util.Helper.CompareTypes.In, "",context);

            Assert.AreEqual(1, feeAdjustments.Count);
        }
    }
}
