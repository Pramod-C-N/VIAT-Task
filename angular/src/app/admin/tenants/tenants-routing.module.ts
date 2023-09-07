import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TenantsComponent } from './tenants.component';
import {CreateTenantModalComponent} from './create-tenant-modal.component';
import {} from './tenant-features-modal.component';
import { EditTenantModalComponent } from './edit-tenant-modal.component';

const routes: Routes = [
    {
        path: '',
        component: TenantsComponent,
        pathMatch: 'full',
    },
    {
        path: 'createOrEdit',
        component: CreateTenantModalComponent,
        pathMatch: 'full',
    },
    {
        path: 'editTenant',
        component: EditTenantModalComponent,
        pathMatch: 'full',
    },

];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TenantsRoutingModule {}
