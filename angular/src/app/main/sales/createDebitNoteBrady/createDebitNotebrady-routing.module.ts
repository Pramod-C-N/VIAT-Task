import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateDebitNotebradyComponent } from './createDebitNotebrady.component';

const routes: Routes = [
  {
    path: '',
    component: CreateDebitNotebradyComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateDebitNotebradyRoutingModule {}