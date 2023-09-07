using System;
using Abp.Application.Services.Dto;

namespace vita.TenantDetails.Dtos
{
    public class TenantBasicDetailsDto : EntityDto
    {
        public string TenantType { get; set; }

        public string ConstitutionType { get; set; }

        public string BusinessCategory { get; set; }

        public string OperationalModel { get; set; }

        public string TurnoverSlab { get; set; }

        public string ContactPerson { get; set; }

        public string ContactNumber { get; set; }

        public string EmailID { get; set; }

        public string Nationality { get; set; }

        public string Designation { get; set; }

        public string VATID { get; set; }

        public string ParentEntityName { get; set; }

        public string LegalRepresentative { get; set; }

        public string ParentEntityCountryCode { get; set; }

        public string LastReturnFiled { get; set; }

        public string VATReturnFillingFrequency { get; set; }

        public string TimeZone { get; set; }

        public bool isPhase1 { get; set; }

        public string FaxNo { get; set; }

        public string Website { get; set; }

    }
}