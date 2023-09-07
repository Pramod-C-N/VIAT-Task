import { Injector, Component, ViewEncapsulation, Inject, Input, OnInit } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DOCUMENT } from '@angular/common';
import * as KTUtil from '@metronic/app/kt/_utils';
import { GlobalConstsCustomService } from '@shared/customService/global-consts-service';

@Component({
    templateUrl: './default-logo.component.html',
    styleUrls: ['./default-logo.component.css'],
    selector: 'default-logo',
    encapsulation: ViewEncapsulation.None,
})
export class DefaultLogoComponent extends AppComponentBase implements OnInit {
    @Input() customHrefClass = '';
    @Input() skin = null;

    defaultLogo = '';
    defaultSmallLogo = '';
    profilePicture = '';
    profilePictureSmall = '';
    remoteServiceBaseUrl: string = AppConsts.remoteServiceBaseUrl;
    vitamenu: boolean = AppConsts.vitaMenu;
    constructor(injector: Injector, @Inject(DOCUMENT) private document: Document,
    private _GlobalConstsCustomService: GlobalConstsCustomService,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.setLogoUrl();
        this._GlobalConstsCustomService.data$.subscribe(e=>{
            this.vitamenu=e.isVita;
        })
    }

    onResize() {
        this.setLogoUrl();
    }

    setLogoUrl(): void{
        if(this.vitamenu == true)
        {
       this.profilePicture = AppConsts.appBaseUrl + '/assets/common/images/logo-vita.png';
       this.profilePictureSmall = AppConsts.appBaseUrl + '/assets/common/images/VITA-Icon-Svg.svg';
        }
        else
        {
          this.profilePicture = AppConsts.appBaseUrl + '/assets/common/images/app-logo-on-' + this.skin + '.svg';
          this.profilePictureSmall = AppConsts.appBaseUrl + '/assets/common/images/app-logo-on-' + this.skin + '-sm.svg';
        }
    }
}
