using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Customer.Dtos
{
    public class CreateOrEditCustomerSectorDetailDto : EntityDto<long?>
    {

        public string CustomerID { get; set; }

        public Guid CustomerUniqueIdentifier { get; set; }

        public string SubIndustryCode { get; set; }

        public string SubIndustryName { get; set; }

    }
}