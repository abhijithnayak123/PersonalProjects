

using OPT.TalentRadar.Common.Data;

namespace OPT.TalentRadar.BIZ.Contract
{
    public interface IEmailService
    {
        bool CandidateMail(Mail mail);
        bool RecruiterMail(Mail Mail);
    }
}
