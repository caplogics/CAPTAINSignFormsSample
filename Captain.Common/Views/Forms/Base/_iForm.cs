using DevExpress.Drawing.Internal.Fonts.Interop;
using System;
using Wisej.Web;

namespace Captain.Common.Views.Forms.Base
{
    public partial class _iForm : Form
    {
        int _oWidth = 0;
        int _oHeight = 0;
        int _browserWidth = 0;
        int _browserHeight = 0;
        double _xwidth = 0;
        double _xheight = 0;
        int _OGWidth = 0;
        int _OGHeight = 0;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //if (this.Width >= Screen.WorkingArea.Width ||
            //    this.Height >= Screen.WorkingArea.Height)
            //{
            //    //this.WindowState = FormWindowState.Maximized;
            //    this.Width = Screen.WorkingArea.Width - 70;
            //    this.Height = Screen.WorkingArea.Height - 70;
            //}

            _oWidth = this.Width;
            _oHeight = this.Height;

            _OGWidth = this.Width;
            _OGHeight = this.Height;

            //if (this.Width >= Application.Browser.Size.Width ||
            //  this.Height >= Application.Browser.Size.Height || (Application.Browser.Size.Width >= 1500 && Application.Browser.Size.Width <= 1600))
            //{
            //    //this.WindowState = FormWindowState.Maximized;
            //    //** this.Width = Application.Browser.Size.Width - 70;
            //    this.Height = Application.Browser.Size.Height - 70;

            //    // if (Application.Browser.Size.Width >= 1500 && Application.Browser.Size.Width <= 1600)
            //    // {

            //    //int scrnWidth = Screen.Bounds.Size.Width;
            //    //int frmWidth = this.Width;
            //    this.Location = new System.Drawing.Point(350, 60);//(50, 60);
            //    //}
            //}


            //NEW CODE
            _browserWidth = Application.Browser.Size.Width;
            _browserHeight = Application.Browser.Size.Height;



            this.Location = new System.Drawing.Point(350, 60);//(50, 60);
            //this.StartPosition = FormStartPosition.CenterScreen;

            /*30% width calculation*/
            _xwidth = 0.30 * _browserWidth;

            /*25% width calculation*/
            _xheight = 0.25 * _browserHeight;


            if (_browserWidth < 1920)
            {

                if ((_oWidth + 350) >= _browserWidth)
                    this.Width = Application.Browser.Size.Width - Convert.ToInt32(_xwidth);
                if ((_oHeight + 60) >= _browserHeight)
                    this.Height = Application.Browser.Size.Height - Convert.ToInt32(_xheight);
            }
            else
            {
                this.Width = _OGWidth;
                this.Height = _OGHeight;
            }




            //switch (_browserWidth)
            //{
            //    case 1920:
            //        this.Width = this.Width;
            //        this.Height = this.Height;
            //        break;
            //    default:
            //        if (_oWidth > _browserWidth)
            //            this.Width = Application.Browser.Size.Width - Convert.ToInt32(_xwidth);
            //        if (_oHeight > _browserHeight)
            //            this.Height = Application.Browser.Size.Height - Convert.ToInt32(_xheight);
            //        break;
            //        //case 1680: this.Width = Application.Browser.Size.Width - 70; break;
            //        //case 1600: this.Width = Application.Browser.Size.Width - 70; break;
            //        //case 1366: this.Width = Application.Browser.Size.Width - 70; break;
            //        //case 1360: this.Width = Application.Browser.Size.Width - 70; break;
            //        //case 1280: this.Width = Application.Browser.Size.Width - 70; break;
            //        //case 1176: this.Width = Application.Browser.Size.Width - 70; break;
            //        //case 1152: this.Width = Application.Browser.Size.Width - 70; break;
            //        //case 1024: this.Width = Application.Browser.Size.Width - 70; break;
            //        //case 800: this.Width = Application.Browser.Size.Width - 70; break;
            //}






            Application.BrowserSizeChanged += Application_BrowserSizeChanged;
        }

        private void Application_BrowserSizeChanged(object sender, EventArgs e)
        {
            //this.Size = Application.Browser.Size;
            //if (this.Width >= Application.Browser.Size.Width ||
            // this.Height >= Application.Browser.Size.Height)
            //{
            //this.WindowState = FormWindowState.Maximized;
            //**this.Width = Application.Browser.Size.Width - 70;
            //**this.Height = Application.Browser.Size.Height - 70;
            //}
            //else {
            //    this.Width = _oWidth;
            //    this.Height = _oHeight;
            //}



            //NEW CODE
            _oWidth = this.Width;
            _oHeight = this.Height;

            this.Location = new System.Drawing.Point(350, 60);//(50, 60);
            _browserWidth = Application.Browser.Size.Width;
            _browserHeight = Application.Browser.Size.Height;

            /*30% width calculation*/
            _xwidth = 0.30 * _browserWidth;

            /*25% width calculation*/
            _xheight = 0.25 * _browserHeight;

            if (_browserWidth < 1920)
            {

                if ((_oWidth + 350) > _browserWidth)
                    this.Width = Application.Browser.Size.Width - Convert.ToInt32(_xwidth);
                if ((_oHeight + 60) > _browserHeight)
                    this.Height = Application.Browser.Size.Height - Convert.ToInt32(_xheight);
            }
            else
            {
                this.Width = _OGWidth;
                this.Height = _OGHeight;
            }
        }

        //private void _iForm_Load(object sender, EventArgs e)
        //{
        //    this.Size = Application.Browser.Size;
        //    Application.BrowserSizeChanged += Application_BrowserSizeChanged;
        //}
    }

}
