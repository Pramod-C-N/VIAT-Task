import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrganisationTypeComponent } from './organisationType.component';
import { CreateOrEditOrganisationTypeComponent } from './create-or-edit-organisationType.component';
import { ViewOrganisationTypeComponent } from './view-organisationType.component';

const routes: Routes = [
    {
        path: '',
        component: OrganisationTypeComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditOrganisationTypeComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewOrganisationTypeComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OrganisationTypeRoutingModule {}
