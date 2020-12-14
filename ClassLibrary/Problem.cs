using CenterSpace.NMath.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public (string, string) Run()
        {
            (_A, _B) = _experiment.GenerateAB(_N);
            _l = _experiment.GenerateL(_R);
            _alpha = _experiment.GenerateAlpha(_R);
            _Cs = new DoubleVector[_R];
            for (var i = 0; i < _R; i++)
                _Cs[i] = _experiment.GenerateMatrix(_N);

            (_, var solutions) = Solver.GetSolutions(_Cs, _A, _B);
            var solution = Solver.SolveSeveral(_Cs, _A, _B, _l, _alpha, solutions);
            return (TransfromToOutput(solution.OptimalX),  
                Solver.RoundValue(solution.OptimalObjectiveFunctionValue).ToString());
        }
        private string TransfromToOutput(DoubleVector optimalX)
        {
            var result = "";
            int xQuantity = optimalX.Length;
            for (int i = 0; i < xQuantity; i++)
            {
                double number = Solver.RoundValue(optimalX[i]);

                if (number != 0)
                {
                    //result += $"{number}   ";
                    result += string.Format("{0:0.00}", number) + "\t";
                }
                else
                {
                    result += $"-\t";
                }

                if ((i + 1) % _N == 0)
                {
                    result += "\n";
                }

            }
            return result;
        }
    }
}
