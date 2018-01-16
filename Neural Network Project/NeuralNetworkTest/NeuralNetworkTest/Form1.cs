using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace NeuralNetworkTest
{    
    public partial class Form1 : Form
    {
        static Graphics g;
        long memoryDumpCounter;

        public delegate void DrawAbility(bool status); // Delegate for the event that updates the ability to draw
        public delegate void Exitting(); // Delegate to signal when the form is closing

        public event DrawAbility DrawAbilityEvent; // Event for the event that updates the ability to draw
        public event Exitting IsExittingEvent; // Event to signal when the form is closing

        public Form1(BarnsleyEngine Engine)
        {
            InitializeComponent();

            StatusBar.Text = "Busy!";

            // Initializes drawing tools
            g = Display.CreateGraphics();
            Engine.DrawEvent += DrawPoint; // Subscribes to the Barnsley Engine's draw event
            Engine.Initialize(this);

            StatusBar.Text = "Ready!";
        }

        private void DrawPoint(List<double[]> coord, long iteration)
        {
            List<double[]> medCoord = BarnsleyFern.ScaleToDisplay(coord,
                        Display.ClientSize.Height, Display.ClientSize.Width / 2,
                        Display.ClientSize.Width / 4, 0);

            try
            {
                g.DrawEllipse(Pens.Black, new Rectangle((int)medCoord[0][0], (int)medCoord[0][1], 1, 1));
            }
            catch (Exception)
            {
                Console.WriteLine("Memory Overflow...");
            }

            if (++memoryDumpCounter > 20000)
                SaveToFile(); // Prevents running out of RAM
        }

        private void SaveToFile()
        {
            g.Save();
            memoryDumpCounter = 0;
        }

        private void Display_Click(object sender, EventArgs e)
        {

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            StatusBar.Text = "Running...";
            DrawAbilityEvent(true);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Application.DoEvents();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            StatusBar.Text = "Ready!";
            DrawAbilityEvent(false);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            DrawAbilityEvent(false);
            IsExittingEvent();
            Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Image img = Display.Image;
            img.Save("C:\\Barnsley Ferns\\"+DateTime.Now+".png", System.Drawing.Imaging.ImageFormat.Png );
        }
    }
}
