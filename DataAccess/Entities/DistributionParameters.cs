using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class DistributionParameters
    {
        public int Id { get; set; }
        public int DistributionId { get; set; }
        public double DelayMean { get; set; }
        public double Deviation { get; set; }
    }
}
