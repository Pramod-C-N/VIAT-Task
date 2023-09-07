import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { PurchaseDetailedReportRoutingModule } from './purchaseDetailedReport-routing.module';
import { PurchaseDetailedReportComponent } from './purchaseDetailedReport.component';
@NgModule({
  declarations: [PurchaseDetailedReportComponent],
  imports: [AppSharedModule, PurchaseDetailedReportRoutingModule, AdminSharedModule],
})
export class PurchaseDetailedReportModule {}
