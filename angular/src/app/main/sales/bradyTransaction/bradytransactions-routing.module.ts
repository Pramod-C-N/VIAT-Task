import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BradyTransactionsComponent } from './bradytransactions.component';

const routes: Routes = [
  {
    path: '',
    component: BradyTransactionsComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BradyTransactionsRoutingModule {}
