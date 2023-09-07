import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateSalesInvoiceBradyComponent } from './createSalesInvoiceBrady.component';

const routes: Routes = [
  {
    path: '',
    component: CreateSalesInvoiceBradyComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateSalesInvoiceBradyRoutingModule {}