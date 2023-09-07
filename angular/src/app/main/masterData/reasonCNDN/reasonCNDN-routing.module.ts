import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReasonCNDNComponent } from './reasonCNDN.component';
import { CreateOrEditReasonCNDNComponent } from './create-or-edit-reasonCNDN.component';
import { ViewReasonCNDNComponent } from './view-reasonCNDN.component';

const routes: Routes = [
    {
        path: '',
        component: ReasonCNDNComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditReasonCNDNComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewReasonCNDNComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ReasonCNDNRoutingModule {}
