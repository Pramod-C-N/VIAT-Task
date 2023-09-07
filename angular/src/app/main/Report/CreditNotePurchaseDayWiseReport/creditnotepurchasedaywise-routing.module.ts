import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreditNotePurchaseDaywiseReportComponent } from './creditnotepurchasedaywise.component';

const routes: Routes = [
  {
    path: '',
    component: CreditNotePurchaseDaywiseReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreditNotePurchaseDayWiseReportRoutingModule {}