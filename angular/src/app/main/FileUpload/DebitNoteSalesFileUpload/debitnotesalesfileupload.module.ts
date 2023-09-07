import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { NewFileDebitSalesBatchUploadRoutingModule } from './debitnotesalesfileupload-routing.module'
import { NewFileDebitSalesBatchUploadComponent } from './debitnotesalesfileupload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [NewFileDebitSalesBatchUploadComponent],
  imports: [AppSharedModule, NewFileDebitSalesBatchUploadRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule]
})
export class NewFileDebitSalesBatchUploadModule {}
