import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TrialBalanceUploadRoutingModule } from './TrialBalancefileupload-routing.module';
import { TrialBalanceUploadComponent } from './TrialBalancefileupload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [TrialBalanceUploadComponent],
  imports: [AppSharedModule, TrialBalanceUploadRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule]
})
export class TrialBalanceUploadModule {}
