using static System.Windows.Forms.DataFormats;
using System.Security.Cryptography;
using System.Diagnostics.Metrics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Security.Cryptography.Xml;


namespace Homework_4
{
    public partial class Form1 : Form
    {
        Random r;
        Bitmap b, b1;
        Graphics g, g1;
        Pen pen1 = new Pen(Color.IndianRed, 2);
        Pen pen2 = new Pen(Color.LightBlue, 2);
        Pen pen3 = new Pen(Color.Olive, 2);
        Pen pen = new Pen(Color.Black, 2);
        Brush brush1 = new SolidBrush(Color.IndianRed); 
        Brush brush2 = new SolidBrush(Color.LightBlue);
        Brush brush3 = new SolidBrush(Color.Olive); 
        List<Point> points; 
        List<Rectangle> rectangles;
        Rectangle virtualWindow;
        double treshold = 0.2;
        int trialCount = 100;
        int repeat = 20;
        int step = 3;
        double realStep; 
        public Form1()
        {
            InitializeComponent();
            r = new Random();
            timer1.Interval = 500;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
           // richTextBox1.AppendText(values.ElementAt(random.Next() % values.Count()) + Environment.NewLine);
        }

        
        // ABSOLUTE FREQUENCY
        private void button1_Click(object sender, EventArgs e)
        {

            // useful if the button is pressed more than once
            //richTextBox2.Clear();  
 

            // graphic 1
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Bitmap b1 = new Bitmap(pictureBox2.Width, pictureBox2.Height); 

            g = Graphics.FromImage(b);
            Graphics g1 = Graphics.FromImage(b1);
            g1.Clear(Color.White); 
            

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White); // Background color
            virtualWindow = new Rectangle(20, 20, b.Width - 40, b.Height - 40);
            g.DrawRectangle(Pens.Black, virtualWindow);

            parseInput();
            textBox5.Text = textBox2.Text + " / " + trialCount.ToString() + " = " + treshold;

            double d; // double
            int y = 0; // number of successes (face up coin)

            List<Point> points = new List<Point>();
            rectangles = new List<Rectangle>();

            double interval = Math.Ceiling((double) trialCount / step); 
            int[] count = new int[step];
            int i = 0;
            while (i < step)
            {
                count[i] = 0;
                i++; 
            }

            i = 0; 
            int minX = 0;
            int maxX = trialCount;
            int minY = 0;
            int maxY = trialCount;

            
            points = new List<Point>();


            y = 0;

            /* // output
            richTextBox2.AppendText(
                    "Results of " + trialCount.ToString() + " coin tosses" + Environment.NewLine +
                    "Success: Head" + Environment.NewLine +
                    "----------------------------" + Environment.NewLine
            );
            */ 

            for (int j = 0; j < repeat; j++)
            {
                y = 0;
                points = new List<Point>();

                for (int x = 1; x <= trialCount; x++)
                {
                    d = r.NextDouble();
                    if (d <= treshold)
                    {
                        y++;
                    }

                    // GRAPH 1: points to plot ABSOLUTE frequency 
                    int xDevice1 = fromXRealToXVirtual(x, minX, maxX, virtualWindow.Left, virtualWindow.Width);
                    int yDevice1 = fromYRealToYVirtual(y, minY, maxY, virtualWindow.Top, virtualWindow.Height);
                    points.Add(new Point(xDevice1, yDevice1));
                }

                // ISTOGRAM 1: points to plot the istrogram 1   
                count[(int) ((double) y / interval)]++; // increments the current step 
                

                // Summary of the extractions
                /*
                richTextBox2.AppendText(
                    "----------------------------" + Environment.NewLine +
                    "Extraction n." + j.ToString() + Environment.NewLine +
                    "Number of HEADS: " + y + Environment.NewLine +
                    "Number of TAILS: " + (trialCount - y) + Environment.NewLine
                    );
                */

                // Draw the chart
                g.DrawLines(pen1, points.ToArray());
                pictureBox1.Image = b;

                int kk = 1;
                // Draw the Histogram composed by little blocks
                /*
                foreach(int k in count)
                {
                    
                    int wr = fromXRealToXVirtual(k, 0, repeat, 0, b1.Width);
                    int sr = fromXRealToXVirtual(interval, minX, maxX, 0, b1.Height);
                    int yr = fromYRealToYVirtual(kk*interval, minY, maxY, 0, b1.Height); 
                    rectangles.Add(new Rectangle(0, yr, wr, sr));
                    kk++; 
                }
                kk = 1;
                g1.FillRectangles(brush1, rectangles.ToArray());
                g1.DrawRectangles(pen, rectangles.ToArray());
                pictureBox2.Image = b1; 
                */
            }

            // draw the histogram with entires blocks and the horizontal scale refers to the maximum value
            int kkk = 1; 
            foreach (int k in count)
            {

                int wr = fromXRealToXVirtual(k, 0, count.Max(), 0, b1.Width);
                int sr = fromXRealToXVirtual(interval, minX, maxX, 0, b1.Height);
                int yr = fromYRealToYVirtual(kkk * interval, minY, maxY, 0, b1.Height);
                rectangles.Add(new Rectangle(0, yr, wr, sr));
                kkk++;
            }
            kkk = 1;
            g1.FillRectangles(brush1, rectangles.ToArray());
            g1.DrawRectangles(pen, rectangles.ToArray());
            pictureBox2.Image = b1;
        }
        

        private int fromXRealToXVirtual(double x, int minX, double maxX, int left, int w)
        {
            return (int)(left + w * (x - minX) / (maxX - minX)); // left --> traslazione grafico
        }

        private int fromYRealToYVirtual(double y, int minY, double maxY, int top, int h)
        {
            return (int)(top + h - h * (y - minY) / (maxY - minY)); // top --> traslazione grafico 
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void parseInput()
        {

            try
            {
                trialCount = Int32.Parse(textBox1.Text); // number of conin tosses (for each experiment)
            }
            catch (Exception ec)
            {
                // in case of invalid input we force a valid integer
                trialCount = 200; // number of coin tosses 
                textBox1.Text = trialCount.ToString();
            }
            try
            {
                if(Int32.Parse(textBox2.Text) > trialCount)
                {
                    treshold = (50 / trialCount);
                    textBox2.Text = 50.ToString();
                }
                treshold = ((double)Int32.Parse(textBox2.Text)) / trialCount;

            }
            catch (Exception ex)
            {
                treshold = 50 / trialCount;
                textBox2.Text = 50.ToString();
            }
            try
            {
                repeat = Int32.Parse(textBox3.Text); // number of repetitions
            }
            catch (Exception ex)
            {
                repeat = 1500;
                textBox3.Text = repeat.ToString();
            }
            try
            {
                step = trackBar1.Value; 
            }
            catch (Exception ex)
            {
                step = 300;
            }
            if(step > trialCount)
            {
                step = trialCount;
                textBox1.Text = trialCount.ToString();
            }
            return; 
        }

       
    }
}