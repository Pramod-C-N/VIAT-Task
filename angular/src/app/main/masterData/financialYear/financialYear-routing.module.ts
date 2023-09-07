import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FinancialYearComponent } from './financialYear.component';
import { CreateOrEditFinancialYearComponent } from './create-or-edit-financialYear.component';
import { ViewFinancialYearComponent } from './view-financialYear.component';

const routes: Routes = [
    {
        path: '',
        component: FinancialYearComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditFinancialYearComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewFinancialYearComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FinancialYearRoutingModule {}
