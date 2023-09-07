import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvoiceCategoryComponent } from './invoiceCategory.component';
import { CreateOrEditInvoiceCategoryComponent } from './create-or-edit-invoiceCategory.component';
import { ViewInvoiceCategoryComponent } from './view-invoiceCategory.component';

const routes: Routes = [
    {
        path: '',
        component: InvoiceCategoryComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditInvoiceCategoryComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewInvoiceCategoryComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvoiceCategoryRoutingModule {}
