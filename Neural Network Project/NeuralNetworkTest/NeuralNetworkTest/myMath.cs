using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random;

namespace NeuralNetworkTest
{
    static class myMath
    {
        public static double sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public static Matrix sigmoid(Matrix m1)
        {
            // Performs the mathematical operation of adding a constant to a matrix
            Matrix temp = new Matrix();
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = 1 / (1 + Math.Exp(-m1[i, j]));
                }
            }
            return temp;
        }

        public static double tanh_prime(double x)
        {
            return 1 - Math.Pow(Math.Tanh(x), 2);
        }

        public static Matrix tanh_prime(Matrix m1)
        {
            // Performs the mathematical operation of adding a constant to a matrix
            Matrix temp = new Matrix();
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = 1 - Math.Pow(Math.Tanh(m1[i, j]), 2);
                }
            }
            return temp;
        }

        public static double Avg(double[] x)
        {
            return x.Sum() / x.Length;
        }

        public static Matrix log(Matrix m1)
        {
            // Performs the mathematical operation of adding a constant to a matrix
            Matrix temp = new Matrix();
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = Math.Log(m1[i, j]);
                }
            }
            return temp;
        }

        public static List<double> NormalRandMatrix(int size, double loc = 0, double scale = 1)
        {
            NormalDistribution rnd = new NormalDistribution();
            rnd.Mu = loc;
            rnd.Sigma = scale;
            List<double> temp = new List<double>(size);
            double layer;
            for(int i = 0; i < size; i++)
            {
                layer = rnd.NextDouble();
                temp.Add(layer);
            }
            return temp;
        }

        public static Matrix NormalRandMatrix(Tuple<int, int> size, double loc = 0, double scale = 1)
        {
            NormalDistribution rnd = new NormalDistribution();
            rnd.Mu = loc;
            rnd.Sigma = scale;
            Matrix temp = new Matrix(size.Item1, size.Item2);
            for(int i = 0; i < size.Item1; i++)
            {
                for(int j = 0; j < size.Item2; j++)
                {
                    temp[i, j] = rnd.NextDouble();
                }
            }
            return temp;
        }

        public static List<double> BinomialRandMatrix(int size, int trials = 1, double p = 0.5)
        {
            BinomialDistribution rnd = new BinomialDistribution();
            rnd.Alpha = p;
            rnd.Beta = trials;
            List<double> temp = new List<double>(size);
            double layer;
            for (int i = 0; i < size; i++)
            {
                layer = rnd.NextDouble();
                temp.Add(layer);
            }
            return temp;
        }

        public static Matrix BinomialRandMatrix(Tuple<int, int> size, int trials = 1, double p = 0.5)
        {
            BinomialDistribution rnd = new BinomialDistribution();
            rnd.Alpha = p;
            rnd.Beta = trials;
            Matrix temp = new Matrix(size.Item1, size.Item2);
            for (int i = 0; i < size.Item1; i++)
            {
                for (int j = 0; j < size.Item2; j++)
                {
                    temp[i, j] = rnd.NextDouble();
                }
            }
            return temp;
        }
    }
}
