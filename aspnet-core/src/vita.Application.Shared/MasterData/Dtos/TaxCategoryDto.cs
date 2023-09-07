using System;
using Abp.Application.Services.Dto;

namespace vita.MasterData.Dtos
{
    public class TaxCategoryDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public string IsKSAApplicable { get; set; }

        public string TaxSchemeID { get; set; }

        public bool IsActive { get; set; }

    }
}