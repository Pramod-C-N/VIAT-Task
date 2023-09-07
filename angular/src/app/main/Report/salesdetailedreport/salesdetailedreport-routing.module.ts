import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {SalesDetailedReportComponent} from './salesdetailedreport.component'

const routes: Routes = [
  {
    path: '',
    component: SalesDetailedReportComponent,
    pathMatch: 'full',
  }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})


export class SalesDetailedReportRoutingModule { }

