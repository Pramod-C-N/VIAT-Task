import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConstitutionComponent } from './constitution.component';
import { CreateOrEditConstitutionComponent } from './create-or-edit-constitution.component';
import { ViewConstitutionComponent } from './view-constitution.component';

const routes: Routes = [
    {
        path: '',
        component: ConstitutionComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditConstitutionComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewConstitutionComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ConstitutionRoutingModule {}
