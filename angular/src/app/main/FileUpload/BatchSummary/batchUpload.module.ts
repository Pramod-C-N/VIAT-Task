import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BatchUploadRoutingModule } from './batchUpload-routing.module';
import { BatchUploadComponent } from './batchUpload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [BatchUploadComponent],
  imports: [AppSharedModule, BatchUploadRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule],
  exports: [BatchUploadComponent], 
})
export class BatchUploadModule {}
