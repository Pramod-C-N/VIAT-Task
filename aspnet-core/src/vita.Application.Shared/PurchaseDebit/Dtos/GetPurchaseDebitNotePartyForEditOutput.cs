using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.PurchaseDebit.Dtos
{
    public class GetPurchaseDebitNotePartyForEditOutput
    {
        public CreateOrEditPurchaseDebitNotePartyDto PurchaseDebitNoteParty { get; set; }

    }
}