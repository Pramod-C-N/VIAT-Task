import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewInvoiceComponent } from './viewInvoice.component';

const routes: Routes = [
  {
    path: ':type/:id',
    component: ViewInvoiceComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ViewInvoiceRoutingModule {}  