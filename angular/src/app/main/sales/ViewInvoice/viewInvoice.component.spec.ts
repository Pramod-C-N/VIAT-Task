import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewInvoiceComponent } from './viewInvoice.component';

describe('ViewInvoiceComponent', () => {
  let component: ViewInvoiceComponent;
  let fixture: ComponentFixture<ViewInvoiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewInvoiceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
