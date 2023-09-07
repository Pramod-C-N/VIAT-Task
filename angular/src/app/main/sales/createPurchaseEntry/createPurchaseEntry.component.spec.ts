import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePurchaseEntryComponent } from './createPurchaseEntry.component';
describe('CreatePurchaseEntryComponent', () => {
  let component: CreatePurchaseEntryComponent;
  let fixture: ComponentFixture<CreatePurchaseEntryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreatePurchaseEntryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatePurchaseEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
