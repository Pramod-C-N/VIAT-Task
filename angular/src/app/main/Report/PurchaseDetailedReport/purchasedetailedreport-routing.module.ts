import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PurchaseDetailedReportComponent } from './purchaseDetailedReport.component';

const routes: Routes = [
  {
    path: '',
    component: PurchaseDetailedReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PurchaseDetailedReportRoutingModule {}