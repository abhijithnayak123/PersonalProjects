﻿using System;
using System.Collections.Generic;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using System.Data;
using P3Net.Data;
using P3Net.Data.Common;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : IPricingCluster
    {
        public List<Pricing> GetBaseFee(Helper.TransactionType transactionType, string productType, ZeoContext context)
        {
            List<Pricing> pricings = new List<Pricing>();
            Pricing pricing;

            StoredProcedure corePricingCluster = new StoredProcedure("usp_PricingCLuster");

            corePricingCluster.WithParameters(InputParameter.Named("channelPartnerId").WithValue(context.ChannelPartnerId));
            corePricingCluster.WithParameters(InputParameter.Named("productId").WithValue(Convert.ToInt32(transactionType)));
            corePricingCluster.WithParameters(InputParameter.Named("locationId").WithValue(context.LocationID));
            corePricingCluster.WithParameters(InputParameter.Named("productType").WithValue(productType));
            corePricingCluster.WithParameters(InputParameter.Named("productProviderCode").WithValue(context.ProviderId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(corePricingCluster))
            {
                while (datareader.Read())
                {
                    pricing = new Pricing();
                    pricing.CompareType = datareader.GetInt32OrDefault("CompareTypeId");
                    pricing.MinimumAmount = datareader.GetDecimalOrDefault("MinimumAmount");
                    pricing.MaximumAmount = datareader.GetDecimalOrDefault("MaximumAmount");
                    pricing.MinimumFee = datareader.GetDecimalOrDefault("MinimumFee");
                    pricing.MaximumFee = datareader.GetDecimalOrDefault("MaximumFee");
                    pricing.Value = datareader.GetDecimalOrDefault("Value");
                    pricing.IsPercentage = datareader.GetBooleanOrDefault("IsPercentage");
                    pricings.Add(pricing);
                }

            }

            return pricings;
        }
    }
}
