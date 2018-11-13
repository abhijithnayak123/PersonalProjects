using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Reporting.X9
{
    class Program
    {
        #region Definitions
        private static readonly string[] fileTypes = { "OffUs", "OnUs", "MoneyOrder" };
        private static readonly string MONEY_ORDER = "MoneyOrder";
        private static readonly string _fileName = "{0}_{1}_{2}_{3}.csv";
        private static readonly string _connString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;
        private static int maxItemCountPerFile = 0;
        private static DateTime runDate = DateTime.MinValue;
        private static bool ignorePreviousRun = false;
        #region Email Template
        /*
		private static readonly string emailTemplate = @"
			<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
			<html>
				<head>
					<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>
					<style type=""text/css"">
					table.emailTemplate, table tr.emailTemplate, table td.emailTemplate 
							{ border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; border: 0; }
					table.emailTemplate td {
							padding: 7px 7px 7px 7px;
							border-spacing: 0;
							border: #B4BED0 1px solid;
							background-color: transparent;
							text-align : right;
							}
					table td.leftAlign {
							padding: 7px 7px 7px 7px;
							border-spacing: 0;
							border: #B4BED0 1px solid;
							background-color: transparent;
							text-align : left;
							}
					.emailTemplate	{
							color: #000000;
							font: normal 11px arial, sans-serif;
							border: #000000 1px solid;
							}
					table td.etTitle {
							color: #FFFFFF;
							font: bold 14px arial, sans-serif;
							white-space: nowrap;
							background-color: #92A2C2;
							}
					</style>
				</head>
				<body>
				<h2>X9 transmission details between: $StartDateTime and $EndDateTime</h2>
				<table class=""emailTemplate"">
					<tr>
						<td class=""etTitle"">Item Type<br></td>
						<td class=""etTitle"">Count<br></td>
						<td class=""etTitle"">Total<br></td>
					</tr>
					<tr>
						<td class=""leftAlign"">Off Us<br></td>
						<td>$OffUsCount<br></td>
						<td>$OffUsTotal<br></td>
					</tr>
					<tr>
						<td class=""leftAlign"">Money Order<br></td>
						<td>$MOCount<br></td>
						<td>$MOTotal<br></td>
					</tr>
					<tr>
						<td class=""leftAlign"">On Us<br></td>
						<td>$OnUsCount<br></td>
						<td>$OnUsTotal<br></td>
					</tr>
				</table>
				</body>
			</html>
		";
		*/
        #endregion
        private static readonly string _partnerQuery = @"SELECT 
															ChannelPartnerID,
															ChannelPartnerPK,
															name
														  FROM $dbPrefixPTNR.dbo.tChannelPartners
														 WHERE IsX9ReportingEnabled = 1";
        #region OffUs Checks Query
        private static readonly string _offUsQuery = @"SELECT  
															ch.TxnPK as ItemID,
															a.BusinessDate as BusinessDate,
															l.BranchID as BranchID,
															l.BankID as BankID,
                                                            a.ClientAgentIdentifier as AgentID,
														    c.CustomerID as NexxoPAN,
															ch.Amount as Amount,
															im.Front as FrontImage,
															im.Back as RearImage
													  FROM $dbPrefixPTNR..tTxn_Check ch WITH (NOLOCK)
															JOIN $dbPrefixPTNR..tCustomerSessions cs WITH (NOLOCK) ON ch.CustomerSessionPK = cs.CustomerSessionPK
															JOIN $dbPrefixPTNR..tAgentSessions a WITH (NOLOCK) ON cs.AgentSessionPK = a.AgentSessionPK
															JOIN $dbPrefixPTNR..tPartnerCustomers c WITH (NOLOCK) ON cs.CustomerPK = c.CustomerPK
															JOIN $dbPrefixPTNR..tChannelPartners p WITH (NOLOCK) ON c.ChannelPartnerPK = p.ChannelPartnerPK
															JOIN $dbPrefixPTNR..tTerminals t WITH (NOLOCK) ON a.TerminalPK = t.TerminalPK
															JOIN $dbPrefixPTNR..tLocations l WITH (NOLOCK) ON t.LocationPK = l.LocationPK
															JOIN $dbPrefixCXE..tTxn_Check_Commit cx WITH (NOLOCK) ON ch.CXEID = cx.CheckID
															JOIN $dbPrefixCXE..tCheckImages im WITH (NOLOCK) ON cx.CheckPK = im.CheckPK
															LEFT JOIN $dbPrefixPTNR..tChannelPartner_X9_Audit_Detail d WITH (NOLOCK) ON d.ItemPK = ch.TxnPK
													  WHERE cx.Status = 4
														AND ch.DTServerLastModified > '$startDate' AND ch.DTServerLastModified <= '$endDate'
														AND p.Name = '$partnerName'
														AND d.ItemPK IS NULL
														AND l.BankID IN ($banklocations)
													ORDER BY l.BankID
													";
        #endregion
        #region Money Order Query
        private static readonly string _moneyOrderQuery = @"SELECT  
		                                                    ItemID,
                                                            a.BusinessDate AS BusinessDate,
                                                            l.BranchID AS BranchID,
                                                            l.BankID AS BankID,
                                                            a.ClientAgentIdentifier AS AgentID,
                                                            c.CustomerID AS NexxoPAN,
		                                                    Amount,
                                                            im.CheckFrontImage AS FrontImage,
                                                            im.CheckBackImage AS RearImage
		                                                    FROM 
		                                                    (
		                                                    SELECT 
                                                            mo.TxnPK AS ItemID,
                                                            mo.Amount AS Amount,
		                                                    mo.CustomerSessionPK
                                                            FROM $dbPrefixPTNR..tTxn_MoneyOrder mo WITH (NOLOCK) WHERE mo.CXEState = 4 AND mo.DTServerLastModified > '$startDate' AND mo.DTServerLastModified <= '$endDate'
		                                                    ) T
                                                            JOIN $dbPrefixPTNR..tCustomerSessions cs WITH (NOLOCK) ON T.CustomerSessionPK = cs.CustomerSessionPK
                                                            JOIN $dbPrefixPTNR..tAgentSessions a WITH (NOLOCK) ON cs.AgentSessionPK = a.AgentSessionPK
                                                            JOIN $dbPrefixPTNR..tPartnerCustomers c WITH (NOLOCK) ON cs.CustomerPK = c.CustomerPK
                                                            JOIN $dbPrefixPTNR..tChannelPartners p WITH (NOLOCK) ON c.ChannelPartnerPK = p.ChannelPartnerPK
                                                            JOIN $dbPrefixPTNR..tTerminals tr WITH (NOLOCK) ON a.TerminalPK = tr.TerminalPK
                                                            JOIN $dbPrefixPTNR..tLocations l WITH (NOLOCK) ON tr.LocationPK = l.LocationPK
                                                            JOIN $dbPrefixPTNR..tMoneyOrderImage im WITH (NOLOCK) ON T.ItemId = im.TrxID
                                                            LEFT JOIN $dbPrefixPTNR..tChannelPartner_X9_Audit_Detail d WITH (NOLOCK) ON d.ItemPK = T.ItemId
                                                            WHERE 
                                                                p.NAME = '$partnerName'
                                                                AND d.ItemPK IS NULL
                                                                AND l.BankID IN ($banklocations)
                                                            ORDER BY l.BankID";
        #endregion
        #endregion

        static void Main(string[] args)
        {
            #region Setup activities
            // Get the command line arguments, if any
            // App needs to support the scenario of generating files for a particular client
            // on a particular day
            // Format is "--rundate 20150112 --mode 1 --ignorepreviousrun false"
            var commandLineOptions = CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args);
            if (commandLineOptions.Errors.Any())
            {
                foreach (var error in commandLineOptions.Errors)
                {
                    Console.WriteLine("Invalid options in command line, error: {0}", error.ToString());
                    //logger.Error(string.Format("Invalid options in command line, error: {0}", error.ToString()));
                }
                return;
            }


            // Set run date either from command line argument or default
            //DateTime runDate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(commandLineOptions.Value.RunDate))
            {
                DateTime.TryParseExact(commandLineOptions.Value.RunDate, "yyyyMMdd", new CultureInfo("EN-US"), DateTimeStyles.None, out runDate);
            }
            if (DateTime.MinValue == runDate)
            {
                runDate = DateTime.Now;
            }
            if (runDate.TimeOfDay.ToString() == "00:00:00")
            {
                long time = DateTime.Now.TimeOfDay.Ticks;
                runDate = runDate.AddTicks(time);
            }

            if (!string.IsNullOrEmpty(commandLineOptions.Value.IgnorePreviousRun.ToString()))
            {
                ignorePreviousRun = commandLineOptions.Value.IgnorePreviousRun;
            }

            if ((commandLineOptions.Value.RunMode < 0 || commandLineOptions.Value.RunMode > 3))
            {
                Console.WriteLine("Invalid run Mode: {0}", commandLineOptions.Value.RunMode);
                return;
            }

            int runMode = commandLineOptions.Value.RunMode;

            DateTime startDateTime = DateTime.MinValue;
            DateTime endDateTime = DateTime.MinValue;

            switch (runMode)
            {
                case 1:
                    startDateTime = new DateTime(runDate.AddDays(-1).Year, runDate.AddDays(-1).Month, runDate.AddDays(-1).Day, 20, 30, 1);
                    endDateTime = new DateTime(runDate.Year, runDate.Month, runDate.Day, 12, 45, 0);
                    break;
                case 2:
                    startDateTime = new DateTime(runDate.Year, runDate.Month, runDate.Day, 12, 45, 1);
                    endDateTime = new DateTime(runDate.Year, runDate.Month, runDate.Day, 15, 45, 0);
                    break;
                case 3:
                    startDateTime = new DateTime(runDate.Year, runDate.Month, runDate.Day, 15, 45, 1);
                    endDateTime = new DateTime(runDate.Year, runDate.Month, runDate.Day, 20, 30, 0);
                    break;
                default:
                    break;
            }

            // Get the App settings from config file & interested keys
            var appSettings = ConfigurationManager.AppSettings;
            string dbPrefix = appSettings["dbPrefix"] ?? string.Empty;
            string banklocations = appSettings["tcfbanklocations"] ?? string.Empty;

            string _outputDirectory = appSettings["outputDirectory"] ?? Environment.CurrentDirectory;
            // Add runDate as a sub-directory
            string outputDirectory = _outputDirectory + "\\" + runDate.ToString("yyyyMMdd");

            // default it to 10000
            maxItemCountPerFile = 10000;

            // Get the maximum item count per file type
            string tempStr = appSettings["maxItemCountPerFile"];
            if (!string.IsNullOrEmpty(tempStr))
            {
                maxItemCountPerFile = Convert.ToInt32(tempStr);
            }

            // ensure we've the right DB with the configured dbPrefix
            StringBuilder partnerQuery = new StringBuilder(_partnerQuery.Replace("$dbPrefix", dbPrefix));

            // if a specific partner in command line arguments was specified, use it
            if (!string.IsNullOrEmpty(commandLineOptions.Value.Partner))
            {
                partnerQuery.Append(" AND name in ('" + commandLineOptions.Value.Partner + "')");
            }
            #endregion

            using (DataContext db = new DataContext(_connString))
            {

                // Get the partners for whom X9 is enabled
                IEnumerable<Partner> partners = db.ExecuteQuery<Partner>(partnerQuery.ToString());
                #region Partner process
                foreach (var partner in partners)
                {
                    int offUsItemCount = 0;
                    decimal offUsTotal = 0;
                    int onUsItemCount = 0;
                    decimal onUsTotal = 0;
                    int moneyOrderItemCount = 0;
                    decimal moneyOrderTotal = 0;
                    string partnerOutputDirectory = outputDirectory + "\\" + partner.name;

                    #region File type (OffUs, MoneyOrder, OnUs) process
                    foreach (var fileType in fileTypes)
                    {
                        int fileTypeItemCount = 0;
                        decimal fileTypeTotal = 0;

                        try
                        {
                            CreateCSVFile(partner.ChannelPartnerPK, partner.name, fileType, dbPrefix, startDateTime, endDateTime, partnerOutputDirectory,banklocations, out fileTypeItemCount, out fileTypeTotal);

                            switch (fileType.ToLower())
                            {
                                case "onus":
                                    {
                                        onUsItemCount = fileTypeItemCount;
                                        onUsTotal = fileTypeTotal;
                                        break;
                                    }
                                case "offus":
                                    {
                                        offUsItemCount = fileTypeItemCount;
                                        offUsTotal = fileTypeTotal;
                                        break;
                                    }
                                case "moneyorder":
                                    {
                                        moneyOrderItemCount = fileTypeItemCount;
                                        moneyOrderTotal = fileTypeTotal;
                                        break;
                                    }
                                default:
                                    {
                                        // none of the above, so bail!
                                        return;
                                    }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error encountered while creating file for partner: {0}, FileType: {1}. Exception:{2}", partner.name, fileType, ex.ToString());
                        }
                    }
                    #endregion
                    #region Send Email -- Not used
                    /* Not sending emails anymore, now writing a notification file as per AL-859
					// Send email to client
					try
					{

						// Get SMTPServer name & To: email address
						string SMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
						string toEmailAddress = ConfigurationManager.AppSettings["toEmailAddress"];

						if (string.IsNullOrEmpty(SMTPServer) || string.IsNullOrEmpty(toEmailAddress))
						{
							throw new ArgumentNullException("Either SMTPServer or toEmailAddress is null");
						}

						//Construct email body
						StringBuilder emailBody = new StringBuilder(emailTemplate);
						emailBody.Replace("$StartDateTime", startDateTime.ToString("g"));
						emailBody.Replace("$EndDateTime", endDateTime.ToString("g"));
						emailBody.Replace("$OffUsCount", offUsItemCount.ToString());
						emailBody.Replace("$OffUsTotal", offUsTotal.ToString("c"));
						emailBody.Replace("$MOCount", moneyOrderItemCount.ToString());
						emailBody.Replace("$MOTotal", moneyOrderTotal.ToString("c"));
						emailBody.Replace("$OnUsCount", onUsItemCount.ToString());
						emailBody.Replace("$OnUsTotal", onUsTotal.ToString("c"));

						// Send email
						MailMessage emailMessage = new MailMessage("support@moneygram.com", toEmailAddress, "X9 File Transmission Details", emailBody.ToString());
						emailMessage.IsBodyHtml = true;
						SmtpClient smtpClient = new SmtpClient(SMTPServer);
						smtpClient.Send(emailMessage);

					}
					catch (Exception ex)
					{
						Console.Write("Error sending email to partner {0}. Exception: {1}", partner.name, ex.ToString());
					}
					*/
                    #endregion
                    string notificationFileName = partnerOutputDirectory + "\\" + string.Format("mgi_{0}_{1}.txt", partner.name, runDate.ToString("yyyyMMddHHmmss"));
                    using (var fs = File.Create(notificationFileName))
                    {
                        StringBuilder notification = new StringBuilder();
                        string header = "X9 transmission details between: {0} and {1}" + System.Environment.NewLine + System.Environment.NewLine;
                        notification.Append(string.Format(header, startDateTime.ToString("g"), endDateTime.ToString("g")));
                        string lineFormat = "{0}\t{1}\t{2}" + System.Environment.NewLine;
                        notification.Append(string.Format(lineFormat, "Item", "Count", "Total"));
                        notification.Append(string.Format(lineFormat, "Off Us", offUsItemCount.ToString(), offUsTotal.ToString("c")));
                        notification.Append(string.Format(lineFormat, "Money Order", moneyOrderItemCount.ToString(), moneyOrderTotal.ToString("c")));
                        notification.Append(string.Format(lineFormat, "On Us", onUsItemCount.ToString(), onUsTotal.ToString("c")));
                        try
                        {
                            AddText(fs, notification.ToString());
                            fs.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error encountered in creating file: {0}. Exception: {1}", notificationFileName, ex.ToString());
                        }
                    }
                }
            }
                #endregion
        }

        private static void CreateCSVFile(Guid partnerID, string partnerName, string fileType, string dbPrefix, DateTime startDateTime, DateTime endDateTime, string outputDirectory,string banklocations, out int itemCount, out decimal itemTotal)
        {
            #region initial setup for file type
            itemCount = 0;
            itemTotal = 0;

            string destinationDirectory = outputDirectory + "\\" + fileType;
            if (!Directory.Exists(destinationDirectory))
            {
                try
                {
                    Directory.CreateDirectory(destinationDirectory);
                }
                catch (Exception ex)
                {
                    Console.Write("Cannot create Directory {0}, exception: {1}", destinationDirectory, ex.InnerException);
                    return;
                }
            }

            string query = string.Empty;
            switch (fileType.ToLower())
            {
                case "onus":
                    {
                        Console.WriteLine("OnUs checks not implemented");
                        break;
                    }
                case "offus":
                    {
                        // use the Off Us query
                        query = _offUsQuery.Replace("$dbPrefix", dbPrefix).Replace("$partnerName", partnerName).Replace("$startDate", startDateTime.ToString("G")).Replace("$endDate", endDateTime.ToString("G")).Replace("$banklocations", banklocations);
                        //query = _offUsQuery.Replace("$banklocations", banklocations);
                        break;
                    }
                case "moneyorder":
                    {
                        // use the Money Order query
                        query = _moneyOrderQuery.Replace("$dbPrefix", dbPrefix).Replace("$partnerName", partnerName).Replace("$startDate", startDateTime.ToString("G")).Replace("$endDate", endDateTime.ToString("G")).Replace("$banklocations", banklocations);
                        //query = _moneyOrderQuery.Replace("$banklocations", banklocations);
                        break;
                    }
                default:
                    {
                        // none of the above, so bail!
                        Console.WriteLine("Invalid file type {0} encountered", fileType);
                        return;
                    }
            };

            // Nothing to do
            if (string.IsNullOrEmpty(query))
            {
                return;
            }

            if (ignorePreviousRun)
            {
                query = query.Replace("SELECT", "SELECT DISTINCT").Replace("AND d.ItemPK IS NULL", "-- AND d.ItemPK IS NULL");
            }

            Console.WriteLine("Query to generate {0} CSV is {1}", fileType, query);
            #endregion

            using (DataContext db = new DataContext(_connString))
            {

                string previousBankID = string.Empty;
                string fileName = string.Empty;
                string fullyQualifiedFileName = string.Empty;
                FileStream fs = null;
                List<Guid> itemIDs = null;

                IEnumerable<Item> items = db.ExecuteQuery<Item>(query); ;
                foreach (var item in items)
                {
                    if (itemCount > maxItemCountPerFile)
                    {
                        break;
                    }
                    if (item.BankID != previousBankID)
                    {
                        try
                        {
                            // Close the file for the previous "batch" & create an audit trail
                            // For the first record, file is null, so additional check
                            if (fs != null && fs.Length > 0)
                            {
                                fs.Close();
                                // Create Audit Trail
                                CreateAuditTrail(db, partnerID, fileType, fullyQualifiedFileName, itemIDs);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error encountered in closing file: {0}. Exception: {1}", fullyQualifiedFileName, ex.ToString());
                        }

                        try
                        {
                            fileName = string.Format(_fileName, partnerName, item.BankID, fileType, runDate.ToString("yyyyMMddHHmmss"));
                            fullyQualifiedFileName = destinationDirectory + "\\" + fileName;
                            fs = File.Create(fullyQualifiedFileName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error encountered in creating file: {0}. Exception: {1}", fullyQualifiedFileName, ex.ToString());
                        }
                        // Reset the control variables
                        previousBankID = item.BankID;
                        itemIDs = new List<Guid>();
                    }

                    string checkFrontImageFQName = string.Empty;
                    string checkBackImageFQName = string.Empty;
                    try
                    {
                        checkFrontImageFQName = CreateTiffFile(destinationDirectory, item.ItemId.ToString(), item.FrontImage, true);
                        checkBackImageFQName = CreateTiffFile(destinationDirectory, item.ItemId.ToString(), item.RearImage, false);
                    }
                    catch (Exception ex)
                    {
                        // Exception already logged below, so skip logging and move onto next item
                        continue;
                    }

                    // Build the CSV record for the file & write it
                    try
                    {
                        StringBuilder sb = new StringBuilder();

                        // Front Image
                        sb.Append(checkFrontImageFQName + ",");
                        // Back Image
                        sb.Append(checkBackImageFQName + ",");
                        //Item Amount
                        sb.Append(item.Amount.ToString("F") + ",");
                        itemTotal += item.Amount;
                        itemCount++;
                        //MICR Data: Not used
                        sb.Append(",");
                        //Sequence Number: Not used 
                        sb.Append(",");
                        //Payee Name: Is used for line 1 endorsement that requires
                        // this format: "BBB  BR#  TTTTT  Z" where BBB is 3 character BankID, 3 character BranchID
                        // and TTTTT is the 5-digit teller ID (ClientAgentIdentifier) followed by a "Z"
                        sb.Append(((item.BankID.Length > 3) ? item.BankID.Right(3) : item.BankID) + " ");
                        sb.Append(((item.BranchID.Length > 3) ? item.BranchID.Right(3) : item.BranchID) + " ");
                        sb.Append((string.IsNullOrEmpty(item.AgentID) ? "00000" : ((item.AgentID.Length > 5) ? item.AgentID.Right(5) : item.AgentID)));
                        sb.Append(" Z,");
                        //Deposit Account Number: Is used for line 1 endorsement that requires
                        // this format: "yyyyMMdd nnnnnnnnn" where yyyyMMdd is the txn Date, 
                        // and nnnnnnnnn is the last 9 characters of the NexxoPAN
                        sb.Append((((item.BusinessDate == null || item.BusinessDate == DateTime.MinValue)) ? DateTime.Now.ToString("yyyyMMdd") : item.BusinessDate.ToString("yyyyMMdd")) + " ");
                        string tempStr = item.NexxoPAN.ToString();
                        sb.Append(((tempStr.Length > 9) ? tempStr.Right(9) : tempStr) + ",");
                        //Branch
                        sb.Append(item.BranchID);
                        sb.Append(System.Environment.NewLine);
                        AddText(fs, sb.ToString());

                        // Add it to the audit list 
                        itemIDs.Add(item.ItemId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error encountered in writing to file: {0}. Exception: {1}", fullyQualifiedFileName, ex.ToString());
                    }
                }
                try
                {
                    // Close the file for the final "batch" & create an audit trail
                    if (fs != null && fs.Length > 0)
                    {
                        fs.Close();
                        // Create Audit Trail
                        CreateAuditTrail(db, partnerID, fileType, fullyQualifiedFileName, itemIDs);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error encountered in closing file: {0}. Exception: {1}", fullyQualifiedFileName, ex.ToString());
                }
            }
        }

        private static void CreateAuditTrail(DataContext dbContext, Guid ChannelPartnerID, string FileType, string FQFileName, List<Guid> ItemIDs)
        {

            int batchRecordCount = ItemIDs.Count;
            // Add Audit Header Record
            int auditHeaderID = InsertX9AuditHeader(dbContext, ChannelPartnerID, FileType, FQFileName, batchRecordCount);
            // Add Audit Detail Record
            InsertX9AuditDetailRecords(dbContext, auditHeaderID, ((FileType.ToLower() == MONEY_ORDER.ToLower()) ? "MoneyOrder" : "Check"), ItemIDs);

        }

        private static int InsertX9AuditHeader(DataContext dbContext, Guid ChannelPartnerID, string FileType, string FQFileName, int RecordCount)
        {

            Table<X9AuditHeader> x9AuditHeaderTable = dbContext.GetTable<X9AuditHeader>();

            X9AuditHeader newX9AuditHeader = new X9AuditHeader
            {
                ChannelPartnerID = new Guid(ChannelPartnerID.ToString()),
                DateGenerated = DateTime.Now,
                RecordCount = RecordCount,
                FileSpec = FQFileName,
                FileType = FileType,
                DTServerCreate = DateTime.Now,
                DTServerLastModified = DateTime.Now
            };

            x9AuditHeaderTable.InsertOnSubmit(newX9AuditHeader);

            try
            {
                dbContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                Console.Write("X9 Audit Header creation exception: {0}", ex.ToString());
            }
            return newX9AuditHeader.AuditHeaderID;
        }

        private static void InsertX9AuditDetailRecords(DataContext dbContext, int AuditHeaderID, string ItemType, List<Guid> ItemIDs)
        {
            Table<X9AuditDetail> x9AuditDetailTable = dbContext.GetTable<X9AuditDetail>();

            List<X9AuditDetail> x9AuditDetails = new List<X9AuditDetail>();

            foreach (var itemID in ItemIDs)
            {
                X9AuditDetail newX9AuditDetail = new X9AuditDetail
                {
                    AuditHeaderID = AuditHeaderID,
                    ItemType = ItemType,
                    ItemPK = itemID,
                    DTServerCreate = DateTime.Now,
                    DTServerLastModified = DateTime.Now
                };
                x9AuditDetails.Add(newX9AuditDetail);
            }
            try
            {
                x9AuditDetailTable.InsertAllOnSubmit(x9AuditDetails);
                dbContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                Console.Write("X9 Audit Detail creation exception: {0}", ex.ToString());
            }
        }

        private static void AddText(FileStream fs, string value)
        {
            try
            {
                byte[] info = new UTF8Encoding(true).GetBytes(value);
                fs.Write(info, 0, info.Length);
            }
            catch (Exception ex)
            {
                Console.Write("Error encountered writing to file. Exception: {0}", ex.ToString());
                throw ex;
            }
        }

        private static string CreateTiffFile(string path, string id, byte[] value, bool isFront)
        {
            string fileName = path + "\\ItemImage_" + id + (isFront ? "_Front_" : "_Rear_") + runDate.ToString("yyyyMMddHHmmss") + ".tiff";
            try
            {
                //var bytes = new UTF8Encoding(true).GetBytes(value);
                byte[] bs = new byte[value.Length - 27];
                for (int i = 27; i < value.Length; i++)
                {
                    bs[i - 27] = value[i];
                }
                var stream = new MemoryStream(bs);
                var image = Image.FromStream(stream);
                image.Save(fileName);
            }
            catch (Exception ex)
            {
                Console.Write("Error creating Tiff Image for file: {0}. Exception: {1}", fileName, ex.ToString());
                throw ex;
            }
            return fileName;
        }
    }
    static class Extensions
    {
        /// <summary>
        /// Get substring of specified number of characters on the right.
        /// </summary>
        public static string Right(this string value, int length)
        {
            if (value.Length < length)
                return value;
            return value.Substring(value.Length - length);
        }
    }
}
