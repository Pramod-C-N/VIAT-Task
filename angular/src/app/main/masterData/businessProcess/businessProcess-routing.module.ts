import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessProcessComponent } from './businessProcess.component';
import { CreateOrEditBusinessProcessComponent } from './create-or-edit-businessProcess.component';
import { ViewBusinessProcessComponent } from './view-businessProcess.component';

const routes: Routes = [
    {
        path: '',
        component: BusinessProcessComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditBusinessProcessComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewBusinessProcessComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessProcessRoutingModule {}
