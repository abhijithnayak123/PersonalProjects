using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Common.Util;

using MGI.Common.DataAccess.Contract;

using AutoMapper;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Core.CXE.Impl
{
	public class CustomerServiceImpl : ICustomerService
	{
		private IRepository<Customer> _customerRepo;
		public IRepository<Customer> CustomerRepo { set { _customerRepo = value; } }

		private IIDNumberBuilder _idBuilder;
		public IIDNumberBuilder IDBuilder { set { _idBuilder = value; } }
        public NLoggerCommon NLogger { get; set; }
        public TLoggerCommon MongoDBLogger { get; set; }
		public CustomerServiceImpl()
		{

		}

		public Customer Register( Customer customer )
		{
            try
            {
                _customerRepo.AddWithFlush(customer);
            }
            catch (Exception ex)
            {
                NLogger.Error(string.Format("EXCEPTION SAVING CUSTOMER: {0}", ex.Message));                
                var innerException = (SqlException)ex.InnerException;
                if (innerException != null && innerException.Number == 2627)
                    throw new CXECustomerException(CXECustomerException.REGISTRATION_FAILED_DUPLICATE_ID, ex);
                throw new CXECustomerException(CXECustomerException.REGISTRATION_FAILED_DATABASE, ex);
            }

            return customer;
		}

		public Customer Lookup(long alloyId)
		{
			try
			{
				var customer = _customerRepo.FindBy(x => x.Id == alloyId);
                return customer;
			}
			catch ( Exception ex )
			{
                //AL-3370 Transactional Log User Story
                MongoDBLogger.Error<string>(Convert.ToString(alloyId), "Lookup", AlloyLayerName.CXE, ModuleName.Transaction,
                "Error in Lookup -MGI.Core.CXE.Impl.CustomerServiceImpl", ex.Message, ex.StackTrace);
				throw new CXECustomerException( CXECustomerException.CUSTOMER_NOT_FOUND, ex );
			}
		}

		public void Save( Customer customer )
		{
			Customer savedCustomer = Lookup( customer.Id );

			savedCustomer.UpdateProfile( customer );
			savedCustomer.AddOrUpdateGovernmentId( customer.GovernmentId );
			savedCustomer.AddOrUpdateEmployment( customer.EmploymentDetails );

			try
			{
				_customerRepo.Merge( savedCustomer );
				_customerRepo.Flush();
			}
			catch ( Exception ex )
			{
				throw new CXECustomerException( CXECustomerException.CUSTOMER_UPDATE_FAILED, ex );
			}
		}

		public List<Customer> Lookup( CustomerSearchCriteria criteria )
		{
			List<Customer> customers = new List<Customer>();

			if ((!string.IsNullOrWhiteSpace(criteria.FirstName) || !string.IsNullOrWhiteSpace(criteria.LastName)
				|| criteria.DateOfBirth != null || !string.IsNullOrWhiteSpace(criteria.PhoneNumber) || !string.IsNullOrWhiteSpace(criteria.GovernmentId)
				|| criteria.AlloyID > 0 || !string.IsNullOrWhiteSpace(criteria.SSN)) && criteria.IsIncludeClosed)
			{
				customers =
					_customerRepo.FilterBy(
						c => (string.IsNullOrWhiteSpace(criteria.FirstName) || c.FirstName == criteria.FirstName)
							&& (string.IsNullOrWhiteSpace(criteria.LastName) || c.LastName == criteria.LastName)
							&& ( criteria.DateOfBirth == null || c.DateOfBirth == criteria.DateOfBirth )
							&& (string.IsNullOrWhiteSpace(criteria.PhoneNumber) || c.Phone1 == criteria.PhoneNumber)
							&& (string.IsNullOrWhiteSpace(criteria.GovernmentId) || c.GovernmentId.Identification == criteria.GovernmentId)
							&& (string.IsNullOrWhiteSpace(criteria.SSN) || c.SSN == criteria.SSN)
                            && ( criteria.AlloyID <= 0 || c.Id == criteria.AlloyID )
						).ToList<Customer>();
			}
			else if (!string.IsNullOrWhiteSpace(criteria.FirstName) || !string.IsNullOrWhiteSpace(criteria.LastName)
				|| criteria.DateOfBirth != null || !string.IsNullOrWhiteSpace(criteria.PhoneNumber) || !string.IsNullOrWhiteSpace(criteria.GovernmentId)
				|| criteria.AlloyID > 0 || !string.IsNullOrWhiteSpace(criteria.SSN))
			{
				customers =
					_customerRepo.FilterBy(
						c => (string.IsNullOrWhiteSpace(criteria.FirstName) || c.FirstName == criteria.FirstName)
							&& (string.IsNullOrWhiteSpace(criteria.LastName) || c.LastName == criteria.LastName)
							&& (criteria.DateOfBirth == null || c.DateOfBirth == criteria.DateOfBirth)
							&& (string.IsNullOrWhiteSpace(criteria.PhoneNumber) || c.Phone1 == criteria.PhoneNumber)
							&& (string.IsNullOrWhiteSpace(criteria.GovernmentId) || c.GovernmentId.Identification == criteria.GovernmentId)
							&& (string.IsNullOrWhiteSpace(criteria.SSN) || c.SSN == criteria.SSN)
							&& (criteria.AlloyID <= 0 || c.Id == criteria.AlloyID)
							&& (c.ProfileStatus != ProfileStatus.Closed)
						).ToList<Customer>();
			}

			return customers;
		}

        public long Get(string Phone, string PIN)
        {
            Customer customer = _customerRepo.FilterBy(x => x.Phone1 == Phone && x.PIN == PIN).FirstOrDefault();
            if (customer != null)
                return customer.Id;
            else
                throw new CXECustomerException(CXECustomerException.CUSTOMER_NOT_FOUND, "Customer Not Found For the Phone and PIN");
        }


        public Customer Lookup(long channelPartnerId, string firstName, string lastName)
        {
            try
            {
                var customer = _customerRepo.FindBy(x => x.ChannelPartnerId == channelPartnerId && x.FirstName == firstName && x.LastName == lastName);
                return customer;
            }
            catch (Exception ex)
            {
                throw new CXECustomerException(CXECustomerException.CUSTOMER_NOT_FOUND, ex);
            }
        }

		public void ValidateStatus(long alloyId)
        {
			var customer = Lookup(alloyId);

            if (customer.ProfileStatus==ProfileStatus.Inactive)
                throw new CXECustomerException(CXECustomerException.CUSTOMER_VALIDATESTATUS_FAILED);
        }


		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee 
		/// Lookup for Customers ends with AlloyID No(8 digits)
		/// </summary>
		/// <param name="alloyId"></param>
		/// <returns></returns>
		public Customer Lookup(string alloyId)
		{
			var customers = from customer in _customerRepo.All()
							where customer.Id.ToString().Trim().EndsWith(alloyId.Trim())
							select customer;


			return customers.FirstOrDefault();
		}
	}
}
