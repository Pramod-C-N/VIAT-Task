import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { SalesDaywiseReportRoutingModule } from './salesDaywiseReport-routing.module';
import { SalesDaywiseReportComponent } from './salesDaywiseReport.component';
@NgModule({
  declarations: [SalesDaywiseReportComponent],
  imports: [AppSharedModule, SalesDaywiseReportRoutingModule, AdminSharedModule],
})
export class SalesDaywiseReportModule {}
