import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreatePurchaseCreditNoteRoutingModule } from './createPurchaseCreditNote-routing.module';
import { CreatePurchaseCreditNoteComponent } from './createPurchaseCreditNote.component';
@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [CreatePurchaseCreditNoteComponent],
  imports: [AppSharedModule, CreatePurchaseCreditNoteRoutingModule, AdminSharedModule],
})
export class CreatePurchaseCreditNoteModule {}
