import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DetailedReportComponent } from './detailedReport.component';

const routes: Routes = [
  {
    path: '',
    component: DetailedReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],

})
export class DetailedReportRoutingModule { }
