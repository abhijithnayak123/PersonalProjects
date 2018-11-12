import { Recruiter } from './recruiter';
import { Document } from './document';
import { Position } from './position';
import { Practice } from './practice';
import { InterviewStage } from './interview-stage';
import { Notification } from './notification';
import { InterviewType } from './interview-type';
import { CandidateStatus } from './candidate-status-enum';

export class CandidateRegistration {

    Id: number;
    FirstName: string;
    MiddleName: string;
    LastName: string;
    Mobile: string;
    Recruiter: Recruiter;
    Documents: Document[];
    Position: Position;
    Practice: Practice;
    Stage: InterviewStage;
    IsActive: boolean;
    Email: string;
    Notifications: Notification[];
    JobDescription: string;
    Comment: string;
    InterviewDate: Date;
    InterviewTime: string;
    InterviewType: InterviewType;
    DeclineReason: string;
    Token: string;
    OnBoardDescription: string;
    IsNewCandidate: boolean;
    Search: string;
    CandidateStatus: CandidateStatus;
    FeedBackComment: string;
}
