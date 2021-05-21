namespace ClassLibrary.Models
{
    public class ExperimentModel
    {
        public ExperimentModel()
        {

        }
        public ExperimentModel(int distrId, double accuracy)
        {
            DistributionId = distrId;
            Accuracy = accuracy;
        }
        public int DistributionId { get; set; }
        public double Accuracy { get; set; }
    }
}
