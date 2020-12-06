using System;

namespace ClassLibrary
{
    public class Generator
    {
        private static Random rand = new Random();
        public double GetValue(string distr, double lambda)
        {
            return Exponential(lambda);
        }
        public double GetValue(string distr, double mean, double dev)
        {
            switch (distr)
            {
                case "norm":
                    return Normal(mean, dev);
                case "unif":
                    return Uniform(mean - dev, mean + dev);
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
            return rand.NextDouble()*(max - min) + min;
        }
    }
}
