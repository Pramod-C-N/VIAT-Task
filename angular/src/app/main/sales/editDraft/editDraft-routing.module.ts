import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditDraftComponent } from './editDraft.component';

const routes: Routes = [
  {
    path: ':type/:theme/:id',
    component: EditDraftComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EditDraftRoutingModule {}