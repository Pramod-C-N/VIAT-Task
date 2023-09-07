import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivecurrencyComponent } from './activecurrency.component';
import { CreateOrEditActivecurrencyComponent } from './create-or-edit-activecurrency.component';
import { ViewActivecurrencyComponent } from './view-activecurrency.component';

const routes: Routes = [
    {
        path: '',
        component: ActivecurrencyComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditActivecurrencyComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewActivecurrencyComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ActivecurrencyRoutingModule {}
