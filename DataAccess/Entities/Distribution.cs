using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class Distribution
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<DistributionParameters> DistributionParametersCs { get; set; }
        public ICollection<DistributionParameters> DistributionParametersAB { get; set; }
        public ICollection<DistributionParameters> DistributionParametersL { get; set; }
    }
}
