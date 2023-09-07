import { Component, ElementRef, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective,Validators } from '@angular/forms';

@Component({
    selector: 'multi-language',
    templateUrl: './multi-language.component.html',
    styleUrls: ['./multi-language.component.css'],
})
export class MultiLanguageComponent {
    formName: FormGroup;

    constructor(private formgroupDirective: FormGroupDirective, private el: ElementRef,
        private fb: FormBuilder,) {            
    }


    ngOnInit() {
        this.formName = this.formgroupDirective.control;
    }

    ngAfterViewInit() {}

    onSubmit(form: any) {
        if (form.valid) {
        }
      }
}
