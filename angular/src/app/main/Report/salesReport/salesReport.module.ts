import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { SalesReportRoutingModule } from './salesReport-routing.module';
import { SalesReportComponent } from './salesReport.component';
@NgModule({
  declarations: [SalesReportComponent],
  imports: [AppSharedModule, SalesReportRoutingModule, AdminSharedModule],
})
export class SalesReportModule {}
