import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreatePurchaseDebitNoteRoutingModule } from './createPurchaseDebitNote-routing.module';
import { CreatePurchaseDebitNoteComponent } from './createPurchaseDebitNote.component';
@NgModule({
  declarations: [CreatePurchaseDebitNoteComponent],
  imports: [AppSharedModule, CreatePurchaseDebitNoteRoutingModule, AdminSharedModule],
})
export class CreatePurchaseDebitNoteModule {}
