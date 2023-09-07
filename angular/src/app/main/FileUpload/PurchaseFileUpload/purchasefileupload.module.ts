import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { NewFilePurchaseBatchUploadRoutingModule } from './purchasefileupload-routing.module';
import { NewFilePurchaseBatchUploadComponent } from './purchasefileupload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [NewFilePurchaseBatchUploadComponent],
  imports: [AppSharedModule, NewFilePurchaseBatchUploadRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule]
})
export class NewFilePurchaseBatchUploadModule {}
