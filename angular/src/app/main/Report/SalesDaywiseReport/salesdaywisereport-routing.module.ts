import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SalesDaywiseReportComponent } from './salesDaywiseReport.component';

const routes: Routes = [
  {
    path: '',
    component: SalesDaywiseReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SalesDaywiseReportRoutingModule {}