import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BradyTransactionsComponent } from './bradytransactions.component';

describe('TransactionsComponent', () => {
  let component: BradyTransactionsComponent;
  let fixture: ComponentFixture<BradyTransactionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BradyTransactionsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BradyTransactionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
