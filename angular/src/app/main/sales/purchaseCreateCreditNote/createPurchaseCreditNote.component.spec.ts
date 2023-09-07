import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePurchaseCreditNoteComponent } from './createPurchaseCreditNote.component';

describe('CreateCreditNoteComponent', () => {
  let component: CreatePurchaseCreditNoteComponent;
  let fixture: ComponentFixture<CreatePurchaseCreditNoteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreatePurchaseCreditNoteComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatePurchaseCreditNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
