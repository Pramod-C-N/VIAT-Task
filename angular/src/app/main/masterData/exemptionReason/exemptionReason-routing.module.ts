import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExemptionReasonComponent } from './exemptionReason.component';
import { CreateOrEditExemptionReasonComponent } from './create-or-edit-exemptionReason.component';
import { ViewExemptionReasonComponent } from './view-exemptionReason.component';

const routes: Routes = [
    {
        path: '',
        component: ExemptionReasonComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditExemptionReasonComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewExemptionReasonComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ExemptionReasonRoutingModule {}
