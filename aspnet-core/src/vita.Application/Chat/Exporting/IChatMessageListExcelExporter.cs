using System.Collections.Generic;
using Abp;
using vita.Chat.Dto;
using vita.Dto;

namespace vita.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
