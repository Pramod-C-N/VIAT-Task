import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PurchaseTypeComponent } from './purchaseType.component';
import { CreateOrEditPurchaseTypeComponent } from './create-or-edit-purchaseType.component';
import { ViewPurchaseTypeComponent } from './view-purchaseType.component';

const routes: Routes = [
    {
        path: '',
        component: PurchaseTypeComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditPurchaseTypeComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewPurchaseTypeComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PurchaseTypeRoutingModule {}
