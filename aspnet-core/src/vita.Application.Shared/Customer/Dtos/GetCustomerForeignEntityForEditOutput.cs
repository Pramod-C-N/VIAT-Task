using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Customer.Dtos
{
    public class GetCustomerForeignEntityForEditOutput
    {
        public CreateOrEditCustomerForeignEntityDto CustomerForeignEntity { get; set; }

    }
}