import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListempComponent } from './listemp.component';

describe('ListempComponent', () => {
  let component: ListempComponent;
  let fixture: ComponentFixture<ListempComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListempComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListempComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
