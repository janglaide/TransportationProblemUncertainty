namespace ClassLibrary.Models
{
    public class PercentageModel
    {
        public PercentageModel()
        {

        }
        public PercentageModel(int expId, int n, double value)
        {
            ExperimentId = expId;
            N = n;
            Value = value;
        }
        public int ExperimentId { get; set; }
        public int N { get; set; }
        public double Value { get; set; }
    }
}
