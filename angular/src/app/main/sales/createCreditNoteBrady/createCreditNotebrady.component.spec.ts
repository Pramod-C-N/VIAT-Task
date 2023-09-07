import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCreditNotebradyComponent } from './createCreditNotebrady.component';

describe('CreateCreditNoteComponent', () => {
  let component: CreateCreditNotebradyComponent;
  let fixture: ComponentFixture<CreateCreditNotebradyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateCreditNotebradyComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateCreditNotebradyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
