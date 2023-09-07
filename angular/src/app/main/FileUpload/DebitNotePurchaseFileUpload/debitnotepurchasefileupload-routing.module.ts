import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewFileDebitPurchaseBatchUploadComponent } from './debitnotepurchasefileupload.component';

const routes: Routes = [
  {
    path: '',
    component: NewFileDebitPurchaseBatchUploadComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NewFileDebitPurchaseBatchUploadRoutingModule {}