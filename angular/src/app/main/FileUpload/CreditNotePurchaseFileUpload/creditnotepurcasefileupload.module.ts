import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { NewFileCreditPurchaseBatchUploadRoutingModule } from './creditnotepurchasefileupload-routing.module'
import { NewFileCreditPurchaseBatchUploadComponent } from './creditnotepurcasefileupload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [NewFileCreditPurchaseBatchUploadComponent],
  imports: [AppSharedModule, NewFileCreditPurchaseBatchUploadRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule]
})
export class NewFileCreditPurchaseBatchUploadModule {}
