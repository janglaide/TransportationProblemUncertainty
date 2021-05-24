using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class ChangeParameter
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public ICollection<Percentage> Percentages { get; set; }
    }
}
