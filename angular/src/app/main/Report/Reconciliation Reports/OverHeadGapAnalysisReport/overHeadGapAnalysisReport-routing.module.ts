import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { overHeadGapAnalysisReportComponent } from './overHeadGapAnalysisReport.component';

const routes: Routes = [
  {
    path: '',
    component: overHeadGapAnalysisReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class overHeadGapAnalysisReportRoutingModule {}
