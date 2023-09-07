import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewFilePurchaseBatchUploadComponent } from './purchasefileupload.component';

const routes: Routes = [
  {
    path: '',
    component: NewFilePurchaseBatchUploadComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NewFilePurchaseBatchUploadRoutingModule {}