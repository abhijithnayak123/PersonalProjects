using MGI.Common.TransactionalLogging.Contract;
using MGI.Common.TransactionalLogging.Data;
using MGI.Common.TransactionalLogging.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Net;
using System.Collections;
using System.Xml;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace MGI.Common.Util
{
	public class TLoggerCommon
	{
		private ITLogger _logger;
		public ITLogger Logger;

		public NLoggerCommon NLogger { get; set; }
		public bool IsTransactionLogRequired { get; set; }
		public TLoggerCommon()
		{
			_logger = new TransactionLogImpl();
		}

		/// <summary>
		/// Author : Abhijith
		/// User Stroy: AL-1071 - As Operations, I want Alloy to use Transactional Log class for MO flow
		/// Description : Get the data from Reflection for object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="customerSessionId"></param>
		/// <param name="obj"></param>
		/// <param name="methodName"></param>
		/// <param name="alloyLayerName"></param>
		/// <param name="moduleName"></param>
		/// <param name="message"></param>
		/// <param name="context"></param>
		public void Info<T>(long customerSessionId, T obj, string methodName, AlloyLayerName alloyLayerName,
					ModuleName moduleName, string message, MGIContext context)
		{
			TransactionLogEntry tLogEntry;

			try
			{
				if (IsTransactionLogRequired)
				{
					tLogEntry = getLogEntry(customerSessionId, obj, methodName, alloyLayerName,
							moduleName, message, context);

					if (!isMethodExist(methodName, alloyLayerName, message))
						_logger.Savelog(tLogEntry);
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
			}
		}


		/// <summary>
		/// Author : Abhijith
		/// User Stroy: AL-3371 - As Tier 1 or Tier 2 support personnel, I need step-by-step transaction information for Check Processing
		/// Description : Get the serialized data for object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="customerSessionId"></param>
		/// <param name="obj"></param>
		/// <param name="methodName"></param>
		/// <param name="alloyLayerName"></param>
		/// <param name="moduleName"></param>
		/// <param name="message"></param>
		/// <param name="type"></param>
		/// <param name="context"></param>
		public void Info<T>(long customerSessionId, T obj, string methodName, AlloyLayerName alloyLayerName,
					ModuleName moduleName, string message, string type, MGIContext context)
		{
			TransactionLogEntry tLogEntry = null;

			try
			{
				if (IsTransactionLogRequired)
				{
					tLogEntry = getSerializeLogEntry(customerSessionId, obj, methodName, alloyLayerName,
						moduleName, message, type, context);

					tLogEntry.CXNType = type;

					if (!isMethodExist(methodName, alloyLayerName, message, type))
						_logger.Savelog(tLogEntry);
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
			}
		}

		/// <summary>
		/// Author : Abhijith
		/// User Stroy: AL-1071 - As Operations, I want Alloy to use Transactional Log class for MO flow
		/// Description : Get the data from Reflection for List<> types.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="customerSessionId"></param>
		/// <param name="obj"></param>
		/// <param name="methodName"></param>
		/// <param name="alloyLayerName"></param>
		/// <param name="productName"></param>
		/// <param name="message"></param>
		/// <param name="context"></param>
		public void ListInfo<T>(long customerSessionId, List<T> obj, string methodName, AlloyLayerName alloyLayerName,
					ModuleName moduleName, string message, MGIContext context)
		{
			try
			{
				if (IsTransactionLogRequired)
				{
					TransactionLogEntry tLogEntry = getLogEntry(customerSessionId, obj, methodName, alloyLayerName,
						moduleName, message, context);

					if (!isMethodExist(methodName, alloyLayerName, message))
						_logger.Savelog(tLogEntry);
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
			}
		}

		public void Info(string message)
		{
			try
			{
				if (IsTransactionLogRequired)
				{
					_logger.Savelog(new TransactionLogEntry() { EventSeverity = EventSeverity.INFO, MethodName = message });
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
			}
		}

		public void Info(string message, string category)
		{
			try
			{
				if (IsTransactionLogRequired)
				{
					_logger.Savelog(new TransactionLogEntry() { EventSeverity = EventSeverity.INFO, MethodName = message });
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
			}
		}

		public void Info(string message, string agentSessionId, string customerSessionId)
		{
			try
			{
				if (IsTransactionLogRequired)
				{
					_logger.Savelog(new TransactionLogEntry() { EventSeverity = EventSeverity.INFO, MethodName = message, AgentSessionId = agentSessionId, CustomerSessionId = customerSessionId });
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
			}
		}

		public void Info(string message, string agentSessionId, string customerSessionId, string bankId, string branchName)
		{
			try
			{
				if (IsTransactionLogRequired)
				{
					_logger.Savelog(new TransactionLogEntry() { EventSeverity = EventSeverity.INFO, MethodName = message, AgentSessionId = agentSessionId, CustomerSessionId = customerSessionId, BankId = bankId, BranchId = branchName });
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
			}
		}

		public void Warn(string message)
		{
			try
			{
				if (IsTransactionLogRequired)
				{
					_logger.Savelog(new TransactionLogEntry() { EventSeverity = EventSeverity.WARNING, MethodName = message });
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
			}
		}

		public void Error(string message)
		{
			try
			{
				if (IsTransactionLogRequired)
				{
					_logger.Savelog(new TransactionLogEntry() { EventSeverity = EventSeverity.ERROR, MethodName = message });
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
			}
		}

		public void Error(string message, string MajorCode, string MinorCode, string exceptionMessage, string stackTrace)
		{
			try
			{
				if (IsTransactionLogRequired)
				{
					List<string> desc = new List<string>();
					{
						desc.Add(message);
						desc.Add("MajorCode: " + MajorCode.ToString());
						desc.Add("MinorCode: " + MinorCode.ToString());
						desc.Add("Exception Message: " + exceptionMessage);
						desc.Add("StackTrace: " + stackTrace);
					}

					TransactionLogEntry tlog = new TransactionLogEntry();
					tlog.Desc = desc;

					_logger.Savelog(tlog);
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
			}
		}

		/// <summary>
		/// Author : Abhijith
		/// User Stroy: AL-1071 - As Operations, I want Alloy to use Transactional Log class for MO flow
		/// Description : Get the data from Reflection for object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="methodName"></param>
		/// <param name="alloyLayerName"></param>
		/// <param name="moduleName"></param>
		/// <param name="message"></param>
		/// <param name="exceptionMessage"></param>
		/// <param name="stackTrace"></param>
		public void Error<T>(T obj, string methodName, AlloyLayerName alloyLayerName, ModuleName moduleName,
			string message, string exceptionMessage, string stackTrace)
		{
			try
			{
				if (IsTransactionLogRequired)
				{
					// This context is used by the below private method - "getLogEntry" and its not used. For reuse purpose i am passing here.
					MGIContext context = new MGIContext();

					TransactionLogEntry tLogEntry = getLogEntry(0, obj, methodName, alloyLayerName,
						moduleName, message, context);

					tLogEntry.EventSeverity = EventSeverity.ERROR;
					tLogEntry.Desc.Add(message);
					tLogEntry.Desc.Add("Exception Message: " + exceptionMessage);
					tLogEntry.Desc.Add("StackTrace: " + stackTrace);

					if (!isMethodExist(methodName, alloyLayerName, message))
						_logger.Savelog(tLogEntry);
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
			}
		}

		/// <summary>
		/// Author : Abhijith
		/// User Stroy: AL-1071 - As Operations, I want Alloy to use Transactional Log class for MO flow
		/// Description : Get the data from Reflection for object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="methodName"></param>
		/// <param name="alloyLayerName"></param>
		/// <param name="moduleName"></param>
		/// <param name="message"></param>
		/// <param name="exceptionMessage"></param>
		/// <param name="stackTrace"></param>
		public void ListError<T>(List<T> obj, string methodName, AlloyLayerName alloyLayerName, ModuleName moduleName,
			string message, string exceptionMessage, string stackTrace)
		{
			try
			{
				if (IsTransactionLogRequired)
				{
					// This context is used by the below private method - "getLogEntry" and its not used. For reuse purpose i am passing here.
					MGIContext context = new MGIContext();

					TransactionLogEntry tLogEntry = getLogEntry(0, obj, methodName, alloyLayerName,
						moduleName, message, context);

					tLogEntry.EventSeverity = EventSeverity.ERROR;
					tLogEntry.Desc.Add(message);
					tLogEntry.Desc.Add("Exception Message: " + exceptionMessage);
					tLogEntry.Desc.Add("StackTrace: " + stackTrace);

					if (!isMethodExist(methodName, alloyLayerName, message))
						_logger.Savelog(tLogEntry);
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
			}
		}

		public void WriteLogIf(bool condition, string message)
		{
			try
			{
				if (IsTransactionLogRequired)
				{
					if (condition)
						_logger.Savelog(new TransactionLogEntry() { EventSeverity = EventSeverity.INFO, MethodName = message });
				}
			}
			catch (Exception ex)
			{
				NLogger.Error(ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
			}
		}

		/// <summary>
		/// Author : Abhijith
		/// User Stroy: AL-1071 - As Operations, I want Alloy to use Transactional Log class for MO flow
		/// Description : Get the data from Reflection for object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="customerSessionId"></param>
		/// <param name="obj"></param>
		/// <param name="methodName"></param>
		/// <param name="alloyLayerName"></param>
		/// <param name="moduleName"></param>
		/// <param name="message"></param>
		/// <param name="context"></param>
		/// <param name="severity"></param>
		/// <returns></returns>
		private TransactionLogEntry getLogEntry<T>(long customerSessionId, T obj, string methodName, AlloyLayerName alloyLayerName,
					ModuleName moduleName, string message, MGIContext context)
		{
			TransactionLogEntry logEntry = PopulateTransactionLogEntry(customerSessionId, methodName, alloyLayerName, moduleName, message, context);

			if (obj == null)
				return logEntry;

			// If the type is String or primitive types.
			if (obj.GetType().IsValueType || obj.GetType().FullName == "System.String")
			{
				logEntry.Desc.Add(Convert.ToString(obj));
			}
			else
			{
				var properties = obj.GetType().GetProperties();

				foreach (PropertyInfo prop in properties)
				{
					TypeCode typeCode = Type.GetTypeCode(prop.PropertyType);

					if ((prop.PropertyType.IsClass && prop.PropertyType.Assembly.FullName == typeof(T).Assembly.FullName)
							|| prop.PropertyType.IsGenericType || typeCode == TypeCode.Object)
						continue;

					logEntry.Desc.Add(prop.Name + " : " + obj.GetType().GetProperty(prop.Name).GetValue(obj, null));
				}
			}

			return logEntry;
		}

		/// <summary>
		/// Author : Abhijith
		/// User Stroy: AL-3371 - As Tier 1 or Tier 2 support personnel, I need step-by-step transaction information for Check Processing
		/// Description : Get the serialized data for object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="customerSessionId"></param>
		/// <param name="obj"></param>
		/// <param name="methodName"></param>
		/// <param name="alloyLayerName"></param>
		/// <param name="moduleName"></param>
		/// <param name="message"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		private TransactionLogEntry getSerializeLogEntry<T>(long customerSessionId, T obj, string methodName, AlloyLayerName alloyLayerName,
					ModuleName moduleName, string message, string type, MGIContext context)
		{
			TransactionLogEntry logEntry = PopulateTransactionLogEntry(customerSessionId, methodName, alloyLayerName, moduleName, message, context);

			if (obj == null)
				return logEntry;

			logEntry.Desc.Add(type + ": " + WriteLog<T>(obj, type));

			return logEntry;
		}


		/// <summary>
		/// Author : Abhijith
		/// User Stroy: AL-1071 - As Operations, I want Alloy to use Transactional Log class for MO flow
		/// Description : Get the data from Reflection for List<> types.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="customerSessionId"></param>
		/// <param name="obj"></param>
		/// <param name="methodName"></param>
		/// <param name="alloyLayerName"></param>
		/// <param name="productName"></param>
		/// <param name="message"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		private TransactionLogEntry getLogEntry<T>(long customerSessionId, List<T> obj, string methodName, AlloyLayerName alloyLayerName,
					ModuleName moduleName, string message, MGIContext context)
		{
			int count = 1;

			TransactionLogEntry logEntry = PopulateTransactionLogEntry(customerSessionId, methodName, alloyLayerName, moduleName, message, context);

			if (obj == null)
				return logEntry;

			IList objList = obj as IList;

			foreach (var tobj in objList)
			{
				// If the type is String or primitive types.
				if (tobj.GetType().IsValueType || tobj.GetType().FullName == "System.String")
				{
					logEntry.Desc.Add(Convert.ToString(tobj));
				}
				else
				{
					//var properties = typeof(T).GetProperties();
					var properties = tobj.GetType().GetProperties();

					if (alloyLayerName != AlloyLayerName.CXN)
						logEntry.Desc.Add(Convert.ToString(moduleName) + "Transaction Number : " + count);

					foreach (PropertyInfo prop in properties)
					{
						if ((prop.PropertyType.IsClass && prop.PropertyType.Assembly.FullName == typeof(T).Assembly.FullName)
							|| prop.PropertyType.IsGenericType)
							continue;

						logEntry.Desc.Add(prop.Name + " : " + tobj.GetType().GetProperty(prop.Name).GetValue(tobj, null));
					}

					count++;
				}
			}

			return logEntry;
		}


		/// <summary>
		/// Author : Abhijith
		/// User Stroy: AL-3371 - As Tier 1 or Tier 2 support personnel, I need step-by-step transaction information for Check Processing
		/// Description : Get the serialized data for object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="message"></param>
		/// <param name="messageHeader"></param>
		private string WriteLog<T>(T message, string messageHeader)
		{
			string xmlStr = string.Empty;

			List<string> nologsattributes = new List<string>() { "governmentidnumber", "id_number", "cardnumber", "Pan", "AlloyID", "idNbr", "ssn", "SSN", "ID", "debtor_account_number", "CardNbr", "cardNumber", "ns2:number", "acctNbr" };

			XmlDocument document = new XmlDocument();
			document.XmlResolver = null;

			XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
			using (StringWriter sww = new StringWriter())
			using (XmlWriter writer = XmlWriter.Create(sww))
			{
				try
				{
					xsSubmit.Serialize(writer, message);
					xmlStr = sww.ToString(); // Your XML
				}
				catch
				{
					MemoryStream memoryStream = new MemoryStream();
					StreamReader reader = new StreamReader(memoryStream);
					DataContractSerializer serializer = new DataContractSerializer(message.GetType());
					serializer.WriteObject(memoryStream, message);
					memoryStream.Position = 0;
					xmlStr = reader.ReadToEnd();
				}
			}

			document.LoadXml(xmlStr);

			foreach (var item in nologsattributes)
			{
				XmlNodeList xelements = document.GetElementsByTagName(item);


				foreach (XmlNode element in xelements)
				{
					if (element.Name == "ID" && element.SelectSingleNode("Value") == null)
						continue;

					if (element.Name == "ID" && element.SelectSingleNode("Value") != null && element.SelectSingleNode("Value").InnerText != "")
					{
						XmlNode idNode = element.SelectSingleNode("Value");
						int itemlength = idNode.InnerText.Length;
						idNode.InnerText = idNode.InnerText.Substring(itemlength - 4);
						idNode.InnerText = idNode.InnerText.PadLeft(itemlength, 'X');
						continue;
					}

					if (element.InnerText != "" && element.InnerText.Length > 3)
					{
						if (element.Name == "CardNbr" && element.InnerText != "" && element.InnerText.Length == 6)
						{
							int itemlength = element.InnerText.Length;
						}
						else
						{
							int itemlength = element.InnerText.Length;
							element.InnerText = element.InnerText.Substring(itemlength - 4);
							element.InnerText = element.InnerText.PadLeft(itemlength, 'X');
						}
					}

				}
			}

			return FormatXMLString(document.InnerXml.ToString());

		}


		/// <summary>
		/// Author : Abhijith
		/// User Stroy: AL-3371 - As Tier 1 or Tier 2 support personnel, I need step-by-step transaction information for Check Processing
		/// Description : Get the serialized data for object.
		/// </summary>
		/// <param name="sUnformattedXML"></param>
		/// <returns></returns>
		private static string FormatXMLString(string sUnformattedXML)
		{
			XmlDocument xd = new XmlDocument();
			//Purpose: On Vera Code Scan, Improper Restriction of XML External Entity Reference ('XXE') FLaw, to overcome it we are making xmlresolver null
			xd.XmlResolver = null;
			xd.LoadXml(sUnformattedXML);
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			XmlTextWriter xtw = null;
			try
			{
				xtw = new XmlTextWriter(sw);
				xtw.Formatting = Formatting.Indented;
				xd.WriteTo(xtw);
			}
			finally
			{
				if (xtw != null)
					xtw.Close();
			}
			return sb.ToString();
		}


		/// <summary>
		/// Author : Abhijith
		/// User Stroy: AL-3371 - As Tier 1 or Tier 2 support personnel, I need step-by-step transaction information for Check Processing
		/// Description : Get the serialized data for object.
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="methodName"></param>
		/// <param name="alloyLayerName"></param>
		/// <param name="moduleName"></param>
		/// <param name="message"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		private TransactionLogEntry PopulateTransactionLogEntry(long customerSessionId, string methodName, AlloyLayerName alloyLayerName,
					ModuleName moduleName, string message, MGIContext context)
		{
			TransactionLogEntry logEntry = new TransactionLogEntry()
			{
				Id = ObjectId.GenerateNewId(),
				CustomerSessionId = Convert.ToString(customerSessionId),
				ApplicationServer = TransactionLogApplicationServer.ALLOY,
				AgentSessionId = Convert.ToString(context.AgentSessionId),
				AlloyLayerName = alloyLayerName,
				LocationId = Convert.ToString(context.LocationId),
				LocationName = Convert.ToString(context.LocationName),
				TerminalName = Convert.ToString(context.TerminalName),
				AgentName = context.AgentName,
				ChannelPartnerName = Convert.ToString(context.ChannelPartnerName),
				BankId = context.BankId,
				BranchId = Convert.ToString(context.BranchId),
				EventSeverity = EventSeverity.INFO,
				MethodName = methodName,
				HostDevice = Dns.GetHostName(),
				Message = message,
				ModuleName = Convert.ToString(moduleName),
				Timestamps = DateTime.Now
			};

			return logEntry;
		}


		private bool isMethodExist(string methodName, AlloyLayerName alloyLayerName, string message)
		{
			List<TransactionLogEntry> entries = _logger.Readlog();

			bool isRes = entries.Any(e => e.AlloyLayerName == alloyLayerName && e.MethodName == methodName &&
					e.Message == message);

			return isRes;
		}


		private bool isMethodExist(string methodName, AlloyLayerName alloyLayerName, string message, string type)
		{
			List<TransactionLogEntry> entries = _logger.Readlog();

			bool isRes = entries.Any(e => e.AlloyLayerName == alloyLayerName && e.MethodName == methodName &&
					e.Message == message && e.CXNType == type);

			return isRes;
		}
	}
}
