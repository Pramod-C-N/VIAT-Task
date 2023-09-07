import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { SalesInvoiceFileUploadRoutingModule } from './salesInvoiceFileUpload-routing.module';
import { SalesInvoiceFileUploadComponent } from './salesInvoiceFileUpload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [SalesInvoiceFileUploadComponent],
  imports: [AppSharedModule, SalesInvoiceFileUploadRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule]
})
export class SalesInvoiceFileUploadModule {}
