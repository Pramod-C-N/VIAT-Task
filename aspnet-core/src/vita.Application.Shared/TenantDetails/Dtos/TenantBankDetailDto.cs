using System;
using Abp.Application.Services.Dto;

namespace vita.TenantDetails.Dtos
{
    public class TenantBankDetailDto : EntityDto
    {
        public Guid UniqueIdentifier { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string IBAN { get; set; }

        public string BankName { get; set; }

        public string SwiftCode { get; set; }

        public bool IsActive { get; set; }

        public string BranchName { get; set; }

        public string BranchAddress { get; set; }

    }
}