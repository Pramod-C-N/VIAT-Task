import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DebitSalesReportComponent } from './debitSalesReport.component';

const routes: Routes = [
  {
    path: '',
    component: DebitSalesReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DebitSalesReportRoutingModule {}