import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  Input,
  ViewChild,
  ViewContainerRef
} from "@angular/core";
import {FormGroup, FormGroupDirective} from "@angular/forms";
import {DynamicInputComponent} from "./dynamic-input/dynamic-input.component";
import {DynamicSelectComponent} from "./dynamic-select/dynamic-select.component";
import { DynamicTextAreaComponent } from "./dynamic-text-area/dynamic-text-area.component";


@Component({
  selector: "vita-field-input",
  templateUrl: "./dynamic-field.component.html",
  styleUrls: ["./dynamic-field.component.css"],
})
export class DynamicFieldComponent implements AfterViewInit{

  supportedDynamicComponents = [
    {
      name: 'text',
      component: DynamicInputComponent
    },
    {
      name: 'number',
      component: DynamicInputComponent
    },
    {
      name: 'select',
      component: DynamicSelectComponent
    },
    {
      name: 'date',
      component: DynamicInputComponent
    },
    {
      name: 'textarea',
      component: DynamicTextAreaComponent
    },
  ]
  @ViewChild('dynamicInputContainer', { read: ViewContainerRef}) dynamicInputContainer!: ViewContainerRef;
  @Input() field: any;
  formName: FormGroup;

  constructor(private formgroupDirective: FormGroupDirective, private cd: ChangeDetectorRef) {

  }

  ngAfterViewInit(): void {
    this.registerDynamicField();
  }

  private registerDynamicField() {
    this.dynamicInputContainer.clear();
    const componentInstance = this.getComponentByType(this.field.type)
    const dynamicComponent = this.dynamicInputContainer.createComponent(componentInstance)
    dynamicComponent.setInput('field', this.field);
    this.cd.detectChanges();
  }

  getComponentByType(type: string): any {
    let componentDynamic = this.supportedDynamicComponents.find(c => c.name === type);
    return componentDynamic.component || DynamicInputComponent;
  }

}