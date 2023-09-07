import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DocumentMasterComponent } from './documentMaster.component';
import { CreateOrEditDocumentMasterComponent } from './create-or-edit-documentMaster.component';
import { ViewDocumentMasterComponent } from './view-documentMaster.component';

const routes: Routes = [
    {
        path: '',
        component: DocumentMasterComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditDocumentMasterComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewDocumentMasterComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DocumentMasterRoutingModule {}
