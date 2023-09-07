import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreditSalesReportComponent } from './creditSalesReport.component';

const routes: Routes = [
  {
    path: '',
    component: CreditSalesReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreditSalesReportRoutingModule {}