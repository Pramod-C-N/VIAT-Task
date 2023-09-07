import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MultiLanguageComponent } from './multi-language/multi-language.component'
import { filterComponent } from './filter-component/filter.component';
@NgModule({
    schemas: [NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA],
    declarations: [MultiLanguageComponent,filterComponent
    ],
    imports: [FormsModule, ReactiveFormsModule, AppSharedModule, AdminSharedModule],
    exports: [MultiLanguageComponent,filterComponent
    ],
})
export class InvoiceComponentsModule {}
