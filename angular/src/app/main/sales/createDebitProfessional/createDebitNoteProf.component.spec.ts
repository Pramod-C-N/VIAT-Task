import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDebitNoteProfComponent } from './createDebitNoteProf.component';

describe('CreateDebitNoteComponent', () => {
  let component: CreateDebitNoteProfComponent;
  let fixture: ComponentFixture<CreateDebitNoteProfComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateDebitNoteProfComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateDebitNoteProfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
