import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerUploadComponent } from './customerfileupload.component';

const routes: Routes = [
  {
    path: '',
    component: CustomerUploadComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CustomerUploadRoutingModule {}