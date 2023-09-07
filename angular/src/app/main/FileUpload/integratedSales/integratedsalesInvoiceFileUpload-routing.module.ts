import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IntegratedSalesInvoiceFileUploadComponent } from './integratedsalesInvoiceFileUpload.component';

const routes: Routes = [
  {
    path: '',
    component: IntegratedSalesInvoiceFileUploadComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class IntegratedSalesInvoiceFileUploadRoutingModule {}