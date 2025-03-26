using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;

namespace Captain.Common.Utilities
{
     public class ChildFieldEvent : IPdfPCellEvent
     {
         /** A parent field to which a child field has to be added. */
         protected PdfFormField parent;
         /** The child field that has to be added */
         protected PdfFormField kid;
         /** The padding of the field inside the cell */
         protected float padding;

         /**
          * Creates a ChildFieldEvent.
          * @param parent the parent field
          * @param kid the child field
          * @param padding a padding
          */
         public ChildFieldEvent(PdfFormField parent, PdfFormField kid, float padding)
         {
             this.parent = parent;
             this.kid = kid;
             this.padding = padding;
         }

         /**
          * Add the child field to the parent, and sets the coordinates of the child field.
          */
         public void CellLayout(PdfPCell cell, iTextSharp.text.Rectangle rect, PdfContentByte[] cb)
         {
             parent.AddKid(kid);
             kid.SetWidget(new iTextSharp.text.Rectangle(
                                                         rect.GetLeft(padding),
                                                         rect.GetBottom(padding),
                                                         rect.GetRight(padding),
                                                         rect.GetTop(padding)
                                                         ),
                                                         PdfAnnotation.HIGHLIGHT_INVERT
                                                         );
         }
     }

}
