using CenterSpace.NMath.Core;

namespace ClassLibrary
{
    public class Solution
    {
        public string OptimalX { get; }
        public string FunctionValue { get; }
        public string Alpha { get; }
        public string AlphaChanged { get; set; }
        public string[] Cs { get; }
        public string[] FsForX { get; }
        public string[] Xs { get; }
        public string[] FsForXs { get; }
        public string[] Deltas { get; }
        public string[] Ys { get; }
        public string[] Distances { get; }
        public int N { get; }
        public Solution(DoubleVector optimalX, double functionValue, DoubleVector alpha, DoubleVector alphaChanged, DoubleVector[] cs, DoubleVector fsForX, DoubleVector[] xs, DoubleVector fsForXs, DoubleVector deltas, DoubleVector ys, DoubleVector distances, int columns)
        {
            OptimalX = OutputTransform.MatrixTransform(optimalX, columns);
            FunctionValue = OutputTransform.ValueTransform(functionValue);
            Alpha = OutputTransform.VectorTransform(alpha, "\t");
            AlphaChanged = OutputTransform.VectorTransform(alphaChanged, "\t");
            Cs = OutputTransform.ArrayMatrixTransform(cs, columns);
            FsForX = OutputTransform.ArrayValuesTransform(fsForX);
            Xs = OutputTransform.ArrayMatrixTransform(xs, columns);
            FsForXs = OutputTransform.ArrayValuesTransform(fsForXs);
            Deltas = OutputTransform.ArrayValuesTransform(deltas);
            Ys = OutputTransform.ArrayValuesTransform(ys);
            Distances = OutputTransform.ArrayValuesTransform(distances);
            N = columns;
        }
    }
}
