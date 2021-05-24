namespace ClassLibrary.Models
{
    public class PercentageModel
    {
        public PercentageModel()
        {

        }
        public PercentageModel(int expId, int n, double value, int r, int parametersId)
        {
            ExperimentId = expId;
            N = n;
            Value = value;
            R = r;
            ChangeParameterId = parametersId;
        }
        public int ExperimentId { get; set; }
        public int N { get; set; }
        public double Value { get; set; }
        public int R { get; set; }
        public int ChangeParameterId { get; set; }
    }
}
