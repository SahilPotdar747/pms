import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportingInchargeComponent } from './supporting-incharge.component';

describe('SupportingInchargeComponent', () => {
  let component: SupportingInchargeComponent;
  let fixture: ComponentFixture<SupportingInchargeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupportingInchargeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SupportingInchargeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
