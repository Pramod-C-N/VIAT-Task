import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DebitNotePurchaseDaywiseReportComponent } from './debitnotepurchasedaywise.component';

const routes: Routes = [
  {
    path: '',
    component: DebitNotePurchaseDaywiseReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DebitNotePurchaseDayWiseReportRoutingModule {}