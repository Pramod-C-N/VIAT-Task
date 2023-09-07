import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { overheadsreconciliationreportRoutingModule } from './overheadsreconciliationreport-routing.module';
import { overheadsreconciliationreportComponent } from './overheadsreconciliationreport.component';
import { reconciliationdataModule } from '../ReconciliationData/reconciliationdata.module';
@NgModule({
  declarations: [overheadsreconciliationreportComponent],
  imports: [AppSharedModule, overheadsreconciliationreportRoutingModule, AdminSharedModule, reconciliationdataModule],
})
export class overheadsreconciliationreportNewModule { }
