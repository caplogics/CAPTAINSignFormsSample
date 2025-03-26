using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Captain
{
    public partial class imagePreview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string imgPath = Request.QueryString["srcpath"].ToString();

                imgSrc.ImageUrl = imgPath;
            }
        }
    }
}