import { NO_ERRORS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { FileMapperRoutingModule } from './file-mapper-routing.module';
import { FileMapperComponent } from './file-mapper.component';
import { ImportstandardfilesErrorListModule } from '@app/main/importstandardfilesErrorLists/importstandardfilesErrorList.module';
import {NgFileMapperModule,NgFileMapperComponent} from '@abylle/ng-file-mapper'
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [FileMapperComponent],
  schemas:[NO_ERRORS_SCHEMA],
  imports: [CommonModule, AppSharedModule, FileMapperRoutingModule, AdminSharedModule,ImportstandardfilesErrorListModule,NgFileMapperModule]
})
export class FileMapperModule {}
