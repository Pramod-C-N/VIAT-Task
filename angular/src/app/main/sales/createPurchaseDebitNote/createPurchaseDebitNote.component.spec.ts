import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePurchaseDebitNoteComponent } from './createPurchaseDebitNote.component';

describe('CreateDebitNoteComponent', () => {
  let component: CreatePurchaseDebitNoteComponent;
  let fixture: ComponentFixture<CreatePurchaseDebitNoteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreatePurchaseDebitNoteComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatePurchaseDebitNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
