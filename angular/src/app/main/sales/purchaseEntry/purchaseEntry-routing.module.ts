import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PurchaseEntryComponent } from './purchaseEntry.component';

const routes: Routes = [
  {
    path: '',
    component: PurchaseEntryComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PurchaseEntryRoutingModule {}