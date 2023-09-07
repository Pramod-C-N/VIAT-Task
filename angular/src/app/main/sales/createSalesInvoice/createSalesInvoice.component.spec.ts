import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSalesInvoiceComponent } from './createSalesInvoice.component';

describe('CreateSalesInvoiceComponent', () => {
  let component: CreateSalesInvoiceComponent;
  let fixture: ComponentFixture<CreateSalesInvoiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateSalesInvoiceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateSalesInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
