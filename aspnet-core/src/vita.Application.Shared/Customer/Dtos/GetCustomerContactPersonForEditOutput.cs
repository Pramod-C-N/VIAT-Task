using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Customer.Dtos
{
    public class GetCustomerContactPersonForEditOutput
    {
        public CreateOrEditCustomerContactPersonDto CustomerContactPerson { get; set; }

    }
}