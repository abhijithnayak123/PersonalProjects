import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateRegistrationComponent } from './candidate-registration.component';

describe('CandidateRegistrationComponent', () => {
  let component: CandidateRegistrationComponent;
  let fixture: ComponentFixture<CandidateRegistrationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CandidateRegistrationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CandidateRegistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
