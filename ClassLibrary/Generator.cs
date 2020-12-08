using System;

namespace ClassLibrary
{
    public class Generator
    {
        private static Random rand = new Random();
        public int GetIntValue(string distr, (double, double) parameters)
        {
            return (int)Math.Round(GetDoubleValue(distr, parameters));
        }
        public double GetDoubleValue(string distr, (double, double) parameters)
        {
            switch (distr)
            {
                case "exp":
                    return Exponential(parameters.Item1);
                case "norm":
                    return Normal(parameters.Item1, parameters.Item2);
                case "unif":
                    return Uniform(parameters.Item1, parameters.Item2);
            }
            return double.MinValue;
        }
        public double Exponential(double lambda)
        {
            return -Math.Log(rand.NextDouble()) / lambda;
        }
        public double Normal(double mean, double deviation)
        {
            double mu = 0;
            for (int j = 0; j < 12; j++)
            {
                mu += rand.NextDouble();
            }
            mu -= 6;
            return deviation * mu + mean;
        }
        public double Uniform(double min, double max)
        {
            return rand.NextDouble() * (max - min) + min;
        }
    }
}
