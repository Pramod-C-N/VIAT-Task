import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HeadOfPaymentComponent } from './headOfPayment.component';
import { CreateOrEditHeadOfPaymentComponent } from './create-or-edit-headOfPayment.component';
import { ViewHeadOfPaymentComponent } from './view-headOfPayment.component';

const routes: Routes = [
    {
        path: '',
        component: HeadOfPaymentComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditHeadOfPaymentComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewHeadOfPaymentComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HeadOfPaymentRoutingModule {}
