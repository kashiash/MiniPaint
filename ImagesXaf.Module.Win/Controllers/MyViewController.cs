using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImagesXaf.Module.Win.Controllers
{
    public class MyViewController : ViewController<DetailView>
    {
      


        Image mainImage = null;
        bool startPaint = false;
        Graphics graphics;
        //nullable int for storing Null value
        int? initX = null;
        int? initY = null;
        PointF ulCorner;
        XafPictureEdit pEdit;

        protected override void OnActivated()
        {
            base.OnActivated();
            ((CompositeView)View).ItemsChanged += PictureditorController_ItemsChanged;
            TryInitializePictureItem();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            ((CompositeView)View).ItemsChanged -= PictureditorController_ItemsChanged;
        }


        private void PictureditorController_ItemsChanged(Object sender, ViewItemsChangedEventArgs e)
        {
            if (e.ChangedType == ViewItemsChangedType.Added && e.Item.Id == "Photo")
            {
                TryInitializePictureItem();
            }
        }

        public void TryInitializePictureItem()
        {

            ImagePropertyEditor imageEditor = View.FindItem("Photo") as ImagePropertyEditor;
            if (imageEditor != null)
            {
                if (imageEditor.Control != null)
                {
                    InitPhotoEditor(imageEditor);
                }
                else
                {
                    imageEditor.ControlCreated += new EventHandler<EventArgs>(imageEditor_ControlCreated);
                }
            }
        }

        private void imageEditor_ControlCreated(object sender, EventArgs e)
        {
            InitPhotoEditor((ImagePropertyEditor)sender);
        }

        private void InitPhotoEditor(ImagePropertyEditor imageEditor)
        {
            var ctrl = imageEditor.Control;
            pEdit = (XafPictureEdit)ctrl;
            if (pEdit != null)
            {
                pEdit.MouseDown += MouseDown;
                pEdit.MouseUp += MouseUp;
                pEdit.MouseMove += MouseMove;
                pEdit.Invalidated += Invalidated;
                pEdit.LoadCompleted += LoadCompleted;
                pEdit.ImageChanged += ImageChanged;
                pEdit.Resize += Resize;

            }

        }

        private void Resize(object sender, EventArgs e)
        {

            UpdateGraphics();

        }

        private void ImageChanged(object sender, EventArgs e)
        {

            if (graphics == null)
            {
                UpdateGraphics();
            }
        }

        private void UpdateGraphics()
        {
            graphics = pEdit.CreateGraphics();
            mainImage = pEdit.Image;

            ulCorner = new PointF(0, 0);
            if (mainImage != null && graphics != null)
            {
                graphics.DrawImage(mainImage, 0, 0, pEdit.Width, pEdit.Height);
            }
        }

        private void LoadCompleted(object sender, EventArgs e)
        {
            UpdateGraphics();
        }

        private void Invalidated(object sender, InvalidateEventArgs e)
        {
            //  throw new NotImplementedException();
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (startPaint && graphics != null)
            {
                //Setting the Pen BackColor and line Width
                Pen p = new Pen(Color.Red, 3);
                //Drawing the line.
                graphics.DrawLine(p, new Point(initX ?? e.X, initY ?? e.Y), new Point(e.X, e.Y));
                initX = e.X;
                initY = e.Y;
            }
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            // startPaint = false;
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            // startPaint = true;

            //  SolidBrush sb = new SolidBrush(Color.Red);
            Pen pen = new Pen(Color.Red, 3);
            graphics.DrawEllipse(pen, e.X - 50, e.Y - 50, 100, 100);
        }
    }
}
