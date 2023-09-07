import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreditNotePurchaseDetailedReportComponent } from './creditnotepurchasedetailedreport.component';

const routes: Routes = [
  {
    path: '',
    component: CreditNotePurchaseDetailedReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreditNotePurchaseDetailedReportRoutingModule {}