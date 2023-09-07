import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { purchasecreditreportRoutingModule } from './purchasecredit-routing.module';
import { purchasecreditreportComponent } from './purchasecredit.component';
import { reconciliationdataModule } from '../ReconciliationData/reconciliationdata.module';
@NgModule({
  declarations: [purchasecreditreportComponent],
  imports: [AppSharedModule, purchasecreditreportRoutingModule, AdminSharedModule,reconciliationdataModule],
})
export class purchasecreditreportNewModule {}
