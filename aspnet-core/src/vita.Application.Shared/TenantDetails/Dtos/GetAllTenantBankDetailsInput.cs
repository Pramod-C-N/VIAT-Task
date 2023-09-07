using Abp.Application.Services.Dto;
using System;

namespace vita.TenantDetails.Dtos
{
    public class GetAllTenantBankDetailsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public Guid? UniqueIdentifierFilter { get; set; }

        public string AccountNameFilter { get; set; }

        public string AccountNumberFilter { get; set; }

        public string IBANFilter { get; set; }

        public string BankNameFilter { get; set; }

        public string SwiftCodeFilter { get; set; }

        public string BranchNameFilter { get; set; }

        public string BranchAddressFilter { get; set; }

    }
}