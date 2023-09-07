import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DebitNoteRoutingModule } from './debitNote-routing.module';
import { DebitNoteComponent } from './debitNote.component';
@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [DebitNoteComponent],
  imports: [AppSharedModule, DebitNoteRoutingModule, AdminSharedModule],
})
export class DebitNoteModule {}
