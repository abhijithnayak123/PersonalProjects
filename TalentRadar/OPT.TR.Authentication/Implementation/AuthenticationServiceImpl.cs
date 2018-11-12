using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Web;
using System.DirectoryServices.AccountManagement;
using System.Configuration;
using OPT.TalentRadar.Common.Data;
using OPT.TalentRadar.Authentication.Contract;

namespace OPT.TalentRadar.Authentication.Implementation
{
    public class AuthenticationServiceImpl : IAuthenticationService
    {

        private string _domainName = string.Empty;
        private string _userName = string.Empty;
        private string _password = string.Empty;
        private string _groupName = string.Empty;

        private const string SamAccountName = "samaccountname=";
        private const string DisplayName = "cn";
        private const string UserAccountControl = "useraccountcontrol";

        private const int InvalidDomainPath = -2147467259;
        private const int NotOperational = -2147016646;
        private const int Unknown = -2147463168;


        /// <summary>
        /// To validate Login User in Ad 
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ADError ValidateUser(string userName, string password)
        {
            ADError err = ADError.AD_Success;
            string domainName = ConfigurationManager.AppSettings.Get("DomainName");

            this._domainName = HttpUtility.HtmlDecode(domainName);
            this._userName = HttpUtility.HtmlDecode(userName);
            this._password = HttpUtility.HtmlDecode(password);

            //Validate Input Parameters
            if ((err = ValidateParams()) != ADError.AD_Success)
                return err;

            //Get AD Directory Entry
            if ((err = ValidateInAD()) != ADError.AD_Success)
                return err;

            return err;
        }

        private ADError ValidateParams()
        {
            ADError err = ADError.AD_Success;

            if (string.IsNullOrWhiteSpace(_domainName) || string.IsNullOrWhiteSpace(_userName) || string.IsNullOrWhiteSpace(_password))
                return ADError.AD_InvalidParams;

            return err;
        }

        private ADError ValidateInAD()
        {
            ADError err = ADError.AD_Success;
            DirectoryEntry directoryEntry = null;
            DirectorySearcher directorySearcher = null;
            try
            {
                directoryEntry = new DirectoryEntry(_domainName, _userName, _password);
                directorySearcher = new DirectorySearcher(directoryEntry);
                if (true)
                {

                    directorySearcher.Filter = SamAccountName + _userName;
                    directorySearcher.PropertiesToLoad.Add(DisplayName);
                    directorySearcher.PropertiesToLoad.Add(UserAccountControl);
                    SearchResult searchResult = directorySearcher.FindOne();

                    if (searchResult == null)
                    {
                        err = ADError.AD_InvalidCredentials;
                    }
                    else
                    {
                        if (!IsActive(searchResult))
                        {
                            err = ADError.AD_InvalidCredentials;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == NotOperational || ex.HResult == Unknown)
                    err = ADError.AD_NotOperational;
                else if (ex.HResult == InvalidDomainPath)
                    err = ADError.AD_InvalidDomainPath;
                else
                    err = ADError.AD_InvalidCredentials;
            }
            finally
            {
                if (directorySearcher != null)
                {
                    directorySearcher.Dispose();
                }
                if (directoryEntry != null)
                {
                    directoryEntry.Close();
                    directoryEntry.Dispose();
                }
            }
            return err;
        }

        private bool IsActive(SearchResult searchResult)
        {
            if (searchResult.Properties["userAccountControl"].Count > 0)
            {
                int flags = (int)searchResult.Properties["userAccountControl"][0];
                return !Convert.ToBoolean(flags & 0x0002);
            }
            return false;
        }
        /// <summary>
        /// Method to Check the User belongs to AAM Group.
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="userName"></param>
        /// <param name="applicationName"></param>
        /// <returns></returns>
        public ADError ValidatepGroupUser(string userName)
        {
            #region object Declaration
            ADError err = ADError.AD_Success;

            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, userName);
            #endregion object Declaration

            try
            {
                _groupName = ConfigurationManager.AppSettings.Get("ADGroup");
                string applicationName = ConfigurationManager.AppSettings.Get("AppName");

                if (applicationName.Trim().ToLower() == ADConstants.ApplicationTool.ToLower())
                {
                    GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, _groupName);

                    if (user != null)
                    {
                        // check if user is member of that group
                        if (user.IsMemberOf(group))
                        {
                            err = ADError.AD_Success;
                        }
                        else
                        {
                            err = ADError.AD_InvalidMemberInGroup;
                        }
                    }
                    else
                    {
                        err = ADError.AD_InvalidCredentials;
                    }
                }
                else
                {
                    err = ADError.AD_NotOperational;
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == NotOperational || ex.HResult == Unknown)
                    err = ADError.AD_NotOperational;
                else if (ex.HResult == InvalidDomainPath)
                    err = ADError.AD_InvalidDomainPath;
                else
                    err = ADError.AD_InvalidCredentials;
            }
            finally
            {
                if (user != null)
                {
                    user.Dispose();
                }
                if (ctx != null)
                {
                    ctx.Dispose();
                }
            }
            return err;
        }

        public bool ValidateExistingGroupUser(string userName, string domainName, List<string> userGroup)
        {
            #region object Declaration
            ADError err = ADError.AD_Success;


            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domainName);
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, userName);
            bool vallidateExistingGroupUser = false;
            #endregion object Declaration

            try
            {
                foreach (string Group in userGroup)
                {
                    GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, Group);
                    if (user != null)
                    {
                        // check if user is member of that group
                        if (user.IsMemberOf(group))
                        {
                            err = ADError.AD_Success;
                            vallidateExistingGroupUser = true;
                        }
                        else
                        {
                            err = ADError.AD_InvalidMemberInGroup;
                        }
                    }
                    else
                    {
                        err = ADError.AD_InvalidCredentials;
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.HResult == NotOperational || ex.HResult == Unknown)
                    err = ADError.AD_NotOperational;
                else if (ex.HResult == InvalidDomainPath)
                    err = ADError.AD_InvalidDomainPath;
                else
                    err = ADError.AD_InvalidCredentials;
            }
            finally
            {
                if (user != null)
                {
                    user.Dispose();
                }
                if (ctx != null)
                {
                    ctx.Dispose();
                }
            }

            return vallidateExistingGroupUser;
        }

        public List<ADUser> GetADUserDetails(string userName)
        {
            List<ADUser> ADUsers = new List<ADUser>();
            DirectoryEntry directory = null;
            DirectorySearcher search = null;
            string domainName = ConfigurationManager.AppSettings.Get("DomainName");

            try
            {
                directory = new DirectoryEntry(domainName);
                search = new DirectorySearcher(directory);
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    search.Filter = SamAccountName + userName;
                }
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("givenName");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("AccountExpires");
                search.PropertiesToLoad.Add("sn");
                search.PropertiesToLoad.Add("UserAccountControl");
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("manager");
                search.PropertiesToLoad.Add("mobile");
                search.PropertiesToLoad.Add("telephoneNumber");
                search.PropertiesToLoad.Add("otherTelephone");


                SearchResult result;
                SearchResultCollection resultCollection = search.FindAll();

                if (resultCollection != null)
                {
                    for (int counter = 0; counter < resultCollection.Count; counter++)
                    {
                        result = resultCollection[counter];
                        if (result.Properties.Contains("UserAccountControl") && ((Int32)result.Properties["UserAccountControl"][0] & 512) == 512)
                        {
                            ADUser adUser = new ADUser();
                            adUser.ADStatus = ((Int32)result.Properties["UserAccountControl"][0] & 512) == 512 ? "Active" : "Inactive";
                            adUser.FirstName = PropertyValue(result, "givenName");
                            adUser.LastName = PropertyValue(result, "sn");
                            adUser.UserName = PropertyValue(result, "mail");
                            adUser.ADName = PropertyValue(result, "samaccountname");
                            adUser.Manager = PropertyValue(result, "manager") == " " ? " " : PropertyValue(result, "manager").Split(',')[0].Replace("CN=", "");
                            adUser.FullName = PropertyValue(result, "cn");
                            if (result.Properties.Contains("AccountExpires"))
                            {
                                long date = (long)result.Properties["AccountExpires"][0];
                                adUser.ExpirationDate = LongToDateTime(date);
                            }
                            adUser.Mobile = PropertyValue(result, "mobile");
                            adUser.Phone1 = PropertyValue(result, "telephoneNumber");
                            adUser.Phone2 = PropertyValue(result, "otherTelephone");
                            ADUsers.Add(adUser);
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (search != null)
                {
                    search.Dispose();
                }
                if (directory != null)
                {
                    directory.Close();
                    directory.Dispose();
                }
            }
            return ADUsers;
        }

        private string PropertyValue(SearchResult result, string p)
        {
            string Value = string.Empty;
            if (result.Properties.Contains(p))
            {
                Value = (String)result.Properties[p][0];
            }
            return Value;
        }

        private DateTime LongToDateTime(long lAccountExpirationDate)
        {
            long defaultExpireDate = 0x7FFFFFFFFFFFFFFF;

            if (lAccountExpirationDate == 0)
                lAccountExpirationDate = defaultExpireDate;

            if (lAccountExpirationDate == defaultExpireDate)
            {
                return new DateTime(2100, 12, 31);
            }
            return DateTime.FromFileTime(lAccountExpirationDate);
        }
    }
}
