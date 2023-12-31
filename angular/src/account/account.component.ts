import { Component, Injector, OnInit, ViewContainerRef, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AppUiCustomizationService } from '@shared/common/ui/app-ui-customization.service';
import { filter as _filter } from 'lodash-es';
import { LoginService } from './login/login.service';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppPreBootstrap } from 'AppPreBootstrap';

@Component({
    templateUrl: './account.component.html',
    styleUrls: ['./account.component.less'],
    encapsulation: ViewEncapsulation.None,
})
export class AccountComponent extends AppComponentBase implements OnInit {
    currentYear: number = this._dateTimeService.getYear();
    remoteServiceBaseUrl: string = AppConsts.remoteServiceBaseUrl;
    skin = this.appSession.theme.baseSettings.layout.darkMode ? 'dark' : 'light';
    defaultLogo = AppConsts.appBaseUrl + '/assets/common/images/app-logo-on-' + this.skin + '.svg';
    backgroundImageName = this.appSession.theme.baseSettings.layout.darkMode ? 'login-dark' : 'login';
    registerpage = false;
    profilePicture = AppConsts.appBaseUrl + '/assets/common/images/logo-vita.png';
    profilelogo = AppConsts.appBaseUrl + '/assets/common/images/agency.png';
    tenantChangeDisabledRoutes: string[] = [
        'select-edition',
        'buy',
        'upgrade',
        'extend',
        'register-tenant',
        'stripe-purchase',
        'stripe-subscribe',
        'stripe-update-subscription',
        'paypal-purchase',
        'stripe-payment-result',
        'payment-completed',
        'stripe-cancel-payment',
        'session-locked',
        'login',
    ];

    private viewContainerRef: ViewContainerRef;
    // eslint-disable-next-line @typescript-eslint/member-ordering
    fromOnboardingPage = false;

    public constructor(
        injector: Injector,
        private _router: Router,
        private _loginService: LoginService,
        private _uiCustomizationService: AppUiCustomizationService,
        private _dateTimeService: DateTimeService,
        private _activatedRoute: ActivatedRoute,
        viewContainerRef: ViewContainerRef
    ) {
        super(injector);
        this._activatedRoute.queryParams.subscribe(
            params => {
                this.fromOnboardingPage = params['fromOnboardingPage'];
            }
        );
        this.viewContainerRef = viewContainerRef;
    }
    showTenantChange(): boolean {
        if (!this._router.url) {
            return false;
        }
        if (
            _filter(this.tenantChangeDisabledRoutes, (route) => this._router.url.indexOf('/account/' + route) >= 0)
                .length
        ) {
            return false;
        }
        return abp.multiTenancy.isEnabled && !this.supportsTenancyNameInUrl();
    }

    isSelectEditionPage(): boolean {
        return this._router.url.indexOf('/account/select-edition') >= 0;
    }

    ngOnInit(): void {
        this._loginService.init();
        document.body.className = this._uiCustomizationService.getAccountModuleBodyClass();
    }

    goToHome(): void {
        (window as any).location.href = '/';
    }

    getBgUrl(): string {
        return 'url(./assets/metronic/themes/' + this.currentTheme.baseSettings.theme + '/images/bg/bg-4.jpg)';
    }

    private supportsTenancyNameInUrl() {
        return AppPreBootstrap.resolveTenancyName(AppConsts.appBaseUrlFormat) != null;
    }
}
