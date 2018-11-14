using System;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.Common;

namespace TCF.Zeo.Cxn.Customer.Data
{
    public class CustomerProfile : BaseRequest
    {
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string LastName2 { get; set; }
        public Helper.Gender? Gender { get; set; }
        public Phone Phone1 { get; set; }
        public Phone Phone2 { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public bool IsExistingCustomer { get; set; }
        public Helper.ProfileStatus ProfileStatus { get; set; }

        //Identification Screen

        public string MothersMaidenName { get; set; }
        public string CountryOfBirth { get; set; }
        public string PrimaryCountryCitizenShip { get; set; }
        public string SecondaryCountryCitizenShip { get; set; }
        public string LegalCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string IdIssuingCountry { get; set; }
        public string IdType { get; set; } //TODO:AO - Change this to IdType
        public string IdIssuingState { get; set; }
        public string IdNumber { get; set; } //TODO:AO- CXN has GovernmentId for this and it has to change
        public DateTime? IdIssueDate { get; set; }
        public DateTime? IdExpirationDate { get; set; }
        //This Field is added for Get NexxoIDType Name for Get Customer Details for Kiosk
        public string IDTypeName { get; set; } //TODO:AO - Do we need this?
        public string CustomerId { get; set; }
        public string ClientCustomerId { get; set; } //TODO-AO, PartnerAccountNumber in CXN, and it need to change as ClientCustomerId

        //Employment Screen
        public string EmployerName { get; set; }
        public string EmployerPhone { get; set; }
        public string Occupation { get; set; }
        public string OccupationDescription { get; set; } // This is hidden and displayed based on channel partner

        //PIN Sscreen
        public string PIN { get; set; }

        //Other properties not displayed in screen
        //IDCode is used along with SSN property to determine, where it is SS or ITIN.
        public Helper.TaxIDCode? IDCode { get; set; }

        //TODO:AO-Do we need this elsewhere?
        public string CardNumber { get; set; }

        //Synovus Specific???
        public string ProgramId { get; set; }
        public string RelationshipAccountNumber { get; set; }
        public string PartnerAccountNumber { get; set; }
        public string BankId { get; set; }
        public string BranchId { get; set; }
        public string IDIssuingStateCode { get; set; }
        public bool CustInd { get; set; }
        public Helper.CustomerType CustomerType { get; set; }
    }

    public class Phone
    {
        public string Number { get; set; }
        public string Type { get; set; }
        public string Provider { get; set; }
    }

    public class Address
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }
}