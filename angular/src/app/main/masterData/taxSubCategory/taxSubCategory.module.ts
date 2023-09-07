import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TaxSubCategoryRoutingModule } from './taxSubCategory-routing.module';
import { TaxSubCategoryComponent } from './taxSubCategory.component';
import { CreateOrEditTaxSubCategoryComponent } from './create-or-edit-taxSubCategory.component';
import { ViewTaxSubCategoryComponent } from './view-taxSubCategory.component';

@NgModule({
    declarations: [TaxSubCategoryComponent, CreateOrEditTaxSubCategoryComponent, ViewTaxSubCategoryComponent],
    imports: [AppSharedModule, TaxSubCategoryRoutingModule, AdminSharedModule],
})
export class TaxSubCategoryModule {}
