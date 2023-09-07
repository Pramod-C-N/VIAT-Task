import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreditSalesReportRoutingModule } from './creditSalesReport-routing.module';
import { CreditSalesReportComponent } from './creditSalesReport.component';
@NgModule({
  declarations: [CreditSalesReportComponent],
  imports: [AppSharedModule, CreditSalesReportRoutingModule, AdminSharedModule],
})
export class CreditSalesReportModule {}
