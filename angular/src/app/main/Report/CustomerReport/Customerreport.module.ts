import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CustomerReportRoutingModule } from './Customerreport-routing.module';
import { CustomerReportComponent } from './Customerreport.component';
@NgModule({
  declarations: [CustomerReportComponent],
  imports: [AppSharedModule, CustomerReportRoutingModule, AdminSharedModule],
})
export class CustomerReportModule {}
