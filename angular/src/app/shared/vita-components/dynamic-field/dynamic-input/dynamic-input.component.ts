import { Component, ElementRef, Input, OnInit } from '@angular/core';
import { FormGroup, FormGroupDirective } from '@angular/forms';

@Component({
    selector: 'vita-dynamic-input',
    templateUrl: './dynamic-input.component.html',
    styleUrls: ['./dynamic-input.component.css'],
})
export class DynamicInputComponent {
    @Input() field: any;
    formName: FormGroup;
    isDate: boolean = false;


    constructor(private formgroupDirective: FormGroupDirective, private el: ElementRef) {
        this.formName = formgroupDirective.control;
    }

    ngOnInit() {
        if (this.field.type == 'date') {
            this.field.type = 'text';
            this.isDate = true;
        }
    }

    ngAfterViewInit() {
        // document.getElementsByTagName('input') : to gell all Docuement imputs
        const inputList = [].slice.call((<HTMLElement>this.el.nativeElement).getElementsByTagName('input'));
        inputList.forEach((input: HTMLElement) => {
          if(this.isDate){
            input.addEventListener('focus', () => {
                input.setAttribute('type', 'date');
                input.removeAttribute('placeholder');
            });
            input.addEventListener('blur', () => {
                input.setAttribute('type', 'text');
                input.setAttribute('placeholder', this.field?.placeholder || 'placeholder');
            });
          }
        });
    }

}
