import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CustomReportRoutingModule } from './customReport-routing.module';
import { CustomReportComponent } from './customReport.component';
@NgModule({
  declarations: [CustomReportComponent],
  imports: [AppSharedModule, CustomReportRoutingModule, AdminSharedModule],
})
export class CustomReportModule {}
