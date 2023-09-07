import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SectorComponent } from './sector.component';
import { CreateOrEditSectorComponent } from './create-or-edit-sector.component';
import { ViewSectorComponent } from './view-sector.component';

const routes: Routes = [
    {
        path: '',
        component: SectorComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditSectorComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewSectorComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SectorRoutingModule {}
