import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DebitNotePurchaseDetailedReportRoutingModule } from './debitnotepurchasedetailedreport-routing.module';
import { DebitNotePurchaseDetailedReportComponent } from './debitnotepurchasedetailedreport.component';
@NgModule({
  declarations: [DebitNotePurchaseDetailedReportComponent],
  imports: [AppSharedModule, DebitNotePurchaseDetailedReportRoutingModule, AdminSharedModule],
})
export class DebitNotePurchaseDetailedReportModule {}
