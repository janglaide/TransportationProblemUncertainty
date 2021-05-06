using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class Percentage
    {
        public int Id { get; set; }
        public int ExperimentId { get; set; }
        public int N { get; set; }
        public double Value { get; set; }
    }
}
