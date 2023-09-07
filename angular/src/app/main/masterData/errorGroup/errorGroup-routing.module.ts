import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ErrorGroupComponent } from './errorGroup.component';
import { CreateOrEditErrorGroupComponent } from './create-or-edit-errorGroup.component';
import { ViewErrorGroupComponent } from './view-errorGroup.component';

const routes: Routes = [
    {
        path: '',
        component: ErrorGroupComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditErrorGroupComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewErrorGroupComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ErrorGroupRoutingModule {}
