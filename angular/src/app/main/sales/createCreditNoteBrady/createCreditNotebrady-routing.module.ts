import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateCreditNotebradyComponent } from './createCreditNotebrady.component';

const routes: Routes = [
  {
    path: '',
    component: CreateCreditNotebradyComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateCreditNoteRoutingModule {}