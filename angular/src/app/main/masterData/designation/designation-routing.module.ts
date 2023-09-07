import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DesignationComponent } from './designation.component';
import { CreateOrEditDesignationComponent } from './create-or-edit-designation.component';
import { ViewDesignationComponent } from './view-designation.component';

const routes: Routes = [
    {
        path: '',
        component: DesignationComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditDesignationComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewDesignationComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DesignationRoutingModule {}
