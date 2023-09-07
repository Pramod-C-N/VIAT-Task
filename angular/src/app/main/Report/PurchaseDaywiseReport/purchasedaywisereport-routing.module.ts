import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { purchaseEntryDaywiseSummaryComponent } from './purchasedaywisereport.component';

const routes: Routes = [
  {
    path: '',
    component: purchaseEntryDaywiseSummaryComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class purchaseEntryDaywiseSummaryRoutingModule {}