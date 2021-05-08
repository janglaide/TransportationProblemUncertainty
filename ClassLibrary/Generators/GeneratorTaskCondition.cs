using System;
using System.Linq;

namespace ClassLibrary
{
    public class GeneratorTaskCondition
    {
        private readonly string _distributionC;
        private readonly string _distributionAB;
        private readonly string _distributionL;
        private readonly (double, double) _parametersC;
        private readonly (double, double) _parametersAB;
        private readonly (double, double) _parametersL;
        private readonly GeneratorValues generator = new GeneratorValues();
        public string DistributionC
        {
            get { return _distributionC; }
        }
        public string DistributionAB
        {
            get { return _distributionAB; }
        }
        public string DistributionL
        {
            get { return _distributionL; }
        }
        public GeneratorTaskCondition((string, string, string) distribution, (double, double) paramC, (double, double) paramAB, (double, double) paramL)
        {
            _distributionC = distribution.Item1;
            _distributionAB = distribution.Item2;
            _distributionL = distribution.Item3;
            _parametersC = paramC;
            _parametersAB = paramAB;
            _parametersL = paramL;
        }
        public double[] GenerateMatrix(int size)
        {
            int fullSize = size * size;
            double[] matrix = new double[fullSize];
            for (int i = 0; i < fullSize; i++)
            {
                matrix[i] = generator.GetDoubleValue(_distributionC, _parametersC);

            }
            return matrix;
        }
        public (double[], double[]) GenerateAB(int size)
        {
            bool success = false;
            double[] a, b;
            do
            {
                a = new double[size];
                b = new double[size];
                for (int i = 0; i < size; i++)
                {
                    a[i] = generator.GetIntValue(_distributionAB, _parametersAB);
                    if (i < size - 1)
                    {
                        b[i] = generator.GetIntValue(_distributionAB, _parametersAB);
                    }
                }
                double value = a.Sum() - b.Sum();
                if (value > 0)
                {
                    success = true;
                    b[size - 1] = value;
                }
            } while (!success);

            return (a, b);
        }
        public double[] GenerateL(int quantity)
        {
            double[] l = new double[quantity];
            for (int i = 0; i < quantity; i++)
            {
                l[i] = Math.Round(generator.GetDoubleValue(_distributionL, _parametersL));
            }
            return l;
        }
        public double[] GenerateAlpha(int quantity)
        {
            double[] alpha = new double[quantity];
            for (int i = 0; i < quantity; i++)
            {
                alpha[i] = 1;
            }
            return alpha;
        }
        public GeneratorTaskCondition Copy()
        {
            return new GeneratorTaskCondition((_distributionC, _distributionAB, _distributionL), _parametersC, _parametersAB, _parametersL);
        }
    }
}
