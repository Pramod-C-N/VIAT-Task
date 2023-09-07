import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FileUploadOutputComponent } from './fileUploadOutput.component';

const routes: Routes = [
  {
    path: '',
    component: FileUploadOutputComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FileUploadOutputRoutingModule {}
