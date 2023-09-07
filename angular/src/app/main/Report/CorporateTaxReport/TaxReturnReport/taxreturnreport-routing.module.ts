import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaxReturnReportNewComponent } from './taxreturnreport.component';

const routes: Routes = [
  {
    path: '',
    component: TaxReturnReportNewComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TaxReturnReportNewRoutingModule { }
