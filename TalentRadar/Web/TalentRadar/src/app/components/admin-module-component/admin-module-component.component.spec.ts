import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminModuleComponentComponent } from './admin-module-component.component';

describe('AdminModuleComponentComponent', () => {
  let component: AdminModuleComponentComponent;
  let fixture: ComponentFixture<AdminModuleComponentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminModuleComponentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminModuleComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
