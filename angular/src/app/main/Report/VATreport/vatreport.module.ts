import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { VatReportNewRoutingModule } from './vatreport-routing.module';
import { VatReportNewComponent } from './vatreport.component';
@NgModule({
  declarations: [VatReportNewComponent],
  imports: [AppSharedModule, VatReportNewRoutingModule, AdminSharedModule],
})
export class VatReportNewModule {}
