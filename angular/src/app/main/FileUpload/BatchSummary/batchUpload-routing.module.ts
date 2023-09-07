import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BatchUploadComponent } from './batchUpload.component';

const routes: Routes = [
  {
    path: '',
    component: BatchUploadComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BatchUploadRoutingModule {}