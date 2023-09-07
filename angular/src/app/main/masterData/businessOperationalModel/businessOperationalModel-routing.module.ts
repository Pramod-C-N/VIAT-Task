import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessOperationalModelComponent } from './businessOperationalModel.component';
import { CreateOrEditBusinessOperationalModelComponent } from './create-or-edit-businessOperationalModel.component';
import { ViewBusinessOperationalModelComponent } from './view-businessOperationalModel.component';

const routes: Routes = [
    {
        path: '',
        component: BusinessOperationalModelComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditBusinessOperationalModelComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewBusinessOperationalModelComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessOperationalModelRoutingModule {}
