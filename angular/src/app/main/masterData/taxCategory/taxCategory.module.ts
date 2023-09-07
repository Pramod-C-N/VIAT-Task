import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TaxCategoryRoutingModule } from './taxCategory-routing.module';
import { TaxCategoryComponent } from './taxCategory.component';
import { CreateOrEditTaxCategoryComponent } from './create-or-edit-taxCategory.component';
import { ViewTaxCategoryComponent } from './view-taxCategory.component';

@NgModule({
    declarations: [TaxCategoryComponent, CreateOrEditTaxCategoryComponent, ViewTaxCategoryComponent],
    imports: [AppSharedModule, TaxCategoryRoutingModule, AdminSharedModule],
})
export class TaxCategoryModule {}
