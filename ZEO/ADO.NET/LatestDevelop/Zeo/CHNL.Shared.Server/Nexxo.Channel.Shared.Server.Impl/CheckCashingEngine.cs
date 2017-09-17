﻿using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;
using MGI.Channel.Shared.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using Spring.Transaction.Interceptor;

using MGI.Biz.CPEngine.Contract;

using BizCommon = MGI.Biz.Common.Data;

using SharedData = MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.Shared.Server.Impl
{
    public partial class SharedEngine : ICheckCashingService
    {
        #region Injected Services

        public ICPEngineService CPEngineService { private get; set; }

        #endregion

        #region CheckCashingService Data Mapper

        internal static void CheckCashingConverter()
        {
           
        }

        #endregion

        #region ICheckCashingService Impl

	    #endregion
    }
}
