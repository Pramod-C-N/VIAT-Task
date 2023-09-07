import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { PaymentMeansRoutingModule } from './paymentMeans-routing.module';
import { PaymentMeansComponent } from './paymentMeans.component';
import { CreateOrEditPaymentMeansComponent } from './create-or-edit-paymentMeans.component';
import { ViewPaymentMeansComponent } from './view-paymentMeans.component';

@NgModule({
    declarations: [PaymentMeansComponent, CreateOrEditPaymentMeansComponent, ViewPaymentMeansComponent],
    imports: [AppSharedModule, PaymentMeansRoutingModule, AdminSharedModule],
})
export class PaymentMeansModule {}
