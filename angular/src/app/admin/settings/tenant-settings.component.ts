import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    SettingScopes,
    SendTestEmailInput,
    TenantSettingsEditDto,
    TenantSettingsServiceProxy,
    JsonClaimMapDto,
    CreateOrEditTenantConfigurationDto,
    TenantConfigurationServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { finalize } from 'rxjs/operators';
import { KeyValueListManagerComponent } from '@app/shared/common/key-value-list-manager/key-value-list-manager.component';
import { UntypedFormControl } from '@angular/forms';

@Component({
    templateUrl: './tenant-settings.component.html',
    styleUrls: ['./tenant-settings.component.css'],
    animations: [appModuleAnimation()],
})
export class TenantSettingsComponent extends AppComponentBase implements OnInit {
    @ViewChild('wsFederationClaimsMappingManager') wsFederationClaimsMappingManager: KeyValueListManagerComponent;
    @ViewChild('openIdConnectClaimsMappingManager') openIdConnectClaimsMappingManager: KeyValueListManagerComponent;
    @ViewChild('emailSmtpSettingsForm') emailSmtpSettingsForm: UntypedFormControl;
    @ViewChild('uploadLogoInputLabel') uploadLogoInputLabel: ElementRef;
    @ViewChild('uploadCustomCSSInputLabel') uploadCustomCSSInputLabel: ElementRef;

    shipmentDetailsModel: any = {
        crNumber: {
            type: 'text',
            value: '',
            placeholder: ' Code',
            // rules: {
            //     required: true,
            // },
            col: '3',
        },
        registrationName: {
            type: 'text',
            value: '',
            placeholder: ' Name',
            col: '5',
        },
        vatid: {
            type: 'text',
            value: '',
            placeholder: ' VAT Number',
            col: '4',
        },
        address: {
            isChildGroup: true,
            buildingNo: {
                type: 'text',
                value: '',
                placeholder: ' Building No',
                col: '2',
            },
            additionalNo: {
                type: 'text',
                value: '',
                placeholder: ' Additional No',
                col: '2',
            },
            street: {
                type: 'text',
                value: '',
                placeholder: ' Street',
                col: '4',
            },
            additionalStreet: {
                type: 'text',
                value: '',
                placeholder: ' Additional Street',
                col: '4',
            },
            city: {
                type: 'text',
                value: '',
                placeholder: ' City',
                col: '2',
            },
            neighbourhood: {
                type: 'text',
                value: '',
                placeholder: ' District',
                col: '2',
            },
            state: {
                type: 'text',
                value: '',
                placeholder: ' Province/State',
                col: '3',
            },
            postalCode: {
                type: 'text',
                value: '',
                placeholder: ' Postal Code',
                col: '2',
            },
            countryCode: {
                type: 'text',
                value: '',
                placeholder: ' Country Code',
                col: '3',
            },
        },
        contactPerson: {
            isChildGroup: true,
            name: {
                type: 'text',
                value: '',
                placeholder: ' Attn',
                col: '5',
            },
            contactNumber: {
                type: 'text',
                value: '',
                placeholder: ' Contact',
                col: '4',
            },
        },
    };

    usingDefaultTimeZone = false;
    initialTimeZone: string = null;
    testEmailAddress: string = undefined;
    setRandomPassword: boolean;

    configurations: CreateOrEditTenantConfigurationDto = new CreateOrEditTenantConfigurationDto();
    emailCongiguration: any = {};
    additionalFieldsConfiguration: any = {};
    shipmentFieldsConfiguration: any = {};
    configurationsList: CreateOrEditTenantConfigurationDto[] = [];
    isShipmentEnabled: boolean = false;
    additionalFieldList: any[] = [];
    deleteList: number[] = [];

    isMultiTenancyEnabled: boolean = this.multiTenancy.isEnabled;
    showTimezoneSelection: boolean = abp.clock.provider.supportsMultipleTimezone;
    activeTabIndex: number = abp.clock.provider.supportsMultipleTimezone ? 0 : 1;
    loading = false;
    settings: TenantSettingsEditDto = undefined;

    logoUploader: FileUploader;
    customCssUploader: FileUploader;

    remoteServiceBaseUrl = AppConsts.remoteServiceBaseUrl;

    defaultTimezoneScope: SettingScopes = SettingScopes.Tenant;

    enabledSocialLoginSettings: string[];
    useFacebookHostSettings: boolean;
    useGoogleHostSettings: boolean;
    useMicrosoftHostSettings: boolean;
    useWsFederationHostSettings: boolean;
    useOpenIdConnectHostSettings: boolean;
    useTwitterHostSettings: boolean;

    wsFederationClaimMappings: { key: string; value: string }[];
    openIdConnectClaimMappings: { key: string; value: string }[];
    openIdConnectResponseTypeCode: boolean;
    openIdConnectResponseTypeToken: boolean;
    openIdConnectResponseTypeIdToken: boolean;

    initialEmailSettings: string;

    generateDraft:boolean=false;

    constructor(
        injector: Injector,
        private _tenantSettingsService: TenantSettingsServiceProxy,
        private _tokenService: TokenService,
        private _tenantConfigurationService: TenantConfigurationServiceProxy
    ) {
        super(injector);
        this.getConfigurations();
    }

    ngOnInit(): void {
        this.testEmailAddress = this.appSession.user.emailAddress;
        this.getSettings();
        this.initUploaders();
        this.loadSocialLoginSettings();
    }

    getConfigurations(): void {
        this._tenantConfigurationService.getAll('', '', 0, 100000).subscribe((e) => {
            console.log(e);
            if (e.totalCount > 0) {
                e.items.forEach((val) => {
                    if (val.tenantConfiguration.transactionType == 'General') {
                        this.configurations.isPhase1 = val.tenantConfiguration.isPhase1;
                        this.emailCongiguration = JSON.parse(val.tenantConfiguration.emailJson);
                        this.configurations.language = val.tenantConfiguration.language;
                        this.configurations.additionalData1 = val.tenantConfiguration.additionalData1;
                        this.configurations.additionalData2 = val.tenantConfiguration.additionalData2 ?? 'false';
                        this.generateDraft = val.tenantConfiguration.additionalData2 === "true"
                    }
                    this.configurationsList.push(
                        new CreateOrEditTenantConfigurationDto({
                            shipmentJson: val.tenantConfiguration.shipmentJson,
                            additionalFieldsJson: val.tenantConfiguration.additionalFieldsJson,
                            emailJson: val.tenantConfiguration.emailJson,
                            additionalData1: val.tenantConfiguration.additionalData1,
                            additionalData2: val.tenantConfiguration.additionalData2,
                            additionalData3: val.tenantConfiguration.additionalData3,
                            isPhase1: val.tenantConfiguration.isPhase1,
                            transactionType: val.tenantConfiguration.transactionType,
                            isActive: val.tenantConfiguration.isActive,
                            id: val.tenantConfiguration.id,
                            language: val.tenantConfiguration.language,
                        })
                    );
                });
            }
        });
    }

    getSettings(): void {
        this.loading = true;
        this._tenantSettingsService
            .getAllSettings()
            .pipe(
                finalize(() => {
                    this.loading = false;
                })
            )
            .subscribe((result: TenantSettingsEditDto) => {
                this.settings = result;
                if (this.settings.general) {
                    this.initialTimeZone = this.settings.general.timezone;
                    this.usingDefaultTimeZone =
                        this.settings.general.timezoneForComparison === abp.setting.values['Abp.Timing.TimeZone'];
                }
                this.useFacebookHostSettings = !(
                    this.settings.externalLoginProviderSettings.facebook.appId ||
                    this.settings.externalLoginProviderSettings.facebook_IsDeactivated
                );
                this.useGoogleHostSettings = !(
                    this.settings.externalLoginProviderSettings.google.clientId ||
                    this.settings.externalLoginProviderSettings.google_IsDeactivated
                );
                this.useMicrosoftHostSettings = !(
                    this.settings.externalLoginProviderSettings.microsoft.clientId ||
                    this.settings.externalLoginProviderSettings.microsoft_IsDeactivated
                );
                this.useWsFederationHostSettings = !(
                    this.settings.externalLoginProviderSettings.wsFederation.clientId ||
                    this.settings.externalLoginProviderSettings.wsFederation_IsDeactivated
                );
                this.useOpenIdConnectHostSettings = !(
                    this.settings.externalLoginProviderSettings.openIdConnect.clientId ||
                    this.settings.externalLoginProviderSettings.openIdConnect_IsDeactivated
                );
                this.useTwitterHostSettings = !(
                    this.settings.externalLoginProviderSettings.twitter.consumerKey ||
                    this.settings.externalLoginProviderSettings.twitter_IsDeactivated
                );

                this.wsFederationClaimMappings =
                    this.settings.externalLoginProviderSettings.openIdConnectClaimsMapping.map((item) => ({
                        key: item.key,
                        value: item.claim,
                    }));

                this.openIdConnectClaimMappings =
                    this.settings.externalLoginProviderSettings.openIdConnectClaimsMapping.map((item) => ({
                        key: item.key,
                        value: item.claim,
                    }));

                if (this.settings.externalLoginProviderSettings.openIdConnect.responseType) {
                    var openIdConnectResponseTypes =
                        this.settings.externalLoginProviderSettings.openIdConnect.responseType.split(',');

                    this.openIdConnectResponseTypeCode = openIdConnectResponseTypes.indexOf('code') > -1;
                    this.openIdConnectResponseTypeIdToken = openIdConnectResponseTypes.indexOf('id_token') > -1;
                    this.openIdConnectResponseTypeToken = openIdConnectResponseTypes.indexOf('token') > -1;
                }

                this.initialEmailSettings = JSON.stringify(this.settings.email);
            });
    }

    initUploaders(): void {
        this.logoUploader = this.createUploader('/TenantCustomization/UploadLogo', (result) => {
            this.appSession.tenant.logoFileType = result.fileType;
            this.appSession.tenant.logoId = result.id;
        });

        this.customCssUploader = this.createUploader('/TenantCustomization/UploadCustomCss', (result) => {
            this.appSession.tenant.customCssId = result.id;

            let oldTenantCustomCss = document.getElementById('TenantCustomCss');
            if (oldTenantCustomCss) {
                oldTenantCustomCss.remove();
            }

            let tenantCustomCss = document.createElement('link');
            tenantCustomCss.setAttribute('id', 'TenantCustomCss');
            tenantCustomCss.setAttribute('rel', 'stylesheet');
            tenantCustomCss.setAttribute(
                'href',
                AppConsts.remoteServiceBaseUrl +
                    '/TenantCustomization/GetCustomCss?tenantId=' +
                    this.appSession.tenant.id
            );
            document.head.appendChild(tenantCustomCss);
        });
    }

    createUploader(url: string, success?: (result: any) => void): FileUploader {
        const uploader = new FileUploader({ url: AppConsts.remoteServiceBaseUrl + url });

        uploader.onAfterAddingFile = (file) => {
            file.withCredentials = false;
        };

        uploader.onSuccessItem = (item, response, status) => {
            const ajaxResponse = <IAjaxResponse>JSON.parse(response);
            if (ajaxResponse.success) {
                this.notify.info(this.l('SavedSuccessfully'));
                if (success) {
                    success(ajaxResponse.result);
                }
            } else {
                this.message.error(ajaxResponse.error.message);
            }
        };

        const uploaderOptions: FileUploaderOptions = {};
        uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
        uploaderOptions.removeAfterUpload = true;
        uploader.setOptions(uploaderOptions);
        return uploader;
    }

    uploadLogo(): void {
        this.logoUploader.uploadAll();
    }

    uploadCustomCss(): void {
        this.customCssUploader.uploadAll();
    }

    clearLogo(): void {
        this._tenantSettingsService.clearLogo().subscribe(() => {
            this.appSession.tenant.logoFileType = null;
            this.appSession.tenant.logoId = null;
            this.notify.info(this.l('ClearedSuccessfully'));
        });
    }

    clearCustomCss(): void {
        this._tenantSettingsService.clearCustomCss().subscribe(() => {
            this.appSession.tenant.customCssId = null;

            let oldTenantCustomCss = document.getElementById('TenantCustomCss');
            if (oldTenantCustomCss) {
                oldTenantCustomCss.remove();
            }

            this.notify.info(this.l('ClearedSuccessfully'));
        });
    }

    mapClaims(): void {
        if (this.wsFederationClaimsMappingManager) {
            this.settings.externalLoginProviderSettings.wsFederationClaimsMapping =
                this.wsFederationClaimsMappingManager.getItems().map(
                    (item) =>
                        new JsonClaimMapDto({
                            key: item.key,
                            claim: item.value,
                        })
                );
        }

        if (this.openIdConnectClaimsMappingManager) {
            this.settings.externalLoginProviderSettings.openIdConnectClaimsMapping =
                this.openIdConnectClaimsMappingManager.getItems().map(
                    (item) =>
                        new JsonClaimMapDto({
                            key: item.key,
                            claim: item.value,
                        })
                );
        }
    }

    deleteConfiguration(index) {
        this.deleteList.push(this.configurationsList[index].id);
        this.configurationsList.splice(index, 1);
    }

    editConfiguration(index) {
        this.configurations = this.configurationsList[index];
        this.configurationsList.splice(index, 1);
        this.additionalFieldList = [];

        let conf = JSON.parse(this.configurations.additionalFieldsJson);
        this.additionalFieldList = Object.keys(conf).map((val) => {
            return {
                name: conf[val]?.placeholder,
                col: conf[val]?.col,
                type: conf[val]?.type,
                isRequired: conf[val]?.rules?.required ?? false,
            };
        });

        this.isShipmentEnabled = this.configurations.shipmentJson != null;
    }

    addConfiguration() {
        let conf = {};
        this.additionalFieldList.forEach((val) => {
            conf[val?.name?.toLowerCase()?.replaceAll(' ', '_')] = {
                type: val?.type,
                value: '',
                placeholder: val?.name,
                col: val?.col,
            };
            val?.isRequired
                ? (conf[val?.name?.toLowerCase()?.replaceAll(' ', '_')].rules = {
                      required: val?.isRequired,
                  })
                : null;
        });

        this.additionalFieldList.length > 0
            ? (this.configurations.additionalFieldsJson = JSON.stringify(conf))
            : (this.configurations.additionalFieldsJson = null);

        this.isShipmentEnabled
            ? (this.configurations.shipmentJson = JSON.stringify(this.shipmentDetailsModel))
            : (this.configurations.shipmentJson = null);

        console.log(this.configurations);

        this.configurationsList.push(JSON.parse(JSON.stringify(this.configurations)));

        this.configurations = new CreateOrEditTenantConfigurationDto();
        this.additionalFieldList = [];
        this.isShipmentEnabled = false;
    }

    addField() {
        this.additionalFieldList.push({
            name: 'Field ' + this.additionalFieldList.length,
            col: '3',
            val: 'text',
            isRequired: false,
        });
    }

    deleteField(i) {
        this.additionalFieldList.splice(i, 1);
    }

    saveConfigurations(): void {
        // this.configurations.additionalFieldsJson = JSON.stringify(this.additionalFieldsConfiguration);
        // this.configurations.shipmentJson = JSON.stringify(this.shipmentFieldsConfiguration);
        let configurations = new CreateOrEditTenantConfigurationDto();
        let general = this.configurationsList.find((a) => a.transactionType == 'General');

        configurations.transactionType = 'General';
        configurations.emailJson = JSON.stringify(this.emailCongiguration);
        configurations.isActive = true;
        configurations.isPhase1 = this.configurations.isPhase1;
        configurations.language = this.configurations.language;
        configurations.additionalData1 = this.configurations.additionalData1;
        configurations.additionalData2 = this.generateDraft?'true':'false';
        configurations.id = general != undefined ? general.id : null;
        this._tenantConfigurationService.createOrEdit(configurations).subscribe((e) => {});

        this.deleteList.forEach((val) => {
            this._tenantConfigurationService.delete(val).subscribe((e) => {});
        });

        this.configurationsList
            .filter((a) => a.transactionType != 'General')
            .forEach((val) => {
                this._tenantConfigurationService.createOrEdit(val).subscribe((e) => {});
            });
        // this.configurations = new CreateOrEditTenantConfigurationDto();
        //   this.configurations.isActive
        //   this.configurations.transactionType
    }

    saveAll(): void {
        this.saveConfigurations();

        if (!this.isSmtpSettingsFormValid()) {
            return;
        }

        this.settings.externalLoginProviderSettings.openIdConnect.responseType =
            this.getSelectedOpenIdConnectResponseTypes();

        this.mapClaims();
        this._tenantSettingsService.updateAllSettings(this.settings).subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            setTimeout(() => {
                window.location.reload();
            }, 1500);

            if (
                abp.clock.provider.supportsMultipleTimezone &&
                this.usingDefaultTimeZone &&
                this.initialTimeZone !== this.settings.general.timezone
            ) {
                this.message.info(this.l('TimeZoneSettingChangedRefreshPageNotification')).then(() => {
                    window.location.reload();
                });
            }
            this.initialEmailSettings = JSON.stringify(this.settings.email);
        });

        // setTimeout(()=>{
        //     window.location.reload();
        // },2000)
    }

    getSelectedOpenIdConnectResponseTypes(): string {
        var openIdConnectResponseTypes = '';
        if (this.openIdConnectResponseTypeToken) {
            openIdConnectResponseTypes += 'token';
        }

        if (this.openIdConnectResponseTypeIdToken) {
            if (openIdConnectResponseTypes.length > 0) {
                openIdConnectResponseTypes += ',';
            }
            openIdConnectResponseTypes += 'id_token';
        }

        if (this.openIdConnectResponseTypeCode) {
            if (openIdConnectResponseTypes.length > 0) {
                openIdConnectResponseTypes += ',';
            }
            openIdConnectResponseTypes += 'code';
        }

        return openIdConnectResponseTypes;
    }

    sendTestEmail(): void {
        const input = new SendTestEmailInput();
        input.emailAddress = this.testEmailAddress;

        if (this.initialEmailSettings !== JSON.stringify(this.settings.email)) {
            this.message.confirm(this.l('SendEmailWithSavedSettingsWarning'), this.l('AreYouSure'), (isConfirmed) => {
                if (isConfirmed) {
                    this._tenantSettingsService.sendTestEmail(input).subscribe((result) => {
                        this.notify.info(this.l('TestEmailSentSuccessfully'));
                    });
                }
            });
        } else {
            this._tenantSettingsService.sendTestEmail(input).subscribe((result) => {
                this.notify.info(this.l('TestEmailSentSuccessfully'));
            });
        }
    }

    loadSocialLoginSettings(): void {
        const self = this;
        this._tenantSettingsService.getEnabledSocialLoginSettings().subscribe((setting) => {
            self.enabledSocialLoginSettings = setting.enabledSocialLoginSettings;
        });
    }

    clearFacebookSettings(): void {
        this.settings.externalLoginProviderSettings.facebook.appId = '';
        this.settings.externalLoginProviderSettings.facebook.appSecret = '';
        this.settings.externalLoginProviderSettings.facebook_IsDeactivated = false;
    }

    clearGoogleSettings(): void {
        this.settings.externalLoginProviderSettings.google.clientId = '';
        this.settings.externalLoginProviderSettings.google.clientSecret = '';
        this.settings.externalLoginProviderSettings.google.userInfoEndpoint = '';
        this.settings.externalLoginProviderSettings.google_IsDeactivated = false;
    }

    clearMicrosoftSettings(): void {
        this.settings.externalLoginProviderSettings.microsoft.clientId = '';
        this.settings.externalLoginProviderSettings.microsoft.clientSecret = '';
        this.settings.externalLoginProviderSettings.microsoft_IsDeactivated = false;
    }

    clearWsFederationSettings(): void {
        this.settings.externalLoginProviderSettings.wsFederation.clientId = '';
        this.settings.externalLoginProviderSettings.wsFederation.authority = '';
        this.settings.externalLoginProviderSettings.wsFederation.wtrealm = '';
        this.settings.externalLoginProviderSettings.wsFederation.metaDataAddress = '';
        this.settings.externalLoginProviderSettings.wsFederation.tenant = '';
        this.settings.externalLoginProviderSettings.wsFederationClaimsMapping = [];
        this.settings.externalLoginProviderSettings.wsFederation_IsDeactivated = false;
    }

    clearOpenIdSettings(): void {
        this.settings.externalLoginProviderSettings.openIdConnect.clientId = '';
        this.settings.externalLoginProviderSettings.openIdConnect.clientSecret = '';
        this.settings.externalLoginProviderSettings.openIdConnect.authority = '';
        this.settings.externalLoginProviderSettings.openIdConnect.loginUrl = '';
        this.settings.externalLoginProviderSettings.openIdConnectClaimsMapping = [];
        this.settings.externalLoginProviderSettings.openIdConnect_IsDeactivated = false;
    }

    clearTwitterSettings(): void {
        this.settings.externalLoginProviderSettings.twitter.consumerKey = '';
        this.settings.externalLoginProviderSettings.twitter.consumerSecret = '';
    }

    isSocialLoginEnabled(name: string): boolean {
        return this.enabledSocialLoginSettings && this.enabledSocialLoginSettings.indexOf(name) !== -1;
    }

    isSmtpSettingsFormValid(): boolean {
        if (!this.emailSmtpSettingsForm) {
            return true;
        }
        return this.emailSmtpSettingsForm.valid;
    }

    onUploadLogoInputChange(files: FileList) {
        this.uploadLogoInputLabel.nativeElement.innerText = Array.from(files)
            .map((f) => f.name)
            .join(', ');
    }

    onUploadCustomCSSInputChange(files: FileList) {
        this.uploadCustomCSSInputLabel.nativeElement.innerText = Array.from(files)
            .map((f) => f.name)
            .join(', ');
    }
}
