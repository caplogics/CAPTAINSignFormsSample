using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Captain.Common.Utilities
{
    public class DynamicCheckbox : IPdfPCellEvent
    {
        private string fieldname;
        private string cap;

        public DynamicCheckbox(string name, String caption)
        {
            fieldname = name;
            cap = caption;
        }

        public void CellLayout(PdfPCell cell, Rectangle rectangle, PdfContentByte[] canvases)
        {
            PdfWriter writer = canvases[0].PdfWriter;
            //iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(0, 8, 8, 0);
            RadioCheckField ckbx = new RadioCheckField(writer, rectangle, fieldname, "Yes");
            ckbx.CheckType = RadioCheckField.TYPE_CHECK;
            ckbx.Rotation = 90;
            ckbx.Text = cap;
            PdfFormField field = ckbx.CheckField;
            writer.AddAnnotation(field);
        }
    }
}
