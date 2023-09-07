import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TrialBalanceUploadComponent } from './TrialBalancefileupload.component';

const routes: Routes = [
  {
    path: '',
    component: TrialBalanceUploadComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TrialBalanceUploadRoutingModule {}