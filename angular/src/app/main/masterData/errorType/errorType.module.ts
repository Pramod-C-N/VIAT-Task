import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ErrorTypeRoutingModule } from './errorType-routing.module';
import { ErrorTypeComponent } from './errorType.component';
import { CreateOrEditErrorTypeComponent } from './create-or-edit-errorType.component';
import { ViewErrorTypeComponent } from './view-errorType.component';

@NgModule({
    declarations: [ErrorTypeComponent, CreateOrEditErrorTypeComponent, ViewErrorTypeComponent],
    imports: [AppSharedModule, ErrorTypeRoutingModule, AdminSharedModule],
})
export class ErrorTypeModule {}
