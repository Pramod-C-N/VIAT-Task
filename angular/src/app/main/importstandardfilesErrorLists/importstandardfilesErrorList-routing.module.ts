import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ImportstandardfilesErrorListsComponent } from './importstandardfilesErrorLists.component';

const routes: Routes = [
  {
    path: '',
    component: ImportstandardfilesErrorListsComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ImportstandardfilesErrorListRoutingModule {}
