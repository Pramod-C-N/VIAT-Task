using System.Collections.Generic;
using vita.Authorization.Users.Importing.Dto;
using vita.Dto;

namespace vita.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
