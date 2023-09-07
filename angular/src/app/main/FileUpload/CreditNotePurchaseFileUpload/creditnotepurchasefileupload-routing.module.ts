import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewFileCreditPurchaseBatchUploadComponent } from './creditnotepurcasefileupload.component';

const routes: Routes = [
  {
    path: '',
    component: NewFileCreditPurchaseBatchUploadComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NewFileCreditPurchaseBatchUploadRoutingModule {}