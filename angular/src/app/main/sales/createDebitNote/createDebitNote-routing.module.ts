import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateDebitNoteComponent } from './createDebitNote.component';

const routes: Routes = [
  {
    path: '',
    component: CreateDebitNoteComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateDebitNoteRoutingModule {}