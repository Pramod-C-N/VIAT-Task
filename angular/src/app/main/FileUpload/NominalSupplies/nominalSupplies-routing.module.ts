import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NominalSuppliesComponent } from './nominalSupplies.component';

const routes: Routes = [
    {
      path: '',
      component: NominalSuppliesComponent,
      pathMatch: 'full',
    },
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
  })
  export class NominalSuppliesRoutingModule {}