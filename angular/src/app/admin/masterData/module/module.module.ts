import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ModuleRoutingModule } from './module-routing.module';
import { ModuleComponent } from './module.component';
import { CreateOrEditModuleModalComponent } from './create-or-edit-module-modal.component';
import { ViewModuleModalComponent } from './view-module-modal.component';

@NgModule({
    declarations: [ModuleComponent, CreateOrEditModuleModalComponent, ViewModuleModalComponent],
    imports: [AppSharedModule, ModuleRoutingModule, AdminSharedModule],
})
export class ModuleModule {}
