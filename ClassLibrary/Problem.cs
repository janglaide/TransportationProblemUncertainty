using CenterSpace.NMath.Core;
using System.Linq;

namespace ClassLibrary
{
    public class Problem
    {
        private int _N;
        private int _R;
        private string _distribution;
        private string _distributionAB;
        private string _distributionL;
        private Experiment _experiment;
        private DoubleVector _A;
        private DoubleVector _B;
        private DoubleVector _l;
        private DoubleVector _alpha;
        private DoubleVector[] _Cs;
        public Problem(int N, int R, Experiment experiment)
        {
            _N = N;
            _R = R;
            _experiment = experiment;
            _distribution = experiment.DistributionC;
            _distributionAB = experiment.DistributionAB;
            _distributionL = experiment.DistributionL;
        }
        public Problem(DoubleVector a, DoubleVector b, DoubleVector l, DoubleVector alpha, DoubleVector[] cs, Experiment experiment)
        {
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
                _Cs = new DoubleVector[_R];
                for (var i = 0; i < _R; i++)
                    _Cs[i] = _experiment.GenerateMatrix(_N);
            }
            (DoubleVector[] xs, DoubleVector fsForXs) = Solver.GetSolutions(_Cs, _A, _B);
            var solution = Solver.SolveSeveral(_Cs, _A, _B, _l, _alpha, fsForXs);
            DoubleVector newAlpha = _alpha;
            DoubleVector optimalX = Solver.UpdateX(_Cs, _A, _B, _l, ref newAlpha, fsForXs, solution.OptimalX);
            DoubleVector fsForX = Solver.CalculateFs(_Cs, optimalX);
            DoubleVector deltas = Solver.CalculateDeltas(fsForX, fsForXs);
            DoubleVector ys = Solver.CalculateYs(deltas, _l);
            double functionValue = Solver.CalculateOptimanFunc(ys, newAlpha);
            DoubleVector distances = Solver.CalculateDistances(_Cs, optimalX);

            Solution solutionWithoutChange = new Solution(optimalX, functionValue, _alpha, newAlpha, _Cs, fsForX, xs, fsForXs, deltas, ys, distances, _B.Length);

            DoubleVector[] newCs = (DoubleVector[])_Cs.Clone();
            (double persentOfChange, DoubleVector newX) = _experiment.FindPercentOfChange(optimalX, ref newCs, _A, _B, _l, _alpha);
            DoubleVector newNewAlpha = newAlpha;
            (DoubleVector[] newXs, DoubleVector newFsForXs) = Solver.GetSolutions(newCs, _A, _B);
            newX = Solver.UpdateX(newCs, _A, _B, _l, ref newNewAlpha, newFsForXs, newX);
            DoubleVector newfsForX = Solver.CalculateFs(newCs, newX);
            DoubleVector newDeltas = Solver.CalculateDeltas(newfsForX, newFsForXs);
            DoubleVector newYs = Solver.CalculateYs(newDeltas, _l);
            double newFunctionValue = Solver.CalculateOptimanFunc(newYs, newNewAlpha);
            DoubleVector newDistances = Solver.CalculateDistances(newCs, newX);
            Solution solutionWithChange = new Solution(newX, newFunctionValue, newAlpha, newNewAlpha, newCs, newfsForX, newXs, newFsForXs, newDeltas, newYs, newDistances, _B.Length);

            return new FullSolution(_A, _B, _l, solutionWithoutChange, persentOfChange, solutionWithChange);
        }
    }
}
