import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NatureofServicesComponent } from './natureofServices.component';
import { CreateOrEditNatureofServicesComponent } from './create-or-edit-natureofServices.component';
import { ViewNatureofServicesComponent } from './view-natureofServices.component';

const routes: Routes = [
    {
        path: '',
        component: NatureofServicesComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditNatureofServicesComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewNatureofServicesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class NatureofServicesRoutingModule {}
