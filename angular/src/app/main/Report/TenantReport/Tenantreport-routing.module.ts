import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TenantReportComponent } from './Tenantreport.component';

const routes: Routes = [
  {
    path: '',
    component: TenantReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TenantReportRoutingModule {}