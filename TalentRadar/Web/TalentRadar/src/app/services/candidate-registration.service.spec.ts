import { TestBed, inject } from '@angular/core/testing';

import { CandidateRegistrationService } from './candidate-registration.service';

describe('CandidateRegistrationService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CandidateRegistrationService]
    });
  });

  it('should ...', inject([CandidateRegistrationService], (service: CandidateRegistrationService) => {
    expect(service).toBeTruthy();
  }));
});
