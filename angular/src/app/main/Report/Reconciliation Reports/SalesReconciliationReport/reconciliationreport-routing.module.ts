import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { reconciliationreportComponent } from './reconciliationreport.component';

const routes: Routes = [
  {
    path: '',
    component: reconciliationreportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class reconciliationreportRoutingModule {}