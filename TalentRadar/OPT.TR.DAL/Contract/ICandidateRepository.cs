using OPT.TalentRadar.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.DAL.Contract
{
    public interface ICandidateRepository
    {
        /// <summary>
        /// Add a new candidate information
        /// </summary>
        /// <param name="candidate">candidate information is passed</param>
        /// <returns>unique token</returns>
        string AddCandidate(Candidate candidate);

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
        Candidate GetCandidate(long candidateId);

        /// <summary>
        /// Get candidate information by email-Id/Phone Number/Token
        /// </summary>
        /// <param name="filter">filter value EmailId,Token or PhoneNumber</param>
        /// <returns>candidate information</returns>
        Candidate GetCandidate(string filter);

        /// <summary>
        /// Get all exsisting candidates 
        /// </summary>
        /// <returns>collection of candidate information</returns>
        List<Candidate> GetCandidates();

        /// <summary>
        /// Update the decline reason
        /// </summary>
        /// <param name="reason">interview decline reason</param>
        /// <returns></returns>
        bool UpdateCandidateStatus(CandidateStatus status);

        //List<Candidate> GetCandidateBasedOnEmailAndMobile(Candidate candidate);

        CandidateDetails GetCandidateDetails(CandidateLogin login);

        List<CandidateDetails> GetListCandidate(CandidateDetails candidateDetail, int filter, int numberOfRecord);

        bool UpdateFeedBack(FeedBack feedBack);
    }
}
