import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { purchasereconciliationreportComponent } from './purchasereconciliationreport.component';

const routes: Routes = [
  {
    path: '',
    component: purchasereconciliationreportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class purchasereconciliationreportRoutingModule {}
