using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetworkTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            // Tests full functionality of the Matrix toolkit
            Console.Write("Generating test data...");

            Matrix m1 = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
            Matrix m1_copy = m1.Clone();
            Matrix m2 = new Matrix(new double[,] { { 3, 4, 5 }, { 5, 7, 8 } });
            Matrix m2_copy = m2.Clone();
            Matrix m3 = new Matrix(new double[,] { { 5, 7 }, { 2, 6 }, { 9, 3 } });

            // Tests equality operators
            Console.WriteLine("Testing operators:");
            Console.WriteLine("Testing equality operators:");
            Console.WriteLine("This line should be TRUE: {0}", (m1 == m1_copy));
            Console.WriteLine("This line should be FALSE: {0}", (m1 == m2));

            // Tests arithmetic
            Console.WriteLine("\nTesting arithmetic operators:");
            Console.WriteLine("Testing multiplication:");
            Matrix m4 = m1_copy * m3;
            m4.PrintContents();
            Console.WriteLine("\nTesting Inversion:");
            m4 = new Matrix(new double[,] { { 3, 0, 2 }, { 2, 0, -2 }, { 0, 1, 1 } });
            Matrix m5 = m4.Invert();
            m5.PrintContents();
            Console.WriteLine("\nTesting Transposition:");
            m5 = m1.Transpose();
            m5.PrintContents();

            // Tests miscellaneous
            Console.WriteLine("\nTesting identity generation:");
            Console.WriteLine("Testing identity for multiplication from left side:");
            m5 = m4.identity(IdentityMult.BEFORE);
            m5.PrintContents();
            Console.WriteLine("Testing identity for multiplication from right side:");
            m5 = m4.identity(IdentityMult.AFTER);
            m5.PrintContents();
            */


            //BarnsleyEngine Engine = new BarnsleyEngine();

            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(Engine));
            */

            NetworkController Net = new NetworkController();
            Net.Test();

        }
    }

    public class BarnsleyEngine
    {
        static BarnsleyFern fern;
        static bool Draw = false;
        static bool IsExitting = false;
        static Thread FernEngine;

        public delegate void myDrawer(List<double[]> coord, long iteration);

        public event myDrawer DrawEvent;

        public BarnsleyEngine()
        {
            // Has to wait for the Form1 to initialize before initializing itself
        }

        public void Initialize(Form1 MainHost)
        {
            MainHost.DrawAbilityEvent += DrawAbility; // Subscribes to Form1's DrawAbilityEvent
            MainHost.IsExittingEvent += Exitting; // Subscribes to Form1's IsExittingEvent

            FernEngine = new Thread(new ThreadStart(Engine)); // Launches the engine.
            FernEngine.Start();
        }

        private void Exitting()
        {
            IsExitting = true;
        }

        // Handler for the DrawAbility Event from Form1
        private void DrawAbility(bool status)
        {
            Draw = status;
        }

        private void Engine()
        {
            fern = new BarnsleyFern();
            long iteration = 0;
            Console.WriteLine("Starting the generator...");
            double[] Coord = new double[2] { 0, 0 }; // Starting Coordinate
            List<double[]> medCoord = new List<double[]>(1);
            List<double[]> medCoordUnscaled = new List<double[]>(1);
            while (!IsExitting)
            {
                Thread.Sleep(500);
                while (Draw)
                {
                    medCoordUnscaled = fern.Generate(Coord);

                    Coord[0] = medCoordUnscaled[0][0];
                    Coord[1] = medCoordUnscaled[0][1];

                    DrawEvent(medCoordUnscaled, iteration++);
                }
            }
        }
    }
}
