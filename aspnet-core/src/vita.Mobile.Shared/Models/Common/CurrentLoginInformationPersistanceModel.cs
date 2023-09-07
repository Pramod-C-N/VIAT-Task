using Abp.AutoMapper;
using vita.Sessions.Dto;

namespace vita.Models.Common
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput)),
     AutoMapTo(typeof(GetCurrentLoginInformationsOutput))]
    public class CurrentLoginInformationPersistanceModel
    {
        public UserLoginInfoPersistanceModel User { get; set; }

        public TenantLoginInfoPersistanceModel Tenant { get; set; }

        public ApplicationInfoPersistanceModel Application { get; set; }
    }
}