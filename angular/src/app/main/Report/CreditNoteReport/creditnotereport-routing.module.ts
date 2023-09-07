import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreditNoteReportComponent } from './creditNoteReport.component';

const routes: Routes = [
  {
    path: '',
    component: CreditNoteReportComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreditNoteReportRoutingModule {}