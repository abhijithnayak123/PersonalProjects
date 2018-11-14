using System;
using System.Security.Cryptography.X509Certificates;
using MGI.Providers.WU.DASMonitor.WUService;
using HeartBeatSvc = MGI.Providers.WU.DASMonitor.HeartBeatService;
using System.Collections.Generic;
using System.Linq;
using MGI.Cxn.MoneyTransfer.WU.Data;
using AutoMapper;
using Spring.Transaction.Interceptor;
using MGI.Common.DataAccess.Contract;
using MGI.Cxn.WU.Common.Data;
using Spring.Data.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using System.Text.RegularExpressions;


namespace MGI.Providers.WU.DASMonitor
{
	public class WesternUnionIO : IDASServices
	{

		private const string Spanish = "es";
		private const string English = "en";

		channel channel = null;
		foreign_remote_system remotesystem = null;
		X509Certificate2 wuCertificate = null;
		static long channelPartnerID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("ChannelPartnerID"));
		static int ProviderID = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings.Get("ProviderID"));
        string _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PTNRDBConnection"].ConnectionString;
		string PTNRDatabase = System.Configuration.ConfigurationManager.AppSettings.Get("PTNRDatabase");
		string _wUServiceURL = System.Configuration.ConfigurationManager.AppSettings.Get("WUServiceURL");
		static string BillerImportPartnerIDs = System.Configuration.ConfigurationManager.AppSettings.Get("BillerImportPartnerIDs");


		#region Constructor

		public WesternUnionIO()
		{
			DASServiceMapper();
		}

		#endregion

		public IRepository<WUCountry> WUCountryRepo { get; set; }
		public IRepository<WUState> WUStateRepo { get; set; }
		public IRepository<WUQQCcompanyNames> WUBillersRepo { get; set; }
		public IRepository<WUCountryCurrency> WUCountryCurrencyRepo { get; set; }
		public IRepository<WUErrorMessages> WUErrorMessagesRepo { get; set; }
		public IRepository<WUCity> WUCitiesRepo { get; set; }
		public IRepository<WUCredential> WUCredentialRepo { get; set; }
		public IRepository<DeliveryServiceTransalation> WUDeliveryServiceRepo { get; set; }
		public IRepository<CountryTransalation> WUContryTranslationRepo { get; set; }


		#region Public Methods

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public void CheckHeartBeat()
		{
			try
			{
				WUCredential wuCredentials = new WUCredential();
				HeartBeatSvc.HeartBeatPortTypeClient hc;
				wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerID);
				ConfigureWUOject(wuCredentials);
				string url;
				if (_wUServiceURL == string.Empty)
				{
					hc = new HeartBeatSvc.HeartBeatPortTypeClient("HeartBeat_SOAP_HTTP_Port", wuCredentials.WUServiceUrl.ToString());
					url = wuCredentials.WUServiceUrl.ToString();
				}
				else
				{
					hc = new HeartBeatSvc.HeartBeatPortTypeClient("HeartBeat_SOAP_HTTP_Port", _wUServiceURL);
					url = _wUServiceURL;
				}
				hc.ClientCredentials.ClientCertificate.Certificate = wuCertificate;
				HeartBeatSvc.espheartbeatrequest request = new HeartBeatSvc.espheartbeatrequest();
				HeartBeatSvc.espheartbeatreply response = new HeartBeatSvc.espheartbeatreply();
				request.external_reference_no = DateTime.Now.ToString("yyyyMMddhhmmssff");
				response = hc.HeartBeatService(request);
				Logger.WriteLogHeartBeat("WebService URL :" + url + "\r\n" + "\t\t      " + "HeartBeat Strength:" + response.status.message.ToString());
			}
			catch (Exception ex)
			{
				Logger.WriteLogHeartBeat("DASMonitor Exception -  HeartBeat checking:" + ex.Message);
			}

		}

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public void PopulateWUCountries()
		{
			try
			{
				List<WUCountry> countries = WUCountryRepo.All().ToList<WUCountry>();
				List<WUCountry> WUCountries = GetDestinationCountries(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")), English);

				if (WUCountries.Count > 0)
				{
					WUCountryRepo.Delete(countries);
					WUCountryRepo.Add(WUCountries);
					WUCountryRepo.Flush();
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog("PopulateWUCountries failed " + ex.Message);
			}

		}

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public void PopulateWUCountryStates()
		{
			try
			{
				List<WUState> states = WUStateRepo.All().ToList<WUState>();
				List<WUState> usStates = GetUSStates(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")));
				List<WUState> mxStates = GetMexicoCityState(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")));

				if (states.Where(c => c.ISOCountryCode.ToLower().ToString() == "ca").Count() == 0)
				{
					List<WUState> caStates = PopulateCanadaStates();
					WUStateRepo.Add(caStates);
					WUStateRepo.Flush();
				}

				if (usStates.Count() > 0 & mxStates.Count() > 0)
				{
					WUStateRepo.Delete(states.Where(c => c.ISOCountryCode.ToLower().ToString() != "ca"));
					WUStateRepo.Add(usStates);
					WUStateRepo.Flush();
					WUStateRepo.Add(mxStates);
					WUStateRepo.Flush();
				}

			}
			catch (Exception ex)
			{
				Logger.WriteLog("PopulateWUCountryStates failed " + ex.Message);
			}
		}

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public void PopulateWUMexxicoCity()
		{
			try
			{

				List<WUCity> cities = WUCitiesRepo.All().ToList<WUCity>();
				List<WUCity> mxcities = GetMexicoCities(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")));
				if (mxcities.Count() > 0)
				{
					WUCitiesRepo.Delete(cities);
					WUCitiesRepo.Add(mxcities);
					WUCitiesRepo.Flush();
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog("PopulateWUMexxicoCity failed " + ex.Message);
			}
		}

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public void PopulateWUQQCompanyNames()
		{
			try
			{
				List<WUQQCcompanyNames> wuBillers = GetBillers(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")));
				int _result = 0;
				if (wuBillers.Count != 0)
				{
					XElement root = new XElement("Billers");
					for (int i = 0; i < wuBillers.Count(); i++)
					{

						XElement Element = new XElement("Biller", new XElement("Name", wuBillers[i].CompanyName.ToString()),
																  new XElement("ISOCountryCode", wuBillers[i].ISOCountryCode.ToString()),
																  new XElement("Country", wuBillers[i].Country.ToString()),
																  new XElement("CurrencyCode", wuBillers[i].CurrencyCode.ToString()));
						root.Add(Element);
					}

					//Populating Catalog table.
					SqlConnection con = null;
					try
					{
						con = _GetConnection(_ConnectionString);
						con.Open();
						SqlCommand cmd = new SqlCommand("USP_PopulateCXNCatalog", con);
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
						_result = Convert.ToInt32(cmd.Parameters["@result"].Value);
						CloseConnection(con);
						Logger.WriteLog("==============" + "Populating tWUnion_Catalog  Succeeded.");
					}
					catch (SqlException ex)
					{
						Logger.WriteLog("==============" + "Populating tWUnion_Catalog failed - " + ex.Message);
					}
					finally
					{
						if (con.State != ConnectionState.Open)
							con.Close();
						con.Dispose();
					}

					if (_result == 1)
					{
						//Populating Mastercatalog and PartnerCatalog.						
						try
						{
							string[] partnerIDs = BillerImportPartnerIDs.Split(',');
							con = _GetConnection(_ConnectionString);
							con.Open();
							using (SqlConnection cxnconnection = _GetConnection(_ConnectionString))
                            {
                                PTNRDatabase = cxnconnection.Database;   
                            }
							foreach (string channelPartnerID in partnerIDs)
							{
								SqlCommand cmd = new SqlCommand("PopulateCatalog", con);
								cmd.CommandType = System.Data.CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("@DATABASENAME", PTNRDatabase);
								cmd.Parameters.AddWithValue("@PROVIDERID", ProviderID);
								cmd.Parameters.AddWithValue("@CHANNELPARTNERID", channelPartnerID);
								SqlParameter parm2 = new SqlParameter("@RESULT", System.Data.SqlDbType.Int);
								parm2.Direction = ParameterDirection.Output;
								cmd.Parameters.Add(parm2);
								cmd.ExecuteNonQuery();
								int _result1 = Convert.ToInt32(cmd.Parameters["@RESULT"].Value);

							}
							CloseConnection(con);
							Logger.WriteLog("==============" + "Populating Mastercatalog and PartnerCatalog  Succeeded.");
						}
						catch (SqlException ex)
						{
							Logger.WriteLog("==============" + "Populating Mastercatalog and PartnerCatalog  failed - " + ex.Message);
						}
						finally
						{
							if (con.State != ConnectionState.Open)
								con.Close();
							con.Dispose();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog("==============" + "PopulateWUQQCompanyNames failed " + ex.Message);

			}
		}


		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public void PopulateWUCountriesCurrencies()
		{
			try
			{
				List<WUCountryCurrency> countryCurrencies = WUCountryCurrencyRepo.All().ToList<WUCountryCurrency>();
				List<WUCountryCurrency> wuCountryCurrencies = GetCountriesCurrencies(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")));

				if (wuCountryCurrencies.Count > 0)
				{
					WUCountryCurrencyRepo.Delete(countryCurrencies);
					WUCountryCurrencyRepo.Flush();
					WUCountryCurrencyRepo.Add(wuCountryCurrencies);
					WUCountryCurrencyRepo.Flush();
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog("PopulateWUCountriesCurrencies failed " + ex.Message);
			}
		}

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public void PopulateWUGetErrorMessagesInfo()
		{
			try
			{
				List<WUErrorMessages> errormesages = WUErrorMessagesRepo.All().ToList<WUErrorMessages>();
				List<WUErrorMessages> wuerrormessages = GetErrorMesages(Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff")));

				if (wuerrormessages.Count > 0)
				{
					WUErrorMessagesRepo.Delete(errormesages);
					WUErrorMessagesRepo.Add(wuerrormessages);
					WUErrorMessagesRepo.Flush();
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog("PopulateWUGetErrorMessagesInfo failed " + ex.Message);

			}

		}

		#endregion

		#region Private Methods

		private SqlConnection _GetConnection(string connectionstring)
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


		private List<WUCountry> GetDestinationCountries(long transactionId, string language)
		{

			filters_type filters = new filters_type();
			//TODO: check the query filter values.
			filters.queryfilter1 = language;
			filters.queryfilter2 = "US USD";

			bool hasMoreRecords = true;
			List<WUCountry> destinationCountries = new List<WUCountry>();

			Logger.WriteLog("==============" + "GetDestinationCountries - DASEnquiry : GetDestinationCountries call started");
			while (hasMoreRecords)
			{
				if (destinationCountries != null & destinationCountries.Count > 0)
				{ filters.queryfilter3 = destinationCountries.Last().Name; }

				List<ISOCOUNTRY_Type> countries = getdasResponse(transactionId, MGI.Cxn.MoneyTransfer.WU.Data.DASServices.GetDestinationCountries.ToString(), out hasMoreRecords, filters).ConvertAll<ISOCOUNTRY_Type>(t => (ISOCOUNTRY_Type)t);
				destinationCountries.AddRange(Mapper.Map<List<ISOCOUNTRY_Type>, List<WUCountry>>(countries));

			}

			Logger.WriteLog("==============" + "GetDestinationCountries - DASEnquiry: GetDestinationCountries ended");
			return destinationCountries;


		}

		private List<WUState> GetUSStates(long transactionId)
		{
			filters_type filters = new filters_type();
			//TODO: check the query filter values.
			filters.queryfilter1 = English;

			bool hasMoreRecords = true;
			List<WUState> usStates = new List<WUState>();


			Logger.WriteLog("==============" + "GetUSStates - DASEnquiry Service : GetUSStateList call started");

			while (hasMoreRecords)
			{
				if (usStates.Count > 0)
				{ filters.queryfilter2 = usStates.Last().Name; }

				List<USSTATEINFO_Type> states = getdasResponse(transactionId, MGI.Cxn.MoneyTransfer.WU.Data.DASServices.GetUSStateList.ToString(), out hasMoreRecords, filters).ConvertAll<USSTATEINFO_Type>(t => (USSTATEINFO_Type)t);
				usStates.AddRange(Mapper.Map<List<USSTATEINFO_Type>, List<WUState>>(states));
			}
			Logger.WriteLog("==============" + "GetUSStates - DASEnquiry Service : GetUSStateList ended");

			return usStates;
		}

		private List<WUCity> GetMexicoCities(long transactionId)
		{
			filters_type filters = new filters_type();
			//TODO: check the query filter values.
			filters.queryfilter1 = English;

			bool hasMoreRecords = true;
			List<WUCity> mxStates = new List<WUCity>();
			List<MEXICOCITYSTATEINFO_Type> mexicoStates = null;

			Logger.WriteLog("==============" + "GetMexicoCities - DASEnquiry Service : GetMexicoCityState call started");

			while (hasMoreRecords)
			{
				if (mxStates.Count > 0)
				{
					filters.queryfilter2 = mexicoStates.Last().CITY;
					filters.queryfilter3 = mexicoStates.Last().STATE_NAME;
				}
				mexicoStates = getdasResponse(transactionId, MGI.Cxn.MoneyTransfer.WU.Data.DASServices.GetMexicoCityState.ToString(), out hasMoreRecords, filters).ConvertAll<MEXICOCITYSTATEINFO_Type>(t => (MEXICOCITYSTATEINFO_Type)t);
				mxStates.AddRange(Mapper.Map<List<MEXICOCITYSTATEINFO_Type>, List<WUCity>>(mexicoStates));
			}

			Logger.WriteLog("==============" + "GetMexicoCities - DASEnquiry Service : GetMexicoCityState ended");

			return mxStates.Distinct().ToList();
		}

		private List<WUState> GetMexicoCityState(long transactionId)
		{
			filters_type filters = new filters_type();
			//TODO: check the query filter values.
			filters.queryfilter1 = English;

			bool hasMoreRecords = true;
			List<WUState> mxStates = new List<WUState>();
			List<MEXICOCITYSTATEINFO_Type> mexicoStates = null;

			Logger.WriteLog("==============" + "GetMexicoCityState - DASEnquiry Service : GetMexicoCityState call started");

			while (hasMoreRecords)
			{
				if (mxStates.Count > 0)
				{
					filters.queryfilter2 = mexicoStates.Last().CITY;
					filters.queryfilter3 = mexicoStates.Last().STATE_NAME;
				}
				mexicoStates = getdasResponse(transactionId, MGI.Cxn.MoneyTransfer.WU.Data.DASServices.GetMexicoCityState.ToString(), out hasMoreRecords, filters).ConvertAll<MEXICOCITYSTATEINFO_Type>(t => (MEXICOCITYSTATEINFO_Type)t);
				mxStates.AddRange(Mapper.Map<List<MEXICOCITYSTATEINFO_Type>, List<WUState>>(mexicoStates));
			}

			Logger.WriteLog("==============" + "GetMexicoCityState - DASEnquiry Service : GetMexicoCityState call ended");

			return mxStates.Distinct().ToList();
		}

		private List<WUCountryCurrency> GetCountriesCurrencies(long transactionId)
		{
			filters_type filters = new filters_type();
			filters.queryfilter1 = English;
			filters.queryfilter2 = "US USD";

			bool hasMoreRecords = true;
			List<WUCountryCurrency> countriesCurrencieslst = new List<WUCountryCurrency>();
			List<COUNTRY_CURRENCY_Type> countriesCurrencies = null;

			Logger.WriteLog("==============" + "GetCountriesCurrencies - DASEnquiry Service : GetCountriesCurrencies call started");

			while (hasMoreRecords)
			{
				if (countriesCurrencies != null && countriesCurrencies.Count > 0)
				{
					filters.queryfilter3 = countriesCurrencies.Last().COUNTRY_LONG;
					filters.queryfilter4 = countriesCurrencies.Last().CURRENCY_NAME;
				}

				countriesCurrencies = getdasResponse(transactionId, MGI.Cxn.MoneyTransfer.WU.Data.DASServices.GetCountriesCurrencies.ToString(), out hasMoreRecords, filters).ConvertAll<COUNTRY_CURRENCY_Type>(t => (COUNTRY_CURRENCY_Type)t);
				countriesCurrencieslst.AddRange(Mapper.Map<List<COUNTRY_CURRENCY_Type>, List<WUCountryCurrency>>(countriesCurrencies));
			}

			Logger.WriteLog("==============" + "GetCountriesCurrencies - DASEnquiry Service : GetCountriesCurrencies call ended");

			return countriesCurrencieslst;
		}

		private List<WUErrorMessages> GetErrorMesages(long transactionId)
		{
			filters_type filters = new filters_type();
			filters.queryfilter1 = English;

			bool hasMoreRecords = true;
			List<WUErrorMessages> errorMsglst = new List<WUErrorMessages>();
			List<ERRORINFO_Type> errormsgs = null;

			Logger.WriteLog("==============" + "GetErrorMesages - DASEnquiry Service : GetErrorMessagesInfo call started");

			while (hasMoreRecords)
			{
				if (errormsgs != null && errormsgs.Count > 0)
				{
					filters.queryfilter3 = errormsgs.Last().ERROR_CODE;
				}

				errormsgs = getdasResponse(transactionId, MGI.Cxn.MoneyTransfer.WU.Data.DASServices.GetErrorMessagesInfo.ToString(), out hasMoreRecords, filters).ConvertAll<ERRORINFO_Type>(t => (ERRORINFO_Type)t);
				errorMsglst.AddRange(Mapper.Map<List<ERRORINFO_Type>, List<WUErrorMessages>>(errormsgs));
			}

			Logger.WriteLog("==============" + "GetErrorMesages -DASEnquiry Service : GetErrorMessagesInfo call ended");
			return errorMsglst;
		}

		private List<WUState> PopulateCanadaStates()
		{
			List<WUState> States = new List<WUState>();
			WUState item1 = new WUState();
			item1.rowguid = Guid.NewGuid();
			item1.StateCode = "AB";
			item1.Name = "Alberta";
			item1.ISOCountryCode = "CA";
			item1.DTServerCreate = DateTime.Now;
			States.Add(item1);
			WUState item2 = new WUState();
			item2.rowguid = Guid.NewGuid();
			item2.StateCode = "BC";
			item2.Name = "British Columbia";
			item2.ISOCountryCode = "CA";
			item2.DTServerCreate = DateTime.Now;
			States.Add(item2);
			WUState item3 = new WUState();
			item3.rowguid = Guid.NewGuid();
			item3.StateCode = "MB";
			item3.Name = "Manitoba";
			item3.ISOCountryCode = "CA";
			item3.DTServerCreate = DateTime.Now;
			States.Add(item3);
			WUState item4 = new WUState();
			item4.rowguid = Guid.NewGuid();
			item4.StateCode = "NB";
			item4.Name = "New Brunswick";
			item4.ISOCountryCode = "CA";
			item4.DTServerCreate = DateTime.Now;
			States.Add(item4);
			WUState item5 = new WUState();
			item5.rowguid = Guid.NewGuid();
			item5.StateCode = "NL";
			item5.Name = "Newfoundland and Labrador";
			item5.ISOCountryCode = "CA";
			item5.DTServerCreate = DateTime.Now;
			States.Add(item5);
			WUState item6 = new WUState();
			item6.rowguid = Guid.NewGuid();
			item6.StateCode = "NT";
			item6.Name = "Northwest Territories";
			item6.ISOCountryCode = "CA";
			item6.DTServerCreate = DateTime.Now;
			States.Add(item6);
			WUState item7 = new WUState();
			item7.rowguid = Guid.NewGuid();
			item7.StateCode = "NS";
			item7.Name = "Nova Scotia";
			item7.ISOCountryCode = "CA";
			item7.DTServerCreate = DateTime.Now;
			States.Add(item7);
			WUState item8 = new WUState();
			item8.rowguid = Guid.NewGuid();
			item8.StateCode = "NU";
			item8.Name = "Nunavut";
			item8.ISOCountryCode = "CA";
			item8.DTServerCreate = DateTime.Now;
			States.Add(item8);
			WUState item9 = new WUState();
			item9.rowguid = Guid.NewGuid();
			item9.StateCode = "ON";
			item9.Name = "Ontario";
			item9.ISOCountryCode = "CA";
			item9.DTServerCreate = DateTime.Now;
			States.Add(item9);
			WUState item10 = new WUState();
			item10.rowguid = Guid.NewGuid();
			item10.StateCode = "PE";
			item10.Name = "Prince Edward Island";
			item10.ISOCountryCode = "CA";
			item10.DTServerCreate = DateTime.Now;
			States.Add(item10);
			WUState item11 = new WUState();
			item11.rowguid = Guid.NewGuid();
			item11.StateCode = "QC";
			item11.Name = "Quebec";
			item11.ISOCountryCode = "CA";
			item11.DTServerCreate = DateTime.Now;
			States.Add(item11);
			WUState item12 = new WUState();
			item12.rowguid = Guid.NewGuid();
			item12.StateCode = "SK";
			item12.Name = "Saskatchewan";
			item12.ISOCountryCode = "CA";
			item12.DTServerCreate = DateTime.Now;
			States.Add(item12);
			WUState item13 = new WUState();
			item13.rowguid = Guid.NewGuid();
			item13.StateCode = "YT";
			item13.Name = "Yukon";
			item13.ISOCountryCode = "CA";
			item13.DTServerCreate = DateTime.Now;
			States.Add(item13);
			return States;
		}

		private List<WUQQCcompanyNames> GetBillers(long transactionId)
		{
			filters_type filters = new filters_type();
			filters.queryfilter1 = English;
			filters.queryfilter2 = "US";
			filters.queryfilter7 = "FUSION";

			bool hasMoreRecords = true;
			List<WUQQCcompanyNames> billerlst = new List<WUQQCcompanyNames>();
			List<QQCCOMPANYNAME_Type> billers = null;

			Logger.WriteLog("==============" + "GetBillers - DASEnquiry Service : GetQQCCompanyName call started");

			while (hasMoreRecords)
			{
				if (billers != null && billers.Count > 0)
				{ filters.queryfilter5 = billers.Last().CLIENT_ID; }

				billers = getdasResponse(transactionId, MGI.Cxn.MoneyTransfer.WU.Data.DASServices.GetQQCCompanyName.ToString(), out hasMoreRecords, filters).ConvertAll<QQCCOMPANYNAME_Type>(t => (QQCCOMPANYNAME_Type)t);
				billerlst.AddRange(Mapper.Map<List<QQCCOMPANYNAME_Type>, List<WUQQCcompanyNames>>(billers));
			}

			Logger.WriteLog("==============" + "GetBillers - DASEnquiry Service : GetQQCCompanyName call ended");

			return billerlst;
		}

		private void ConfigureWUOject(WUCredential wuCredentials)
		{
			if (string.IsNullOrWhiteSpace(wuCredentials.AccountIdentifier) || string.IsNullOrWhiteSpace(wuCredentials.CounterId))
			{ throw new ArgumentNullException("Invalid AccountIdentifier or CounterId.AccountIdentier or CounterID cannot be null or empty string"); }

			channel = channel ?? SetChannelSetup(wuCredentials.ChannelName.ToString(), wuCredentials.ChannelVersion.ToString());
			remotesystem = remotesystem ?? SetRemoteSystem(wuCredentials.AccountIdentifier, wuCredentials.CounterId);
			wuCertificate = wuCertificate ?? SetWUCredentialCertificate(wuCredentials.WUClientCertificateSubjectName);
		}

		private X509Certificate2 SetWUCredentialCertificate(string wucertificatename)
		{
			//TODO: Check where the certificate will be stored.
			// get the cert from personel store, if it is installed in different store, then what???

			X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);

			// Open the store.
			certificateStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

			// Find the certificate with the specified subject.
			X509Certificate2Collection certificates = certificateStore.Certificates.Find(X509FindType.FindBySubjectName, wucertificatename, false);


			certificateStore.Close();

			if (certificates.Count < 1)
			{
				throw new Exception("Certificate not found for WU Partner Integration");
			}

			return certificates[0];
		}

		private channel SetChannelSetup(string channelName, string channelVersion)
		{

			channel chn = new channel();
			chn.type = channel_type.H2H;
			chn.name = channelName;
			chn.version = channelVersion;
			chn.typeSpecified = true;
			return chn;
		}

		private foreign_remote_system SetRemoteSystem(string wuAccountIdentifier, string counterId)
		{
			//TODO: Get the identifier and counterid from db
			foreign_remote_system frs = new foreign_remote_system();
			frs.identifier = wuAccountIdentifier;// "WGHH614900T"; // need to get this from db
			frs.counter_id = counterId;// "6149PT00001A"; // need to get this from db
			return frs;
		}

		private List<object> getdasResponse(long transactionId, string dasServiceName, out bool hasMoreRecords, filters_type queryfilters = null)
		{

			//long channelPartnerId = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings.Get("ChannelPartnerID"));			
			WUCredential wuCredentials = new WUCredential();
			DASInquiryPortTypeClient dc;
			wuCredentials = WUCredentialRepo.FindBy(x => x.ChannelPartnerId == channelPartnerID);
			ConfigureWUOject(wuCredentials);

			Logger.WriteLog("==============" + "Certificate found for WU Partner Integration ");

			List<object> responseList = new List<object>();

			if (_wUServiceURL == string.Empty)
			{
				dc = new DASInquiryPortTypeClient("SOAP_HTTP_Port", wuCredentials.WUServiceUrl.ToString());
				Logger.WriteLog("==============" + "WU Servive URL : " + wuCredentials.WUServiceUrl.ToString());
			}
			else
			{
				dc = new DASInquiryPortTypeClient("SOAP_HTTP_Port", _wUServiceURL);
				Logger.WriteLog("==============" + "WU Servive URL : " + _wUServiceURL);
			}

			dc.ClientCredentials.ClientCertificate.Certificate = wuCertificate;

			h2hdasrequest request = new h2hdasrequest();
			request.channel = channel;
			remotesystem.reference_no = transactionId.ToString();
			request.foreign_remote_system = remotesystem;
			request.name = dasServiceName;

			request.filters = queryfilters;
			REPLYType responseItems = null;
			try
			{
				Logger.WriteLog("==============" + "Connecting to DAS Service");
				responseItems = (REPLYType)dc.DAS_Service(request).MTML.Item;
			}
			catch (Exception ex)
			{
				Logger.WriteLog("==============" + "Failed to retrieve the data from DAS Service" + ex.InnerException.ToString());
			}

			hasMoreRecords = responseItems.DATA_CONTEXT.HEADER.DATA_MORE.ToString().ToUpper() == "Y" ? true : false;

			// Srini - Checked for the null condition on RECORDSET for the error occured: Object reference not set to an instance of an object
			if (responseItems.DATA_CONTEXT.RECORDSET != null)
				responseList.AddRange(responseItems.DATA_CONTEXT.RECORDSET.Items.ToList<object>());

			return responseList;
		}

		#endregion

		internal static void DASServiceMapper()
		{
			Mapper.CreateMap<WUCountry, CountryTransalation>()
				.AfterMap((s, d) =>
				{
					d.DTServerCreate = DateTime.Now;
				})
				.ForMember(c => c.CountryCode, o => o.MapFrom(s => s.CountryCode))
				.ForMember(c => c.Name, o => o.MapFrom(s => s.Name));
			Mapper.CreateMap<DASDELIVERYTRANSLATE_Type, DeliveryServiceTransalation>()
				.AfterMap((s, d) =>
				{
					d.DTServerCreate = DateTime.Now;
				})
				.ForMember(c => c.EnglishName, o => o.MapFrom(s => s.ENGLISH_MESSAGE))
				.ForMember(c => c.Name, o => o.MapFrom(s => s.TRANSL_MESSAGE));

			Mapper.CreateMap<ISOCOUNTRY_Type, WUCountry>()
				.AfterMap((s, d) =>
				{
					d.DTServerCreate = DateTime.Now;
				})
				.ForMember(c => c.CountryCode, o => o.MapFrom(s => s.ISO_COUNTRY_CD))
				.ForMember(c => c.Name, o => o.MapFrom(s => s.COUNTRY_LONG));

			Mapper.CreateMap<ISOCURRENCY_Type, WUCountryCurrency>()
				.ForMember(c => c.CountryCode, o => o.MapFrom(s => s.CURRENCY_CD.ToUpper()))
				.ForMember(c => c.CurrencyCode, o => o.MapFrom(s => s.CURRENCY_NAME.ToUpper()));

			Mapper.CreateMap<USSTATEINFO_Type, WUState>()
				.AfterMap((s, d) =>
				{
					d.ISOCountryCode = "US";
					d.DTServerCreate = DateTime.Now;
				})
				.ForMember(c => c.StateCode, o => o.MapFrom(s => s.STATE_CODE.ToUpper()))
				.ForMember(c => c.Name, o => o.MapFrom(s => s.STATE_NAME.ToUpper()));

			Mapper.CreateMap<MEXICOCITYSTATEINFO_Type, WUState>()
				.AfterMap((s, d) =>
				{
					d.ISOCountryCode = "MX";
					d.DTServerCreate = DateTime.Now;
				})
				.ForMember(c => c.StateCode, o => o.MapFrom(s => s.STATE_CODE.ToUpper()))
				.ForMember(c => c.Name, o => o.MapFrom(s => s.STATE_NAME.ToUpper())
				);

			Mapper.CreateMap<QQCCOMPANYNAME_Type, WUQQCcompanyNames>()
				.AfterMap((s, d) =>
				{
					d.DTServerCreate = DateTime.Now;
					d.IsActive = true;
					d.ChannelPartnerId = channelPartnerID;
				})
				.ForMember(c => c.CompanyName, o => o.MapFrom(s => s.COMPANY_NAME.ToUpper()))
				.ForMember(c => c.Country, o => o.MapFrom(s => s.COUNTRY.ToUpper()))
				.ForMember(c => c.CurrencyCode, o => o.MapFrom(s => s.CURRENCY_CD.ToUpper()))
				.ForMember(c => c.ISOCountryCode, o => o.MapFrom(s => s.ISO_COUNTRY_CD.ToUpper()));


			Mapper.CreateMap<COUNTRY_CURRENCY_Type, WUCountryCurrency>()
					.AfterMap((s, d) =>
					{
						d.DTServerCreate = DateTime.Now;
					})
					.ForMember(c => c.CountryCode, o => o.MapFrom(s => s.ISO_COUNTRY_CD.ToUpper()))
					.ForMember(c => c.CountryName, o => o.MapFrom(s => s.COUNTRY_LONG.ToUpper()))
					.ForMember(c => c.CountryNumCode, o => o.MapFrom(s => s.ISO_COUNTRY_NUM_CD.ToUpper()))
					.ForMember(c => c.CurrencyCode, o => o.MapFrom(s => s.CURRENCY_CD.ToUpper()))
					.ForMember(c => c.CurrencyName, o => o.MapFrom(s => s.CURRENCY_NAME.ToUpper()))
					.ForMember(c => c.CurrencyNumCode, o => o.MapFrom(s => s.ISO_CURRENCY_NUM_CD.ToUpper())
					);
			Mapper.CreateMap<ERRORINFO_Type, WUErrorMessages>()
				.AfterMap((s, d) =>
				{
					d.DTServerCreate = DateTime.Now;
				})
				.ForMember(c => c.ErrorCode, o => o.MapFrom(s => s.ERROR_CODE.ToUpper()))
				.ForMember(c => c.ErrorDesc, o => o.MapFrom(s => s.ERROR_DESC.ToUpper()));

			Mapper.CreateMap<MEXICOCITYSTATEINFO_Type, WUCity>()
			.AfterMap((s, d) =>
			{
				d.DTServerCreate = DateTime.Now;
			})
			.ForMember(c => c.StateCode, o => o.MapFrom(s => s.STATE_CODE.ToUpper()))
			.ForMember(c => c.Name, o => o.MapFrom(s => s.CITY.ToUpper()));

			Mapper.CreateMap<WUQQCcompanyNames, WUMasterCatalog>()
			.AfterMap((s, d) =>
				{
					d.DTServerCreate = DateTime.Now;
					d.ProviderId = ProviderID; //hard coded for now, this needs to taken from enum. 
				})
			.ForMember(c => c.BillerName, o => o.MapFrom(s => s.CompanyName))
			.ForMember(c => c.ChannelPartnerId, o => o.MapFrom(s => s.ChannelPartnerId))
			.ForMember(c => c.IsActive, o => o.MapFrom(s => s.IsActive))
			.ForMember(c => c.ProviderCatalogId, o => o.MapFrom(s => s.Id));

			Mapper.CreateMap<WUMasterCatalog, WUPartnerCatalog>()
			.AfterMap((s, d) =>
				{
					d.DTServerCreate = DateTime.Now;
				})
				.ForMember(c => c.BillerName, o => o.MapFrom(s => s.BillerName))
				.ForMember(c => c.ChannelPartnerId, o => o.MapFrom(s => s.ChannelPartnerId))
				.ForMember(c => c.ProviderId, o => o.MapFrom(s => s.ProviderId))
				.ForMember(c => c.Id, o => o.MapFrom(s => s.Id));
		}

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public void PopulateCountryTransalations(long transactionId, string language)
		{
			try
			{
				List<CountryTransalation> countries = WUContryTranslationRepo.All().ToList<CountryTransalation>();
				List<WUCountry> wuCountries = GetDestinationCountries(transactionId, Spanish);
				List<CountryTransalation> spanishCountryNames = Mapper.Map<List<CountryTransalation>>(wuCountries);

				foreach (var country in spanishCountryNames)
				{
					country.Name = Regex.Unescape(country.Name);
					country.Language = Spanish;
				}

				if (spanishCountryNames.Count > 0)
				{
					WUContryTranslationRepo.Delete(countries);
					WUContryTranslationRepo.Add(spanishCountryNames);
					WUCountryRepo.Flush();
				}

			}
			catch (Exception ex)
			{
				Logger.WriteLog("PopulateWUCountries failed " + ex.Message);
			}

		}

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public void PopulateDeliveryServiceTransalations(long transactionId, string language)
		{
			try
			{
				List<DeliveryServiceTransalation> deliveryOption = WUDeliveryServiceRepo.All().ToList();
				List<DeliveryServiceTransalation> wuDeliveryOption = GetDeliveryTranslations(transactionId, Spanish);

				foreach (var option in wuDeliveryOption)
				{
					option.Name = Regex.Unescape(option.Name);
					option.EnglishName = option.EnglishName.Trim(' ');
					option.Language = Spanish;
				}
				if (wuDeliveryOption.Count > 0)
				{
					WUDeliveryServiceRepo.Delete(deliveryOption);
					WUDeliveryServiceRepo.Add(wuDeliveryOption);
					WUDeliveryServiceRepo.Flush();
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog("PopulateDeliveryServiceTransalations failed " + ex.Message);
			}
		}

		private List<DeliveryServiceTransalation> GetDeliveryTranslations(long transactionId, string language)
		{

			filters_type filters = new filters_type()
			{
				queryfilter1 = language
			};

			bool hasMoreRecords = true;
			List<DeliveryServiceTransalation> translatedNames = new List<DeliveryServiceTransalation>();

			Logger.WriteLog("==============" + "GetDeliveryTranslations - DASEnquiry : GetDeliveryServiceTranslation call started");

			while (hasMoreRecords)
			{

				List<DASDELIVERYTRANSLATE_Type> deliveryServiceName = getdasResponse((Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff"))), MGI.Cxn.MoneyTransfer.WU.Data.DASServices.GetDeliveryTranslations.ToString(), out hasMoreRecords, filters).ConvertAll<DASDELIVERYTRANSLATE_Type>(t => (DASDELIVERYTRANSLATE_Type)t);
				translatedNames.AddRange(Mapper.Map<List<DASDELIVERYTRANSLATE_Type>, List<DeliveryServiceTransalation>>(deliveryServiceName));
			}


			Logger.WriteLog("==============" + "GetCountryTransalations - DASEnquiry: GetCountryTransalations ended");

			return translatedNames;
		}

	}
}
