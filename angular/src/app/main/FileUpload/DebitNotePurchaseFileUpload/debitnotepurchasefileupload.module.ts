import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { NewFileDebitPurchaseBatchUploadRoutingModule } from './debitnotepurchasefileupload-routing.module'
import { NewFileDebitPurchaseBatchUploadComponent } from './debitnotepurchasefileupload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [NewFileDebitPurchaseBatchUploadComponent],
  imports: [AppSharedModule, NewFileDebitPurchaseBatchUploadRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule]
})
export class NewFileDebitPurchaseBatchUploadModule {}
