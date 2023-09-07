using System.Collections.Generic;
using vita.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace vita.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
