import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { WTHPaymentUploadRoutingModule } from './paymentfileupload-routing.module';
import { WTHPaymentUploadComponent } from './paymentfileupload.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
import { ImportstandardfilesErrorListsComponent } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorLists.component';
@NgModule({
  declarations: [WTHPaymentUploadComponent],
  imports: [AppSharedModule, WTHPaymentUploadRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule]
})
export class WTHPaymentUploadModule {}
