import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDebitNotebradyComponent } from './createDebitNotebrady.component';

describe('CreateDebitNoteComponent', () => {
  let component: CreateDebitNotebradyComponent;
  let fixture: ComponentFixture<CreateDebitNotebradyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateDebitNotebradyComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateDebitNotebradyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
