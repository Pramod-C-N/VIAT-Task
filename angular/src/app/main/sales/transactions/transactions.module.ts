import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TransactionsRoutingModule } from './transactions-routing.module';
import { TransactionsComponent } from './transactions.component';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { VitaComponentsModule } from '@app/shared/vita-components/vita-components.module';
import { ViewInvoiceComponent } from '../ViewInvoice/viewInvoice.component';
import { ViewInvoiceModule } from '../ViewInvoice/viewInvoice.module';

@NgModule({
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: [TransactionsComponent],
  imports: [AppSharedModule, TransactionsRoutingModule, AdminSharedModule,VitaComponentsModule,ViewInvoiceModule],
  exports:[TransactionsComponent]
})
export class TransactionsModule {}
