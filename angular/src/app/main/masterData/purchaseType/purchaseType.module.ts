import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { PurchaseTypeRoutingModule } from './purchaseType-routing.module';
import { PurchaseTypeComponent } from './purchaseType.component';
import { CreateOrEditPurchaseTypeComponent } from './create-or-edit-purchaseType.component';
import { ViewPurchaseTypeComponent } from './view-purchaseType.component';

@NgModule({
    declarations: [PurchaseTypeComponent, CreateOrEditPurchaseTypeComponent, ViewPurchaseTypeComponent],
    imports: [AppSharedModule, PurchaseTypeRoutingModule, AdminSharedModule],
})
export class PurchaseTypeModule {}
