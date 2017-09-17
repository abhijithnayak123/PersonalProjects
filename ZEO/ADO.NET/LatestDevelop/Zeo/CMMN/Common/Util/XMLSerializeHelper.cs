using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MGI.Common.Util
{
	public static class XMLSerializeHelper
	{
		public static string Serialize<T>(T item)
		{
			try
			{
				string xmlText;
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				MemoryStream memoryStream = new MemoryStream();
				using (XmlTextWriter xmlTextWriter =
					new XmlTextWriter(memoryStream, Encoding.UTF8) { Formatting = Formatting.Indented })
				{
					xmlSerializer.Serialize(xmlTextWriter, item);
					memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
					xmlText = Encoding.UTF8.GetString(memoryStream.ToArray());
					memoryStream.Dispose();
					return xmlText;
				}
			}
			catch
			{
				return null;
			}
		}

		public static string Serialize<T>(T item, string defaultNamespace)
		{
			try
			{
				string xmlText;
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), defaultNamespace);
				MemoryStream memoryStream = new MemoryStream();
				using (XmlTextWriter xmlTextWriter =
					new XmlTextWriter(memoryStream, Encoding.UTF8) { Formatting = Formatting.Indented })
				{
					xmlSerializer.Serialize(xmlTextWriter, item);
					memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
					xmlText = Encoding.UTF8.GetString(memoryStream.ToArray());
					memoryStream.Dispose();
					return xmlText;
				}
			}
			catch
			{
				return null;
			}
		}

		public static T DeSerialize<T>(string xmlText)
		{
			Object obj = null;
			try
			{
				XmlSerializer xs = new XmlSerializer(typeof(T));
				byte[] byteArray = new UTF8Encoding().GetBytes(xmlText);
				using (MemoryStream memoryStream = new MemoryStream(byteArray))
				{
					XmlTextReader xtr = new XmlTextReader(memoryStream);
					obj = xs.Deserialize(xtr);
				}
				return (T)obj;
			}
			catch
			{
				return (T)obj;
			}
		}
	}
}
