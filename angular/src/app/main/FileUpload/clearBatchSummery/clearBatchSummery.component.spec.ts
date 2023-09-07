import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClearBatchSummeryComponent } from './clearBatchSummery.component';

describe('ClearBatchSummeryComponent', () => {
  let component: ClearBatchSummeryComponent;
  let fixture: ComponentFixture<ClearBatchSummeryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClearBatchSummeryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClearBatchSummeryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
