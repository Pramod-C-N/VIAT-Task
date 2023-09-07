using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Purchase.Dtos
{
    public class GetPurchaseEntryPartyForEditOutput
    {
        public CreateOrEditPurchaseEntryPartyDto PurchaseEntryParty { get; set; }

    }
}