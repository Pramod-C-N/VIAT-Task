import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DocumentMasterRoutingModule } from './documentMaster-routing.module';
import { DocumentMasterComponent } from './documentMaster.component';
import { CreateOrEditDocumentMasterComponent } from './create-or-edit-documentMaster.component';
import { ViewDocumentMasterComponent } from './view-documentMaster.component';

@NgModule({
    declarations: [DocumentMasterComponent, CreateOrEditDocumentMasterComponent, ViewDocumentMasterComponent],
    imports: [AppSharedModule, DocumentMasterRoutingModule, AdminSharedModule],
})
export class DocumentMasterModule {}
