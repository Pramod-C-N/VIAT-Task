import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { overHeadGapAnalysisReportRoutingModule } from './overHeadGapAnalysisReport-routing.module';
import { overHeadGapAnalysisReportComponent } from './overHeadGapAnalysisReport.component';
import { reconciliationdataModule } from '../ReconciliationData/reconciliationdata.module';
@NgModule({
  declarations: [overHeadGapAnalysisReportComponent],
  imports: [AppSharedModule, overHeadGapAnalysisReportRoutingModule, AdminSharedModule, reconciliationdataModule],
})
export class overHeadGapAnalysisReportNewModule { }
