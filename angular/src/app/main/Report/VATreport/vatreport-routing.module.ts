import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VatReportNewComponent } from './vatreport.component';

const routes: Routes = [
  {
    path: '',
    component: VatReportNewComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VatReportNewRoutingModule {}