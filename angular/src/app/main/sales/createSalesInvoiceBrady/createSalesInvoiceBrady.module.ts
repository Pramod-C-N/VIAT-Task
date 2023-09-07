import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { CreateSalesInvoiceBradyRoutingModule } from './createSalesInvoiceBrady-routing.module';
import { CreateSalesInvoiceBradyComponent } from './createSalesInvoiceBrady.component';
import { VitaComponentsModule } from '@app/shared/vita-components/vita-components.module';
import { FormGroupDirective, ReactiveFormsModule } from '@angular/forms';
import { InvoiceComponentsModule } from '@app/shared/invoice-components/invoice-components.module';
@NgModule({
  declarations: [CreateSalesInvoiceBradyComponent],
  imports: [AppSharedModule, ReactiveFormsModule,CreateSalesInvoiceBradyRoutingModule, AdminSharedModule,VitaComponentsModule,
  InvoiceComponentsModule],
  providers: [FormGroupDirective]
})
export class CreateSalesInvoiceBradyModule {}
