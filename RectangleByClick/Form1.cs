using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace RectangleByClick
{
    public partial class Form1 : Form
    {

        int x, y,counter;
        bool IsLeftKeyPressed = false;
        List<RectangleF> rectangles;
        RectangleF selection;
        bool selected = false;
       
       
       

        public Form1()
        {
            InitializeComponent();
            rectangles = new List<RectangleF>();
            this.Width = 700;
            this.Height = 700;
            
        }
        private void refreashForm()
        {
            Brush brush = Brushes.Red;
            Graphics g = this.CreateGraphics();
            g.Clear(BackColor);
            if (rectangles.Count > 0)
            {
                if (counter % 2 == 0)
                {
                    
                    g.FillRectangles(brush, rectangles.ToArray());
                }
                else
                    brush = Brushes.Green;
                g.FillRectangles(brush, rectangles.ToArray());
            }
        }
        private void addNewRectangle(RectangleF rectangle)
        {
            bool IsIntersect = false;
            foreach (var item in rectangles)
            {
                if (item.IntersectsWith(rectangle))
                {
                    IsIntersect = true;
                }
            }
            if (!IsIntersect)
            {
                rectangles.Add(rectangle);
                addRectangle(rectangle);
            }
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var rectangle = new RectangleF(e.X - 25, e.Y - 25, 50, 50);
                addNewRectangle(rectangle);
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    IsLeftKeyPressed = true;
                    this.x = e.X;
                    this.y = e.Y;
                    foreach (var item in rectangles)
                    {
                        if (item.Contains(new PointF(x, y)))
                        {
                            selected = true;
                        }
                    }
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsLeftKeyPressed)
            {
                
                refreashForm();
                int x1, y1, x2, y2;
                x1 = (x > e.X) ? e.X : x;
                y1 = (y > e.Y) ? e.Y : y;
                x2 = (x <= e.X) ? e.X : x;
                y2 = (y <= e.Y) ? e.Y : y;
                if (!selected)
                {
                    var rectangle = new RectangleF(x1, y1, x2 - x1, y2 - y1);
                    var pen_foreColor = new Pen(Color.DarkRed, 2);
                    addMovingRectangle(pen_foreColor, rectangle);
                }
                else
                {
                    var rectangle = new RectangleF(e.X - 25, e.Y - 25, 50, 50);
                    foreach (var item in rectangles)
                    {
                        if (item.IntersectsWith(rectangle))
                     
                        {
                            rectangles.Remove(item);
                            break; 
                        }
                    }
                    addNewRectangle(rectangle);
                }
            }

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            IsLeftKeyPressed = false;
            selected = false;
       
            for (int i = 0; i < rectangles.Count; i++)
            {
                if (selection.IntersectsWith(rectangles[i]))
                {
                    rectangles.Remove(rectangles[i]);
                    i--;
                }
            }
            selection = RectangleF.Empty; 
            refreashForm();
        }
        private void addRectangle(RectangleF rectangle)
        {
           
            refreashForm();
      
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(new Point(this.Location.X, this.Location.Y), new Point(0, 0), this.Size);
                    }
            saveFileDialog1.Title = "Сохранить картинку как ...";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.Filter =
                "Bitmap File(*.bmp)|*.bmp|" +
                "GIF File(*.gif)|*.gif|" +
                "JPEG File(*.jpg)|*.jpg|" +
                "TIF File(*.tif)|*.tif|" +
                "PNG File(*.png)|*.png";
            saveFileDialog1.ShowHelp = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(saveFileDialog1.FileName);
                
            }

        }

        

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                counter++;
            }
        }

        private void addMovingRectangle(Pen pen, RectangleF rectangle)
        {
            refreashForm();
            Graphics g = this.CreateGraphics();
            g.DrawRectangle(pen,
                rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            selection = rectangle;
        }
    }
}


