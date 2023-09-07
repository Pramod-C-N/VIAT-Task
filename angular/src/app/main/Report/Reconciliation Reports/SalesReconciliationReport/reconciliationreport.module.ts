import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { reconciliationreportRoutingModule } from './reconciliationreport-routing.module';
import { reconciliationreportComponent } from './reconciliationreport.component';
import { reconciliationdataModule } from '../ReconciliationData/reconciliationdata.module';
@NgModule({
  declarations: [reconciliationreportComponent],
  imports: [AppSharedModule, reconciliationreportRoutingModule, AdminSharedModule, reconciliationdataModule],
})
export class reconciliationreportNewModule {}
