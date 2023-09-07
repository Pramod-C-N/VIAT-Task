import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CountryRoutingModule } from './country-routing.module';
import { CountryComponent } from './country.component';
import { CreateOrEditCountryComponent } from './create-or-edit-country.component';
import { ViewCountryComponent } from './view-country.component';

@NgModule({
    declarations: [CountryComponent, CreateOrEditCountryComponent, ViewCountryComponent],
    imports: [AppSharedModule, CountryRoutingModule, AdminSharedModule],
})
export class CountryModule {}
