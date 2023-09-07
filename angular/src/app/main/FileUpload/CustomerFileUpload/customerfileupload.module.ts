import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CustomerUploadRoutingModule } from './customerfileupload-routing.module';
import { CustomerUploadComponent } from './customerfileupload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [CustomerUploadComponent],
  imports: [AppSharedModule, CustomerUploadRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule]
})
export class CustomerUploadModule {}
