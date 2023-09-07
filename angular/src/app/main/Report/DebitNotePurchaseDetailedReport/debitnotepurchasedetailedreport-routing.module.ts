import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DebitNotePurchaseDetailedReportComponent } from './debitnotepurchasedetailedreport.component';

const routes: Routes = [
  {
    path: '',
    component: DebitNotePurchaseDetailedReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DebitNotePurchaseDetailedReportRoutingModule {}