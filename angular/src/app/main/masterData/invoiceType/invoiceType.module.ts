import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { InvoiceTypeRoutingModule } from './invoiceType-routing.module';
import { InvoiceTypeComponent } from './invoiceType.component';
import { CreateOrEditInvoiceTypeComponent } from './create-or-edit-invoiceType.component';
import { ViewInvoiceTypeComponent } from './view-invoiceType.component';

@NgModule({
    declarations: [InvoiceTypeComponent, CreateOrEditInvoiceTypeComponent, ViewInvoiceTypeComponent],
    imports: [AppSharedModule, InvoiceTypeRoutingModule, AdminSharedModule],
})
export class InvoiceTypeModule {}
