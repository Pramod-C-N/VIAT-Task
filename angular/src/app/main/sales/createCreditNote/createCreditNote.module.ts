import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreateCreditNoteRoutingModule } from './createCreditNote-routing.module';
import { CreateCreditNoteComponent } from './createCreditNote.component';
@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [CreateCreditNoteComponent],
  imports: [AppSharedModule, CreateCreditNoteRoutingModule, AdminSharedModule],
})
export class CreateCreditNoteModule {}
