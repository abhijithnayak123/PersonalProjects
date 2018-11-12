import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateShortlistComponent } from './candidate-shortlist.component';

describe('CandidateShortlistComponent', () => {
  let component: CandidateShortlistComponent;
  let fixture: ComponentFixture<CandidateShortlistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CandidateShortlistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CandidateShortlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
