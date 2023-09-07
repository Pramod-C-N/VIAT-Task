import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TenantUploadRoutingModule } from './tenantfileupload-routing.module';
import { TenantUploadComponent } from './tenantfileupload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [TenantUploadComponent],
  imports: [AppSharedModule, TenantUploadRoutingModule, AdminSharedModule, ImportstandardfilesErrorListModule]
})
export class TenantUploadModule { }
