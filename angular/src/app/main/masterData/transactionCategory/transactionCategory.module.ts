import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TransactionCategoryRoutingModule } from './transactionCategory-routing.module';
import { TransactionCategoryComponent } from './transactionCategory.component';
import { CreateOrEditTransactionCategoryComponent } from './create-or-edit-transactionCategory.component';
import { ViewTransactionCategoryComponent } from './view-transactionCategory.component';

@NgModule({
    declarations: [
        TransactionCategoryComponent,
        CreateOrEditTransactionCategoryComponent,
        ViewTransactionCategoryComponent,
    ],
    imports: [AppSharedModule, TransactionCategoryRoutingModule, AdminSharedModule],
})
export class TransactionCategoryModule {}
