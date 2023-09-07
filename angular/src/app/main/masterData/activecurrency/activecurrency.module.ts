import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ActivecurrencyRoutingModule } from './activecurrency-routing.module';
import { ActivecurrencyComponent } from './activecurrency.component';
import { CreateOrEditActivecurrencyComponent } from './create-or-edit-activecurrency.component';
import { ViewActivecurrencyComponent } from './view-activecurrency.component';

@NgModule({
    declarations: [ActivecurrencyComponent, CreateOrEditActivecurrencyComponent, ViewActivecurrencyComponent],
    imports: [AppSharedModule, ActivecurrencyRoutingModule, AdminSharedModule],
})
export class ActivecurrencyModule {}
