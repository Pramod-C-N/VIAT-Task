using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.PurchaseCredit.Dtos
{
    public class GetPurchaseCreditNoteItemForEditOutput
    {
        public CreateOrEditPurchaseCreditNoteItemDto PurchaseCreditNoteItem { get; set; }

    }
}