import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TitleComponent } from './title.component';
import { CreateOrEditTitleComponent } from './create-or-edit-title.component';
import { ViewTitleComponent } from './view-title.component';

const routes: Routes = [
    {
        path: '',
        component: TitleComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditTitleComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewTitleComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TitleRoutingModule {}
