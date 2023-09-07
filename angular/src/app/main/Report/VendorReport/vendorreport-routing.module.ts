import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {VendorReportComponent} from './vendorreport.component'

const routes: Routes = [
  {
    path: '',
    component: VendorReportComponent,
    pathMatch: 'full',
  }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})


export class VendorReportRoutingModule { }

