import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { MasterBatchUploadRoutingModule } from './MasterbatchUpload-routing.module';
import { MasterBatchUploadComponent } from './MasterbatchUpload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [MasterBatchUploadComponent],
  imports: [AppSharedModule, MasterBatchUploadRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule],
  exports: [MasterBatchUploadComponent], 
})
export class MasterBatchUploadModule {}
