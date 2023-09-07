import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WTHPaymentUploadComponent } from './paymentfileupload.component';

const routes: Routes = [
  {
    path: '',
    component: WTHPaymentUploadComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WTHPaymentUploadRoutingModule {}