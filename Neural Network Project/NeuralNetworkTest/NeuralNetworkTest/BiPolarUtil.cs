using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkTest
{
    static class BiPolarUtil
    {
        public static double Bipolar2double(bool bipolar)
        {
            if(bipolar)
            {
                return 1.0f;
            }
            return -1.0f;
        }

        public static double[] Bipolar2double(bool[] bipolar)
        {
            double[] temp = new double[bipolar.Length];
            for (int i = 0; i < bipolar.Length; i++)
            {
                temp[i] = Bipolar2double(bipolar[i]);
            }
            return temp;
        }

        public static double[,] Bipolar2double(bool[,] bipolar)
        {
            double[,] temp = new double[bipolar.GetLength(0), bipolar.GetLength(1)];
            for (int i = 0; i < bipolar.GetLength(0); i++)
            {
                for(int j = 0; j < bipolar.GetLength(1); j++)
                temp[i, j] = Bipolar2double(bipolar[i, j]);
            }
            return temp;
        }

        public static bool Double2bipolar(double number)
        {
            if(number == 1)
            {
                return true;
            }
            return false;
        }

        public static bool[] Double2bipolar(double[] number)
        {
            bool[] temp = new bool[number.Length];
            for(int i = 0; i < number.Length; i++)
            {
                temp[i] = Double2bipolar(number[i]);
            }
            return temp;
        }

        public static bool[,] Double2bipolar(double[,] number)
        {
            bool[,] temp = new bool[number.GetLength(0), number.GetLength(1)];
            for (int i = 0; i < number.GetLength(0); i++)
            {
                for (int j = 0; j < number.GetLength(1); j++)
                {
                    temp[i, j] = Double2bipolar(number[i, j]);
                }
            }
            return temp;
        }
    }
}
