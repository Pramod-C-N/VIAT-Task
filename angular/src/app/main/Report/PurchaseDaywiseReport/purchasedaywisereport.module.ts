import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { purchaseEntryDaywiseSummaryRoutingModule } from './purchasedaywisereport-routing.module';
import { purchaseEntryDaywiseSummaryComponent } from './purchasedaywisereport.component';
@NgModule({
  declarations: [purchaseEntryDaywiseSummaryComponent],
  imports: [AppSharedModule, purchaseEntryDaywiseSummaryRoutingModule, AdminSharedModule],
})
export class purchaseEntryDaywiseSummaryModule {}
