using System;
using System.Collections.Generic;


namespace ClassLibrary
{
    public class Experiments
    {
        private Generator generator = new Generator();
        public List<double> GenerateMatrix(int size, string distr, double lambda)
        {
            List<double> matrix = new List<double>();
            for (int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    matrix.Add(generator.GetValue(distr, lambda));
                }
            }
            return matrix;
        }
        public List<double> GenerateMatrix(int size, string distr, int mean, int dev)
        {
            List<double> matrix = new List<double>();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix.Add(generator.GetValue(distr, mean, dev));
                }
            }
            return matrix;
        }
        public void ChangeMatrix(ref List<double> matrix, double percent, string distr)
        {
            int size = (int)Math.Round(Math.Sqrt(matrix.Count));
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    double e = matrix[i * size + j] * (percent / 100);
                    double factE;
                    if(distr == "exp")
                    {
                        factE = generator.GetValue(distr, e);
                    }
                    else
                    {
                        factE = generator.GetValue(distr, 0, e);
                    }
                    matrix[i * size + j] += factE;
                }
            }
        }
        public double IWantToDieSorryIWillFixThisNameForSearchMeanPercentMethod(int size, string distr)
        {
            double average = 0;
            double diff = double.MaxValue;
            int runAmount = 0;
            while (diff > 0.1)
            {
                diff = runAmount != 0 ? average / runAmount : 0;                
                average += SomeMagicMethodForExactPercentOfChange(size, distr);
                runAmount++;
                diff = average / runAmount - diff;
            }
        }
    }
}
