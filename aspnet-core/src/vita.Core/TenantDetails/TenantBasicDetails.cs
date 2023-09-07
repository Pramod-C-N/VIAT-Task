using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.TenantDetails
{
    [Table("TenantBasicDetails")]
    [Audited]
    public class TenantBasicDetails : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string TenantType { get; set; }

        public virtual string ConstitutionType { get; set; }

        public virtual string BusinessCategory { get; set; }

        public virtual string OperationalModel { get; set; }

        public virtual string TurnoverSlab { get; set; }

        public virtual string ContactPerson { get; set; }

        public virtual string ContactNumber { get; set; }

        public virtual string EmailID { get; set; }

        public virtual string Nationality { get; set; }

        public virtual string Designation { get; set; }

        public virtual string VATID { get; set; }

        public virtual string ParentEntityName { get; set; }

        public virtual string LegalRepresentative { get; set; }

        public virtual string ParentEntityCountryCode { get; set; }

        public virtual string LastReturnFiled { get; set; }

        public virtual string VATReturnFillingFrequency { get; set; }

        public virtual string TimeZone { get; set; }

        public virtual bool isPhase1 { get; set; }

        public virtual string FaxNo { get; set; }

        public virtual string Website { get; set; }

        public virtual string LangTenancyName { get; set; }

    }
}