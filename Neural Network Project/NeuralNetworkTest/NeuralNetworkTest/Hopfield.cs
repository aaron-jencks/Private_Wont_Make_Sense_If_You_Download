using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkTest
{
    class Hopfield
    {
        // Has only a single layer
        // No hidden layers

        // Trained to recognize 0101 and 1010
        // when the network is trained to recognize one binary pattern, it automatically recognizes the inverse
        // To train multiple pattern, you just add the matrices

        public Matrix LayerMatrix;
        public int Size; // How many neurons are used
        public Matrix weightMatrix;

        public Hopfield(int size)
        {
            Size = size;
            weightMatrix = new Matrix(size, size);
        }

        public bool[] Present(bool[] pattern)
        {
            bool[] output = new bool[pattern.Length];
            Matrix inputMatrix = Matrix.CreateRowMatrix(BiPolarUtil.Bipolar2double(pattern));
            for(int col = 0; col < pattern.Length; col++)
            {
                Matrix columnMatrix = Matrix.CreateRowMatrix(weightMatrix.GetCol(col).ToArray());
                double dotProduct = Matrix.Dot(inputMatrix.RowData()[0].ToArray(), columnMatrix.RowData()[0].ToArray());
                if (dotProduct > 0)
                    output[col] = true;
                else
                    output[col] = false;

            }
            return output;
        }
        
        public void Train(bool[] pattern)
        {
            Matrix m2 = Matrix.CreateRowMatrix(BiPolarUtil.Bipolar2double(pattern));
            Matrix m1 = m2.Clone().Transpose();
            Matrix m3 = m1 * m2;
            Matrix identity = m3.Identity(IdentityMult.BEFORE);
            m3 -= identity;
            weightMatrix += m3;
        }
    }
}
