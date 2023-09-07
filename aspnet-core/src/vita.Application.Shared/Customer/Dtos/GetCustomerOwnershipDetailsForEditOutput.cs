using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Customer.Dtos
{
    public class GetCustomerOwnershipDetailsForEditOutput
    {
        public CreateOrEditCustomerOwnershipDetailsDto CustomerOwnershipDetails { get; set; }

    }
}