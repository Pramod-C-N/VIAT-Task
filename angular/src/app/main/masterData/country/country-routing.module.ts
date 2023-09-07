import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CountryComponent } from './country.component';
import { CreateOrEditCountryComponent } from './create-or-edit-country.component';
import { ViewCountryComponent } from './view-country.component';

const routes: Routes = [
    {
        path: '',
        component: CountryComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditCountryComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewCountryComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CountryRoutingModule {}
