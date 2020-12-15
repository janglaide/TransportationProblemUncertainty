using CenterSpace.NMath.Core;

namespace ClassLibrary
{
    public class FullSolution
    {
        public string A { get; }
        public string B { get; }
        public string L { get; }
        public Solution SolutionWithoutChange { get; }
        public string PersentOfChange { get; }
        public Solution SolutionWithChange { get; }
        public FullSolution(DoubleVector a, DoubleVector b, DoubleVector l, Solution solutionWithoutChange, double persentOfChange, Solution solutionWithChange)
        {
            A = OutputTransform.VectorTransform(a, "\n");
            B = OutputTransform.VectorTransform(b, "\n");
            L = OutputTransform.VectorTransform(l, "\n");
            SolutionWithoutChange = solutionWithoutChange;
            PersentOfChange = OutputTransform.ValueTransform(persentOfChange);
            SolutionWithChange = solutionWithChange;
        }
    }
}
