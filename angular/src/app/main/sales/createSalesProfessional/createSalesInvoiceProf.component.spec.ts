import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSalesInvoiceProfComponent } from './createSalesInvoiceProf.component';

describe('CreateSalesInvoiceComponent', () => {
  let component: CreateSalesInvoiceProfComponent;
  let fixture: ComponentFixture<CreateSalesInvoiceProfComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateSalesInvoiceProfComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateSalesInvoiceProfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
