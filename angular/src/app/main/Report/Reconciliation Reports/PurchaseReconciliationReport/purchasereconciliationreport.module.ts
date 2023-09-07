import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { purchasereconciliationreportComponent } from './purchasereconciliationreport.component';
import { reconciliationdataModule } from '../ReconciliationData/reconciliationdata.module';
import { purchasereconciliationreportRoutingModule } from './purchasereconciliationreport-routing.module';
@NgModule({
  declarations: [purchasereconciliationreportComponent],
  imports: [AppSharedModule, purchasereconciliationreportRoutingModule, AdminSharedModule, reconciliationdataModule],
})
export class purchasereconciliationreportNewModule {}
