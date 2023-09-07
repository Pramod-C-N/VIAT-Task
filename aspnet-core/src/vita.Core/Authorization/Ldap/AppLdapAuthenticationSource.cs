using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using vita.Authorization.Users;
using vita.MultiTenancy;

namespace vita.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}