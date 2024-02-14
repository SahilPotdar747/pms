import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TasktypeMasterComponent } from './tasktype-master.component';

describe('TasktypeMasterComponent', () => {
  let component: TasktypeMasterComponent;
  let fixture: ComponentFixture<TasktypeMasterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TasktypeMasterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TasktypeMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
