namespace DataAccess.Entities
{
    public class Percentage
    {
        public int Id { get; set; }
        public int ExperimentId { get; set; }
        public Experiment Experiment { get; set; }
        public int N { get; set; }
        public double Value { get; set; }
        public int R { get; set; }
        public int ChangeParameterId { get; set; }
        public ChangeParameter ChangeParameter { get; set; }
    }
}
