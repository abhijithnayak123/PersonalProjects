import { CandidateStatus } from './candidate-status-enum';

export class CandidateDetails {
    FirstName: string;
    MiddleName: string;
    LastName: string;
    Mobile: string;
    IsActive: boolean;
    Email: string;
    JobDescription: string;
    InterviewDate: Date;
    InterviewTime: string;
    Token: string;
    IsNewCandidate: boolean;
    CandidateStatus: CandidateStatus;
    PositionName: string;
    PracticeName: string;
    InterviewStageName: string;
    InterviewTypeName: string;
    NumberCandidate: number;
}
