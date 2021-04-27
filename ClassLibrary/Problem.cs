namespace ClassLibrary
{
    public class Problem
    {
        public readonly int _N;
        public readonly int _R;
        private readonly string _distribution;
        private readonly string _distributionAB;
        private readonly string _distributionL;
        private readonly Experiment _experiment;
        public double[] _A;
        public double[] _B;
        public double[] _l;
        public double[] _alpha;
        public double[][] _Cs;
        public Problem(int N, int R, Experiment experiment)
        {
            _N = N;
            _R = R;
            _experiment = experiment;
            _distribution = experiment.DistributionC;
            _distributionAB = experiment.DistributionAB;
            _distributionL = experiment.DistributionL;
        }
        public Problem(int N, int R, double[] a, double[] b, double[] l, double[] alpha, double[][] cs, Experiment experiment)
        {
            _N = N;
            _R = R;
            _A = a;
            _B = b;
            _l = l;
            _alpha = alpha;
            _Cs = cs;
            _experiment = experiment;
        }
        public FullSolution Run()
        {
            if (_A is null)
            {
                (_A, _B) = _experiment.GenerateAB(_N);
                _l = _experiment.GenerateL(_R);
                _alpha = _experiment.GenerateAlpha(_R);
                _Cs = new double[_R][];
                for (var i = 0; i < _R; i++)
                    _Cs[i] = _experiment.GenerateMatrix(_N);
            }

            (double[][] xs, double[] fsForXs) = Solver.GetSolutions(_Cs, _A, _B);
            (double[] solution, _) = Solver.SolveSeveral(_Cs, _A, _B, _l, _alpha, fsForXs);
            double[] newAlpha = new double[_R];
            _alpha.CopyTo(newAlpha, 0);
            double[] optimalX = Solver.UpdateX(_Cs, _A, _B, _l, ref newAlpha, fsForXs, solution);
            double[] fsForX = Solver.CalculateFs(_Cs, optimalX);
            double[] deltas = Solver.CalculateDeltas(fsForX, fsForXs);
            double[] ys = Solver.CalculateYs(deltas, _l);
            double functionValue = Solver.CalculateOptimanFunc(ys, newAlpha);
            double[] distances = Solver.CalculateDistances(_Cs, optimalX);

            Solution solutionWithoutChange = new Solution(optimalX, functionValue, _alpha, newAlpha, _Cs, fsForX, xs, fsForXs, deltas, ys, distances, _B.Length);

            double[][] newCs = new double[_R][];
            Experiment.CopyMultidimensional(_Cs, ref newCs);

            (double persentOfChange, double[] newX) = _experiment.FindPercentOfChange(optimalX, ref newCs, _A, _B, _l, _alpha);
            double[] newNewAlpha = new double[_R];
            newAlpha.CopyTo(newNewAlpha, 0);
            (double[][] newXs, double[] newFsForXs) = Solver.GetSolutions(newCs, _A, _B);
            newX = Solver.UpdateX(newCs, _A, _B, _l, ref newNewAlpha, newFsForXs, newX);
            double[] newfsForX = Solver.CalculateFs(newCs, newX);
            double[] newDeltas = Solver.CalculateDeltas(newfsForX, newFsForXs);
            double[] newYs = Solver.CalculateYs(newDeltas, _l);
            double newFunctionValue = Solver.CalculateOptimanFunc(newYs, newNewAlpha);
            double[] newDistances = Solver.CalculateDistances(newCs, newX);
            Solution solutionWithChange = new Solution(newX, newFunctionValue, newAlpha, newNewAlpha, newCs, newfsForX, newXs, newFsForXs, newDeltas, newYs, newDistances, _B.Length);

            return new FullSolution(_A, _B, _l, solutionWithoutChange, persentOfChange, solutionWithChange);
        }
    }
}
