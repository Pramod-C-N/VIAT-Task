import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreditNoteSalesDaywiseReportRoutingModule } from './creditnotesalesdaywise-routing.module';
import { CreditNoteSalesDaywiseReportComponent } from './creditnotesalesdaywise.component';
@NgModule({
  declarations: [CreditNoteSalesDaywiseReportComponent],
  imports: [AppSharedModule, CreditNoteSalesDaywiseReportRoutingModule, AdminSharedModule],
})
export class CreditNoteSalesDaywiseReportModule {}
