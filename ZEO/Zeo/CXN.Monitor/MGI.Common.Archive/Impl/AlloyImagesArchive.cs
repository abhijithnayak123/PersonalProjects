using MGI.Common.Archive.Data;
using P3Net.Data;
using P3Net.Data.Common;
using P3Net.Data.Sql;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;


namespace MGI.Common.Archive.Impl
{
    public class AlloyImagesArchive
    {

        public bool BackupAlloyImages(DataModel dataModel)
        {
			Logger.WriteLog("Backup Alloy images started..");
            SqlConnectionManager sqlconnection = new SqlConnectionManager(dataModel.ConnectionString);
            
			// read data from transaction table and populate collection
            var transactions = ReadAlloyImages(dataModel, sqlconnection);

            // Write to disk and populate table
            var dt = CreateTiffFile(dataModel, transactions);

            if (dt != null && dt.HasRows())
            {
				// Update DB images to null and add file paths
                var rowsCountAffected = UpdateImageTable(dataModel, dt, sqlconnection);
                if (dt.Rows.Count != rowsCountAffected)
                {
					Logger.WriteLogError("Updated records are not matching with retrieved records ");
                    return false;
                }
            }
            else
            {
				Logger.WriteLog("There are no qualified records to archive.");
                return false;
            }

            return true;
        }

		private List<Transaction> ReadAlloyImages(DataModel dataModel, SqlConnectionManager sqlconnection)
		{
			Logger.WriteLog("Retrieving Alloy images from database..");
			List<Transaction> transactions = new List<Transaction>();

			var keyColumn = GetKeyCoulmunName(dataModel.TransactionType);
			var frontImage = GetFrontImageColumnName(dataModel.TransactionType);
			var backImage = GetBackImageCoulmnName(dataModel.TransactionType);

			var storedProcedure = new StoredProcedure("usp_GetAlloyImages");

			storedProcedure.WithParameters(InputParameter.Named("transactionType").WithValue(dataModel.TransactionType));
			storedProcedure.WithParameters(InputParameter.Named("archivePriorToDays").WithValue(dataModel.ArchivePriorToDays));

			try
			{
				IDataReader imageReader = sqlconnection.ExecuteReader(storedProcedure);


				while (imageReader.Read())
				{
					if (imageReader[frontImage] != null && imageReader[frontImage] != DBNull.Value
						&& imageReader[backImage] != null && imageReader[backImage] != System.DBNull.Value)
					{
						Transaction transaction = new Transaction()
						{
							Id = (long)imageReader[keyColumn],
							ChannelPartnerName = (string)imageReader["ChannelPartnerName"],
							Images = new Dictionary<string, byte[]>() 
							{ 
								{frontImage, (byte[])imageReader[frontImage]},
								{backImage, (byte[])imageReader[backImage]}
							}
						};

						transactions.Add(transaction);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLogError(ex.Message);
				Logger.WriteLogError(ex.StackTrace);
			}

			Logger.WriteLog(string.Format("{0} records retrieved successfully from database", transactions.Count));

			return transactions;
		}


		private int UpdateImageTable(DataModel dataModel, DataTable table, SqlConnectionManager sqlconnection)
		{
			Logger.WriteLog("Database update started..");

			string archieveStoredProc = "usp_UpdateAlloyImageTable";

			var storedProcedure = new StoredProcedure(archieveStoredProc);

			DataParameter transactionTypeflag = new DataParameter("transactionType", DbType.Int16);
			transactionTypeflag.Value = dataModel.TransactionType;

			var transactionTable = new DataParameter("transactionTable", DbType.Xml);
			transactionTable.Value = table;

			DataParameter[] dataParameters = new DataParameter[] 
			{
				transactionTypeflag,
                transactionTable
            };

			storedProcedure.WithParameters(dataParameters);

			int resultCount = 0;

			try
			{
				resultCount = sqlconnection.ExecuteNonQuery(storedProcedure);
			}
			catch (Exception ex)
			{
				Logger.WriteLogError(ex.Message);
				Logger.WriteLogError(ex.StackTrace);
			}

			Logger.WriteLog(resultCount + " " + dataModel.TransactionType.ToString() + " rows updated in database");
			return resultCount;
		}

        private string GetKeyCoulmunName(TransactionType transactionType)
        {
            return (transactionType == TransactionType.MoneyOrder ? "TrxId" : "CheckID");
        }

        private string GetFrontImageColumnName(TransactionType transactionType)
        {
            return (transactionType == TransactionType.MoneyOrder ? "CheckFrontImage" : "Front");
        }

        private string GetBackImageCoulmnName(TransactionType transactionType)
        {
            return (transactionType == TransactionType.MoneyOrder ? "CheckBackImage" : "Back");
        }

        private bool CreateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch (Exception ex)
                {
                    Console.Write("Cannot create Directory {0}, exception: {1}", directoryPath, ex.InnerException);
                    return false;
                }
            }
            return true;
        }

        private DataTable CreateDataTable()
        {
            DataTable table = new DataTable();
			table.Columns.Add("TransactionId", typeof(long));
            table.Columns.Add("FrontImagePath", typeof(string));
            table.Columns.Add("BackImagePath", typeof(string));
            return table;
        }

        private DataTable CreateTiffFile(DataModel dm, List<Transaction> transactions)
        {
			Logger.WriteLog("Archiving images started..");

            string fileName = string.Empty;
            DataTable table = CreateDataTable();
            DataRow row;

			var groupedTransactions = transactions.GroupBy(t => t.ChannelPartnerName);

			foreach (var channelPartnerTransactions in groupedTransactions)
			{
				//Logger.WriteLog(string.Format("{0} channel partner has {1} qualified images to archive.", channelPartnerTransactions.Key, channelPartnerTransactions.Count()));
				Logger.WriteLog(string.Format("{0} - {1} {2} images to archive.", channelPartnerTransactions.Key, channelPartnerTransactions.Count(), dm.TransactionType.ToString()));
				
				string targetDirectory = string.Format("{0}\\{1}\\{2}\\{3}", dm.OutputDirectory, channelPartnerTransactions.Key, dm.RunTime.ToString("yyyyMMdd"), dm.TransactionType.ToString());
				var imagesCount = 0;
				
				foreach (var transaction in channelPartnerTransactions)
				{
					if (CreateDirectory(targetDirectory))
					 {
						 row = table.NewRow();
						 row["TransactionId"] = transaction.Id;
						 foreach (var img in transaction.Images)
						 {
							 try
							 {
								 fileName = string.Format("{0}\\{1}_{2}.tiff", targetDirectory, transaction.Id, img.Key);
								 var value = (byte[])img.Value;

								 byte[] bs = new byte[value.Length - 27];
								 for (int i = 27; i < value.Length; i++)
								 {
									 bs[i - 27] = value[i];
								 }

								 var stream = new MemoryStream(bs);
								 var image = Image.FromStream(stream);
								 image.Save(fileName);

								 if (GetFrontImageColumnName(dm.TransactionType) == img.Key)
								 {
									 row["FrontImagePath"] = fileName;
								 }
								 else
								 {
									 row["BackImagePath"] = fileName;
								 }
								 
								 imagesCount++;
							 }
							 catch (Exception ex)
							 {
								 Logger.WriteLogError(string.Format("Error creating Image for file:{0}. Exception {1}",fileName, ex.ToString()));
							 }
						 }
						 table.Rows.Add(row);
					 }
				}
				//Logger.WriteLog(string.Format("{0} images archived for {1} channel partner including front and back images ", imagesCount, channelPartnerTransactions.Key));
				Logger.WriteLog(string.Format("{0} - {1} {2} images archived.", channelPartnerTransactions.Key, channelPartnerTransactions.Count(), dm.TransactionType.ToString()));
			}
			//Logger.WriteLog("Archieved " + table.Rows.Count + "  images");

            return table;
        }
    }
}
