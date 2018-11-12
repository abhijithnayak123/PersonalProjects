using OPT.TalentRadar.BIZ.Contract;
using OPT.TalentRadar.BIZ.Implementation;
using OPT.TalentRadar.Common.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace Opteamix.TalentRadar.Controllers
{
    [RoutePrefix("api/Candidate")]
    public class CandidateController : ApiController
    {
        public IMasterDataService masterData { get; set; }
        ICandidateService candidateService;
        IAuthService authService;
        // GET: api/Candidate
        [Route("")]
        public IList<Candidate> Get()
        {
            candidateService = new CandidateServiceImpl();

            return candidateService.GetAllCandidates();
        }

        // GET: api/Candidate/5
        //[Route("{token}/find")]
        //public Candidate Get(string token)
        //{
        //    candidateService = new CandidateServiceImpl();
        //    //string decodedId =  CryptographyHelper.Decode(id, ConfigurationManager.AppSettings.Get("EncryptionKey"));
        //    return candidateService.GetCandidate(token);
        //}

        [Route("{filter}/find")]
        public Candidate GetCandidate(string filter)
        {
            candidateService = new CandidateServiceImpl();

            return candidateService.FindCandidate(filter);
        }

        // POST: api/Candidate
        [Route("")]
        public string Post(Candidate candidate)
        {
            try
            {
                candidateService = new CandidateServiceImpl();
                string userName = string.Empty;
                //TODO : HttpContext.Current.Request.LogonUserIdentity.Name;
                if (candidate.Recruiter.Email.Contains("@"))
                {
                    userName = candidate.Recruiter.Email.Split('@')[0];
                }
                else
                {
                    userName = candidate.Recruiter.Email;
                }

                candidate.Recruiter = getRecruiterInfo(userName);
                if (candidate.Id == 0)
                {
                    candidateService.NewCandidate(candidate);
                }
                else
                {
                    candidateService.UpdateCandidate(candidate);
                }

                return "true";
            }
            catch (Exception ex)
            {

                return ex.Message + ex.InnerException + ex.StackTrace;
            }
        }

        // PUT: api/Candidate/5
        [Route("")]
        public bool Put(Candidate candidate)
        {
            candidateService = new CandidateServiceImpl();
            Candidate oldCandite = new Candidate(); //candidateService.GetCandidate(candidate.Id);
            if (oldCandite.Notifications.Exists(x => x.Stage.Id != candidate.Stage.Id))
            {
                masterData = new MasterDataServiceImpl();
                Notification notification = new Notification()
                {
                    Stage = masterData.GetInterviewStage(candidate.Stage.Id),
                    DTCreate = DateTime.Now
                };
                oldCandite.Notifications.Add(notification);
            }
            candidate.Notifications = oldCandite.Notifications;
            candidateService.UpdateCandidate(candidate);
            return true;
        }

        [HttpPost]
        [Route("update")]
        public bool UpdateReason(CandidateStatus status)
        {
            try
            {
                candidateService = new CandidateServiceImpl();
                candidateService.UpdateCandidateStatus(status);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        [HttpPost]
        [Route("validate")]
        public string ValidateEmailAndMobile(CandidateValidation candidate)
        {
            candidateService = new CandidateServiceImpl();

            return candidateService.ValidateCandidate(candidate);
        }

        [HttpPost]
        [Route("candidatedetails")]
        public CandidateDetails GetCandidateDetails(CandidateLogin candidateLogin)
        {
            candidateService = new CandidateServiceImpl();

            return candidateService.GetCandidateDetails(candidateLogin);
        }
        [Route("{filter}/getcandidate")]
        public CandidateDetails GetCandidateBasedOnToken(string filter)
        {
            candidateService = new CandidateServiceImpl();

            return candidateService.GetCandidateBasedOnToken(filter);
        }

        [Route("{filter}/getnotification")]
        public CandidateNotification GetNotification(string filter)
        {
            candidateService = new CandidateServiceImpl();

            return candidateService.GetNotifications(filter);
        }
        [Route("{filter}/getrecuirter")]
        public Recruiter GetRecuirterDetails(string filter)
        {
            candidateService = new CandidateServiceImpl();

            return candidateService.GetRecuiterDetails(filter);
        }
        [HttpPost]
        [Route("{filter}/{numberOfRecord}/listCandidates")]
        public List<CandidateDetails> GetListCandidates(CandidateDetails candidateDetails, int filter, int numberOfRecord)
        {
            candidateService = new CandidateServiceImpl();

            return candidateService.GetListCandidate(candidateDetails, filter, numberOfRecord);
        }

        [HttpPost]
        [Route("updatefeedback")]
        public bool UpdateFeedBack(FeedBack feedback)
        {
            candidateService = new CandidateServiceImpl();

            return candidateService.UpdateFeedBack(feedback);
        }

		[HttpPost]
		[Route("{filter}/{numberOfRecord}/exportCandidates")]
		public System.Web.Mvc.FileResult ExportCandidates(CandidateDetails candidateDetails, int filter, int numberOfRecord)
		{
			candidateService = new CandidateServiceImpl();
			List<CandidateDetails> _lstOfCandidates = candidateService.GetListCandidate(candidateDetails, filter, numberOfRecord);
			if (_lstOfCandidates != null && _lstOfCandidates.Count > 0)
			{
				StringBuilder str = new StringBuilder();
				str.Append("<table border=" + "1px" + "b>");
				str.Append("<tr>");
				str.Append("<td><b><font face=Arial Narrow size=3>First Name</font></b></td>");
				str.Append("<td><b><font face=Arial Narrow size=3>Last Name</font></b></td>");
				str.Append("<td><b><font face=Arial Narrow size=3>Is Active</font></b></td>");
				str.Append("<td><b><font face=Arial Narrow size=3>Is NewCandidate</font></b></td>");
				str.Append("<td><b><font face=Arial Narrow size=3>Mobile</font></b></td>");
				str.Append("<td><b><font face=Arial Narrow size=3>Email</font></b></td>");
				str.Append("<td><b><font face=Arial Narrow size=3>Job Description</font></b></td>");
				str.Append("<td><b><font face=Arial Narrow size=3>Position</font></b></td>");
				str.Append("<td><b><font face=Arial Narrow size=3>Practice</font></b></td>");
				str.Append("<td><b><font face=Arial Narrow size=3>Interview Stage</font></b></td>");
				str.Append("<td><b><font face=Arial Narrow size=3>Interview Type</font></b></td>");
				str.Append("</tr>");

				foreach (var val in _lstOfCandidates)
				{
					str.Append("<tr>");
					str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.FirstName + "</font></td>");
					str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.LastName + "</font></td>");
					str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.IsActive.ToString() + "</font></td>");
					str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.IsNewCandidate.ToString() + "</font></td>");
					str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.Mobile + "</font></td>");
					str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.Email + "</font></td>");
					str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.JobDescription + "</font></td>");
					str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.PositionName + "</font></td>");
					str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.PracticeName + "</font></td>");
					str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.InterviewStageName + "</font></td>");
					str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.InterviewTypeName + "</font></td>");
					str.Append("</tr>");
				}
				str.Append("</table>");

				byte[] data=Encoding.ASCII.GetBytes(str.ToString()); ;
				return new System.Web.Mvc.FileContentResult(data, "application/vnd.ms-excel") { FileDownloadName = "Candidates_" + DateTime.Now + ".xls" };
			}
			return null;
		}

		private Recruiter getRecruiterInfo(string userName)
        {
            if (userName.Contains("\\"))
            {
                userName = userName.Split('\\')[1];
            }
            authService = new AuthServiceImpl();
            ADUser user = authService.GetADUserDetails(userName);
            Recruiter recruiter = new Recruiter();
            if (user != null)
            {
                recruiter = new Recruiter()
                {
                    FullName = user.FullName,
                    MobileNo = user.Mobile ?? user.Phone1,
                    Email = user.UserName
                };
            }
            return recruiter;
        }

    }
}
