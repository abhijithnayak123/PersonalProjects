using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Impl;
using System.ServiceModel;
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IDataStructuresService
	{
        public Response Cities(long agentSessionId, string state, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.Cities(agentSessionId, state, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
		}

		public Response Countries(long agentSessionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.Countries(agentSessionId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
		}

		public Response States(long agentSessionId, string country, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.States(agentSessionId, country, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

			return response;
		}

        public Response USStates(long agentSessionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.USStates(agentSessionId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

			return response;
		}

        public Response IdCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.IdCountries(agentSessionId, channelPartnerId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

			return response;
		}

		public Response IdStates(long agentSessionId, long channelPartnerId, string country, string idType, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.IdStates(agentSessionId, channelPartnerId, country, idType, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            
            return response;
		}

		public Response IdTypes(long agentSessionId, long channelPartnerId, string country, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.IdTypes(agentSessionId, channelPartnerId, country, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

			return response;
		}
		public Response GetLegalCodes(long agentSessionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.GetLegalCodes(agentSessionId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

			return response;
		}

        public Response GetOccupations(long agentSessionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.GetOccupations(agentSessionId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
		}

        public Response IdRequirements(long agentSessionId, long channelPartnerId, string country, string idType, string state, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.IdRequirements(agentSessionId, channelPartnerId, country, idType, state, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

            return response;
		}

		public Response PhoneTypes(long agentSessionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.PhoneTypes(agentSessionId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

			return response;
		}

        public Response MobileProviders(long agentSessionId, MGIContext mgiContext)
		{
             Response response = new Response();
             try
             {
                 response = DesktopEngine.MobileProviders(agentSessionId, mgiContext);
             }
             catch (FaultException<NexxoSOAPFault> ex)
             {
                 response.Error = PrepareError(ex);
             }

			 return response;
		}

		public Response MasterCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.MasterCountries(agentSessionId, channelPartnerId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

			return response;
		}

        public Response GetMasterCountryByCode(long agentSessionId, string masterCountryAbbr2, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.GetMasterCountryByCode(agentSessionId, masterCountryAbbr2, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

			return response;
		}

		public Response ProfileStatus(long agentSessionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.ProfileStatus(agentSessionId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }

			return response;
		}
	}
}
