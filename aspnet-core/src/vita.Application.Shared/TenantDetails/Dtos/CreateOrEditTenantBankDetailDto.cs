using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.TenantDetails.Dtos
{
    public class CreateOrEditTenantBankDetailDto : EntityDto<int?>
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