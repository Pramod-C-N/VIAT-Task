using System.Collections.Generic;
using vita.Editions.Dto;

namespace vita.MultiTenancy.Dto
{
    public class EditionsSelectOutput
    {
        public EditionsSelectOutput()
        {
            AllFeatures = new List<FlatFeatureSelectDto>();
            EditionsWithFeatures = new List<EditionWithFeaturesDto>();
        }

        public List<FlatFeatureSelectDto> AllFeatures { get; set; }

        public List<EditionWithFeaturesDto> EditionsWithFeatures { get; set; }
    }
}
