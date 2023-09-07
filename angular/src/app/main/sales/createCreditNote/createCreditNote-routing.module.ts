import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateCreditNoteComponent } from './createCreditNote.component';

const routes: Routes = [
  {
    path: '',
    component: CreateCreditNoteComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateCreditNoteRoutingModule {}