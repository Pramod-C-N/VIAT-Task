using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using vita.ImportBatch.Dtos;
using vita.Dto;
using System.Data;

namespace vita.ImportBatch
{
    public interface IImportBatchDatasAppService : IApplicationService
    {
        Task<PagedResultDto<GetImportBatchDataForViewDto>> GetAll(GetAllImportBatchDatasInput input);

        Task<GetImportBatchDataForEditOutput> GetImportBatchDataForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditImportBatchDataDto input);

        Task Delete(EntityDto<long> input);

        Task<DataTable> execgetdataSP(int batchid,int? para);


    }
}