import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerReportComponent } from './Customerreport.component';

const routes: Routes = [
  {
    path: '',
    component: CustomerReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CustomerReportRoutingModule {}