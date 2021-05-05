using System;
using System.Collections.Generic;
using System.Text;
using static ClassLibrary.PercentFinder;

namespace ClassLibrary.Logic
{
    public class Counter
    {
        public double Average { get; set; }
        public int N;

        public PercentDelegate percentDelegate;
        public SearchParameters percentParameters;
        public Counter(int _n, PercentDelegate SearchPercent, SearchParameters parameters)
        {
            percentDelegate = SearchPercent;
            percentParameters = parameters;
            Average = 0;
            N = _n;
        }
    }
}
