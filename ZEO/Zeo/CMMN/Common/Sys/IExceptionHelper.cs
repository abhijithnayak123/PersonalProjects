using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Common.Sys
{
    public interface IExceptionHelper
    {
        string GetProviderErrorCode(string message);

		bool IsExceptionHandled(Exception ex);
    }
}
