import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreateCreditNoteProfRoutingModule } from './createCreditNoteProf-routing.module';
import { CreateCreditNoteProfComponent } from './createCreditNoteProf.component';
import { VitaComponentsModule } from '@app/shared/vita-components/vita-components.module';
import { InvoiceComponentsModule } from '@app/shared/invoice-components/invoice-components.module';
@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [CreateCreditNoteProfComponent],
  imports: [AppSharedModule, CreateCreditNoteProfRoutingModule, AdminSharedModule, VitaComponentsModule,InvoiceComponentsModule],
})
export class CreateCreditNoteProfModule {}
