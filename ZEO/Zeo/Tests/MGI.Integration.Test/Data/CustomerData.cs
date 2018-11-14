using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using System;
using System.Text;

namespace MGI.Integration.Test.Data
{
    public partial class IntegrationTestData
    {
        public static CustomerProfile GetCustomerProspect(ChannelPartner channelPartner)
        {

            CustomerProfile prospect = new CustomerProfile();
            {
               prospect.FirstName = GetName("JAMES");
               prospect.MiddleName = GetName("CHRIST");
               prospect.Gender = HelperGender.MALE;
               prospect.LastName = channelPartner.Name;
               prospect.LastName2 = "HENRY";
               prospect.MothersMaidenName = "JACALBERT";
               prospect.PIN = "80205";
               prospect.SSN = RandomNumber(8);
               prospect.Email = "JAMES@AABC.COM";
               prospect.ProfileStatus = HelperProfileStatus.Active;
               prospect.ChannelPartnerId = channelPartner.Id;
               prospect.DateOfBirth = new DateTime(1950, 10, 10);

               prospect.IdIssuingCountry = "UNITED STATES";
               prospect.CountryOfBirth = "US";
               prospect.IdExpirationDate = new DateTime(2019, 10, 10);
               prospect.IdIssueDate = new DateTime(2011, 10, 10);
               prospect.IdNumber = "ASDFGHJKIU";
               prospect.IdType = "PASSPORT";
               prospect.IDTypeName = "";
                prospect.IdIssuingState = "";

               prospect.Occupation = "00014";
               prospect.EmployerName = "Opteamix";
                prospect.EmployerPhone = "6509878765";

                prospect.Phone1 = new Phone()
                {
                    Number = "3039637881",
                    Type = "Home",
                    Provider = "",
                };

                prospect.Address = new Address();
                {
                    prospect.Address.Address1 = "FORT COLLINS,#121";
                    prospect.Address.Address2 = "COLORADO TOURISM OFFICE";
                    prospect.Address.City = "DENVER";
                    prospect.Address.State = "CO";
                }
            }
            return prospect;
        }

        private static string RandomNumber(int length)
        {
            string ssn = "6";
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                ssn += Convert.ToString(random.Next(0, 9));
            }
            return ssn;
        }

        internal static string GetName(string name)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random random = new Random();
            StringBuilder str = new StringBuilder();
            str = str.Append(name);
            for (int i = 0; i < 5; i++)
            {
                str.Append(chars[random.Next(chars.Length)]);
            }
            return str.ToString();
        }
    }
}
