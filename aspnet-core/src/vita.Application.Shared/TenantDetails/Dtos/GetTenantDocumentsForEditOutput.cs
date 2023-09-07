using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.TenantDetails.Dtos
{
    public class GetTenantDocumentsForEditOutput
    {
        public CreateOrEditTenantDocumentsDto TenantDocuments { get; set; }

    }
}