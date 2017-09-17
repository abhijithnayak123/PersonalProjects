using System.Linq;
using AutoMapper;
using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Data;
using MGI.Cxn.Customer.Contract;
using MGI.Cxn.Customer.Data;
using MGI.Cxn.Customer.FIS.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using FISConnectData = MGI.Cxn.Customer.FIS.Data;
using MGI.Common.Util;

namespace MGI.Cxn.Customer.FIS.Impl
{
    /// <summary>
    /// Class to implement the SQL Injection User Story. US#1789
    /// </summary>
    public class FISConnect : IFISConnect
    {
        public IRepository<FISConnectData.FISConnect> FISConnectRepo { private get; set; }
        public NLoggerCommon NLogger { get; set; }


        public FISConnect()
        {
            // TODO : Need to implement 
        }

        public FISConnectData.FISConnect GetSSNForCustomer(string SSN)
        {
            try
            {
                NLogger.Info(string.Format("Start - GetSSNForCustomer for SQL to LINQ-nHibernate"));
                return FISConnectRepo.FindBy(t => t.CustomerTaxNumber == SSN.Trim().ToString());
            }
            catch (Exception ex)
            {
                NLogger.Error(string.Format("Error while getting SSN for Customer SSN Number :- {0}", SSN) + ex.Message);
                throw new Exception("Error while getting SSN for Customer SSN Number",ex);
            }
        }

        //Get Cutomers form FISConnectDB
        public List<FISConnectData.FISConnect> FISConnectCustomerLookUp(Dictionary<string, object> customerLookUpCriteria)
        {
            string sSN = "", phoneNumber = "", zipCode = "";
            if (customerLookUpCriteria.ContainsKey("SSN")) sSN = Convert.ToString(customerLookUpCriteria["SSN"]);
            if(customerLookUpCriteria.ContainsKey("PhoneNumber")) phoneNumber = Convert.ToString(customerLookUpCriteria["PhoneNumber"]);
            if(customerLookUpCriteria.ContainsKey("ZipCode")) zipCode = Convert.ToString(customerLookUpCriteria["ZipCode"]);

            try
            {
               NLogger.Info(string.Format("Start - CustomerLookUp for SQL to LINQ-nHibernate"));
               List<FISConnectData.FISConnect> fisConnects = FISConnectRepo.FilterBy(t => (t.CustomerTaxNumber == sSN)
                   && (string.IsNullOrEmpty(phoneNumber) || t.PrimaryPhoneNumber == phoneNumber)
                   && (string.IsNullOrEmpty(zipCode) || t.ZipCode == zipCode)
                   ).ToList();
                return fisConnects;
            }
            catch (Exception ex)
            {

                NLogger.Error(string.Format("Error while getting SSN-Phone Number-Zipcode for Customer SSN Number :- {0}:{1}:{2}", sSN,phoneNumber,zipCode ) + ex.Message);
                throw new Exception("Error while getting SSN for Customer SSN Number",ex);
            }

        }
    }
}
