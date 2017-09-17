using System.Runtime.Serialization;
using System.Text;
using MGI.Common.Util;
namespace MGI.Channel.Shared.Server.Data
{
    public class Employment
    {
        public Employment() { }
		[DataMember]
        public string Occupation { get; set; }
        [DataMember]
        public string OccupationDescription { get; set; }
        [DataMember]
        public string Employer { get; set; }
        [DataMember]
        public string EmployerPhone { get; set; }

        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("	Occupation: {0}", NexxoUtil.safeSQLString(Occupation)));
            sb.AppendLine(string.Format("	OccupationDescription: {0}", NexxoUtil.safeSQLString(OccupationDescription)));
            sb.AppendLine(string.Format("	Employer: {0}", NexxoUtil.safeSQLString(Employer)));
            sb.AppendLine(string.Format("	EmployerPhone: {0}", NexxoUtil.safeSQLString(EmployerPhone)));
            return sb.ToString();

        }

    }
}
