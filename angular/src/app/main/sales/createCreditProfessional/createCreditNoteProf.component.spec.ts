import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCreditNoteProfComponent } from './createCreditNoteProf.component';

describe('CreateCreditNoteComponent', () => {
  let component: CreateCreditNoteProfComponent;
  let fixture: ComponentFixture<CreateCreditNoteProfComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateCreditNoteProfComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateCreditNoteProfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
