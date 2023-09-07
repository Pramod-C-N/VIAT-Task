using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Sales.Dtos
{
    public class CreateOrEditSalesInvoicePartyDto : EntityDto<long?>
    {

        public string IRNNo { get; set; }

        public string RegistrationName { get; set; }

        public string VATID { get; set; }

        public string GroupVATID { get; set; }

        public string CRNumber { get; set; }

        public string OtherID { get; set; }

        public string CustomerId { get; set; }
            
        public string Type { get; set; }
        public CreateOrEditSalesInvoiceAddressDto Address { get; set; }
        public CreateOrEditSalesInvoiceContactPersonDto ContactPerson { get; set; }

    }
}