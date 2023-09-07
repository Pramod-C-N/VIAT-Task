import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DebitNoteSalesDaywiseReportComponent } from './debitnotesalesdaywisereport.component';

const routes: Routes = [
  {
    path: '',
    component: DebitNoteSalesDaywiseReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DebitNoteSalesDaywiseReportRoutingModule {}