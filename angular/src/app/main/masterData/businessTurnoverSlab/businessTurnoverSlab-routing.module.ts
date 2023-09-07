import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessTurnoverSlabComponent } from './businessTurnoverSlab.component';
import { CreateOrEditBusinessTurnoverSlabComponent } from './create-or-edit-businessTurnoverSlab.component';
import { ViewBusinessTurnoverSlabComponent } from './view-businessTurnoverSlab.component';

const routes: Routes = [
    {
        path: '',
        component: BusinessTurnoverSlabComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditBusinessTurnoverSlabComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewBusinessTurnoverSlabComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessTurnoverSlabRoutingModule {}
