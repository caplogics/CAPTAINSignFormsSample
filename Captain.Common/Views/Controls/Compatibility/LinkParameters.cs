

using System.Drawing;

namespace Captain.Common.Views.Controls.Compatibility
{
    public class LinkParameters : Wisej.Web.Form
    {

        public LinkParameters()
        {
        }

        public LinkParameters(LinkWindowStyle enmWindowStyle)
        {
        }

        public LinkParameters(LinkWindowStyle enmWindowStyle, System.Drawing.Size objWindowSize)
        {
            menmWindowStyle = enmWindowStyle;
            this.Size = objWindowSize;
        }

        public LinkWindowStyle WindowStyle
        {
            get
            {
                return menmWindowStyle;
            }
            set
            {
                menmWindowStyle = value;
            }
        }

        public bool ShowMenuBar
        {
            get
            {
                return mblnShowMenuBar;
            }
            set
            {
                mblnShowMenuBar = value;
            }
        }

        public bool ShowTitleBar
        {
            get
            {
                return mblnShowTitleBar;
            }
            set
            {
                mblnShowTitleBar = value;
            }
        }

        public bool ShowToolBar
        {
            get
            {
                return mblnShowToolBar;
            }
            set
            {
                mblnShowToolBar = value;
            }
        }

        public bool ShowStatusBar
        {
            get
            {
                return mblnShowStatusBar;
            }
            set
            {
                mblnShowStatusBar = value;
            }
        }

        public bool ShowLocationBar
        {
            get
            {
                return mblnShowLocationBar;
            }
            set
            {
                mblnShowLocationBar = value;
            }
        }

        public string Target 
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        private LinkWindowStyle menmWindowStyle;
        private bool mblnShowStatusBar;
        private bool mblnShowToolBar;
        private bool mblnShowTitleBar;
        private bool mblnShowMenuBar;
        private bool mblnShowLocationBar;
        private string target;
    }
}
