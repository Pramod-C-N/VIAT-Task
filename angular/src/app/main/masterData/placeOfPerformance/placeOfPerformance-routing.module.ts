import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PlaceOfPerformanceComponent } from './placeOfPerformance.component';
import { CreateOrEditPlaceOfPerformanceComponent } from './create-or-edit-placeOfPerformance.component';
import { ViewPlaceOfPerformanceComponent } from './view-placeOfPerformance.component';

const routes: Routes = [
    {
        path: '',
        component: PlaceOfPerformanceComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditPlaceOfPerformanceComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewPlaceOfPerformanceComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PlaceOfPerformanceRoutingModule {}
