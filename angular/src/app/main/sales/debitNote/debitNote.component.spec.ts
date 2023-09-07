import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DebitNoteComponent } from './debitNote.component';

describe('DebitNoteComponent', () => {
  let component: DebitNoteComponent;
  let fixture: ComponentFixture<DebitNoteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DebitNoteComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DebitNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
