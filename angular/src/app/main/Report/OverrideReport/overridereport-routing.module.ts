import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OverrideReportComponent } from './overridereport.component';
const routes: Routes = [
  {
    path: '',
    component: OverrideReportComponent,
    pathMatch: 'full',
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OverrideReportRoutingModule { }
