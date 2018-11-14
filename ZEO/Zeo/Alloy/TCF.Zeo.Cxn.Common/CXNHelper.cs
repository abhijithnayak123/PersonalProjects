using P3Net.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P3Net.Data;
namespace TCF.Zeo.Cxn.Common
{
    public class CXNHelper
    {
        public static Dictionary<string, string> GetGovtIdTypes(int providerId)
        {
            Dictionary<string, string> govtIdTypes = new Dictionary<string, string>();

            StoredProcedure procedure = new StoredProcedure("usp_GetGovtIdTypes");

            procedure.WithParameters(InputParameter.Named("providerId").WithValue(providerId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(procedure))
            {
                while (datareader.Read())
                {
                    govtIdTypes.Add(datareader.GetStringOrDefault("IdType"), datareader.GetStringOrDefault("IdTypeValue"));
                }
            }

            return govtIdTypes;
        }
    }
}
