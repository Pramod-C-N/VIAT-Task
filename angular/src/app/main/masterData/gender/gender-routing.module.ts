import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GenderComponent } from './gender.component';
import { CreateOrEditGenderComponent } from './create-or-edit-gender.component';
import { ViewGenderComponent } from './view-gender.component';

const routes: Routes = [
    {
        path: '',
        component: GenderComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditGenderComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewGenderComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GenderRoutingModule {}
