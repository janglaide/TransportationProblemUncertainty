namespace ClassLibrary.Models
{
    public class DistributionParametersModel
    {
        public DistributionParametersModel(int CsId, int ABId, int LId, double dmC, double? devC,
            double dmAB, double? devAB, double dmL, double? devL)
        {
            DistributionCsId = CsId;
            DistributionABId = ABId;
            DistributionLId = LId;
            DelayMeanCs = dmC;
            DeviationCs = devC;
            DelayMeanAB = dmAB;
            DeviationAB = devAB;
            DelayMeanL = dmL;
            DeviationL = devL;
        }
        public int DistributionCsId { get; set; }
        public double DelayMeanCs { get; set; }
        public double? DeviationCs { get; set; }
        public int DistributionABId { get; set; }
        public double DelayMeanAB { get; set; }
        public double? DeviationAB { get; set; }
        public int DistributionLId { get; set; }
        public double DelayMeanL { get; set; }
        public double? DeviationL { get; set; }

    }
}
