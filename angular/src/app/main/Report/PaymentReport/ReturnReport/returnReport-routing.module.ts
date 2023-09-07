import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReturnReportComponent } from './retunReport.component';

const routes: Routes = [
  {
    path: '',
    component: ReturnReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReturnReportRoutingModule { }
