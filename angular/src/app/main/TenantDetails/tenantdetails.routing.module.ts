import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TenantDetailsComponent } from './tenantdetails.component';

const routes: Routes = [
  {
    path: '',
    component: TenantDetailsComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TenantDetailRoutingModule {}