﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using vita.Sales.Dtos;

namespace vita.DraftFee.Dtos
{
    public class CreateOrEditDraftPartyDto : EntityDto<long?>
    {

        public string IRNNo { get; set; }

        public string RegistrationName { get; set; }

        public string VATID { get; set; }

        public string GroupVATID { get; set; }

        public string CRNumber { get; set; }

        public string OtherID { get; set; }

        public string CustomerId { get; set; }

        public string Type { get; set; }

        public string AdditionalData1 { get; set; }

        public string Language { get; set; }

        public string OtherDocumentTypeId { get; set; }

        public CreateOrEditDraftAddressDto Address { get; set; }
        public CreateOrEditDraftContactPersonDto ContactPerson { get; set; }

    }
}