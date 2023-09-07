import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { VendorUploadRoutingModule } from './vendorfileupload-routing.module';
import { VendorUploadComponent } from './vendorfileupload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [VendorUploadComponent],
  imports: [AppSharedModule, VendorUploadRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule]
})
export class VendorUploadModule {}
