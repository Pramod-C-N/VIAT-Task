import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ReturnReportRoutingModule } from './returnReport-routing.module';
import { ReturnReportComponent } from './retunReport.component';

@NgModule({
  declarations: [ReturnReportComponent],
  imports: [AppSharedModule, ReturnReportRoutingModule, AdminSharedModule],
})
export class ReturnReportModule {}
