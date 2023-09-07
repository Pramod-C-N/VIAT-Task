import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreditNotePurchaseDetailedReportRoutingModule } from './creditnotepurchasedetailedreport-routing.module';
import { CreditNotePurchaseDetailedReportComponent } from './creditnotepurchasedetailedreport.component';
@NgModule({
  declarations: [CreditNotePurchaseDetailedReportComponent],
  imports: [AppSharedModule, CreditNotePurchaseDetailedReportRoutingModule, AdminSharedModule],
})
export class CreditNotePurchaseDetailedReportModule {}
