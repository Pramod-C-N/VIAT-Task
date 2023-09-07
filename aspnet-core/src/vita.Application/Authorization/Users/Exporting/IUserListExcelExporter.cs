using System.Collections.Generic;
using vita.Authorization.Users.Dto;
using vita.Dto;

namespace vita.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}