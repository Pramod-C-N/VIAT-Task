import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { salesdebitreportComponent } from './salesdebit.component';

const routes: Routes = [
  {
    path: '',
    component: salesdebitreportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class salesdebitreportRoutingModule {}