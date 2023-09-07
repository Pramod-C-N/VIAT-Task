import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { CreateSalesInvoiceRoutingModule } from './createSalesInvoice-routing.module';
import { CreateSalesInvoiceComponent } from './createSalesInvoice.component';
@NgModule({
  declarations: [CreateSalesInvoiceComponent],
  imports: [AppSharedModule, CreateSalesInvoiceRoutingModule, AdminSharedModule],
})
export class CreateSalesInvoiceModule {}
