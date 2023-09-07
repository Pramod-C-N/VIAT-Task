import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ErrorTypeComponent } from './errorType.component';
import { CreateOrEditErrorTypeComponent } from './create-or-edit-errorType.component';
import { ViewErrorTypeComponent } from './view-errorType.component';

const routes: Routes = [
    {
        path: '',
        component: ErrorTypeComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditErrorTypeComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewErrorTypeComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ErrorTypeRoutingModule {}
