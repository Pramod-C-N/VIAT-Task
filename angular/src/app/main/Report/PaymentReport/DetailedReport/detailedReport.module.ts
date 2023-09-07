import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DetailedReportRoutingModule } from './detailedReport-routing.module';
import { DetailedReportComponent } from './detailedReport.component';

@NgModule({
  declarations: [DetailedReportComponent],
  imports: [AppSharedModule, DetailedReportRoutingModule, AdminSharedModule],
})
export class DetailedReportModule {}
