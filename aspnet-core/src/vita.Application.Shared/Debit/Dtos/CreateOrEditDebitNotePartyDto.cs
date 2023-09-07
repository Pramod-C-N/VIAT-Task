using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using vita.Sales.Dtos;

namespace vita.Debit.Dtos
{
    public class CreateOrEditDebitNotePartyDto : EntityDto<long?>
    {


        public string IRNNo { get; set; }

        public string RegistrationName { get; set; }

        public string VATID { get; set; }

        public string GroupVATID { get; set; }

        public string CRNumber { get; set; }

        public string OtherID { get; set; }

        public string CustomerId { get; set; }

        public string Type { get; set; }

        public string Language { get; set; }
        public string AdditionalData1 { get; set; }
        public string OtherDocumentTypeId { get; set; }

        public string FaxNo { get; set; }

        public string Website { get; set; }
        public CreateOrEditDebitNoteAddressDto Address { get; set; }
        public CreateOrEditDebitNoteContactPersonDto ContactPerson { get; set; }

    }
}