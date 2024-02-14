import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectWiseReportComponent } from './project-wise-report.component';

describe('ProjectWiseReportComponent', () => {
  let component: ProjectWiseReportComponent;
  let fixture: ComponentFixture<ProjectWiseReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectWiseReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectWiseReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
