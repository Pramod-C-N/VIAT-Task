import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FileMapperComponent } from './file-mapper.component';

const routes: Routes = [
  {
    path: '',
    component: FileMapperComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FileMapperRoutingModule {}