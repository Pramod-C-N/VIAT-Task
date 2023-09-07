import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewFileDebitSalesBatchUploadComponent } from './debitnotesalesfileupload.component';

const routes: Routes = [
  {
    path: '',
    component: NewFileDebitSalesBatchUploadComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NewFileDebitSalesBatchUploadRoutingModule {}