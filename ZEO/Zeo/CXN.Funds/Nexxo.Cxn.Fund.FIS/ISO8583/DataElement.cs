using System;

namespace MGI.Cxn.Fund.FIS.ISO8583
{
	public class DataElement
	{
		public int id;
		public string value;

		public DataElement() { }

		public DataElement( string s )
		{
			string[] pair = s.Split( '|' );
			id = int.Parse(pair[0]);
			value = pair[1];
		}

		public DataElement( int Id, string Value )
		{
			id = Id;
			value = Value;
		}

		public override string ToString()
		{
			return string.Format("{0}|{1}",id,value);
		}
	}
}
