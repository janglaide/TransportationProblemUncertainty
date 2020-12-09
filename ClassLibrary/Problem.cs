﻿using CenterSpace.NMath.Core;
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
        private Experiment _experiment;
        private DoubleVector _A;
        private DoubleVector _B;
        private DoubleVector _l;
        private DoubleVector[] _Cs;
        public Problem(int N, int R, string distribution, double delay)
        {
            _N = N;
            _R = R;
            _distribution = distribution;
            _experiment = new Experiment(distribution, (delay, delay), (delay, delay), (delay, delay));
        }
        public Problem(int N, int R, string distribution, double delay, double deviation)
        {
            _N = N;
            _R = R;
            _distribution = distribution;
            _experiment = new Experiment(distribution, (delay, deviation), (delay, deviation), (delay, deviation));
        }
        public (string, string) Run()
        {
            (_A, _B) = _experiment.GenerateAB(_N);
            _l = _experiment.GenerateL(_R);
            _Cs = new DoubleVector[_R];
            for (var i = 0; i < _R; i++)
                _Cs[i] = _experiment.GenerateMatrix(_N);

            var solutions = Solver.GetSolutions(_Cs, _A, _B);
            var solution = Solver.SolveSeveral(_Cs, _A, _B, _l, solutions);
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