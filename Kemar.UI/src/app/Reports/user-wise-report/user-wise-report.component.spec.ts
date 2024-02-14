import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserWiseReportComponent } from './user-wise-report.component';

describe('UserWiseReportComponent', () => {
  let component: UserWiseReportComponent;
  let fixture: ComponentFixture<UserWiseReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserWiseReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserWiseReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
