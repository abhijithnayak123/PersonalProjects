using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Diagnostics;
using MGI.Common.DataAccess.Contract;
using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Common.Util;
using BillPayStage = MGI.Core.CXE.Data.Transactions.Stage.BillPay;
using BillPayCommit = MGI.Core.CXE.Data.Transactions.Commit.BillPay;
using MGI.Common.DataProtection.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Core.CXE.Impl
{
	public class BillPayServiceImpl : IBillPayService
	{

		public IRepository<BillPayStage> BillPayStageRepo { private get; set; }
		public IRepository<BillPayCommit> BillPayCommitRepo { private get; set; }
        public IDataProtectionService BPDataProtectionSvc { private get; set; }
        public TLoggerCommon MongoDBLogger { get; set; }
        public NLoggerCommon NLogger = new NLoggerCommon();

		public BillPayServiceImpl()
		{
			Mapper.CreateMap<BillPayStage, BillPayCommit>();
		}

		public long Create(BillPayStage billPay)
		{
			try
			{
				BillPayStageRepo.AddWithFlush(billPay);
				return billPay.Id;
			}
			catch (Exception ex)
			{
				
                NLogger.Error("Error while staging BillPay transaction. " + ex.Message);

              //  AL-1014 Transactional Log User Story
				MongoDBLogger.Error<BillPayStage>(billPay, "Create", AlloyLayerName.CXE, ModuleName.BillPayment,
				 "Error in Create -MGI.Core.CXE.Impl.BillPayServiceImpl", ex.Message, ex.StackTrace);
				
				throw new Exception("Error while staging BillPay transaction",ex);
			}
		}

		public void Update(long Id, TransactionStates state, string confirmationNumber)
		{
			try
			{
				BillPayStage billpay = BillPayStageRepo.FindBy(x => x.Id == Id);
				billpay.Status = (int)state;
				billpay.DTServerLastModified = DateTime.Now;
				billpay.ConfirmationNumber = confirmationNumber;
				BillPayStageRepo.Merge(billpay);
			}
			catch (Exception ex)
			{
				
                NLogger.Error("Error while updating Billpay transaction. " + ex.Message);
                //AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Stage Id:" + Convert.ToString(Id));
				details.Add("Transaction States:" + Convert.ToString(state));
				details.Add("Confirmation Number:" + confirmationNumber);
				MongoDBLogger.ListError<string>(details, "Update", AlloyLayerName.CXE, ModuleName.BillPayment,
				 "Error in Update -MGI.Core.CXE.Impl.BillPayServiceImpl", ex.Message, ex.StackTrace);
                
				throw new Exception("Error while updating BillPay transaction",ex);
			}
		}

		public void Update(long Id, TransactionStates state, string confirmationNumber, decimal fee)
		{
			try
			{
				var billpay = BillPayStageRepo.FindBy(x => x.Id == Id);
				billpay.Status = (int)state;
				billpay.Fee = fee;
				billpay.DTServerLastModified = DateTime.Now;
				billpay.ConfirmationNumber = confirmationNumber;
				BillPayStageRepo.Merge(billpay);
			}
			catch (Exception ex)
			{				
                NLogger.Error("Error while updating Billpay transaction. " + ex.Message);
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Stage Id:" + Convert.ToString(Id));
				details.Add("Transaction States:" + Convert.ToString(state));
				details.Add("Confirmation Number:" + confirmationNumber);
				details.Add("Fee:" + Convert.ToString(fee));
				MongoDBLogger.ListError<string>(details, "Update", AlloyLayerName.CXE, ModuleName.BillPayment,
				 "Error in Update -MGI.Core.CXE.Impl.BillPayServiceImpl", ex.Message, ex.StackTrace);
                
				throw new Exception("Error while updating BillPay transaction",ex);
			}
		}

		public void Commit(long Id)
		{
			try
			{
				var billPayStage = BillPayStageRepo.FindBy(x => x.Id == Id);
				var billPayCommit = Mapper.Map<BillPayStage, BillPayCommit>(billPayStage);
				billPayCommit.Id = Id; //Id field will be used to relate stage and commit table.
				billPayCommit.Status = (int)TransactionStates.Committed;
				//BillPayCommitRepo.SaveOrUpdate(billPayCommit); // this does not seem to commit immediately
				BillPayCommitRepo.AddWithFlush(billPayCommit);
			}
			catch (Exception ex)
			{

                NLogger.Error("Error while commiting Billpay transaction. " + ex.Message);

				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(Id), "Commit", AlloyLayerName.CXE, ModuleName.BillPayment,
				 "Error in Commit -MGI.Core.CXE.Impl.BillPayServiceImpl", ex.Message, ex.StackTrace);
				
				throw new Exception("Error while commiting Billpay transaction",ex);
			}
		}

		public BillPayCommit Get(long Id)
		{
			try
			{
				return BillPayCommitRepo.FindBy(x => x.Id == Id);
			}
			catch (Exception ex)
			{
                NLogger.Error("Error in retrieving Billpay Commit transaction data. " + ex.Message);

				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(Id), "Get", AlloyLayerName.CXE, ModuleName.BillPayment,
				"Error in Get -MGI.Core.CXE.Impl.BillPayServiceImpl", ex.Message, ex.StackTrace);
				
				throw new Exception("Error in retrieving Billpay Commit transaction data",ex);
			}
		}


		public BillPayStage GetStage(long Id)
		{
			try
			{
				return BillPayStageRepo.FindBy(x => x.Id == Id);
			}
			catch (Exception ex)
			{				
                NLogger.Error("Error in retrieving Billpay Stage transaction data. " + ex.Message);

				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(Id), "GetStage", AlloyLayerName.CXE, ModuleName.BillPayment,
				"Error in GetStage-MGI.Core.CXE.Impl.BillPayServiceImpl", ex.Message, ex.StackTrace);
				
				throw new Exception("Error in retrieving Billpay Stage transaction data",ex);
			}
		}


		public void Update(long Id, string billerName, string accountNumber, decimal amount)
		{
			try
			{
				BillPayStage billpay = BillPayStageRepo.FindBy(x => x.Id == Id);
				billpay.ProductName = billerName;
				billpay.AccountNumber = Encrypt(accountNumber);
				billpay.Amount = amount;
				BillPayStageRepo.UpdateWithFlush(billpay);
			}
			catch (Exception ex)
			{
			    NLogger.Error("Error while updating Billpay transaction. " + ex.Message);
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(Id));
				details.Add("Biller Name:" + billerName);
				details.Add("Account Number:" + accountNumber);
				details.Add("Amount:" + Convert.ToString(amount));
				MongoDBLogger.ListError<string>(details, "Update", AlloyLayerName.CXE, ModuleName.BillPayment,
				 "Error in Update -MGI.Core.CXE.Impl.BillPayServiceImpl", ex.Message, ex.StackTrace);
                
				throw new Exception("Error while updating BillPay transaction",ex);
			}
		}

        private string Encrypt(string plainString)
        {
            if (!string.IsNullOrWhiteSpace(plainString) && plainString.IsCreditCardNumber())
            {
                return BPDataProtectionSvc.Encrypt(plainString, 0);
            }
            return plainString;
        }

        private string Decrypt(string encryptedString)
        {
            string decryptString = encryptedString;
            if (!string.IsNullOrWhiteSpace(encryptedString))
            {
                try
                {
                    decryptString = BPDataProtectionSvc.Decrypt(encryptedString, 0);
                }
                catch(Exception ex) 
                {
                    decryptString = encryptedString;
					NLogger.Error(string.Format("Error while Decrypting data: {0} \n Stack Trace: {1}" , ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available"));

					//AL-1014 Transactional Log User Story
					MongoDBLogger.Error<string>(encryptedString, "Decrypt", AlloyLayerName.CXE, ModuleName.BillPayment, 
					"Error in Decrypt -MGI.Core.CXE.Impl.BillPayServiceImpl", ex.Message, ex.StackTrace);
				
                }
            }
            return decryptString;
        }
	}
}
