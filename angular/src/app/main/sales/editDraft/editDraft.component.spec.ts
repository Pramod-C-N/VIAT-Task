import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditDraftComponent } from './editDraft.component';

describe('CreateSalesInvoiceBradyComponent', () => {
  let component: EditDraftComponent;
  let fixture: ComponentFixture<EditDraftComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditDraftComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditDraftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
