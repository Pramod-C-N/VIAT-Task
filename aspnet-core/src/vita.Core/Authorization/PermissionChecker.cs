using Abp.Authorization;
using vita.Authorization.Roles;
using vita.Authorization.Users;

namespace vita.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
