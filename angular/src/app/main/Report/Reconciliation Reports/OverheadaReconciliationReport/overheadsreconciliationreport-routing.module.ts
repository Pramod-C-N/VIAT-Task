import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { overheadsreconciliationreportComponent } from './overheadsreconciliationreport.component';

const routes: Routes = [
  {
    path: '',
    component: overheadsreconciliationreportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class overheadsreconciliationreportRoutingModule {}
