using System.Collections.Generic;
using MvvmHelpers;
using vita.Models.NavigationMenu;

namespace vita.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}