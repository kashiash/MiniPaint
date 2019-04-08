using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MiniPaint
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            g = pnl_Draw.CreateGraphics();

            // Create image.
             mainImage = Image.FromFile(@"C:\Users\jkosinski\Pictures\pantogram1.jpg");

            // Create point for upper-left corner of image.
             ulCorner = new PointF(0, 0);
        }
        bool startPaint = false;
        Graphics g;
        //nullable int for storing Null value
        int? initX = null;
        int? initY = null;

        bool drawSquare = false;
        bool drawRectangle = false;
        bool drawCircle = false;

        Image mainImage = null;
        PointF ulCorner;

        //Event fired when the mouse pointer is moved over the Panel(pnl_Draw).
        private void pnl_Draw_MouseMove(object sender, MouseEventArgs e)
        {
            if(startPaint)
            {
                //Setting the Pen BackColor and line Width
                Pen p = new Pen(btn_PenColor.BackColor,float.Parse(cmb_PenSize.Text));
                //Drawing the line.
                g.DrawLine(p, new Point(initX ?? e.X, initY ?? e.Y), new Point(e.X, e.Y));
                initX = e.X;
                initY = e.Y;
            }

            if (drawSquare)
            {
                if (initX == null)
                initX = e.X;
                if (initY == null)
                initY = e.Y;

                int width = e.X - (int)initX;

                int height = e.Y - (int)initY;
                //Use Solid Brush for filling the graphic shapes
                Pen pen = new Pen(Color.Red,3);
                //setting the width and height same for creating square.
                //Getting the width and Heigt value from Textbox(txt_ShapeSize)
                g.DrawImage(mainImage, ulCorner);
                g.DrawRectangle(pen, (int)initX, (int)initY, width, height);
                //setting startPaint and drawSquare value to false for creating one graphic on one click.
                startPaint = false;
          
            }
        }
        //Event Fired when the mouse pointer is over Panel and a mouse button is pressed
        private void pnl_Draw_MouseDown(object sender, MouseEventArgs e)
        {
            startPaint = true;
            if (drawSquare)
            {
                initX = e.X;
                initY = e.Y;
                startPaint = false;
           
            }
            if(drawRectangle)
            {
                SolidBrush sb = new SolidBrush(btn_PenColor.BackColor);
                //setting the width twice of the height
                g.FillRectangle(sb, e.X, e.Y, 2*int.Parse(txt_ShapeSize.Text), int.Parse(txt_ShapeSize.Text));
                startPaint = false;
                drawRectangle = false;
            }
            if (drawCircle)
            {
                initX = e.X;
                initY = e.Y;
                startPaint = false;
            }

        }
        //Fired when the mouse pointer is over the pnl_Draw and a mouse button is released.
        private void pnl_Draw_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawCircle)
            {
                int endX = e.X;
                int endY = e.Y;
                Pen pen = new Pen(btn_PenColor.BackColor);
             //   SolidBrush sb = new SolidBrush(btn_PenColor.BackColor);
                g.DrawEllipse(pen, (int)initX, (int)initY, (int)initX + e.X, (int)initY + e.Y);
                startPaint = false;
                drawCircle = false;
            }

            if (drawSquare)
            {

                int width = e.X - (int)initX ;

                int height = e.Y - (int)initY;
                //Use Solid Brush for filling the graphic shapes
                Pen pen = new Pen(btn_PenColor.BackColor);
                //setting the width and height same for creating square.
                //Getting the width and Heigt value from Textbox(txt_ShapeSize)
                g.DrawRectangle(pen, (int)initX, (int)initY, width, height);
                //setting startPaint and drawSquare value to false for creating one graphic on one click.
               
                CaptureScreen(e.X,e.Y);
                startPaint = false;
                drawSquare = false;
            }

            startPaint = false;
            initX = null;
            initY = null;
        }

        private void CaptureScreen(int pX,int pY)
        {

            int width = pX - (int)initX;

            int height = pY - (int)initY;

            using (Bitmap newImage = new Bitmap(200, 120))
            {

                // Crop and resize the image.
                Rectangle destination = new Rectangle(0, 0, 200, 120);
                using (Graphics graphic = Graphics.FromImage(newImage))
                {
                    graphic.DrawImage(mainImage, destination, (int)initX, (int)initY, width, height, GraphicsUnit.Pixel);
                }
                //   newImage.Save(AppDomain.CurrentDomain.BaseDirectory + @"c:\apps\castle_icon.jpg", ImageFormat.Jpeg);
                Clipboard.SetImage(newImage);
                mainImage = newImage;
                g.DrawImage(mainImage, ulCorner);
            }
   

        }


        public static void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, ref Bitmap destBitmap, Rectangle destRegion)
        {
            using (Graphics grD = Graphics.FromImage(destBitmap))
            {
                grD.DrawImage(srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);
            }
        }


        //Button for Setting pen Color
        private void button1_Click(object sender, EventArgs e)
        {
            //Open Color Dialog and Set BackColor of btn_PenColor if user click on OK
            ColorDialog c = new ColorDialog();
            if(c.ShowDialog()==DialogResult.OK)
            {
                btn_PenColor.BackColor = c.Color;
            }
        }
        //New 
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Clearing the graphics from the Panel(pnl_Draw)
            g.Clear(pnl_Draw.BackColor);
            //Setting the BackColor of pnl_draw and btn_CanvasColor to White on Clicking New under File Menu
            pnl_Draw.BackColor = Color.White;
            btn_CanvasColor.BackColor = Color.White;
        }
       //Setting the Canvas Color
        private void btn_CanvasColor_Click_1(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if(c.ShowDialog()==DialogResult.OK)
            {
                pnl_Draw.BackColor = c.Color;
                btn_CanvasColor.BackColor = c.Color;
            }
        }

        private void btn_Square_Click(object sender, EventArgs e)
        {
            drawSquare = true;
        }

        private void btn_Rectangle_Click(object sender, EventArgs e)
        {
            drawRectangle = true;
        }

        private void btn_Circle_Click(object sender, EventArgs e)
        {
            drawCircle = true;
        }
        //Exit under File Menu
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you want to Exit?","Exit",MessageBoxButtons.YesNo,MessageBoxIcon.Information)==DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        //About under Help Menu
        private void aboutMiniPaintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {


            // Draw image to screen.
            g.DrawImage(mainImage, ulCorner);
        }
    }
}
