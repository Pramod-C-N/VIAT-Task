import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreatePurchaseEntryComponent } from './createPurchaseEntry.component';

const routes: Routes = [
  {
    path: '',
    component: CreatePurchaseEntryComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreatePurchaseEntryRoutingModule {}