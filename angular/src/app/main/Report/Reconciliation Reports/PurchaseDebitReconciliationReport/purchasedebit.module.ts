import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { purchasedebitreportRoutingModule } from './purchasedebit-routing.module';
import { purchasedebitreportComponent } from './purchasedebit.component';
import { reconciliationdataModule } from '../ReconciliationData/reconciliationdata.module';
@NgModule({
  declarations: [purchasedebitreportComponent],
  imports: [AppSharedModule, purchasedebitreportRoutingModule, AdminSharedModule,reconciliationdataModule],
})
export class purchasedebitreportNewModule {}
