import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreditNotePurchaseDayWiseReportRoutingModule } from './creditnotepurchasedaywise-routing.module';
import { CreditNotePurchaseDaywiseReportComponent } from './creditnotepurchasedaywise.component';
@NgModule({
  declarations: [CreditNotePurchaseDaywiseReportComponent],
  imports: [AppSharedModule, CreditNotePurchaseDayWiseReportRoutingModule, AdminSharedModule],
})
export class CreditNotePurchaseDayWiseReportModule {}
