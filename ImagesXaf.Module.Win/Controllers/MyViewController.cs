using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImagesXaf.Module.Win.Controllers
{
    public class MyViewController : ViewController<DetailView>
    {
        protected override void OnActivated()
        {
            base.OnActivated();

            foreach (ViewItem item in ((DetailView)View).Items)
            {
                if ((item is ControlViewItem) & item.Id == "Item1") { /*...*/ }
            }


            ImagePropertyEditor imageEditor = View.FindItem("Photo") as ImagePropertyEditor;
            

            if (imageEditor != null)
            {
                imageEditor.ControlCreated += (s, e) =>
                    {

                        ImagePropertyEditor cos = (ImagePropertyEditor)s;
                        var cos2 = e;
                        var ctrl = ((ImagePropertyEditor)s).Control;
                        XafPictureEdit pEdit = (XafPictureEdit)ctrl;
                        //     XafImageEdit iEdit = (XafImageEdit)ctrl;

                        pEdit.MouseDown += MouseDown;
                        pEdit.MouseUp += MouseUp;
                        pEdit.MouseMove += MouseMove;
                    //    pEdit.ControlsCreated += (s1, e1) => { }
                        //{
                        //    GridListEditor gridListEditor = ((ListView)s1).Editor as GridListEditor;
                        //    if (gridListEditor != null)
                        //    {
                        //        GridControl grid = gridListEditor.Grid;
                        //    }
                        //};
                    };

         
            }

        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
