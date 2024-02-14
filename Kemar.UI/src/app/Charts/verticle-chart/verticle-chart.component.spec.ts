import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerticleChartComponent } from './verticle-chart.component';

describe('VerticleChartComponent', () => {
  let component: VerticleChartComponent;
  let fixture: ComponentFixture<VerticleChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VerticleChartComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerticleChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
