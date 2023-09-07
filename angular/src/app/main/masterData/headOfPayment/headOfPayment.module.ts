import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HeadOfPaymentRoutingModule } from './headOfPayment-routing.module';
import { HeadOfPaymentComponent } from './headOfPayment.component';
import { CreateOrEditHeadOfPaymentComponent } from './create-or-edit-headOfPayment.component';
import { ViewHeadOfPaymentComponent } from './view-headOfPayment.component';

@NgModule({
    declarations: [HeadOfPaymentComponent, CreateOrEditHeadOfPaymentComponent, ViewHeadOfPaymentComponent],
    imports: [AppSharedModule, HeadOfPaymentRoutingModule, AdminSharedModule],
})
export class HeadOfPaymentModule {}
