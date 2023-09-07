﻿import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MasterConfigurationComponent } from './masterConfiguration.component';

const routes: Routes = [
  {
    path: '',
    component: MasterConfigurationComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MasterConfigurationRoutingModule {}