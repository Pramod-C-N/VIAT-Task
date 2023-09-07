import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DebitNotePurchaseDayWiseReportRoutingModule } from './debitnotepurchasedaywise-routing.module';
import { DebitNotePurchaseDaywiseReportComponent } from './debitnotepurchasedaywise.component';
@NgModule({
  declarations: [DebitNotePurchaseDaywiseReportComponent],
  imports: [AppSharedModule, DebitNotePurchaseDayWiseReportRoutingModule, AdminSharedModule],
})
export class DebitNotePurchaseDayWiseReportModule {}
