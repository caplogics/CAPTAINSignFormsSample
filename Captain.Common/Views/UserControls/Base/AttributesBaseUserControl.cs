/****************************************************************************************
 * Class Name   : AttributesBaseUserControl
 * Author       : 
 * Created Date : 
 * Version      : 
 * Description  : Base controls for attributes controls.
 ****************************************************************************************/

#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


using Wisej.Web;
using Captain.Common.Model.Objects;

using Captain.Common.Utilities;
using Captain.Common.Handlers;
using Captain.Common.Views.Forms.Base;

#endregion

namespace Captain.Common.Views.UserControls.Base
{
    public partial class AttributesBaseUserControl : UserControl
    {
        #region Variables

        #endregion

        #region Constructors

        public AttributesBaseUserControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties 

        public BaseForm BaseForm { get; set; } 

         public virtual bool IsModified { get; set; }

        #endregion

        #region Public / Virtual methods

        public virtual void SetToolTip() { }

        public virtual void SetEditable(bool allowEdit) { }

        public virtual void SetSpecialEditable(bool isEditable) { }

        #endregion
    }
}