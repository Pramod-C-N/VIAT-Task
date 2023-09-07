import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PurchaseTransactionComponent } from './purchaseTransaction.component';

const routes: Routes = [
  {
    path: '',
    component: PurchaseTransactionComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PurchaseTransactionRoutingModule {}
