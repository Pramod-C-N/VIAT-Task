import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ImportstandardfilesErrorListRoutingModule } from './importstandardfilesErrorList-routing.module';
import { ImportstandardfilesErrorListsComponent } from './importstandardfilesErrorLists.component';

@NgModule({
  declarations: [
    ImportstandardfilesErrorListsComponent
  ],
  imports: [AppSharedModule, ImportstandardfilesErrorListRoutingModule, AdminSharedModule],
  exports: [ImportstandardfilesErrorListsComponent]
})
export class ImportstandardfilesErrorListModule {}
