import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvoiceTypeComponent } from './invoiceType.component';
import { CreateOrEditInvoiceTypeComponent } from './create-or-edit-invoiceType.component';
import { ViewInvoiceTypeComponent } from './view-invoiceType.component';

const routes: Routes = [
    {
        path: '',
        component: InvoiceTypeComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditInvoiceTypeComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewInvoiceTypeComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvoiceTypeRoutingModule {}
