import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DebitSalesReportRoutingModule } from './debitSalesReport-routing.module';
import { DebitSalesReportComponent } from './debitSalesReport.component';
@NgModule({
  declarations: [DebitSalesReportComponent],
  imports: [AppSharedModule, DebitSalesReportRoutingModule, AdminSharedModule],
})
export class DebitSalesReportModule {}
