export class WEB_API_BASE {

    public static API_URL = 'http://localhost:57417/api/';

    public static GetURL(_data) {
        return this.API_URL + _data;
    }
}

export const WEB_API = Object.freeze({
    PRACTICE_URL: WEB_API_BASE.GetURL('masterdata/practices'),
    POSITION_URL: WEB_API_BASE.GetURL('masterdata/positions'),
    INTERVIEW_STAGES_URL: WEB_API_BASE.GetURL('masterdata/interviewstages'),
    INTERVIEW_TYPES_URL: WEB_API_BASE.GetURL('masterdata/interviewTypes'),
    VALIDATION_URL: WEB_API_BASE.GetURL('candidate/validate'),
    SEARCH_CANDIDATE_URL: WEB_API_BASE.GetURL('candidate/'),
    REGISTER_CANDIDATE_URL: WEB_API_BASE.GetURL('candidate/'),
    FILEUPLOAD_URL : WEB_API_BASE.GetURL('candidate/upload'),
    GET_CANDIDATELIST_URL : WEB_API_BASE.GetURL('candidate/'),
    EXPORT_CANDIDATELIST_URL: WEB_API_BASE.GetURL('candidate/'),
    ADMIN_LOGIN_URL : WEB_API_BASE.GetURL('authentication/authenticate'),
    UPDATE_FEEDBACK_URL : WEB_API_BASE.GetURL('candidate/updatefeedback')
});

export const WEB_API_CANDIDATE_DASHBORD = Object.freeze({
    GETCANDIDATE_URL : WEB_API_BASE.GetURL('candidate/candidatedetails'),
     UPDATECANDIDATESTATUS_URL : WEB_API_BASE.GetURL('candidate/update')
});

