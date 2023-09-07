using System;
using Abp.Application.Services.Dto;

namespace vita.TenantDetails.Dtos
{
    public class TenantDocumentsDto : EntityDto
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