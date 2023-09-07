import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AffiliationComponent } from './affiliation.component';
import { CreateOrEditAffiliationComponent } from './create-or-edit-affiliation.component';
import { ViewAffiliationComponent } from './view-affiliation.component';

const routes: Routes = [
    {
        path: '',
        component: AffiliationComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditAffiliationComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewAffiliationComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AffiliationRoutingModule {}
