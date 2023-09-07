import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PaymentMeansComponent } from './paymentMeans.component';
import { CreateOrEditPaymentMeansComponent } from './create-or-edit-paymentMeans.component';
import { ViewPaymentMeansComponent } from './view-paymentMeans.component';

const routes: Routes = [
    {
        path: '',
        component: PaymentMeansComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditPaymentMeansComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewPaymentMeansComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PaymentMeansRoutingModule {}
