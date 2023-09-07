import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TenantReportRoutingModule } from './Tenantreport-routing.module';
import { TenantReportComponent } from './Tenantreport.component';
@NgModule({
  declarations: [TenantReportComponent],
  imports: [AppSharedModule, TenantReportRoutingModule, AdminSharedModule],
})
export class TenantNoteReportModule {}
