import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { reconciliationdataComponent } from './reconciliationdatacomponent';

const routes: Routes = [
  {
    path: '',
    component: reconciliationdataComponent,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class reconciliationdataRoutingModule {}