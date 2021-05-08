using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class DistributionParameters
    {
        public int Id { get; set; }
        public int DistributionCsId { get; set; }
        public Distribution DistributionCs { get; set; }
        public double DelayMeanCs { get; set; }
        public double? DeviationCs { get; set; }
        public int DistributionABId { get; set; }
        public Distribution DistributionAB { get; set; }
        public double DelayMeanAB { get; set; }
        public double DeviationAB { get; set; }
        public int DistributionLId { get; set; }
        public Distribution DistributionL { get; set; }
        public double DelayMeanL { get; set; }
        public double DeviationL { get; set; }

        public ICollection<Experiment> Experiments { get; set; }
    }
}
