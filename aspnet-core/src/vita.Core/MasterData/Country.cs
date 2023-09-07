using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace vita.MasterData
{
    [Table("Country")]
    [Audited]
    public class Country : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Guid UniqueIdentifier { get; set; }

        public virtual string Name { get; set; }

        public virtual string StateName { get; set; }

        public virtual string Sovereignty { get; set; }

        [RegularExpression(CountryConsts.AlphaCodeRegex)]
        [StringLength(CountryConsts.MaxAlphaCodeLength, MinimumLength = CountryConsts.MinAlphaCodeLength)]
        public virtual string AlphaCode { get; set; }

        [RegularExpression(CountryConsts.NumericCodeRegex)]
        public virtual string NumericCode { get; set; }

        public virtual string InternetCCTLD { get; set; }

        public virtual string SubDivisionCode { get; set; }

        [RegularExpression(CountryConsts.Alpha3CodeRegex)]
        [StringLength(CountryConsts.MaxAlpha3CodeLength, MinimumLength = CountryConsts.MinAlpha3CodeLength)]
        public virtual string Alpha3Code { get; set; }

        public virtual string CountryGroup { get; set; }

        public virtual bool IsActive { get; set; }

    }
}