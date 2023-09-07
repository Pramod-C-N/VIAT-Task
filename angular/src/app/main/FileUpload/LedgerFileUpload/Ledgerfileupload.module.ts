import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { LedgerUploadRoutingModule } from './Ledgerfileupload-routing.module';
import { LedgerUploadComponent } from './Ledgerfileupload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [LedgerUploadComponent],
  imports: [AppSharedModule, LedgerUploadRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule]
})
export class LedgerUploadModule {}
