using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkTest
{
    class NetworkController
    {
        int n_hidden = 10;  // How many values we're going to input into our network
        int n_in = 10;      // How many points we're going to put into our network
        int n_out = 10;     // How many points we want out of the network.

        int num_sample = 300;

        double learning_rate = 0.01;
        double momentum = 0.9;

        List<Matrix> Layers;
        List<Matrix> Biases;
        Matrix Samples;
        Matrix TransposeSamples;
        Matrix[] Params;

        public NetworkController()
        {
            Layers = BuildLayers(2, new List<Tuple<int, int>> { new Tuple<int, int>(n_in, n_hidden), new Tuple<int, int>(n_hidden, n_out)});
            Biases = new List<Matrix> { Matrix.CreateColumnMatrix(n_hidden), Matrix.CreateRowMatrix(n_out) };
            Samples = myMath.BinomialRandMatrix(new Tuple<int, int>(num_sample, n_in));
            TransposeSamples = Samples.Transpose();
            Params = new Matrix[4];
        }

        public void Test()
        {
            // Training time!!!
            Params = new Matrix[4] { Layers[0], Layers[1], Biases[0], Biases[1]};
            for(int i = 0; i < 100; i++)
            {
                Matrix[] upd = new Matrix[4];
                Matrix[] tert = new Matrix[5];
                List<double> err = new List<double>(n_in);
                for (int j = 0; j < n_in; j++)
                {
                    tert = train(Matrix.CreateColumnMatrix(Samples.ColData()[j].ToArray()), 
                        Matrix.CreateColumnMatrix(TransposeSamples.ColData()[j].ToArray()),
                        Params[0], Params[1], Params[2], Params[3]);

                    for(int k = 0; k < 4; k++)
                    {
                        Params[k] -= upd[k];
                        upd[k] = learning_rate * tert[k+1] + momentum * upd[k+1];
                    }
                    err.Add(tert[0][0, 0]);
                }
                Console.WriteLine("Epoch: {0}, Loss: {1}, Time {2}", i, myMath.Avg(err.ToArray()), DateTime.Now.TimeOfDay);
            }

            // Try to predict something
            Samples = Matrix.CreateRowMatrix(myMath.BinomialRandMatrix(n_in).ToArray());
            Console.WriteLine("XOR prediction:");
            Samples.PrintContents();
            predict(Samples, Params[0], Params[1], Params[2], Params[3]).PrintContents();
        }

        public Matrix[] train(Matrix x, Matrix t, Matrix Vlayer, Matrix Wlayer, Matrix biasV, Matrix biasW)
        {
            // Forward Propagation
            Matrix A = (x * Vlayer) + biasV;
            Matrix Z = myMath.tanh_prime(A);

            Matrix B = (Z * Wlayer) + biasW;
            Matrix Y = myMath.sigmoid(B);

            // Backwards propagation
            Matrix Ew = Y - t;
            Matrix Ev = Matrix.Mult(myMath.tanh_prime(A), (Wlayer * Ew));

            // Predict the loss
            Matrix dW = Matrix.Outer(Z.ToPackedArray(), Ew.ToPackedArray());
            Matrix dV = Matrix.Outer(x.ToPackedArray(), Ev.ToPackedArray());

            // Cross Entropy
            double loss = -myMath.Avg((Matrix.Mult(t, myMath.log(Y)) + Matrix.Mult((1 - t), myMath.log(1 - Y))).ToPackedArray());

            return new Matrix[] { new Matrix(1, 1, loss), dV, dW, Ev, Ew };
        }

        public Matrix predict(Matrix X, Matrix Vlayer, Matrix Wlayer, Matrix biasV, Matrix biasW)
        {
            Matrix A = (X * Vlayer) + biasV;
            Matrix B = (myMath.tanh_prime(A) * Wlayer) + biasW;
            return (myMath.sigmoid(B) > 0.5);
        }

        public List<Matrix> BuildLayers(int num, List<Tuple<int, int>> Sizes)
        {
            List<Matrix> temp = new List<Matrix>(num);
            for(int i = 0; i < num; i++)
            {
                temp.Add(myMath.NormalRandMatrix(new Tuple<int, int>(Sizes[i].Item1, Sizes[i].Item2), scale: 0.1));
            }
            return temp;
        }
    }
}
