using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FileHelpers;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data;
using MGI.Security.Voltage;

namespace VerfImport
{
    class Program
    {
        static string logfile = "VerfLog" + DateTime.Today.ToString("yyyyMMdd") + ".log";
        static string cxnconnectionstring = null;
        static string cxeconnectionstring = null;
        static string ptnrconnectionstring = null;
        
        static void Main(string[] args)
        {

            try
            {

                //Try to read connectionstrings
                if (ConfigurationManager.ConnectionStrings["CXNDatabaseConnection"] != null)
                { cxnconnectionstring = ConfigurationManager.ConnectionStrings["CXNDatabaseConnection"].ConnectionString; }

                if (ConfigurationManager.ConnectionStrings["CXNDatabaseConnection"] != null)
                { cxeconnectionstring = ConfigurationManager.ConnectionStrings["CXEDatabaseConnection"].ConnectionString; }

                if (ConfigurationManager.ConnectionStrings["CXNDatabaseConnection"] != null)
                { ptnrconnectionstring = ConfigurationManager.ConnectionStrings["PTNRDatabaseConnection"].ConnectionString; }

                LoadRecordsToDB();
                DataSet ds = ReadAllAccounts();
                int exceptionrows = AssociateAccounts(ds);
                TokenizePAN(ds);
                WriteSummary(ds, exceptionrows);
            }
            catch (Exception ex)
            {
                WriteToConsole("Exception\t:\tProbable unhandled exception");
                WriteToConsole("Exception Message\t:\t" + ex.Message);
                WriteToConsole("Exception Stack Trace\t:\t" + ex.StackTrace);
            }
            WriteToConsole("End of conversion process...");
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            Environment.Exit(0);
        }

        private static void WriteSummary(DataSet ds, int exceptionrows)
        {
            string lineseperator = new String('=', 20);
            WriteToConsole(Environment.NewLine);
            WriteToConsole("INFO\t:\t" + lineseperator);
            WriteToConsole("INFO\t:\tConversion Process Summary");
            WriteToConsole("INFO\t:\tTotal No Of Records in TsysAccount table is " + ds.Tables["TsysAccount"].Rows.Count.ToString());
            WriteToConsole("INFO\t:\tTotal No Of Records in TsysAccount table with duplicate external keys is " + ds.Tables["TsysAccount"].Select("[IsDuplicate] = true").Count().ToString());
            WriteToConsole("INFO\t:\tNo of records migrated from tsys to visa is : " + ds.Tables["TVISATsysAccounts"].Rows.Count.ToString());
            WriteToConsole("INFO\t:\tNo of records failed conversion for various exception scenarios is : " + exceptionrows.ToString());
            WriteToConsole("INFO\t:\t" + lineseperator);
            WriteToConsole(Environment.NewLine);
        }

        private static void TokenizePAN(DataSet sourceds)
        {
            WriteToConsole("INFO\t:\tStarting tokenization process");
            SecureData securedata = new SecureData();
            Dictionary<string, ValuePair> customDictionary = new Dictionary<string, ValuePair>();

            foreach (DataRow row in sourceds.Tables["TVISAAccount"].Rows)
            {
                ValuePair v = new ValuePair() { Format = "First 4 Last 4 SST", FieldValue = row["CardNumber"].ToString() };
                customDictionary.Add(row["CardNumber"].ToString(), v);
            }

            customDictionary = securedata.Tokenize(customDictionary);

            sourceds.Tables["TVISAAccount"].Columns.Add(new DataColumn("TokenizedPan", typeof(string)));
            sourceds.Tables["TVISAAccount"].AcceptChanges();

            foreach (var item in customDictionary)
            {
                DataRow dr = sourceds.Tables["TVISAAccount"].Select("CardNumber=" + item.Key).FirstOrDefault();
                if (dr != null)
                {
                    dr["TokenizedPan"] = item.Value.FieldValue;
                }
            }
            WriteToConsole("INFO\t:\tTokenization process complete");
            WriteToConsole("INFO\t:\tUpdating tokenized data to database");
            SqlConnection sqlconn = null;
            using (sqlconn = new SqlConnection(cxnconnectionstring))
            {

                WriteToConsole("INFO\t:\tUpdating tokenized data");
                SqlDataAdapter da = new SqlDataAdapter();
                da.UpdateCommand = new SqlCommand("Update tvisa_account SET CardNumber = ISNULL(@TokenizedPan,@CardNumber) WHERE CardNumber = @CardNumber", sqlconn);
                da.UpdateCommand.Parameters.Add("@TokenizedPan", SqlDbType.NVarChar, 25, "TokenizedPan");
                da.UpdateCommand.Parameters.Add("@CardNumber", SqlDbType.NVarChar, 25, "CardNumber");
                da.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                da.UpdateBatchSize = 1000;
                da.Update(sourceds.Tables["TVISAAccount"]);
                //da.Fill(ds.Tables.Add("TsysAccount"));
            }
            WriteToConsole("INFO\t:\tTokenized data update to database complete");
        }

        private static int AssociateAccounts(DataSet sourceds)
        {
            //The fields to pick up from VERF are as follows: 
            //Proxy ID Position 1134-1152 Length 19.
            //Alias ID Position 55-73 Length 19.

            //Alloy will receive a file from VISA with TSYS External ID mapped to Proxy ID and a new Alias ID created for existing TSYS customers. 
            //Alloy will read through the file, match Proxy ID against the TSYS External ID and create a VISA DPS record for the customer with the Alias ID.
            //In each CRDBUYPRFL record, look up position 1134-1152 to get the Proxy ID. Lookup Alloy internal table tTSys_Account and match it against the field ExternalKey. 
            //Left fill the 10-digit External Key with 0's to make it 19 chars. If match found, pick up the Alias ID and create the VISA record for the customer in tVISA_Account table. 

            //Populate the table with the rest of the required fields from tFIS_Account (customer profile information table)
            //1. Total Number of TSYS customers in Alloy should be equal to Number of records inserted into VISA table
            //2. Alloy will left fill the External ID with 0 to make it 19 digits. Compared against Proxy ID from VERF, should match. 
            //3. Proxy ID, Alias ID, customer details are copied from the correct fields into corresponding VISA details
            //4. Proxy ID in VISA record will store as a 19-digit number
            //5. For all migrated records, channel partner ID will be Synovus Channel partner ID.
            //6.Total Number of TSYS customers in Alloy should be equal to total number of customers in VISA table with Synovus channel partner ID 
            //7. Any record in Alloy that doesn't have matching record in VERF should be compiled and produced as a report to Product for further analysis and discussion with VISA DPS.

            List<DataRow> ValidTsysAccounts = sourceds.Tables["TsysAccount"].Select("[IsDuplicate] = false").ToList<DataRow>();

            int exceptionrows = 0;

            foreach (DataRow row in ValidTsysAccounts)
            {
                DataRow[] dr = sourceds.Tables["VerfRecords"].Select(string.Format("[ProxyId] = '{0}'", row["ExternalKey"].ToString().PadLeft(19, '0')));

                if (dr.Count() == 1)
                {

                    DataRow ptnrrow = sourceds.Tables["PTNRAccounts"].Select("CXNID=" + row["TsysAccountId"]).FirstOrDefault();
                    if (ptnrrow == null)
                    { exceptionrows += 1; WriteToConsole("Exception Row - PTNRAccounts\t:\tUnable to find corresponding PTNR Account for Tsys CXNID " + row["TsysAccountId"].ToString(), true); continue; }

                    DataRow cxeacctrow = sourceds.Tables["CXEAccounts"].Select("AccountId=" + ptnrrow["CXEId"]).FirstOrDefault();

                    if (cxeacctrow == null)
                    { exceptionrows += 1; WriteToConsole("Exception Row - CXEAccounts\t:\tUnable to find corresponding CXE Account for AccountId " + ptnrrow["CXEId"].ToString(), true); continue; }

                    DataRow cxerow = sourceds.Tables["CXECustomers"].Select("CustomerPK='" + cxeacctrow["CustomerPK"] + "'").FirstOrDefault();

                    if (cxerow == null)
                    { exceptionrows += 1; WriteToConsole("Exception Row - CXECustomer\t:\tUnable to find corresponding CXE Customer for CustomerPK " + cxeacctrow["CustomerPK"].ToString(), true); continue; }
                    
                    DataRow vdr = sourceds.Tables["TVISAAccount"].NewRow();
                    vdr["VisaAccountPK"] = Guid.NewGuid();
                    vdr["ProxyId"] = dr[0]["ProxyID"];
                    vdr["PseudoDDA"] = dr[0]["PseudoDDA"];
                    vdr["CardNumber"] = dr[0]["PrimaryAccountNumber"];
                    vdr["CardAliasId"] = dr[0]["AliasID"];
                    vdr["FirstName"] = cxerow["FirstName"];
                    vdr["MiddleName"] = cxerow["MiddleName"];
                    vdr["LastName"] = cxerow["LastName"];
                    vdr["DateOfBirth"] = cxerow["DOB"];
                    vdr["SSN"] = cxerow["SSN"];
                    vdr["Phone"] = cxerow["Phone1"];
                    vdr["Address1"] = cxerow["Address1"];
                    vdr["Address2"] = cxerow["Address2"];
                    vdr["City"] = cxerow["City"];
                    vdr["State"] = cxerow["State"];
                    vdr["ZipCode"] = cxerow["ZipCode"];
                    vdr["Country"] = cxerow["CountryOfBirth"];
                    vdr["Activated"] = row["Activated"];
                    vdr["FraudScore"] = 0;
                    vdr["ExpirationMonth"] = ((DateTime?)dr[0]["ExpirationDate"]).Value.Month.ToString();
                    vdr["ExpirationYear"] = ((DateTime?)dr[0]["ExpirationDate"]).Value.Year.ToString();
                    vdr["SubClientNodeId"] = -1;
                    vdr["DTTerminalCreate"] = DateTime.Now;
                    vdr["DTTerminalLastModified"] = DateTime.Now;
                    vdr["DTServerCreate"] = DateTime.Now;
                    vdr["DTServerLastModified"] = DateTime.Now;
                    vdr["DTAccountClosed"] = DBNull.Value;
                    vdr["IDCode"] = cxerow["IDCode"];//AL-5140
                    vdr["Email"] = cxerow["Email"];
                    vdr["MothersMaidenName"] = cxerow["MothersMaidenName"];
                    vdr["PrimaryCardAliasId"] = dr[0]["AliasID"];
                    sourceds.Tables["TVISAAccount"].Rows.Add(vdr);
                }
                else if (dr.Count() == 0)//AL-5163
                {
                    exceptionrows += 1;
                    WriteToConsole("Exception Row - Verf\t:\tFound 0 record in verf CSV file for ProxyId " + row["ExternalKey"].ToString().PadLeft(19, '0') + " and ExternalKey " + row["ExternalKey"].ToString() + " combination", true);
                }
                else
                {
                    exceptionrows += 1;
                    WriteToConsole("Exception Row - Verf\t:\tFound more than 1 record in verf CSV file for ProxyId " + row["ExternalKey"].ToString().PadLeft(19, '0') + " and ExternalKey " + row["ExternalKey"].ToString() + " combination", true);
                }
            }

            SqlConnection sqlconn = null;
            using (sqlconn = new SqlConnection(cxnconnectionstring))
            {
                sqlconn.Open();
                SqlBulkCopy bulkcopy = new SqlBulkCopy(sqlconn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkcopy.DestinationTableName = "tvisa_account";
                bulkcopy.WriteToServer(sourceds.Tables["TVISAAccount"]);

                string visasql = "select v.VisaAccountID  as VISAAccountId,t.TSysAccountID as TsysAccountId from tVisa_Account v inner join tTSys_Account t on v.ProxyId = RIGHT(REPLICATE('0',19) + t.ExternalKey,19) order by v.DTServerCreate desc";

                SqlDataAdapter dv = new SqlDataAdapter(visasql, sqlconn);
                dv.Fill(sourceds.Tables.Add("TVISATsysAccounts"));

                sqlconn.Close();
                WriteToConsole("INFO\t:\tNo of records migrated from tsys to visa is : " + sourceds.Tables["TVISATsysAccounts"].Rows.Count);
            }

            //TODO: Change the provider id to 103

            foreach (DataRow row in sourceds.Tables["TVISATsysAccounts"].Rows)
            {
                DataRow dr = sourceds.Tables["PTNRAccounts"].Select("CXNID=" + row["TsysAccountId"]).FirstOrDefault();
                if (dr == null)
                {
                    WriteToConsole("Exception Row - PTNRAccounts\t:\tUnable to find corresponding row in Partner Accounts table for CXNID " + row["TsysAccountId"].ToString() + " and Provider Id = 102 combination", true);
                    continue;
                }
                dr["CXNID"] = row["VISAAccountId"];
                dr["ProviderId"] = 103;
                dr["AccountPK"] = Guid.NewGuid();
                dr["DTServerCreate"] = DateTime.Now;
                dr["DTServerLastModified"] = DateTime.Now;
            }
            sourceds.Tables["PTNRAccounts"].AcceptChanges();

            using (sqlconn = new SqlConnection(ptnrconnectionstring))
            {
                sqlconn.Open();
                SqlBulkCopy bulkcopy = new SqlBulkCopy(sqlconn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkcopy.DestinationTableName = "taccounts";
                bulkcopy.WriteToServer(sourceds.Tables["PTNRAccounts"].Select("ProviderId = 103"));

                sqlconn.Close();
                WriteToConsole("INFO\t:\tNo of records inserted to partner accounts table for provider id 103 is : " + sourceds.Tables["PTNRAccounts"].Select("ProviderId = 103").Count().ToString());
            }
            return exceptionrows;
        }
        
        private static DataSet ReadAllAccounts()
        {
            string tsyssql = "SELECT * FROM tTSys_Account where Activated = 1";
            string ptnrpksql = "select ChannelPartnerPK from tchannelpartners where channelpartnerid = 33";
            string ptnraccountssql = "SELECT * FROM taccounts where providerid = 102";
            string ptnrcustomersql = "SELECT * from tpartnercustomers where channelpartnerpk = ";
            string cxesql = "SELECT * FROM tCustomers where channelpartnerid = 33";
            string cxeacccountsql = "SELECT tc.*  FROM  tCustomerAccounts tc inner join tCustomers c on tc.CustomerPK = c.CustomerPK where ChannelPartnerId = 33 and Type = 2 order by CustomerPK";
            string visasql = "SELECT top 1 * from tvisa_account where VisaAccountID = -1";
            string verfsql = "SELECT * FROM VISAVerfRecords";


            SqlConnection sqlconn = null;
            DataSet ds = new DataSet();

            WriteToConsole("INFO\t:\tStarting to read all account information from databases...");

            using (sqlconn = new SqlConnection(cxnconnectionstring))
            {

                WriteToConsole("INFO\t:\tLoading tsysaccount table data");
                SqlDataAdapter da = new SqlDataAdapter(tsyssql, sqlconn);
                da.Fill(ds.Tables.Add("TsysAccount"));

                //Identify records which has duplicate external keys and mark them as duplicate, so that we don't have to process them
                DataColumn dc = new DataColumn("IsDuplicate", typeof(bool));
                dc.DefaultValue = false;
                ds.Tables["TsysAccount"].Columns.Add(dc);

                WriteToConsole("INFO\t:\tChecking for duplicate rows with same external key in TsysAccounts table");

                var duplicates = ds.Tables["TsysAccount"].AsEnumerable().GroupBy(x => x["ExternalKey"]).Where(y => y.Count() > 1).ToList();
                
                foreach (var group in duplicates)
                {
                    ds.Tables["TsysAccount"].Select(string.Format("[ExternalKey] = '{0}'", group.Key)).ToList<DataRow>().ForEach(r => { r["IsDuplicate"] = true; });
                    WriteToConsole("INFO\t:\tException Row - Tsys\t:\tFound more than 1 record in TsysAccount for ExternalKey " + group.Key.ToString(), true);
                }
                ds.Tables["TsysAccount"].AcceptChanges();

                WriteToConsole("INFO\t:\tLoading verfrecords table data");

                SqlDataAdapter dvr = new SqlDataAdapter(verfsql, sqlconn);
                dvr.Fill(ds.Tables.Add("VerfRecords"));

                WriteToConsole("INFO\t:\tLoading schema for tvisaaccount table");

                SqlDataAdapter dv = new SqlDataAdapter(visasql, sqlconn);
                dv.Fill(ds.Tables.Add("TVISAAccount"));


                sqlconn.Close();
            }

            using (sqlconn = new SqlConnection(ptnrconnectionstring))
            {
                sqlconn.Open();
                WriteToConsole("INFO\t:\tGetting ChannelPartner PK from database");    
                string channelpartnerpk = new SqlCommand(ptnrpksql, sqlconn).ExecuteScalar().ToString();

                WriteToConsole("INFO\t:\tLoading partner accounts table data");
                SqlDataAdapter da = new SqlDataAdapter(ptnraccountssql, sqlconn);
                da.Fill(ds.Tables.Add("PTNRAccounts"));

                WriteToConsole("INFO\t:\tLoading partner custoemrs table data");
                SqlDataAdapter pc = new SqlDataAdapter(string.Format("{0}" + "'{1}'", ptnrcustomersql, channelpartnerpk), sqlconn);
                pc.Fill(ds.Tables.Add("PTNRCustomers"));

                sqlconn.Close();
            }

            using (sqlconn = new SqlConnection(cxeconnectionstring))
            {
                WriteToConsole("INFO\t:\tLoading cxe customers table data");

                SqlDataAdapter cc = new SqlDataAdapter(cxesql, sqlconn);
                cc.Fill(ds.Tables.Add("CXECustomers"));

                WriteToConsole("INFO\t:\tLoading cxe accounts table data for funds account type");

                SqlDataAdapter ca = new SqlDataAdapter(cxeacccountsql, sqlconn);
                ca.Fill(ds.Tables.Add("CXEAccounts"));

                sqlconn.Close();
            }
            return ds;
        }

        //private static void LoadRecordsToDB()
        //{
        //    var fileengine = new FileHelperEngine<VISAVerfFileCustomerRecord>();

        //    string VerffilePath = System.Configuration.ConfigurationManager.AppSettings["VerfFilePath"];

        //    System.Data.DataTable verfrecords = new DataTable();

        //    WriteToConsole("INFO\t:\tVisa verf file import process started");
        //    WriteToConsole("INFO\t:\tAttempting to read verf file from " + VerffilePath);

        //    SqlConnection sqlconn = null;

        //    if (string.IsNullOrWhiteSpace(VerffilePath))
        //    {
        //        WriteToConsole("INFO\t:\tFile path for the verf was not provided or does not exist, check the appsettings for \"VerfFilePath\"", true, true);
        //        return;
        //    }

        //    try
        //    {
        //        verfrecords = fileengine.ReadFileAsDT(VerffilePath);

        //        int footerrecords = int.Parse(fileengine.FooterText.Substring(30, 9));

        //        WriteToConsole("INFO\t:\tReading verf file contents completed");

        //        int record = verfrecords.Rows.Count;
        //        int columns = verfrecords.Columns.Count;

        //        WriteToConsole("INFO\t:\tNo of columns in verf file is " + columns.ToString());
        //        WriteToConsole("INFO\t:\tNo of columns as per verf file specification is 82");

        //        WriteToConsole("INFO\t:\tNo of records in verf file is " + record.ToString());
        //        WriteToConsole("INFO\t:\tNo of records as per trailer record is " + footerrecords);

        //        if (footerrecords != record)
        //        { WriteToConsole("INFO\t:\tNumber of records in file footer does not match with number of records parsed.", true, true); }

        //        if (columns != 82)
        //        { WriteToConsole("INFO\t:\tNumber of columns identified in verf file does not match with specification.", true, true); }

        //        if (string.IsNullOrWhiteSpace(cxnconnectionstring))
        //        { WriteToConsole("INFO\t:\tCould not find connectionstring in config file", true, true); }


        //        WriteToConsole("INFO\t:\tAttempting to load the records to staging table in database");
        //        WriteToConsole("INFO\t:\tChecking for create table script for verf file");

        //        string verfsqlfile = File.ReadAllText("Script\\CreateVisaVerfTable.sql");

        //        sqlconn = new SqlConnection(cxnconnectionstring);
        //        sqlconn.Open();

        //        IEnumerable<string> commandStrings = Regex.Split(verfsqlfile, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        //        WriteToConsole("INFO\t:\tRenaming VisaVerfRecords table if it exists and Creating table VISAVerfRecords in " + sqlconn.Database + " if it does not exist");
        //        foreach (string commandString in commandStrings)
        //        {
        //            if (commandString.Trim() != "")
        //            {
        //                using (var command = new SqlCommand(commandString, sqlconn))
        //                {
        //                    command.ExecuteNonQuery();
        //                }
        //            }
        //        }

        //        int FileLoadNumber = 0;
        //        using (var command = new SqlCommand("select ISNULL(Max(FileLoadNumber),0) from VISAVerfRecords", sqlconn))
        //        {
        //            FileLoadNumber = (int)command.ExecuteScalar();
        //            FileLoadNumber += 1;
        //            WriteToConsole("INFO\t:\tVerified that VISAVerfRecords table exists in " + sqlconn.Database);
        //        }

        //        WriteToConsole("INFO\t:\tThis is file number " + FileLoadNumber + " being loaded");

        //        verfrecords.Columns.Add("FileLoadNumber").Expression = FileLoadNumber.ToString();

        //        WriteToConsole("INFO\t:\tLoading records from verf file to table");

        //        SqlBulkCopy bulkcopy = new SqlBulkCopy(sqlconn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
        //        bulkcopy.DestinationTableName = "VISAVerfRecords";
        //        bulkcopy.WriteToServer(verfrecords);
        //        using (var command = new SqlCommand("select count(*) from VISAVerfRecords where FileLoadNumber =  " + FileLoadNumber, sqlconn))
        //        {
        //            int rowcount = (int)command.ExecuteScalar();
        //            WriteToConsole("INFO\t:\tLoaded records to verf table ");
        //            if (rowcount != record)
        //            {
        //                StringBuilder sb = new StringBuilder();
        //                sb.AppendLine("Record count does not match between verf file and database");
        //                sb.AppendLine("No of records in verf file is " + record.ToString());
        //                sb.AppendLine("No of records in VISAVerfRecords table is " + rowcount.ToString() + " for file load number " + FileLoadNumber);
        //                WriteToConsole(sb.ToString(), true, true);
        //            }

        //            WriteToConsole("INFO\t:\tNo of records loaded to VISAVerfRecords table is " + rowcount.ToString() + " for file load number " + FileLoadNumber);
        //        }
        //        sqlconn.Close();
        //        WriteToConsole("INFO\t:\tVerf file load complete");
        //    }
        //    catch (Exception ex)
        //    {
        //        if (sqlconn != null)
        //        { sqlconn.Close(); }
        //        WriteToConsole("INFO\t:\tException Occurred: " + ex.Message + Environment.NewLine + "Stack Trace:" + ex.StackTrace, true, true);
        //    }
        //}

        private static void LoadRecordsToDB()
        {
            var fileengine = new FileHelperEngine<VISAVerfFileCustomerRecord>();

            string VerffilePath = System.Configuration.ConfigurationManager.AppSettings["VerfFilePath"];

            System.Data.DataTable verfrecords = new DataTable();

            WriteToConsole("INFO\t:\tVisa verf CSV file import process started");
            WriteToConsole("INFO\t:\tAttempting to read verf file from " + VerffilePath);

            SqlConnection sqlconn = null;

            if (string.IsNullOrWhiteSpace(VerffilePath))
            {
                WriteToConsole("INFO\t:\tFile path for the verf CSV was not provided or does not exist, check the appsettings for \"VerfFilePath\"", true, true);
                return;
            }

            try
            {
                verfrecords = fileengine.ReadFileAsDT(VerffilePath);

                WriteToConsole("INFO\t:\tReading verf CSV file contents completed");

                int record = verfrecords.Rows.Count;
                int columns = verfrecords.Columns.Count;

                WriteToConsole("INFO\t:\tNo of columns in verf file is " + columns.ToString());
                WriteToConsole("INFO\t:\tNo of columns as per verf file specification is 5");

                WriteToConsole("INFO\t:\tNo of records in verf file is " + record.ToString());

                if (columns != 5)
                { WriteToConsole("INFO\t:\tNumber of columns identified in verf file does not match with specification.", true, true); }

                if (string.IsNullOrWhiteSpace(cxnconnectionstring))
                { WriteToConsole("INFO\t:\tCould not find connectionstring in config file", true, true); }

                WriteToConsole("INFO\t:\tAttempting to load the records to staging table in database");
                WriteToConsole("INFO\t:\tChecking for create table script for verf CSV file");

                string verfsqlfile = File.ReadAllText("Script\\CreateVisaVerfTable.sql");

                sqlconn = new SqlConnection(cxnconnectionstring);
                sqlconn.Open();

                IEnumerable<string> commandStrings = Regex.Split(verfsqlfile, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                WriteToConsole("INFO\t:\tRenaming VisaVerfRecords table if it exists and Creating table VISAVerfRecords in " + sqlconn.Database + " if it does not exist");
                foreach (string commandString in commandStrings)
                {
                    if (commandString.Trim() != "")
                    {
                        using (var command = new SqlCommand(commandString, sqlconn))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                WriteToConsole("INFO\t:\tLoading records from verf CSV file to table");

                SqlBulkCopy bulkcopy = new SqlBulkCopy(sqlconn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkcopy.DestinationTableName = "VISAVerfRecords";
                bulkcopy.WriteToServer(verfrecords);
                using (var command = new SqlCommand("select count(*) from VISAVerfRecords", sqlconn))
                {
                    int rowcount = (int)command.ExecuteScalar();
                    WriteToConsole("INFO\t:\tLoaded records to verf table ");
                    if (rowcount != record)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Record count does not match between verf file and database");
                        sb.AppendLine("No of records in verf file is " + record.ToString());
                        sb.AppendLine("No of records in VISAVerfRecords table is " + rowcount.ToString());
                        WriteToConsole(sb.ToString(), true, true);
                    }

                    WriteToConsole("INFO\t:\tNo of records loaded to VISAVerfRecords table is " + rowcount.ToString());
                }
                sqlconn.Close();
                WriteToConsole("INFO\t:\tVerf file load complete");
            }
            catch (Exception ex)
            {
                if (sqlconn != null)
                { sqlconn.Close(); }
                WriteToConsole("INFO\t:\tException Occurred: " + ex.Message + Environment.NewLine + "Stack Trace:" + ex.StackTrace, true, true);
            }
        }

        static void WriteToConsole(string Message,bool IsError = false, bool mustExit = false)
        {
            if (mustExit || IsError)
                { Console.ForegroundColor = ConsoleColor.Red; }
            Console.WriteLine(Message);
            WriteLog(Message);
            Console.ResetColor();
            if (mustExit)
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        static void WriteLog(string Message)
        {
            File.AppendAllText(logfile, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + ":\t" + Message + Environment.NewLine);
        }
    }

    [IgnoreFirst]
    [DelimitedRecord(",")]
    class VISAVerfFileCustomerRecord
    { 
        //ALIAS_ID, EXTERNAL_KEY, PSEUDO_DDA, EXPIRATION_DATE,PAN
        [FieldTrim(TrimMode.Both)]
        public string AliasId;
        [FieldTrim(TrimMode.Both)]
        public string ExternalKey;
        [FieldTrim(TrimMode.Both)]
        public string PsuedoDDA;
        [FieldTrim(TrimMode.Both)]
        [FieldConverter(ConverterKind.Date, "yyyyMM")]
        public DateTime? ExpirationDate;
        [FieldTrim(TrimMode.Both)]
        public string PAN;

    }

//    [IgnoreFirst]
//    [IgnoreLast]
//    [ConditionalRecord(RecordCondition.IncludeIfBegins, "CRDBUYPRFL")]
//    [FixedLengthRecord(FixedMode.ExactLength)]
//    class VISAVerfFileCustomerRecord 
//    { 
//        [FieldFixedLength(10)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	RecordType;
//        [FieldFixedLength(1)]
//        [FieldTrim(TrimMode.Both)]
//        public	string	CardProgramType;
//        [FieldFixedLength(16)]
//        [FieldTrim(TrimMode.Both)]
//        public	string	CardProgramID;
//        [FieldFixedLength(1)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	CardTypeID;
//        [FieldFixedLength(16)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	SubClientIdentifier;
//        [FieldFixedLength(10)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	ReportingGroupID;
//        [FieldFixedLength(19)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	AliasID;
//        [FieldFixedLength(19)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	PrimaryAccountNumber;
//        [FieldFixedLength(2)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	ProfileType;
//        [FieldFixedLength(14)]	
//        [FieldTrim(TrimMode.Both)]
//        [FieldConverter(ConverterKind.Date, "yyyyMMddHHmmss")]
//        public	DateTime?	RecordCreationDate;
//        [FieldFixedLength(19)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	BuyerAliasID;
//        [FieldFixedLength(1)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	CustomerIndicatorField;
//        [FieldFixedLength(14)]
//        [FieldTrim(TrimMode.Both)]
//        [FieldConverter(ConverterKind.Date, "yyyyMMddHHmmss")]
//        public	DateTime?	DateofLastWebContact;
//        [FieldFixedLength(50)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	CompanyName;
//        [FieldFixedLength(16)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	IssuerCompanyID;
//        [FieldFixedLength(20)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	LegalFirstName;
//        [FieldFixedLength(1)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	MiddleName;
//        [FieldFixedLength(20)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	LegalLastName;
//        [FieldFixedLength(5)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	Suffix;
//        [FieldFixedLength(4)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	Title;
//        [FieldFixedLength(3)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	GovernmentIDType;
//        [FieldFixedLength(30)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	GovernmentIssuedID;
//        [FieldFixedLength(2)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	DriversLicenseStateProvince;
//        [FieldFixedLength(3)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	GovernmentIDCountry;
//        [FieldFixedLength(8)]
//        [FieldTrim(TrimMode.Both)]
//        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
//        public	DateTime?	BirthDate;
//        [FieldFixedLength(9)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	Filler1;
//        [FieldFixedLength(75)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	Address1;
//        [FieldFixedLength(75)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	Address2;
//        [FieldFixedLength(75)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	Address3;
//        [FieldFixedLength(40)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	City;
//        [FieldFixedLength(4)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	StateProvince;
//        [FieldFixedLength(14)]
//        [FieldTrim(TrimMode.Both)]
//        public	string	PostalCode;
//        [FieldFixedLength(3)]
//        [FieldTrim(TrimMode.Both)]
//        public	string	CountryCode;
//        [FieldFixedLength(20)]
//        [FieldTrim(TrimMode.Both)]
//        public	string	PrimaryPhoneNumber;
//        [FieldFixedLength(1)]
//        [FieldTrim(TrimMode.Both)]
//        public	string	PrimaryPhoneNumberType;
//        [FieldFixedLength(20)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	TextMessageDeviceNumber;
//        [FieldFixedLength(80)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	EmailAddress;
//        [FieldFixedLength(2)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	BuyerPrimaryFundingAccountType;
//        [FieldFixedLength(28)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	FundingAccountNumber;
//        [FieldFixedLength(9)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	RoutingandTransitNumber;
//        [FieldFixedLength(8)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	CardStockID;
//        [FieldFixedLength(2)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	CardShippingType;
//        [FieldFixedLength(2)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	CardOrderType;
//        [FieldFixedLength(2)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	IssuanceType;
//        [FieldFixedLength(26)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	EmbossedMessage;
//        [FieldFixedLength(14)]
//        [FieldTrim(TrimMode.Both)]
//        [FieldConverter(ConverterKind.Date, "yyyyMMddHHmmss")]
//        public	DateTime?	IssuanceDate;
//        [FieldFixedLength(6)]
//        [FieldTrim(TrimMode.Both)]
//        [FieldConverter(ConverterKind.Date, "yyyyMM")]
//        public	DateTime?	ExpirationDate;
//        [FieldFixedLength(8)]
//        [FieldTrim(TrimMode.Both)]
//        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
//        public	DateTime?	ActivationDate;
//        [FieldFixedLength(1)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	CardIssuedActive;
//        [FieldFixedLength(16)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	CardHolderPromotionCode;
//        [FieldFixedLength(12)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	InitialBalance;
//        [FieldFixedLength(3)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	InitialBalanceCurrency;
//        [FieldFixedLength(12)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	AvailableBalance;
//        [FieldFixedLength(3)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	AvailableBalanceCurrency;
//        [FieldFixedLength(1)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	SignofAvailableBalance;
//        [FieldFixedLength(12)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	LedgerBalance;
//        [FieldFixedLength(3)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	LedgerBalanceCurrency;
//        [FieldFixedLength(1)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	SignofLedgerBalance;
//        [FieldFixedLength(2)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	Status;
//        [FieldFixedLength(14)]
//        [FieldTrim(TrimMode.Both)]
//        [FieldConverter(ConverterKind.Date, "yyyyMMddHHmmss")]
//        public	DateTime?	StatusLastUpdated;
//        [FieldFixedLength(25)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	EmployeeID;
//        [FieldFixedLength(28)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	PseudoDDA;
//        [FieldFixedLength(19)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	PrimaryCardAliasID;
//        [FieldFixedLength(19)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	AccountHolderAliasID;
//        [FieldFixedLength(19)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	StoredValueAccountAliasID;
//        [FieldFixedLength(50)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	CompanyWebURL;
//        [FieldFixedLength(12)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	PendingBalance;
//        [FieldFixedLength(3)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	PendingBalanceCurrency;
//        [FieldFixedLength(1)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	SignofPendingBalance;
//        [FieldFixedLength(12)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	ClientTrackingID;
//        [FieldFixedLength(19)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	UpgradePAN;
//        [FieldFixedLength(19)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	PreviousPAN;
//        [FieldFixedLength(19)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	ProxyID;
//        [FieldFixedLength(26)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	EmbossedMessage2;
//        [FieldFixedLength(15)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	OrderConfirmationNumber;
//        [FieldFixedLength(18)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	OrderBatchID;
//        [FieldFixedLength(1)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	BudgetingEnrolled;
//        [FieldFixedLength(12)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	RESERVED;
//        [FieldFixedLength(16)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	FeeScheduleID;
//        [FieldFixedLength(8)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	FormFactor;
//        [FieldFixedLength(1)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	AlternateFundingStatus;
//        [FieldFixedLength(751)]	
//        [FieldTrim(TrimMode.Both)]
//        public	string	Filler;
//}

}
