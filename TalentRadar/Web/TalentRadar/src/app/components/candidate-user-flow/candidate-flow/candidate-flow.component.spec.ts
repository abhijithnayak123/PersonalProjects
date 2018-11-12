import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateFlowComponent } from './candidate-flow.component';

describe('CandidateFlowComponent', () => {
  let component: CandidateFlowComponent;
  let fixture: ComponentFixture<CandidateFlowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CandidateFlowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CandidateFlowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
