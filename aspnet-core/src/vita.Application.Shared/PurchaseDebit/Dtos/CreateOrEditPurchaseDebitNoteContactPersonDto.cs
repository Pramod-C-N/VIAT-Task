using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.PurchaseDebit.Dtos
{
    public class CreateOrEditPurchaseDebitNoteContactPersonDto : EntityDto<long?>
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

    }
}