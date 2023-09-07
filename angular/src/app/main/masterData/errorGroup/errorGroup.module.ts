import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ErrorGroupRoutingModule } from './errorGroup-routing.module';
import { ErrorGroupComponent } from './errorGroup.component';
import { CreateOrEditErrorGroupComponent } from './create-or-edit-errorGroup.component';
import { ViewErrorGroupComponent } from './view-errorGroup.component';

@NgModule({
    declarations: [ErrorGroupComponent, CreateOrEditErrorGroupComponent, ViewErrorGroupComponent],
    imports: [AppSharedModule, ErrorGroupRoutingModule, AdminSharedModule],
})
export class ErrorGroupModule {}
