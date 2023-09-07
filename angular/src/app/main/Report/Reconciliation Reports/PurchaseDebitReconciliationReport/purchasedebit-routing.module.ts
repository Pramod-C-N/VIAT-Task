import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { purchasedebitreportComponent } from './purchasedebit.component';

const routes: Routes = [
  {
    path: '',
    component: purchasedebitreportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class purchasedebitreportRoutingModule {}
