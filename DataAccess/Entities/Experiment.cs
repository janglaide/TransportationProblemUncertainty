using System;
using System.Collections.Generic;
using System.Text;

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
