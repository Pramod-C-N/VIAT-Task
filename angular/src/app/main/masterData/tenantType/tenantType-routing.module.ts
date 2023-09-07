import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TenantTypeComponent } from './tenantType.component';
import { CreateOrEditTenantTypeComponent } from './create-or-edit-tenantType.component';
import { ViewTenantTypeComponent } from './view-tenantType.component';

const routes: Routes = [
    {
        path: '',
        component: TenantTypeComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditTenantTypeComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewTenantTypeComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TenantTypeRoutingModule {}
