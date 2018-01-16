using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkTest
{
    class BarnsleyFern
    {
        // Data
        private double[] stem;
        private double[] leaflets;
        private double[] leftLeaf;
        private double[] rightLeaf;
        private Dictionary<string, double> probabilities;
        private Random rnd = new Random();
        private Matrix Coord;

        // Accessor Methods
        public double[] Stem { get => stem; set => stem = value; }
        public double[] Leaflets { get => leaflets; set => leaflets = value; }
        public double[] LeftLeaf { get => leftLeaf; set => leftLeaf = value; }
        public double[] RightLeaf { get => rightLeaf; set => rightLeaf = value; }
        public Dictionary<string, double> Probabilities { get => probabilities; set => probabilities = value; }

        // Constructors
        public BarnsleyFern()
        {
            stem = new double[6] {0, 0, 0, 0.16, 0, 0 };
            leaflets = new double[6] { 0.85, 0.04, -0.04, 0.85, 0, 1.6 };
            leftLeaf = new double[6] { 0.2, -0.26, 0.23, 0.22, 0, 1.6 };
            rightLeaf = new double[6] { -0.15, 0.28, 0.26, 0.24, 0, 0.44 };
            probabilities = new Dictionary<string, double>(4)
            {
                { "stem", 1 },
                { "leaflets", 85 },
                { "leftLeaf", 7 },
                { "rightLeaf", 7 }
            };
            Console.WriteLine("Setup a new Barnsley Fern, with the default settings.");
        }

        public BarnsleyFern(double[] stem = null, double[] leaflets = null, double[] leftLeaf = null, double[] rightLeaf = null, double[] probabilities = null)
        {
            Console.WriteLine("Setting up a new Barnsley Fern, with the unique settings:\nStem Matrix:");
            if (stem == null)
                this.stem = new double[6] { 0, 0, 0, 0.16, 0, 0 };
            else
                this.stem = stem;
            Console.WriteLine("Leaflets Matrix:");
            if (leaflets == null)
                this.leaflets = new double[6] { 0.85, 0.04, -0.04, 0.85, 0, 1.6 };
            else
                this.leaflets = leaflets;
            Console.WriteLine("LeftLeaflet Matrix:");
            if (leftLeaf == null)
                this.leftLeaf = new double[6] { 0.2, -0.26, 0.23, 0.22, 0, 1.6 };
            else
                this.leftLeaf = leftLeaf;
            Console.WriteLine("RightLeaflet Matrix:");
            if (rightLeaf == null)
                this.rightLeaf = new double[6] { -0.15, 0.28, 0.26, 0.24, 0, 0.44 };
            else
                this.rightLeaf = rightLeaf;
            this.probabilities = new Dictionary<string, double>(4);
            if ((probabilities == null) || (probabilities.Length != 4))
            {
                Console.WriteLine("Invalid probability list supplied, default will be used.");
                this.probabilities.Add("stem", 1);
                this.probabilities.Add("leaflets", 85);
                this.probabilities.Add("leftLeaf", 7);
                this.probabilities.Add("rightLeaf", 7);
            }
            else
            {
                this.probabilities.Add("stem", probabilities[0]);
                this.probabilities.Add("leaflets", probabilities[1]);
                this.probabilities.Add("leftLeaf", probabilities[2]);
                this.probabilities.Add("rightLeaf", probabilities[3]);

                Console.WriteLine("\nProbabilities:\nStem: {0}\nLeaflets: {1}\nLeftLeaflet: {2}\nRightLeaflet: {3}",
                    this.probabilities["stem"], this.probabilities["leaflets"], this.probabilities["leftLeaf"], this.probabilities["rightLeaf"]);
            }
        }

        // methods

        public static List<double[]> ScaleToDisplay(List<double[]> Calculations,
            int Height, int Width,
            int XOrigin, int YOrigin,
            double XMax = 2.6558, double XMin = -2.1820, double YMax = 9.9983, double YMin = 0)
        {
            List<double[]> temp = new List<double[]>(Calculations.Count);

            double tempX, tempY;
            foreach(double[] item in Calculations)
            {
                /*
                 * if y=mx+b where 0 <= x <= 1
                 * then:
                 * m = largest value y can be +b
                 * b = offset from zero
                 */
                tempX = (item[0] - XMin) / XMax; // Converts the points into percentages from 0 - 1
                tempY = (item[1] - YMin) / YMax;

                tempX = tempX * (Width - XOrigin) + XOrigin; // Applies the newly found percantage to the scaled y=mx+b equation
                tempY = tempY * (Height - YOrigin) + YOrigin;

                // Turn back into a point and ship it
                temp.Add(new double[2] { tempX, tempY });
            }

            return temp;
        }

        public List<double[]> Generate(double[] Coord, int iterations = 1)
        {
            // calculates a series of points for the fern.
            //Console.WriteLine("Generating {0} points!", iterations);
            this.Coord = Matrix.CreateColumnMatrix(Coord); // Resets the coordinate vector

            List<double[]> temp = new List<double[]>(iterations);
            List<double[]> rangeData = GetProbabilityRanges();
            Matrix[] CalculationMatrices = new Matrix[2] { new Matrix(2, 2), Matrix.CreateColumnMatrix(2) };
            int randNum = 0;

            for(int i = 0; i < iterations; i++)
            {
                randNum = rnd.Next(1, (int)rangeData[3][1]); // Rolls the dice
                if(randNum >= 0 && randNum <= rangeData[0][1])
                {
                    // Generating a stem piece
                    CalculationMatrices = SetupMatrices(Stem); // Sets up the matrices
                }
                else if (randNum > rangeData[1][0] && randNum <= rangeData[1][1])
                {
                    // Generating a leaflet piece
                    CalculationMatrices = SetupMatrices(Leaflets); // Sets up the matrices
                }
                else if (randNum > rangeData[2][0] && randNum <= rangeData[2][1])
                {
                    // Generating a leftLeaf piece
                    CalculationMatrices = SetupMatrices(LeftLeaf); // Sets up the matrices
                }
                else if (randNum > rangeData[3][0] && randNum <= rangeData[3][1])
                {
                    // Generating a rightLeaf piece
                    CalculationMatrices = SetupMatrices(RightLeaf); // Sets up the matrices
                }
                // Performs the actual calculation
                this.Coord = (CalculationMatrices[0] * this.Coord) + CalculationMatrices[1];

                temp.Add(new double[2] { this.Coord[0, 0], this.Coord[1, 0] }); // Adds the new value to the list
            }
            return temp;
        }

        private Matrix[] SetupMatrices(double[] data)
        {
            // Places the data into the appropriate matrix locations for the calculation
            return new Matrix[2]
            {
                new Matrix(new double[2,2]{ { data[0], data[1] }, { data[2], data[3] } }),
                Matrix.CreateColumnMatrix(new double[2]{ data[4], data[5] })
            };
        }

        private List<double[]> GetProbabilityRanges()
        {
            // Returns the ranges of integers that the rnd needs to generate in order to trigger one of the four points
            List<double[]> temp = new List<double[]>(4);
            double rangeStart = 0;
            double rangeEnd = 0;
            string stringTemp = "";
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case (0):
                        stringTemp = "stem";
                        break;

                    case (1):
                        stringTemp = "leaflets";
                        break;

                    case (2):
                        stringTemp = "leftLeaf";
                        break;

                    case (3):
                        stringTemp = "rightLeaf";
                        break;
                }
                rangeEnd = rangeStart + (probabilities[stringTemp]);
                temp.Add(new double[2] { rangeStart, rangeEnd });
                rangeStart = rangeEnd;
            }
            return temp;
        }
    }
}
