using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;

using UserDetailsDTO = MGI.Channel.DMS.Server.Data.UserDetails;
using UserRoleDTO = MGI.Channel.DMS.Server.Data.UserRole;
using UserStatusDTO = MGI.Channel.DMS.Server.Data.UserStatus;
using UserSearchCriteriaDTO = MGI.Channel.DMS.Server.Data.UserSearchCriteria;
using LocationDTO = MGI.Channel.DMS.Server.Data.Location;

using SessionContextDTO = MGI.Channel.DMS.Server.Data.SessionContext;
using SessionContext = MGI.Biz.Partner.Data.SessionContext;

using UserDetails = MGI.Biz.Partner.Data.UserDetails;
using UserRole = MGI.Biz.Partner.Data.UserRole;
using UserStatus = MGI.Biz.Partner.Data.UserStatus;
using UserSearchCriteria = MGI.Biz.Partner.Data.UserSearchCriteria;
using Location = MGI.Biz.Partner.Data.Location;

using chnlSaveMode = MGI.Channel.DMS.Server.Data.SaveMode;
using bizSaveMode = MGI.Biz.Partner.Data.SaveMode;

using AutoMapper;
using MGI.Common.Util;

using MGI.Biz.Partner.Contract;
using System.Net.Mail;
using Spring.Transaction.Interceptor;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : IUserService
	{
		IManageUsers UserService { get; set; }

		internal static void UserEngineConverter()
		{
			Mapper.CreateMap<UserDetails, UserDetailsDTO>();
			Mapper.CreateMap<UserDetailsDTO, UserDetails>();
			Mapper.CreateMap<UserRole, UserRoleDTO>();
			Mapper.CreateMap<UserStatus, UserStatusDTO>();
			Mapper.CreateMap<UserSearchCriteriaDTO, UserSearchCriteria>();
			Mapper.CreateMap<Location, LocationDTO>();

			Mapper.CreateMap<chnlSaveMode, SaveMode>();

			Mapper.CreateMap<MGI.Biz.Partner.Data.ChannelPartnerSMTPDTO, ChannelPartnerSMTP>();
			Mapper.CreateMap<ChannelPartnerSMTP, MGI.Biz.Partner.Data.ChannelPartnerSMTPDTO>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="UserId"></param>
		/// <returns></returns>
		[Transaction(ReadOnly = true)]
		public Response GetUser(long agentSessionId, int UserId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			var userdetails = UserService.GetUser(agentSessionId, UserId, context);

			Response response = new Response();
			response.Result = Mapper.Map<UserDetails, UserDetailsDTO>(userdetails);
			return response;
		}

		[Transaction(ReadOnly = true)]
		public Response GetUsers(long agentSessionId, long locationId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			var userDetails = UserService.GetUsers(agentSessionId, locationId, context);

			Response response = new Response();
			response.Result = Mapper.Map<List<UserDetails>, List<UserDetailsDTO>>(userDetails);
			return response;
		}
	}
}
