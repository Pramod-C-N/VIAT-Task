import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSalesInvoiceBradyComponent } from './createSalesInvoiceBrady.component';

describe('CreateSalesInvoiceBradyComponent', () => {
  let component: CreateSalesInvoiceBradyComponent;
  let fixture: ComponentFixture<CreateSalesInvoiceBradyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateSalesInvoiceBradyComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateSalesInvoiceBradyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
