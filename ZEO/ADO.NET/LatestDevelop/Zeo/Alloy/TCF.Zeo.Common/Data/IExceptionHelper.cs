using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Zeo.Common.Data
{
    public interface IExceptionHelper
    {
        string GetProviderErrorCode(string message);

		bool IsExceptionHandled(Exception ex);
    }
}
