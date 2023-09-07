import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClearBatchSummeryComponent } from './clearBatchSummery.component';

const routes: Routes = [
  {
    path: '',
    component: ClearBatchSummeryComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ClearBatchSummeryRoutingModule {}
