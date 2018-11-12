import { TestBed, inject } from '@angular/core/testing';

import { CandidateDashboardService } from './candidate-dashboard.service';

describe('CandidateDashboardService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CandidateDashboardService]
    });
  });

  it('should ...', inject([CandidateDashboardService], (service: CandidateDashboardService) => {
    expect(service).toBeTruthy();
  }));
});
