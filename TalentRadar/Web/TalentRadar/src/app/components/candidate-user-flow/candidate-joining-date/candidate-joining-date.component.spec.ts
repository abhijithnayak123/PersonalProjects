import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateJoiningDateComponent } from './candidate-joining-date.component';

describe('CandidateJoiningDateComponent', () => {
  let component: CandidateJoiningDateComponent;
  let fixture: ComponentFixture<CandidateJoiningDateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CandidateJoiningDateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CandidateJoiningDateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
