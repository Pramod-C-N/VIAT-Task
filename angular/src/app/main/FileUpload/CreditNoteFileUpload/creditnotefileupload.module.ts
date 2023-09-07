import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { NewFileCreditBatchUploadRoutingModule } from './creditnotefileupload-routing.module';
import { NewFileCreditBatchUploadComponent } from './creditnotefileupload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [NewFileCreditBatchUploadComponent],
  imports: [AppSharedModule, NewFileCreditBatchUploadRoutingModule, AdminSharedModule, ImportstandardfilesErrorListModule]
})
export class NewFileCreditBatchUploadModule {}
