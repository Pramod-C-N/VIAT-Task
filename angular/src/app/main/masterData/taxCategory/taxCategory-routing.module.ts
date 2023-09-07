import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaxCategoryComponent } from './taxCategory.component';
import { CreateOrEditTaxCategoryComponent } from './create-or-edit-taxCategory.component';
import { ViewTaxCategoryComponent } from './view-taxCategory.component';

const routes: Routes = [
    {
        path: '',
        component: TaxCategoryComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditTaxCategoryComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewTaxCategoryComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TaxCategoryRoutingModule {}
