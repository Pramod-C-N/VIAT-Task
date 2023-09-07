import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { salesdebitreportRoutingModule } from './salesdebit-routing.module';
import { salesdebitreportComponent } from './salesdebit.component';
import { reconciliationdataModule } from '../ReconciliationData/reconciliationdata.module';
@NgModule({
  declarations: [salesdebitreportComponent],
  imports: [AppSharedModule, salesdebitreportRoutingModule, AdminSharedModule, reconciliationdataModule],
})
export class salesdebitModule {}
