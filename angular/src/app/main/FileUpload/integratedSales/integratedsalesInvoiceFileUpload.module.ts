import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { IntegratedSalesInvoiceFileUploadRoutingModule } from './integratedsalesInvoiceFileUpload-routing.module';
import { IntegratedSalesInvoiceFileUploadComponent } from './integratedsalesInvoiceFileUpload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [IntegratedSalesInvoiceFileUploadComponent],
  imports: [AppSharedModule, IntegratedSalesInvoiceFileUploadRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule]
})
export class IntegratedSalesInvoiceFileUploadModule {}
