import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllowanceReasonComponent } from './allowanceReason.component';
import { CreateOrEditAllowanceReasonComponent } from './create-or-edit-allowanceReason.component';
import { ViewAllowanceReasonComponent } from './view-allowanceReason.component';

const routes: Routes = [
    {
        path: '',
        component: AllowanceReasonComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditAllowanceReasonComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewAllowanceReasonComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AllowanceReasonRoutingModule {}
