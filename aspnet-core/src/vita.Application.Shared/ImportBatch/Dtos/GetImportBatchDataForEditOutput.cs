using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace vita.ImportBatch.Dtos
{
    public class GetImportBatchDataForEditOutput
    {
        public CreateOrEditImportBatchDataDto ImportBatchData { get; set; }

    }
}