import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DebitNoteReportComponent } from './debitNoteReport.component';

const routes: Routes = [
  {
    path: '',
    component: DebitNoteReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DebitNoteReportRoutingModule {}