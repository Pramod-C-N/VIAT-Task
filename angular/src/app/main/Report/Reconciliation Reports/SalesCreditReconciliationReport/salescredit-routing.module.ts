import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { salescreditreportComponent } from './salescredit.component';

const routes: Routes = [
  {
    path: '',
    component: salescreditreportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class salescreditreportRoutingModule {}