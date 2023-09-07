import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Phase2Component } from './phase-2.component';

const routes: Routes = [
  {
    path: '',
    component: Phase2Component,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class Phase2RoutingModule {}