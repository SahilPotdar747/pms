import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskTransactionComponent } from './task-transaction.component';

describe('TaskTransactionComponent', () => {
  let component: TaskTransactionComponent;
  let fixture: ComponentFixture<TaskTransactionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TaskTransactionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaskTransactionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
