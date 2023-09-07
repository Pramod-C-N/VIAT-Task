import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UnitOfMeasurementComponent } from './unitOfMeasurement.component';
import { CreateOrEditUnitOfMeasurementComponent } from './create-or-edit-unitOfMeasurement.component';
import { ViewUnitOfMeasurementComponent } from './view-unitOfMeasurement.component';

const routes: Routes = [
    {
        path: '',
        component: UnitOfMeasurementComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditUnitOfMeasurementComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewUnitOfMeasurementComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class UnitOfMeasurementRoutingModule {}
