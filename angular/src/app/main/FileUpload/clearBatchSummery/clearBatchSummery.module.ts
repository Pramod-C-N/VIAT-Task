import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { ClearBatchSummeryRoutingModule } from './clearBatchSummery-routing.module';
import { ClearBatchSummeryComponent } from './clearBatchSummery.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
@NgModule({
  declarations: [ClearBatchSummeryComponent],
  imports: [AppSharedModule, ClearBatchSummeryRoutingModule, AdminSharedModule, ImportstandardfilesErrorListModule],
})
export class ClearBatchSummeryModule {}

