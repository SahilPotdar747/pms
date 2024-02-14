import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllUserReportComponent } from './all-user-report.component';

describe('AllUserReportComponent', () => {
  let component: AllUserReportComponent;
  let fixture: ComponentFixture<AllUserReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllUserReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllUserReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
