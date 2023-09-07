import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VendorUploadComponent } from './vendorfileupload.component';

const routes: Routes = [
  {
    path: '',
    component: VendorUploadComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VendorUploadRoutingModule {}