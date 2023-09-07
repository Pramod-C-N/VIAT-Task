import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TransactionCategoryComponent } from './transactionCategory.component';
import { CreateOrEditTransactionCategoryComponent } from './create-or-edit-transactionCategory.component';
import { ViewTransactionCategoryComponent } from './view-transactionCategory.component';

const routes: Routes = [
    {
        path: '',
        component: TransactionCategoryComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditTransactionCategoryComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewTransactionCategoryComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TransactionCategoryRoutingModule {}
