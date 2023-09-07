import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateDebitNoteProfComponent } from './createDebitNoteProf.component';

const routes: Routes = [
  {
    path: '',
    component: CreateDebitNoteProfComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateDebitNoteProfRoutingModule {}