using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Customer.Dtos
{
    public class GetCustomerSectorDetailForEditOutput
    {
        public CreateOrEditCustomerSectorDetailDto CustomerSectorDetail { get; set; }

    }
}