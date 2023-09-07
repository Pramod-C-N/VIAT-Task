using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Credit.Dtos
{
    public class CreateOrEditCreditNoteContactPersonDto : EntityDto<long?>
    {

        public string IRNNo { get; set; }

        public string Name { get; set; }

        public string EmployeeCode { get; set; }

        public string ContactNumber { get; set; }

        public string GovtId { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Location { get; set; }

        public string Type { get; set; }
        public string Language { get; set; }
        public string AdditionalData1 { get; set; }

    }
}