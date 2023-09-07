import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { CreateSalesInvoiceProfRoutingModule } from './createSalesInvoiceProf-routing.module';
import { CreateSalesInvoiceProfComponent } from './createSalesInvoiceProf.component';
import { InvoiceComponentsModule } from '@app/shared/invoice-components/invoice-components.module';
import { FormGroupDirective } from '@angular/forms';
import { VitaComponentsModule } from '@app/shared/vita-components/vita-components.module';
@NgModule({
  declarations: [CreateSalesInvoiceProfComponent],
  imports: [AppSharedModule, CreateSalesInvoiceProfRoutingModule, AdminSharedModule,InvoiceComponentsModule,VitaComponentsModule],
  providers: [FormGroupDirective]

})
export class CreateSalesInvoiceProfModule {}
