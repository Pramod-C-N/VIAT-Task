import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TenantUploadComponent } from './tenantfileupload.component';

const routes: Routes = [
  {
    path: '',
    component: TenantUploadComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TenantUploadRoutingModule {}