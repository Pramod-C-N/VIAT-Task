import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { PlaceOfPerformanceRoutingModule } from './placeOfPerformance-routing.module';
import { PlaceOfPerformanceComponent } from './placeOfPerformance.component';
import { CreateOrEditPlaceOfPerformanceComponent } from './create-or-edit-placeOfPerformance.component';
import { ViewPlaceOfPerformanceComponent } from './view-placeOfPerformance.component';

@NgModule({
    declarations: [
        PlaceOfPerformanceComponent,
        CreateOrEditPlaceOfPerformanceComponent,
        ViewPlaceOfPerformanceComponent,
    ],
    imports: [AppSharedModule, PlaceOfPerformanceRoutingModule, AdminSharedModule],
})
export class PlaceOfPerformanceModule {}
