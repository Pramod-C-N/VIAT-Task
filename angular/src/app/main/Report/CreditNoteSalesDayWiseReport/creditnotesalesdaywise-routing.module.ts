import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreditNoteSalesDaywiseReportComponent } from './creditnotesalesdaywise.component';

const routes: Routes = [
  {
    path: '',
    component: CreditNoteSalesDaywiseReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreditNoteSalesDaywiseReportRoutingModule {}