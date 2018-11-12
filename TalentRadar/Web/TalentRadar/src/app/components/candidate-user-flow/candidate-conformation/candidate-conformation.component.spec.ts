import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateConformationComponent } from './candidate-conformation.component';

describe('CandidateConformationComponent', () => {
  let component: CandidateConformationComponent;
  let fixture: ComponentFixture<CandidateConformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CandidateConformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CandidateConformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
