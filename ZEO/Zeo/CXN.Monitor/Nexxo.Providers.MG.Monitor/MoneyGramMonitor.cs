using MGI.Cxn.BillPay.MG.Data;
using MGI.CXN.MG.Common.Data;
using MGI.CXN.MG.Common.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using MGI.CXN.MG.Common.Contract;

namespace MGI.Providers.MG.Monitor
{
	public class MoneyGramMonitor
	{
		public IMGCommonIO MoneyGramCommonIO { get; set; }
        string PTNRConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PTNRDBConnection"].ConnectionString;
		string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PTNRDBConnection"].ConnectionString;
		static long channelPartnerID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("ChannelPartnerID"));
		static string BillerImportPartnerIDs = System.Configuration.ConfigurationManager.AppSettings.Get("BillerImportPartnerIDs");
		string PTNRDatabase = System.Configuration.ConfigurationManager.AppSettings.Get("PTNRDatabaseName");
		static int ProviderID = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings.Get("ProviderID"));

		static string BillerReceiveAgentId = System.Configuration.ConfigurationManager.AppSettings.Get("ReceiveAgentIdColumnName");
		static string BillerReceiveCode = System.Configuration.ConfigurationManager.AppSettings.Get("ReceiveCodeColumnName");
		static string BillerName = System.Configuration.ConfigurationManager.AppSettings.Get("BillerNameColumnName");
		static string BillerPoeSvcMsgENText = System.Configuration.ConfigurationManager.AppSettings.Get("PoeSvcMsgENTextColumnName");
		static string BillerPoeSvcMsgESText = System.Configuration.ConfigurationManager.AppSettings.Get("PoeSvcMsgESTextColumnName");
		static string BillerKeywords = System.Configuration.ConfigurationManager.AppSettings.Get("KeywordsColumnName");
		static string SearchFileExtension = System.Configuration.ConfigurationManager.AppSettings.Get("SearchFileExtensionValue");		
		static char SplitKeyword = Convert.ToChar(Regex.Unescape(ConfigurationManager.AppSettings.Get("BillerFileDelimiter")));

		public void PopulateMetaData()
		{
			MGI.Common.Util.MGIContext context = new MGI.Common.Util.MGIContext();
			BaseRequest request = PopulateBaseRequest();
			Console.WriteLine("Retrieving codetable data from MoneyGram...");
			CTResponse codeTable = MoneyGramCommonIO.GetMetaData(request, context);

			StateRegulatorRequest regulatorRequest = PopulateStateRegulatorRequest();
			Console.WriteLine("Retrieving state regulator data from MoneyGram...");
			StateRegulatorResponse stateRegulatorResponse = MoneyGramCommonIO.GetDoddFrankStateRegulatorInfo(regulatorRequest, context);

			Console.WriteLine("Inserting to Nexxo CXN tables...");

			BulkInsert(codeTable, stateRegulatorResponse.StateRegulators);

			Console.WriteLine("Completed successfully....");
		}

		private BaseRequest PopulateBaseRequest()
		{
			BaseRequest baseRequest = new BaseRequest()
			{
				AgentID = "43677869",
				AgentSequence = "1",
				Token = "TEST",
				Language = "en",
				TimeStamp = DateTime.Now,
				ApiVersion = "1305",
				ClientSoftwareVersion = "1"
			};
			return baseRequest;
		}

		private StateRegulatorRequest PopulateStateRegulatorRequest()
		{
			StateRegulatorRequest request = new StateRegulatorRequest()
			{
				AgentID = "43677869",
				AgentSequence = "1",
				Token = "TEST",
				Language = "en",
				TimeStamp = DateTime.Now,
				ApiVersion = "1305",
				ClientSoftwareVersion = "1",
				Languages = new string[] { "ENG" }
			};
			return request;
		}

		private static void BulkInsert(CTResponse codeTable, List<StateRegulator> stateRegulators)
		{
			DataTable countryTable = codeTable.Countries.ToDataTable<MGI.CXN.MG.Common.Data.Country>();
			DataTable stateTable = codeTable.StateProvinces.ToDataTable<StateProvince>();
			DataTable countryCurrencyTable = codeTable.CountryCurrencies.ToDataTable<CountryCurrency>();
			DataTable currencyTable = codeTable.Currencies.ToDataTable<Currency>();
			DataTable deliveryOptionTable = codeTable.DeliveryOptions.ToDataTable<DeliveryOption>();
			DataTable stateRegulatorTable = stateRegulators.ToDataTable<StateRegulator>();

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PTNRDBConnection"].ConnectionString;
			string storedProcedure = "usp_InsertMetadata";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = new SqlCommand(storedProcedure, connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@countryTable", countryTable));
				command.Parameters.Add(new SqlParameter("@stateTable", stateTable));
				command.Parameters.Add(new SqlParameter("@currencyTable", currencyTable));
				command.Parameters.Add(new SqlParameter("@countryCurrencyTable", countryCurrencyTable));
				command.Parameters.Add(new SqlParameter("@deliveryOptionTable", deliveryOptionTable));
				command.Parameters.Add(new SqlParameter("@stateRegulatorTable", stateRegulatorTable));

				connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader != null && reader.HasRows)
				{
					string errorMessage = reader.GetString(5);
					Console.WriteLine(errorMessage);
				}
			}
		}

		public void PopulateBillers()
		{
			try
			{
				Console.WriteLine("Retrieving  Biller details...");

				List<Catalog> mgBillers = GetBillers(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")));
				int result = 0;
				if (mgBillers.Count != 0)
				{

					XElement root = new XElement("Billers");
					for (int i = 1; i < mgBillers.Count; i++)
					{
						XElement Element = new XElement("Biller", new XElement("ReceiveAgentId", mgBillers[i].ReceiveAgentId.ToString()),
																  new XElement("ReceiveCode", mgBillers[i].ReceiveCode.ToString()),
																  new XElement("BillerName", mgBillers[i].BillerName.ToString()),
																  new XElement("PoeSvcMsgENText", mgBillers[i].PoeSvcMsgENText.ToString()),
																  new XElement("PoeSvcMsgESText", mgBillers[i].PoeSvcMsgESText.ToString()),
																   new XElement("Keywords", mgBillers[i].Keywords.ToString())

																  );
						root.Add(Element);
					}
					SqlConnection con = null;
					try
					{
						con = GetConnection(PTNRConnectionString);
						con.Open();
						SqlCommand cmd = new SqlCommand("USP_PopulateMGramCatalog", con);
						cmd.CommandType = System.Data.CommandType.StoredProcedure;
						SqlParameter parm1 = new SqlParameter("@billers", SqlDbType.Xml);
						parm1.Value = root.ToString();
						parm1.Direction = ParameterDirection.Input;
						cmd.Parameters.Add(parm1);
						cmd.Parameters.AddWithValue("@ChannelPartnerId", channelPartnerID);
						SqlParameter parm2 = new SqlParameter("@RESULT", System.Data.SqlDbType.Int);
						parm2.Direction = ParameterDirection.Output;
						cmd.Parameters.Add(parm2);
						cmd.ExecuteNonQuery();
						result = Convert.ToInt32(cmd.Parameters["@result"].Value);
						CloseConnection(con);
						Logger.WriteLog("==============" + "Populating tMGram_Catalog  Succeeded.");
					}
					catch (SqlException ex)
					{
						Logger.WriteLog("==============" + "Populating tMGram_Catalog failed - " + ex.Message);
						Console.WriteLine(ex.Message);
						Console.ReadLine();
					}
					finally
					{
						if (con.State != ConnectionState.Open)
							con.Close();
						con.Dispose();
					}

					if (result == 1)
					{
						//Populating Mastercatalog and PartnerCatalog.						
						try
						{
							string[] partnerIDs = BillerImportPartnerIDs.Split(',');
							con = GetConnection(ConnectionString);
							con.Open();
							foreach (string channelPartnerID in partnerIDs)
							{
								SqlCommand cmd = new SqlCommand("USP_PopulateMGramCatalog", con);
								cmd.CommandType = System.Data.CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("@DATABASENAME", PTNRDatabase);
								cmd.Parameters.AddWithValue("@PROVIDERID", ProviderID);
								cmd.Parameters.AddWithValue("@CHANNELPARTNERID", channelPartnerID);
								SqlParameter parm2 = new SqlParameter("@RESULT", System.Data.SqlDbType.Int);
								parm2.Direction = ParameterDirection.Output;
								cmd.Parameters.Add(parm2);
								cmd.ExecuteNonQuery();
								int result1 = Convert.ToInt32(cmd.Parameters["@RESULT"].Value);
							}
							CloseConnection(con);
							Console.WriteLine("Completed successfully....");
							Logger.WriteLog("==============" + "Populating Mastercatalog and PartnerCatalog  Succeeded.");
						}
						catch (SqlException ex)
						{
							Console.WriteLine();
							Console.ForegroundColor = ConsoleColor.Red;
							Logger.WriteLog("==============" + "Populating Mastercatalog and PartnerCatalog  failed - " + ex.Message);
							Console.WriteLine(ex.Message);
							Console.ResetColor();
							Console.ReadLine();
						}
						finally
						{
							if (con.State != ConnectionState.Open)
								con.Close();
							con.Dispose();
						}
					}
				}
				else
				{
					Console.WriteLine();
					Console.WriteLine("No biller details  files found - Process completed ");
				}
			}
			catch
			{
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("==============" + "Input file was not in a proper format");
				Console.ResetColor();
				Console.ReadLine();
			}
		}

		#region PrivateMethods

		private List<Catalog> GetBillers(long transactionId)
		{
			string BillerFolderPath = string.Empty;
			string BillerFilename = string.Empty;
			string BillerFileNameFormat = string.Empty;
			string BillerFullFilePath = string.Empty;
			BillerFileNameFormat = ConfigurationManager.AppSettings["MoneyGramFileFormat"];
			BillerFolderPath = ConfigurationManager.AppSettings["MoneyGramBillerFolderPath"];
			var CSVFiles = new DirectoryInfo(BillerFolderPath).GetFiles(SearchFileExtension);
			List<Catalog> mgBillersList = new List<Catalog>();
			if (CSVFiles.Length != 0)
			{
				Console.WriteLine("Found {0} Biller files", CSVFiles.Length);
				for (int i = 0; i < CSVFiles.Length; i++)
				{
					BillerFilename = CSVFiles[i].Name;
					BillerFullFilePath = BillerFolderPath + BillerFilename;
					bool IsFilename = false;
					IsFilename = BillerFilename.StartsWith(BillerFileNameFormat);

					if (IsFilename == true)
					{
						Dictionary<string, int> CSVIndexValues = new Dictionary<string, int>();
						CSVIndexValues = GetColumnIndex(BillerFullFilePath);
						var reader = new StreamReader(File.OpenRead(BillerFullFilePath));
						while (!reader.EndOfStream)
						{
							var line = reader.ReadLine();
							string[] values = line.Split(SplitKeyword);
							Catalog billers = new Catalog();
							billers.ReceiveAgentId = values[CSVIndexValues[BillerReceiveAgentId]];
							billers.ReceiveCode = values[CSVIndexValues[BillerReceiveCode]];
							billers.BillerName = values[CSVIndexValues[BillerName]];
							billers.PoeSvcMsgENText = values[CSVIndexValues[BillerPoeSvcMsgENText]];
							billers.PoeSvcMsgESText = values[CSVIndexValues[BillerPoeSvcMsgESText]];
							billers.Keywords = values[CSVIndexValues[BillerKeywords]];
							billers.ChannelPartnerId = channelPartnerID;
							mgBillersList.Add(billers);
						}

						reader.Close();
						string MoneyGramBillerArchieve =
							ConfigurationManager.AppSettings.Get("MGBillerArchiveFolderPath").EndsWith(@"\")
								? ConfigurationManager.AppSettings.Get("MGBillerArchiveFolderPath")
								: ConfigurationManager.AppSettings.Get("MGBillerArchiveFolderPath") + @"\";
						ArchiveMGBillers(MoneyGramBillerArchieve, BillerFolderPath, BillerFilename);
					}

				}
			}
			return mgBillersList;
		}

		private SqlConnection GetConnection(string connectionstring)
		{
			SqlConnection conn = new SqlConnection(connectionstring);
			return conn;
		}

		private void CloseConnection(SqlConnection conn)
		{
			try
			{
				if (conn != null)
					conn.Close();
			}
			catch { }
		}


		private void ArchiveMGBillers(string MoneyGramBillerArchive, string BillerFolderPath, string BillerFilename)
		{
			try
			{
				DirectoryInfo dirMGBillerFolder = new DirectoryInfo(BillerFolderPath);
				FileInfo[] billerFiles = dirMGBillerFolder.GetFiles(SearchFileExtension);
				if (billerFiles.Length > 0)
				{
					if (!Directory.Exists(MoneyGramBillerArchive))
						Directory.CreateDirectory(MoneyGramBillerArchive);
					if (Directory.Exists(MoneyGramBillerArchive))
					{
						foreach (FileInfo aFile in billerFiles)
						{
							if (aFile.Name == BillerFilename)
							{
								aFile.MoveTo(MoneyGramBillerArchive + aFile.Name);
							}
						}
					}
				}
			}
			catch
			{
				//NexxoDALException NeEx = new NexxoDALException(NexxoDALException.ErrorCodes.PaymentArchieveFileCreationFailed, ex.Message);
				// throw NeEx;
			}
		}

		private Dictionary<string, int> GetColumnIndex(string BillerFullFilePath)
		{
			/****************************Begin TA-50 Changes************************************************/
				//     User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
				//     Purpose: On Vera Code Scan, This call contains a Improper Resource Shutdown or Release flaw, which we solved by adding using block for StreamReader
			using (var reader = new StreamReader(File.OpenRead(BillerFullFilePath)))
			{
				var line = reader.ReadLine();
				string[] values = line.Split(SplitKeyword);
				Dictionary<string, int> IndexValues = new Dictionary<string, int>();
				IndexValues.Add(BillerReceiveAgentId, Array.IndexOf(values, BillerReceiveAgentId));
				IndexValues.Add(BillerReceiveCode, Array.IndexOf(values, BillerReceiveCode));
				IndexValues.Add(BillerName, Array.IndexOf(values, BillerName));
				IndexValues.Add(BillerPoeSvcMsgENText, Array.IndexOf(values, BillerPoeSvcMsgENText));
				IndexValues.Add(BillerPoeSvcMsgESText, Array.IndexOf(values, BillerPoeSvcMsgESText));
				IndexValues.Add(BillerKeywords, Array.IndexOf(values, BillerKeywords));
				return IndexValues;
			}
		}

		#endregion PrivateMethods

	}
}
