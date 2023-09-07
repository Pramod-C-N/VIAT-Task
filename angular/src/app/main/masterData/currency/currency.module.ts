import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CurrencyRoutingModule } from './currency-routing.module';
import { CurrencyComponent } from './currency.component';
import { CreateOrEditCurrencyComponent } from './create-or-edit-currency.component';
import { ViewCurrencyComponent } from './view-currency.component';

@NgModule({
    declarations: [CurrencyComponent, CreateOrEditCurrencyComponent, ViewCurrencyComponent],
    imports: [AppSharedModule, CurrencyRoutingModule, AdminSharedModule],
})
export class CurrencyModule {}
