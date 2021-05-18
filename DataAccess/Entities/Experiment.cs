using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class Experiment
    {
        public int Id { get; set; }
        public int DistributionId { get; set; }
        public DistributionParameters Distribution { get; set; }
        public double Accuracy { get; set; }

        public ICollection<Percentage> Percentages { get; set; }
    }
}
