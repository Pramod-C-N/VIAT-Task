import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { purchasecreditreportComponent } from './purchasecredit.component';

const routes: Routes = [
  {
    path: '',
    component: purchasecreditreportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class purchasecreditreportRoutingModule {}
