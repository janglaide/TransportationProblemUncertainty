using ClassLibrary.Enums;
using ClassLibrary.Logic;

namespace ClassLibrary.MethodParameters
{
    public class ParametersForDefined : SearchParameters
    {
        public double[] OldX { get; }
        public double[] A { get; }
        public double[] B { get; }
        public double[] L { get; }
        public double[] Alpha { get; }
        public double[][] Cs { get { return _cs; } }
        public double[] NewX { get; private set; }
        private double[][] _csBase;
        private double[][] _cs;
        public ParametersForDefined(double[] oldX, double[][] cs, double[] a, double[] b, double[] l, double[] alpha, CChangeParameters parameters) : base(parameters)
        {
            A = a;
            B = b;
            L = l;
            Alpha = alpha;
            OldX = oldX;
            _csBase = new double[cs.Length][];
            _cs = new double[cs.Length][];
            PercentFinder.CopyMultidimensional(cs, ref _csBase);
            PercentFinder.CopyMultidimensional(cs, ref _cs);
        }
        public void DefineXs(double[] x)
        {
            NewX = x;
        }
        public override void Clear()
        {
            PercentFinder.CopyMultidimensional(_csBase, ref _cs);
        }
    }
}
