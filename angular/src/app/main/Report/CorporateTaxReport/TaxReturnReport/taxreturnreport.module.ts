import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TaxReturnReportNewRoutingModule } from './taxreturnreport-routing.module';
import { TaxReturnReportNewComponent } from './taxreturnreport.component';
@NgModule({
  declarations: [TaxReturnReportNewComponent],
  imports: [AppSharedModule, TaxReturnReportNewRoutingModule, AdminSharedModule],
})
export class TaxReturnReportNewModule {}
