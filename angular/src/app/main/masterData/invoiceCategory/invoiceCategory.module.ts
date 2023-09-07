import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { InvoiceCategoryRoutingModule } from './invoiceCategory-routing.module';
import { InvoiceCategoryComponent } from './invoiceCategory.component';
import { CreateOrEditInvoiceCategoryComponent } from './create-or-edit-invoiceCategory.component';
import { ViewInvoiceCategoryComponent } from './view-invoiceCategory.component';

@NgModule({
    declarations: [InvoiceCategoryComponent, CreateOrEditInvoiceCategoryComponent, ViewInvoiceCategoryComponent],
    imports: [AppSharedModule, InvoiceCategoryRoutingModule, AdminSharedModule],
})
export class InvoiceCategoryModule {}
