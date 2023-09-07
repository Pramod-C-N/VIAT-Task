import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaxSubCategoryComponent } from './taxSubCategory.component';
import { CreateOrEditTaxSubCategoryComponent } from './create-or-edit-taxSubCategory.component';
import { ViewTaxSubCategoryComponent } from './view-taxSubCategory.component';

const routes: Routes = [
    {
        path: '',
        component: TaxSubCategoryComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditTaxSubCategoryComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewTaxSubCategoryComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TaxSubCategoryRoutingModule {}
