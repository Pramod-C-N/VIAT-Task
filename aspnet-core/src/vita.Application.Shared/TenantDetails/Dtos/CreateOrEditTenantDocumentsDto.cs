using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.TenantDetails.Dtos
{
    public class CreateOrEditTenantDocumentsDto : EntityDto<int?>
    {

        public Guid DocUniqueId { get; set; }

        public string BranchId { get; set; }

        public string BranchName { get; set; }

        public string DocumentType { get; set; }

        public string DocumentId { get; set; }

        public string DocumentNumber { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public string DocumentPath { get; set; }

    }
}