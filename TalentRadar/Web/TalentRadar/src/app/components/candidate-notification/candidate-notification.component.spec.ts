import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateNotificationComponent } from './candidate-notification.component';

describe('CandidateNotificationComponent', () => {
  let component: CandidateNotificationComponent;
  let fixture: ComponentFixture<CandidateNotificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CandidateNotificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CandidateNotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
