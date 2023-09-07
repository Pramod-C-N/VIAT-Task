import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreditNoteRoutingModule } from './creditNote-routing.module';
import { CreditNoteComponent } from './creditNote.component';
@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [CreditNoteComponent],
  imports: [AppSharedModule, CreditNoteRoutingModule, AdminSharedModule],
})
export class CreditNoteModule {}
