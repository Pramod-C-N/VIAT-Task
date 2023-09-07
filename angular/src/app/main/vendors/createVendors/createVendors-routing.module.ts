﻿import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateVendorComponent } from './createVendors.component';

const routes: Routes = [
  {
    path: '',
    component: CreateVendorComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateVendorRoutingModule {}