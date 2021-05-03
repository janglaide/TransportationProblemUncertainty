namespace ClassLibrary
{
    public class ParametersForDefined : SearchParameters
    {
        public double[] OldX { get; }
        public double[] A { get; }
        public double[] B { get; }
        public double[] L { get; }
        public double[] Alpha { get; }
        public double[][] Cs { get; }
        public double[] NewX { get; private set; }
        public ParametersForDefined(double[] oldX, double[][] cs, double[] a, double[] b, double[] l, double[] alpha, CChangeParameters parameters) : base(parameters)
        {
            A = a;
            B = b;
            L = l;
            Alpha = alpha;
            OldX = oldX;
            Cs = cs;
        }
        public void DefineXs(double[] x)
        {
            NewX = x;
        }
    }
}
