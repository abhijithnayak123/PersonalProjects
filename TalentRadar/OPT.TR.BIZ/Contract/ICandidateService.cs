using OPT.TalentRadar.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.BIZ.Contract
{
    public interface ICandidateService
    {
        /// <summary>
        /// Add a new candidate information
        /// </summary>
        /// <param name="candidate">candidate information is passed</param>
        /// <returns></returns>
        bool NewCandidate(Candidate candidate);

        /// <summary>
        /// Update candidate information
        /// </summary>
        /// <param name="candidate">candidate information</param>
        /// <returns></returns>
        bool UpdateCandidate(Candidate candidate);

        /// <summary>
        /// Get candidate information by candidate unique id
        /// </summary>
        /// <param name="candidateId">unique id</param>
        /// <returns>Candidate information</returns>
        Candidate GetCandidate(string token);

        /// <summary>
        /// Find a candidate using search filter
        /// </summary>
        /// <param name="filter">filter by PhoneNumber/Email/Token</param>
        /// <returns></returns>
        Candidate FindCandidate(string filter);

        /// <summary>
        /// Fetch all candiates
        /// </summary>
        /// <returns></returns>
        List<Candidate> GetAllCandidates();

        /// <summary>
        /// Update Candidate Status
        /// </summary>
        /// <param name="CandidateStatus">interview decline reason</param>
        /// <returns></returns>
        bool UpdateCandidateStatus(CandidateStatus status);

        string ValidateCandidate(CandidateValidation candidate);

        CandidateDetails GetCandidateDetails(CandidateLogin candidateLogin);

        CandidateNotification GetNotifications(string filter);

        Recruiter GetRecuiterDetails(string filter);

        CandidateDetails GetCandidateBasedOnToken(string filter);

        List<CandidateDetails> GetListCandidate(CandidateDetails candidateDetail, int filter, int numberOfRecord);

        bool UpdateFeedBack(FeedBack feedBack);
    }
}
