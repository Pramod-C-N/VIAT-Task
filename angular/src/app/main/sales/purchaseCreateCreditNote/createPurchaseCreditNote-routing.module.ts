import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreatePurchaseCreditNoteComponent } from './createPurchaseCreditNote.component';

const routes: Routes = [
  {
    path: '',
    component: CreatePurchaseCreditNoteComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreatePurchaseCreditNoteRoutingModule {}
