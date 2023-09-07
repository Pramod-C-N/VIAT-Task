import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateCreditNoteProfComponent } from './createCreditNoteProf.component';

const routes: Routes = [
  {
    path: '',
    component: CreateCreditNoteProfComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateCreditNoteProfRoutingModule {}