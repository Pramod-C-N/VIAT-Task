import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewFileCreditBatchUploadComponent } from './creditnotefileupload.component';

const routes: Routes = [
  {
    path: '',
    component: NewFileCreditBatchUploadComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NewFileCreditBatchUploadRoutingModule {}