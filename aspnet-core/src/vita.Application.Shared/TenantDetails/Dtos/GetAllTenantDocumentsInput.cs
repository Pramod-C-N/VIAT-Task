using Abp.Application.Services.Dto;
using System;

namespace vita.TenantDetails.Dtos
{
    public class GetAllTenantDocumentsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string BranchIdFilter { get; set; }

        public string BranchNameFilter { get; set; }

        public string DocumentTypeFilter { get; set; }

        public string DocumentIdFilter { get; set; }

        public string DocumentNumberFilter { get; set; }

        public DateTime? MaxRegistrationDateFilter { get; set; }
        public DateTime? MinRegistrationDateFilter { get; set; }

        public string DocumentPathFilter { get; set; }

    }
}