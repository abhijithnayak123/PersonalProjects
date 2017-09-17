using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.SSO
{
	public enum SSOErrorCodes
	{
		NoError = 0,
		TerminalNotSetup,
		CookieNotFound,
		AuthenticationFailure,
		SAMLParseError,
		SAMLCertificateNotFound,
		SAMLCertificateInvalid,
		SAMLSignatureNotFound,
		SAMLVerificationFailure,
		SAMLAttributeStatementNotFound,
		SAMLAttributeNotFound,
        ApplicationError,
        UserRoleNotFound
	}
}