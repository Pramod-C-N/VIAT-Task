export class AppConsts {
    static readonly tenancyNamePlaceHolderInUrl = '{TENANCY_NAME}';

    static remoteServiceBaseUrl: string;
    static pdfUrl: string;
    static VitaVersion:string;
    static vitaMenu:boolean;
    static remoteServiceBaseUrlFormat: string;
    static appBaseUrl: string;
    static appBaseHref: string; // returns angular's base-href parameter value if used during the publish
    static appBaseUrlFormat: string;
    static recaptchaSiteKey: string;
    static subscriptionExpireNootifyDayCount: number;

    static localeMappings: any = [];

    static readonly userManagement = {
        defaultAdminUserName: 'admin',
    };

    static readonly localization = {
        defaultLocalizationSourceName: 'vita',
    };

    static readonly authorization = {
        encrptedAuthTokenName: 'enc_auth_token',
    };

    static readonly grid = {
        defaultPageSize: 10,
    };

    static readonly MinimumUpgradePaymentAmount = 1;

    /// <summary>
    /// Gets current version of the application.
    /// It's also shown in the web page.
    /// </summary>
    static readonly WebAppGuiVersion = '11.4.0';
  

    /// <summary>
    /// Redirects users to host URL when using subdomain as tenancy name for not existing tenants
    /// </summary>
    static readonly PreventNotExistingTenantSubdomains = false;
}
