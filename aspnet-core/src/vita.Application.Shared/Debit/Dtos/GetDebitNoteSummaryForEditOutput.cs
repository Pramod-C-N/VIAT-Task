using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.Debit.Dtos
{
    public class GetDebitNoteSummaryForEditOutput
    {
        public CreateOrEditDebitNoteSummaryDto DebitNoteSummary { get; set; }

    }
}