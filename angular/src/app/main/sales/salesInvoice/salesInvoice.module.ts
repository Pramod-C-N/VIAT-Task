import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { SalesInvoiceRoutingModule } from './salesInvoice-routing.module';
import { SalesInvoiceComponent } from './salesInvoice.component';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [SalesInvoiceComponent],
  imports: [AppSharedModule, SalesInvoiceRoutingModule, AdminSharedModule],
})
export class SalesInvoiceModule {}
