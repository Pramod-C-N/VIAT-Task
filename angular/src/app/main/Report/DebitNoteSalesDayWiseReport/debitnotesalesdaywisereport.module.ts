import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DebitNoteSalesDaywiseReportRoutingModule } from './debitnotesalesdaywisereport-routing.module';
import { DebitNoteSalesDaywiseReportComponent } from './debitnotesalesdaywisereport.component';
@NgModule({
  declarations: [DebitNoteSalesDaywiseReportComponent],
  imports: [AppSharedModule, DebitNoteSalesDaywiseReportRoutingModule, AdminSharedModule],
})
export class DebitNoteSalesDaywiseReportModule {}
