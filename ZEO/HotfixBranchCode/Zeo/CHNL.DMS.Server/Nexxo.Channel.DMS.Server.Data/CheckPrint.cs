using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
    [DataContract]
    public class CheckPrint
    {
        public CheckPrint()
        {

        }

        [DataMember]
        public List<string> Lines { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            if (Lines != null)
            {
                str = string.Concat(str, "Lines = ");
                foreach (var line in Lines)
                {
                    str = string.Concat(str, "Line = ", line, "\r\n");
                }
            }
            return str;
        }
    }

}
