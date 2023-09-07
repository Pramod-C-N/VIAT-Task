import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { salescreditreportRoutingModule } from './salescredit-routing.module';
import { salescreditreportComponent } from './salescredit.component';
import { reconciliationdataModule } from '../ReconciliationData/reconciliationdata.module';
@NgModule({
  declarations: [salescreditreportComponent],
  imports: [AppSharedModule, salescreditreportRoutingModule, AdminSharedModule,reconciliationdataModule],
})
export class salescreditModule {}
