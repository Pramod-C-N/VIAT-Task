import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { FinancialYearRoutingModule } from './financialYear-routing.module';
import { FinancialYearComponent } from './financialYear.component';
import { CreateOrEditFinancialYearComponent } from './create-or-edit-financialYear.component';
import { ViewFinancialYearComponent } from './view-financialYear.component';

@NgModule({
    declarations: [FinancialYearComponent, CreateOrEditFinancialYearComponent, ViewFinancialYearComponent],
    imports: [AppSharedModule, FinancialYearRoutingModule, AdminSharedModule],
})
export class FinancialYearModule {}
