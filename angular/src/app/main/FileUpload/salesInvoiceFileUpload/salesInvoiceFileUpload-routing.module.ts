import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SalesInvoiceFileUploadComponent } from './salesInvoiceFileUpload.component';

const routes: Routes = [
  {
    path: '',
    component: SalesInvoiceFileUploadComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SalesInvoiceFileUploadRoutingModule {}