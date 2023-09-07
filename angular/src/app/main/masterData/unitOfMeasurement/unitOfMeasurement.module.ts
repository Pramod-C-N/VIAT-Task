import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { UnitOfMeasurementRoutingModule } from './unitOfMeasurement-routing.module';
import { UnitOfMeasurementComponent } from './unitOfMeasurement.component';
import { CreateOrEditUnitOfMeasurementComponent } from './create-or-edit-unitOfMeasurement.component';
import { ViewUnitOfMeasurementComponent } from './view-unitOfMeasurement.component';

@NgModule({
    declarations: [UnitOfMeasurementComponent, CreateOrEditUnitOfMeasurementComponent, ViewUnitOfMeasurementComponent],
    imports: [AppSharedModule, UnitOfMeasurementRoutingModule, AdminSharedModule],
})
export class UnitOfMeasurementModule {}
