import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateSalesInvoiceComponent } from './createSalesInvoice.component';

const routes: Routes = [
  {
    path: '',
    component: CreateSalesInvoiceComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateSalesInvoiceRoutingModule {}