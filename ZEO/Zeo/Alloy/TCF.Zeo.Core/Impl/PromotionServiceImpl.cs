using System;
using System.Collections.Generic;
using System.Data;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using P3Net.Data.Common;
using System.IO;
using P3Net.Data;
using TCF.Zeo.Core.Data.Exceptions;
using System.Linq;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : IPromotionService
    {
        public bool CreateAndUpdatePromotion(Promotion promotion, ZeoContext context)
        {
            try
            {
                StringWriter qualifierWriter = new StringWriter();
                StringWriter provisionWriter = new StringWriter();
                if (promotion != null & promotion.Qualifiers.Count > 0)
                {
                    DataSet qualifierDataSet = getQualifiers(promotion.Qualifiers, promotion.PromotionDetail.PromotionId, (DateTime)promotion.PromotionDetail.StartDate, context);
                    qualifierDataSet.WriteXml(qualifierWriter);
                }
                if (promotion != null & promotion.Provisions.Count > 0)
                {
                    DataSet provisionDataSet = getProvisions(promotion.Provisions, promotion.PromotionDetail.PromotionId, context);
                    provisionDataSet.WriteXml(provisionWriter);
                }
                StoredProcedure insertPromotion = new StoredProcedure("usp_InsertOrUpdatePromotion");

                DataParameter[] dataParameters = new DataParameter[] {
                new DataParameter("qualifiers ", DbType.Xml) { Value = qualifierWriter.ToString() },
                new DataParameter("provisions ", DbType.Xml) { Value = provisionWriter.ToString() }
                };

                promotion.DTServerCreate = DateTime.Now;
                promotion.DTTerminalCreate = TCF.Zeo.Common.Util.Helper.GetTimeZoneTime(context.TimeZone);
                insertPromotion.WithParameters(InputParameter.Named("promotionId").WithValue(promotion.PromotionDetail.PromotionId));
                insertPromotion.WithParameters(InputParameter.Named("name").WithValue(promotion.PromotionDetail.PromotionName));
                insertPromotion.WithParameters(InputParameter.Named("description").WithValue(promotion.PromotionDetail.PromotionDescription));
                insertPromotion.WithParameters(InputParameter.Named("productId").WithValue(promotion.PromotionDetail.Product?.Id));
                insertPromotion.WithParameters(InputParameter.Named("startDate").WithValue(promotion.PromotionDetail.StartDate));
                insertPromotion.WithParameters(InputParameter.Named("endDate").WithValue(promotion.PromotionDetail.EndDate));
                insertPromotion.WithParameters(InputParameter.Named("priority").WithValue(promotion.PromotionDetail.Priority));
                insertPromotion.WithParameters(InputParameter.Named("providerId").WithValue(promotion.PromotionDetail.Provider?.Id));
                insertPromotion.WithParameters(InputParameter.Named("isSystemApplied").WithValue(promotion.PromotionDetail.IsSystemApplied));
                insertPromotion.WithParameters(InputParameter.Named("isOverridable").WithValue(promotion.PromotionDetail.IsOverridable));
                insertPromotion.WithParameters(InputParameter.Named("isNextCustomerSession").WithValue(promotion.PromotionDetail.IsNextCustomerSession));
                insertPromotion.WithParameters(InputParameter.Named("isPrintable ").WithValue(promotion.PromotionDetail.IsPrintable));
                insertPromotion.WithParameters(InputParameter.Named("promotionStatus").WithValue((int)promotion.PromotionDetail.PromotionStatus));
                insertPromotion.WithParameters(InputParameter.Named("Stackable").WithValue(promotion.PromotionDetail.Stackable));
                insertPromotion.WithParameters(InputParameter.Named("IsPromotionHidden").WithValue(promotion.PromotionDetail.IsPromotionHidden));
                insertPromotion.WithParameters(InputParameter.Named("FreeTxnCount").WithValue(promotion.PromotionDetail.FreeTxnCount));
                insertPromotion.WithParameters(InputParameter.Named("dTServerDate").WithValue(promotion.DTServerCreate));
                insertPromotion.WithParameters(InputParameter.Named("dTTerminalDate ").WithValue(promotion.DTTerminalCreate));
                insertPromotion.WithParameters(dataParameters);

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(insertPromotion);

                return true;
            }
            catch (Exception ex)
            {
                throw new PromotionException(PromotionException.INSERT_OR_UPDATE_PROMOTION, ex);
            }
        }

        public Promotion GetPromotion(long promotionId, ZeoContext context)
        {
            try
            {
                StoredProcedure getPromotion = new StoredProcedure("usp_GetPromotionById");
                getPromotion.WithParameters(InputParameter.Named("promotionId").WithValue(promotionId));
                getPromotion.WithParameters(InputParameter.Named("channelpartnerId").WithValue(context.ChannelPartnerId));

                Promotion promotion = new Promotion();
                promotion.Qualifiers = new List<Qualifier>();
                promotion.Provisions = new List<Provision>();

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(getPromotion))
                {
                    while (datareader.Read())
                    {
                        promotion.PromotionDetail = new PromotionDetail()
                        {
                            PromotionId = datareader.GetInt64OrDefault("PromotionId"),
                            PromotionName = datareader.GetStringOrDefault("PromotionName"),
                            PromotionDescription = datareader.GetStringOrDefault("Description"),
                            Product = new Product() { Id = datareader.GetInt64OrDefault("ProductId"), Name = datareader.GetStringOrDefault("ProductName") },
                            Provider = new Provider() { Id = (int)datareader.GetInt64OrDefault("ProviderId"), Name = datareader.GetStringOrDefault("ProviderName") },
                            Priority = datareader.IsDBNull("Priority") == true ? (int?)null : datareader.GetInt32OrDefault("Priority"),
                            StartDate = datareader.GetDateTimeOrDefault("StartDate"),
                            EndDate = datareader.GetDateTimeOrDefault("EndDate"),
                            PromotionStatus = (Helper.PromotionStatus)datareader.GetInt32OrDefault("Status"),
                            IsNextCustomerSession = datareader.GetBooleanOrDefault("IsNextCustomerSession"),
                            IsOverridable = datareader.GetBooleanOrDefault("IsOverridable"),
                            IsPrintable = datareader.GetBooleanOrDefault("IsPrintable"),
                            IsSystemApplied = datareader.GetBooleanOrDefault("IsSystemApplied"),
                            FreeTxnCount = datareader.GetInt32OrDefault("FreeTransactionCount"),
                            Stackable = datareader.GetBooleanOrDefault("Stackable"),
                            IsPromotionHidden = datareader.GetBooleanOrDefault("IsPromotionHidden")
                        };
                    }
                    datareader.NextResult();
                    while (datareader.Read())
                    {
                        Qualifier qualifier = new Qualifier()
                        {
                            QualifierId = datareader.GetInt64OrDefault("QualifierId"),
                            QualifierProduct = new Product() { Id = datareader.GetInt64OrDefault("ProductId"), Name = datareader.GetStringOrDefault("ProductName") },
                            TrxEndDate = datareader.GetDateTimeOrDefault("EndDate"),
                            TrxAmount = datareader.IsDBNull("Amount") == true ? (decimal?)null : decimal.Round(datareader.GetDecimalOrDefault("Amount"), 2),
                            MinTrxCount = datareader.IsDBNull("MinTransactionCount") == true ? (int?)null : datareader.GetInt32OrDefault("MinTransactionCount"),
                            TransactionStates = datareader.GetStringOrDefault("TransactionStates"),
                            IsPaidFee = datareader.GetBooleanOrDefault("IsPaidFee"),
                            IsActive = true                            
                        };
                        promotion.Qualifiers.Add(qualifier);
                    }
                    datareader.NextResult();
                    while (datareader.Read())
                    {
                        Provision provision = new Provision()
                        {
                            ProvisionId = datareader.GetInt64OrDefault("ProvisionId"),
                            LocationIds = datareader.GetStringOrDefault("Locations"),
                            CheckTypeIds = datareader.GetStringOrDefault("CheckTypes"),
                            Groups = datareader.GetStringOrDefault("Groups"),
                            MinTrxAmount = datareader.IsDBNull("MinAmount") == true ? (decimal?)null : decimal.Round(datareader.GetDecimalOrDefault("MinAmount"), 2),
                            MaxTrxAmount = datareader.IsDBNull("MaxAmount") == true ? (decimal?)null : decimal.Round(datareader.GetDecimalOrDefault("MaxAmount"), 2),
                            Value = datareader.GetStringOrDefault("Value"),
                            DiscountType = (Helper.DiscountType)datareader.GetInt32OrDefault("DiscountType"),
                            IsActive = true
                        };
                        promotion.Provisions.Add(provision);
                    }
                }
                return promotion;
            }
            catch (Exception ex)
            {
                throw new PromotionException(PromotionException.GET_PROMOTION_BY_ID, ex);
            }
        }

        public List<PromotionDetail> GetPromotions(PromotionSearchCriteria promotionSearchCriteria, ZeoContext context)
        {
            try
            {
                List<PromotionDetail> promotionDetails = new List<PromotionDetail>();
                PromotionDetail promotionDetail = null;

                StoredProcedure getPromotion = new StoredProcedure("usp_GetPromotions");
                getPromotion.WithParameters(InputParameter.Named("promotionName").WithValue(promotionSearchCriteria.PromotionName));
                getPromotion.WithParameters(InputParameter.Named("promotionStartDate").WithValue(promotionSearchCriteria.StartDate));
                getPromotion.WithParameters(InputParameter.Named("promotionEndDate").WithValue(promotionSearchCriteria.EndDate));
                getPromotion.WithParameters(InputParameter.Named("productId").WithValue(promotionSearchCriteria.Product?.Id));
                getPromotion.WithParameters(InputParameter.Named("channelpartnerId").WithValue(context.ChannelPartnerId));
                getPromotion.WithParameters(InputParameter.Named("showExpired").WithValue(promotionSearchCriteria.ShowExpired));
                getPromotion.WithParameters(InputParameter.Named("serverDate").WithValue(DateTime.Now));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(getPromotion))
                {
                    while (datareader.Read())
                    {

                        promotionDetail = new PromotionDetail()
                        {
                            PromotionId = datareader.GetInt64OrDefault("PromotionId"),
                            PromotionName = datareader.GetStringOrDefault("PromotionName"),
                            PromotionDescription = datareader.GetStringOrDefault("Description"),
                            Product = new Product() { Name = datareader.GetStringOrDefault("ProductName") },
                            Provider = new Provider() { Name = datareader.GetStringOrDefault("ProviderName") },
                            Priority = datareader.IsDBNull("Priority") == true ? (int?)null : datareader.GetInt32OrDefault("Priority"),
                            StartDate = datareader.GetDateTimeOrDefault("StartDate"),
                            EndDate = datareader.GetDateTimeOrDefault("EndDate"),
                            PromotionStatus = (Common.Util.Helper.PromotionStatus)datareader.GetInt32OrDefault("PromotionStatus"),
                            FreeTxnCount = datareader.GetInt32OrDefault("FreeTransactionCount"),
                            Stackable = datareader.GetBooleanOrDefault("Stackable"),
                            IsPromotionHidden = datareader.GetBooleanOrDefault("IsPromotionHidden")
                        };

                        promotionDetails.Add(promotionDetail);
                    }
                }

                return promotionDetails;
            }
            catch (Exception ex)
            {
                throw new PromotionException(PromotionException.GET_PROMOTIONS, ex);
            }
        }

        public bool ValidatePromoName(string promotionName, long promotionId, ZeoContext context)
        {
            try
            {
                bool isValid = false;

                StoredProcedure validatePromoName = new StoredProcedure("usp_ValidatePromotionName");
                validatePromoName.WithParameters(InputParameter.Named("promotionName").WithValue(promotionName));
                validatePromoName.WithParameters(InputParameter.Named("promotionId").WithValue(promotionId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(validatePromoName))
                {
                    while (datareader.Read())
                    {
                        isValid = datareader.GetBooleanOrDefault("isValid");
                    }
                }

                return isValid;
            }
            catch (Exception ex)
            {
                throw new PromotionException(PromotionException.VALIDATE_PROMO_NAME, ex);
            }
        }

        public int UpdatePromotionStatus(long promotionId, Helper.PromotionStatus status, ZeoContext context)
        {
            try
            {
                StoredProcedure updatePromotionStatus = new StoredProcedure("usp_UpdatePromotionStatus");
                updatePromotionStatus.WithParameters(InputParameter.Named("promotionId").WithValue(promotionId));
                updatePromotionStatus.WithParameters(InputParameter.Named("status").WithValue((int)status));
                updatePromotionStatus.WithParameters(InputParameter.Named("dtServerDate").WithValue(DateTime.Now));
                updatePromotionStatus.WithParameters(InputParameter.Named("dtTerminalDate").WithValue(TCF.Zeo.Common.Util.Helper.GetTimeZoneTime(context.TimeZone)));
                updatePromotionStatus.WithParameters(OutputParameter.Named("promoStatus").OfType<Int32>());
                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(updatePromotionStatus);
                return Convert.ToInt32(updatePromotionStatus.Parameters["promoStatus"].Value);
            }
            catch (Exception ex)
            {
                throw new PromotionException(PromotionException.UPDATE_STATUS, ex);
            }
        }

        public long SavePromoDetails(PromotionDetail promoDetails, ZeoContext context)
        {
            try
            {
                StoredProcedure insertPromoDetails = new StoredProcedure("usp_SavePromoDetails");
                promoDetails.DTServerCreate = DateTime.Now;
                promoDetails.DTTerminalCreate = TCF.Zeo.Common.Util.Helper.GetTimeZoneTime(context.TimeZone);
                insertPromoDetails.WithParameters(InputParameter.Named("promotionId").WithValue(promoDetails.PromotionId));
                insertPromoDetails.WithParameters(InputParameter.Named("name").WithValue(promoDetails.PromotionName));
                insertPromoDetails.WithParameters(InputParameter.Named("description").WithValue(promoDetails.PromotionDescription));
                insertPromoDetails.WithParameters(InputParameter.Named("productId").WithValue(promoDetails.Product?.Id == 0 ? null : promoDetails.Product?.Id));
                insertPromoDetails.WithParameters(InputParameter.Named("startDate").WithValue(promoDetails.StartDate));
                insertPromoDetails.WithParameters(InputParameter.Named("endDate").WithValue(promoDetails.EndDate));
                insertPromoDetails.WithParameters(InputParameter.Named("priority").WithValue(promoDetails.Priority));
                insertPromoDetails.WithParameters(InputParameter.Named("providerId").WithValue(promoDetails.Provider?.Id));
                insertPromoDetails.WithParameters(InputParameter.Named("isSystemApplied").WithValue(promoDetails.IsSystemApplied));
                insertPromoDetails.WithParameters(InputParameter.Named("isOverridable").WithValue(promoDetails.IsOverridable));
                insertPromoDetails.WithParameters(InputParameter.Named("isNextCustomerSession").WithValue(promoDetails.IsNextCustomerSession));
                insertPromoDetails.WithParameters(InputParameter.Named("isPrintable ").WithValue(promoDetails.IsPrintable));
                insertPromoDetails.WithParameters(InputParameter.Named("PromotionStatus").WithValue((int)promoDetails.PromotionStatus));
                insertPromoDetails.WithParameters(InputParameter.Named("freeTxnCount").WithValue(promoDetails.FreeTxnCount));
                insertPromoDetails.WithParameters(InputParameter.Named("stackable").WithValue(promoDetails.Stackable));
                insertPromoDetails.WithParameters(InputParameter.Named("IsPromotionHidden").WithValue(promoDetails.IsPromotionHidden));
                insertPromoDetails.WithParameters(InputParameter.Named("dTServerDate").WithValue(promoDetails.DTServerCreate));
                insertPromoDetails.WithParameters(InputParameter.Named("dTTerminalDate ").WithValue(promoDetails.DTTerminalCreate));
                insertPromoDetails.WithParameters(OutputParameter.Named("dbPromoId").OfType<long>());

                DataConnectionHelper.GetConnectionManager().ExecuteReader(insertPromoDetails);

                return Convert.ToInt64(insertPromoDetails.Parameters["dbPromoId"].Value);
            }
            catch (Exception ex)
            {
                throw new PromotionException(PromotionException.SAVE_PROMO_DETAILS_FAILED, ex);
            }
        }

        public long SavePromoProvision(Provision provision, ZeoContext context)
        {
            try
            {
                StoredProcedure insertPromoProvision = new StoredProcedure("usp_SaveProvision");
                provision.DTServerCreate = DateTime.Now;
                provision.DTTerminalCreate = TCF.Zeo.Common.Util.Helper.GetTimeZoneTime(context.TimeZone);
                insertPromoProvision.WithParameters(InputParameter.Named("promoProvisionId").WithValue(provision.ProvisionId));
                insertPromoProvision.WithParameters(InputParameter.Named("promotionId").WithValue(provision.PromotionId));
                insertPromoProvision.WithParameters(InputParameter.Named("value").WithValue(provision.Value));
                insertPromoProvision.WithParameters(InputParameter.Named("minAmount").WithValue(provision.MinTrxAmount));
                insertPromoProvision.WithParameters(InputParameter.Named("maxAmount").WithValue(provision.MaxTrxAmount));
                insertPromoProvision.WithParameters(InputParameter.Named("checkTypeIds").WithValue(provision.CheckTypeIds));
                insertPromoProvision.WithParameters(InputParameter.Named("groups").WithValue(provision.Groups));
                insertPromoProvision.WithParameters(InputParameter.Named("discountType").WithValue((int)provision.DiscountType));
                insertPromoProvision.WithParameters(InputParameter.Named("locationIds").WithValue(provision.LocationIds));
                insertPromoProvision.WithParameters(InputParameter.Named("dTServerDate").WithValue(provision.DTServerCreate));
                insertPromoProvision.WithParameters(InputParameter.Named("dTTerminalDate ").WithValue(provision.DTTerminalCreate));
                insertPromoProvision.WithParameters(OutputParameter.Named("dbPromoProvisionId").OfType<long>());

                DataConnectionHelper.GetConnectionManager().ExecuteReader(insertPromoProvision);

                return Convert.ToInt64(insertPromoProvision.Parameters["dbPromoProvisionId"].Value);
            }
            catch (Exception ex)
            {
                throw new PromotionException(PromotionException.SAVE_PROMO_PROVISION_FAILED, ex);
            }
        }

        public long SavePromoQualifier(Qualifier qualifier, ZeoContext context)
        {
            try
            {
                StoredProcedure insertPromoQualifier = new StoredProcedure("usp_SaveQualifier");
                qualifier.DTServerCreate = DateTime.Now;
                qualifier.DTTerminalCreate = TCF.Zeo.Common.Util.Helper.GetTimeZoneTime(context.TimeZone);
                insertPromoQualifier.WithParameters(InputParameter.Named("promoQualifierId").WithValue(qualifier.QualifierId));
                insertPromoQualifier.WithParameters(InputParameter.Named("promotionId").WithValue(qualifier.PromotionId));
                insertPromoQualifier.WithParameters(InputParameter.Named("startDate").WithValue(qualifier.TrxStartDate));
                insertPromoQualifier.WithParameters(InputParameter.Named("endDate").WithValue(qualifier.TrxEndDate));
                insertPromoQualifier.WithParameters(InputParameter.Named("amount").WithValue(qualifier.TrxAmount));
                insertPromoQualifier.WithParameters(InputParameter.Named("minTransactionCount").WithValue(qualifier.MinTrxCount));
                insertPromoQualifier.WithParameters(InputParameter.Named("productId").WithValue(qualifier.QualifierProduct?.Id));
                insertPromoQualifier.WithParameters(InputParameter.Named("transactionStates").WithValue(qualifier.TransactionStates));
                insertPromoQualifier.WithParameters(InputParameter.Named("isPaidFee").WithValue(qualifier.IsPaidFee));
                insertPromoQualifier.WithParameters(InputParameter.Named("isParked").WithValue(qualifier.ConsiderParkedTxns));
                insertPromoQualifier.WithParameters(InputParameter.Named("dTServerDate").WithValue(qualifier.DTServerCreate));
                insertPromoQualifier.WithParameters(InputParameter.Named("dTTerminalDate ").WithValue(qualifier.DTTerminalCreate));
                insertPromoQualifier.WithParameters(OutputParameter.Named("dbQualifierId").OfType<long>());

                DataConnectionHelper.GetConnectionManager().ExecuteReader(insertPromoQualifier);

                return Convert.ToInt64(insertPromoQualifier.Parameters["dbQualifierId"].Value);
            }
            catch (Exception ex)
            {
                throw new PromotionException(PromotionException.SAVE_PROMO_QUALIFIER_FAILED, ex);
            }
        }

        public bool DeletePromoProvision(long provisionId, ZeoContext context)
        {
            try
            {
                StoredProcedure deletePromoProvision = new StoredProcedure("usp_DeleteProvison");

                deletePromoProvision.WithParameters(InputParameter.Named("promoProvisionId").WithValue(provisionId));

                var result = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(deletePromoProvision);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw new PromotionException(PromotionException.DELETE_PROMO_PROVISION_FAILED, ex);
            }
        }

        public bool DeletePromoQualifier(long qualifierId, ZeoContext context)
        {
            try
            {
                StoredProcedure deletePromoQualifier = new StoredProcedure("usp_DeleteQualifier");

                deletePromoQualifier.WithParameters(InputParameter.Named("promoQualifierId").WithValue(qualifierId));

                var result = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(deletePromoQualifier);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw new PromotionException(PromotionException.DELETE_PROMO_QUALIFIER_FAILED, ex);
            }
        }

        public List<Qualifier> AddUpdateQualifiers(List<Qualifier> qualifiers, long promotionId, DateTime startDate, ZeoContext context)
        {
            try
            {
                List<Qualifier> collectionQualifiers = new List<Qualifier>();
                StringWriter qualifierWriter = new StringWriter();
                DataSet qualifierDataSet = getQualifiers(qualifiers, promotionId, DateTime.Now, context);
                qualifierDataSet.WriteXml(qualifierWriter);
                StoredProcedure insertPromotion = new StoredProcedure("usp_InsertOrUpdateQualifiers");

                DataParameter[] dataParameters = new DataParameter[] {
                    new DataParameter("qualifiers ", DbType.Xml) { Value = qualifierWriter.ToString() }
                };

                insertPromotion.WithParameters(InputParameter.Named("promotionId").WithValue(promotionId));
                insertPromotion.WithParameters(dataParameters);

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(insertPromotion))
                {
                    while (datareader.Read())
                    {
                        Qualifier qualifier = new Qualifier()
                        {
                            QualifierId = datareader.GetInt64OrDefault("QualifierId"),
                            QualifierProduct = new Product() { Id = datareader.GetInt64OrDefault("ProductId"), Name = datareader.GetStringOrDefault("ProductName") },
                            TrxEndDate = datareader.GetDateTimeOrDefault("EndDate"),
                            TrxAmount = datareader.IsDBNull("Amount") == true ? (decimal?)null : decimal.Round(datareader.GetDecimalOrDefault("Amount"), 2),
                            MinTrxCount = datareader.IsDBNull("MinTransactionCount") == true ? (int?)null : datareader.GetInt32OrDefault("MinTransactionCount"),
                            TransactionStates = datareader.GetStringOrDefault("TransactionStates"),
                            IsPaidFee = datareader.GetBooleanOrDefault("IsPaidFee"),
                            PromotionId = datareader.GetInt64OrDefault("PromotionId"),
                            IsActive = true
                        };
                        collectionQualifiers.Add(qualifier);
                    }
                }

                return collectionQualifiers;


            }
            catch (Exception ex)
            {
                throw new PromotionException(PromotionException.ADD_UPDATE_QUALIFIERS_FAILED, ex);
            }
        }

        public List<Provision> AddUpdateProvisions(List<Provision> provisions, long promotionId, ZeoContext context)
        {
            try
            {
                List<Provision> collectionProvisions = new List<Provision>();
                StringWriter provisionWriter = new StringWriter();
                DataSet provisionDataSet = getProvisions(provisions, promotionId, context);
                provisionDataSet.WriteXml(provisionWriter);
                StoredProcedure insertPromotion = new StoredProcedure("usp_InsertOrUpdateProvisions");

                DataParameter[] dataParameters = new DataParameter[] {
                    new DataParameter("provisions ", DbType.Xml) { Value = provisionWriter.ToString() }
                };

                insertPromotion.WithParameters(InputParameter.Named("promotionId").WithValue(promotionId));
                insertPromotion.WithParameters(dataParameters);

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(insertPromotion))
                {
                    while (datareader.Read())
                    {
                        Provision provision = new Provision()
                        {
                            ProvisionId = datareader.GetInt64OrDefault("ProvisionId"),
                            LocationIds = datareader.GetStringOrDefault("Locations"),
                            CheckTypeIds = datareader.GetStringOrDefault("CheckTypes"),
                            Groups = datareader.GetStringOrDefault("Groups"),
                            MinTrxAmount = datareader.IsDBNull("MinAmount") == true ? (decimal?)null : decimal.Round(datareader.GetDecimalOrDefault("MinAmount"), 2),
                            MaxTrxAmount = datareader.IsDBNull("MaxAmount") == true ? (decimal?)null : decimal.Round(datareader.GetDecimalOrDefault("MaxAmount"), 2),
                            Value = datareader.GetStringOrDefault("Value"),
                            DiscountType = (Helper.DiscountType)datareader.GetInt32OrDefault("DiscountType"),
                            PromotionId = datareader.GetInt64OrDefault("PromotionId"),
                            IsActive = true
                        };
                        collectionProvisions.Add(provision);
                    }
                }

                return collectionProvisions;

            }
            catch (Exception ex)
            {
                throw new PromotionException(PromotionException.ADD_UPDATE_PROVISIONS_FAILED, ex);
            }
        }

        private DataSet getQualifiers(List<Qualifier> qualifiers, long promotionId, DateTime startDate, ZeoContext context)
        {
            DataSet dataSet = new DataSet("Qualifiers");
            DataTable table = new DataTable("Qualifier");
            table.Columns.Add("PromotionId", typeof(long));
            table.Columns.Add("PromoQualifierId", typeof(long));
            table.Columns.Add("StartDate", typeof(DateTime));
            table.Columns.Add("EndDate", typeof(DateTime));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("MinTransactionCount", typeof(int));
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("TransactionStates", typeof(string));
            table.Columns.Add("IsPaidFee", typeof(bool));
            table.Columns.Add("ConsiderParkedTxns", typeof(bool));
            // table.Columns.Add("IsActive", typeof(bool));
            table.Columns.Add("DTServerDate", typeof(DateTime));
            table.Columns.Add("DTTerminalDate", typeof(DateTime));

            foreach (Qualifier qualifier in qualifiers)
            {
                DataRow dr = table.NewRow();
                dr["PromotionId"] = promotionId;
                dr["PromoQualifierId"] = qualifier.QualifierId;
                dr["StartDate"] = startDate;
                if (qualifier.TrxEndDate == null)
                    dr["EndDate"] = DBNull.Value;
                else
                    dr["EndDate"] = qualifier.TrxEndDate;
                if (qualifier.TrxAmount == null)
                    dr["Amount"] = DBNull.Value;
                else
                    dr["Amount"] = qualifier.TrxAmount;

                if (qualifier.MinTrxCount == null)
                    dr["MinTransactionCount"] = DBNull.Value;
                else
                    dr["MinTransactionCount"] = qualifier.MinTrxCount;
                dr["ProductId"] = qualifier.QualifierProduct?.Id;
                dr["TransactionStates"] = qualifier.TransactionStates;
                dr["IsPaidFee"] = qualifier.IsPaidFee;
                dr["ConsiderParkedTxns"] = qualifier.ConsiderParkedTxns;
                // dr["IsActive"] = qualifier.IsActive;
                dr["DTTerminalDate"] = TCF.Zeo.Common.Util.Helper.GetTimeZoneTime(context.TimeZone);
                dr["DTServerDate"] = DateTime.Now;
                table.Rows.Add(dr);
            }

            dataSet.Tables.Add(table);
            return dataSet;
        }

        private DataSet getProvisions(List<Provision> provisions, long promotionId, ZeoContext context)
        {
            DataSet dataSet = new DataSet("Provisions");
            DataTable table = new DataTable("Provision");
            table.Columns.Add("PromotionId", typeof(long));
            table.Columns.Add("PromoProvisionId", typeof(long));
            table.Columns.Add("Value", typeof(string));
            table.Columns.Add("MinAmount", typeof(decimal));
            table.Columns.Add("MaxAmount", typeof(decimal));
            table.Columns.Add("CheckTypeIds", typeof(string));
            table.Columns.Add("locationIds", typeof(string));
            table.Columns.Add("Groups", typeof(string));
            table.Columns.Add("DiscountType", typeof(int));
            // table.Columns.Add("IsActive", typeof(bool));
            table.Columns.Add("DTServerDate", typeof(DateTime));
            table.Columns.Add("DTTerminalDate", typeof(DateTime));

            foreach (Provision provision in provisions)
            {
                DataRow dr = table.NewRow();
                dr["PromotionId"] = promotionId;
                dr["PromoProvisionId"] = provision.ProvisionId;
                dr["Value"] = provision.Value;
                if (provision.MinTrxAmount == null)
                    dr["MinAmount"] = DBNull.Value;
                else
                    dr["MinAmount"] = provision.MinTrxAmount;

                if (provision.MaxTrxAmount == null)
                    dr["MaxAmount"] = DBNull.Value;
                else
                    dr["MaxAmount"] = provision.MaxTrxAmount;
                dr["CheckTypeIds"] = provision.CheckTypeIds;
                dr["locationIds"] = provision.LocationIds;
                dr["Groups"] = provision.Groups;
                dr["DiscountType"] = (int)provision.DiscountType;
                //dr["IsActive"] = provision.IsActive;
                dr["DTTerminalDate"] = TCF.Zeo.Common.Util.Helper.GetTimeZoneTime(context.TimeZone);
                dr["DTServerDate"] = DateTime.Now;
                table.Rows.Add(dr);
            }

            dataSet.Tables.Add(table);

            return dataSet;
        }
    }
}
