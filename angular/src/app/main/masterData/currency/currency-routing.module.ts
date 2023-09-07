import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CurrencyComponent } from './currency.component';
import { CreateOrEditCurrencyComponent } from './create-or-edit-currency.component';
import { ViewCurrencyComponent } from './view-currency.component';

const routes: Routes = [
    {
        path: '',
        component: CurrencyComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditCurrencyComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewCurrencyComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CurrencyRoutingModule {}
