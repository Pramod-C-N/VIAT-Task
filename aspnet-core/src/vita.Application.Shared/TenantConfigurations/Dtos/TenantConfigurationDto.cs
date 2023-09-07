using System;
using Abp.Application.Services.Dto;

namespace vita.TenantConfigurations.Dtos
{
    public class TenantConfigurationDto : EntityDto<long>
    {
        public bool isActive { get; set; }

        public bool isPhase1 { get; set; }
        public string TransactionType { get; set; }
        public string ShipmentJson { get; set; }
        public string EmailJson { get; set; }
        public string AdditionalFieldsJson { get; set; }
        public string AdditionalData1 { get; set; }
        public string AdditionalData2 { get; set; }
        public string AdditionalData3 { get; set; }

        public string Language { get; set; }
    }
}