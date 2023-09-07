using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using vita.MasterData.Exporting;
using vita.MasterData.Dtos;
using vita.Dto;
using Abp.Application.Services.Dto;
using vita.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using vita.Storage;

namespace vita.MasterData
{
    [AbpAuthorize(AppPermissions.Pages_PaymentMeans)]
    public class PaymentMeansAppService : vitaAppServiceBase, IPaymentMeansAppService
    {
        private readonly IRepository<PaymentMeans> _paymentMeansRepository;
        private readonly IPaymentMeansExcelExporter _paymentMeansExcelExporter;

        public PaymentMeansAppService(IRepository<PaymentMeans> paymentMeansRepository, IPaymentMeansExcelExporter paymentMeansExcelExporter)
        {
            _paymentMeansRepository = paymentMeansRepository;
            _paymentMeansExcelExporter = paymentMeansExcelExporter;

        }

        public async Task<PagedResultDto<GetPaymentMeansForViewDto>> GetAll(GetAllPaymentMeansInput input)
        {

            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var filteredPaymentMeans = _paymentMeansRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

                var pagedAndFilteredPaymentMeans = filteredPaymentMeans
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var paymentMeans = from o in pagedAndFilteredPaymentMeans
                                   select new
                                   {

                                       o.Name,
                                       o.Description,
                                       o.Code,
                                       o.IsActive,
                                       Id = o.Id
                                   };

                var totalCount = await filteredPaymentMeans.CountAsync();

                var dbList = await paymentMeans.ToListAsync();
                var results = new List<GetPaymentMeansForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetPaymentMeansForViewDto()
                    {
                        PaymentMeans = new PaymentMeansDto
                        {

                            Name = o.Name,
                            Description = o.Description,
                            Code = o.Code,
                            IsActive = o.IsActive,
                            Id = o.Id,
                        }
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetPaymentMeansForViewDto>(
                    totalCount,
                    results
                );
            }
        }

        public async Task<GetPaymentMeansForViewDto> GetPaymentMeansForView(int id)
        {
            var paymentMeans = await _paymentMeansRepository.GetAsync(id);

            var output = new GetPaymentMeansForViewDto { PaymentMeans = ObjectMapper.Map<PaymentMeansDto>(paymentMeans) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PaymentMeans_Edit)]
        public async Task<GetPaymentMeansForEditOutput> GetPaymentMeansForEdit(EntityDto input)
        {
            var paymentMeans = await _paymentMeansRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPaymentMeansForEditOutput { PaymentMeans = ObjectMapper.Map<CreateOrEditPaymentMeansDto>(paymentMeans) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPaymentMeansDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_PaymentMeans_Create)]
        protected virtual async Task Create(CreateOrEditPaymentMeansDto input)
        {
            var paymentMeans = ObjectMapper.Map<PaymentMeans>(input);
            paymentMeans.UniqueIdentifier = Guid.NewGuid();
            if (AbpSession.TenantId != null)
            {
                paymentMeans.TenantId = (int?)AbpSession.TenantId;
            }

            await _paymentMeansRepository.InsertAsync(paymentMeans);

        }

        [AbpAuthorize(AppPermissions.Pages_PaymentMeans_Edit)]
        protected virtual async Task Update(CreateOrEditPaymentMeansDto input)
        {
            var paymentMeans = await _paymentMeansRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, paymentMeans);

        }

        [AbpAuthorize(AppPermissions.Pages_PaymentMeans_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _paymentMeansRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPaymentMeansToExcel(GetAllPaymentMeansForExcelInput input)
        {

            var filteredPaymentMeans = _paymentMeansRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredPaymentMeans
                         select new GetPaymentMeansForViewDto()
                         {
                             PaymentMeans = new PaymentMeansDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Code = o.Code,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var paymentMeansListDtos = await query.ToListAsync();

            return _paymentMeansExcelExporter.ExportToFile(paymentMeansListDtos);
        }

    }
}