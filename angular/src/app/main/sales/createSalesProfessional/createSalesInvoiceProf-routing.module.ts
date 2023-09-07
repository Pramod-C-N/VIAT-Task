import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateSalesInvoiceProfComponent } from './createSalesInvoiceProf.component';

const routes: Routes = [
  {
    path: '',
    component: CreateSalesInvoiceProfComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateSalesInvoiceProfRoutingModule {}