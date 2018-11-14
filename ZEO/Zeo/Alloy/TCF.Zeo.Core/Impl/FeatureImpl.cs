using P3Net.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Common.Util;
using static TCF.Zeo.Common.Util.Helper;
using P3Net.Data;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : IFeatureService
    {
        public List<FeatureDetails> GetFeatures(ZeoContext context)
        {
            try
            {
                List<FeatureDetails> features = new List<FeatureDetails>();

                StoredProcedure featureListProcedure = new StoredProcedure("usp_GetFeatures");
                
                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(featureListProcedure))
                {
                    while (datareader.Read())
                    {
                        FeatureDetails featureDetail = new FeatureDetails();
                        featureDetail.FeatureId = datareader.GetInt32OrDefault("FeatureID");
                        featureDetail.FeatureName = datareader.GetStringOrDefault("Name");
                        featureDetail.IsEnabled = datareader.GetBooleanOrDefault("IsEnable");

                        features.Add(featureDetail);
                    }
                }

                return features;
            }
            catch (Exception ex)
            {
                throw new FeatureException(FeatureException.GET_FEATRUES_FAILED, ex);
            }
        }

        public bool UpdateFeatures(List<FeatureDetails> features, ZeoContext context)
        {
            try
            {
                StoredProcedure blockedCountriesProcedure = new StoredProcedure("usp_UpdateFeatures");

                StringWriter blockedCountriesWriter = new StringWriter();

                DataSet blockedCountriesDataset = GetFeaturesDataset(features, context);

                blockedCountriesDataset.WriteXml(blockedCountriesWriter);

                DataParameter[] dataParameters = new DataParameter[] {
                    new DataParameter("blockedCountries", DbType.Xml) { Value = blockedCountriesWriter.ToString() }
                };

                blockedCountriesProcedure.WithParameters(dataParameters);

                int result = DataHelper.GetConnectionManager().ExecuteNonQuery(blockedCountriesProcedure);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw new FeatureException(FeatureException.FEATURE_UPDATE_FAILED, ex);
            }
        }


        private DataSet GetFeaturesDataset(List<FeatureDetails> features, ZeoContext context)
        {
            DataSet dataSet = new DataSet("FeaturesList");
            DataTable table = new DataTable("Feature");
            table.Columns.Add("FeatureID", typeof(long));
            table.Columns.Add("IsEnable", typeof(bool));
            table.Columns.Add("DTTerminalDate", typeof(DateTime));

            foreach (FeatureDetails feature in features)
            {
                DataRow dr = table.NewRow();
                dr["FeatureID"] = feature.FeatureId;
                dr["IsEnable"] = feature.IsEnabled;
                dr["DTTerminalDate"] = GetTimeZoneTime(context.TimeZone);
                table.Rows.Add(dr);
            }

            dataSet.Tables.Add(table);

            return dataSet;
        }
    }
}
