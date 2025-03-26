using CarlosAg.ExcelXmlWriter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Captain.Common.Utilities
{
    public class CarlosAg_Excel_Properties : CarlosAg_Excel_Properties_Font
    {

        private Random random = new Random();
        private string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());

        }

        public string xfnCELL_STYLE(Workbook sxlbook, string Styleclassname, string _FontName, int _FontSize, string _FontColor, bool FontBold, string _BackColor, string _Aligenment,
            int _Tborder, int _Rborder, int _Bborder, int _Lborder, string _borderColor)
        {

            if (_FontName == "")
                _FontName = sxlbodyFont;

            string classname = RandomString();//Styleclassname; // RandomString();
            WorksheetStyle mainstyle = sxlbook.Styles.Add(classname);
            mainstyle.Font.FontName = _FontName;
            mainstyle.Font.Size = _FontSize;
            mainstyle.Font.Bold = FontBold;
            mainstyle.Font.Color = _FontColor;
            mainstyle.Interior.Color = _BackColor;
            mainstyle.Interior.Pattern = StyleInteriorPattern.Solid;

            //ALIGNMENT
            if (_Aligenment.ToLower() == "left")
                mainstyle.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            else if (_Aligenment.ToLower() == "right")
                mainstyle.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            else
                mainstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;

            mainstyle.Alignment.Vertical = StyleVerticalAlignment.Center;

            if (_borderColor == "")
                _borderColor = "#FFFFFF";

            // BORDER STYLE
            mainstyle.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, _Bborder, _borderColor);
            mainstyle.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, _Lborder, _borderColor);
            mainstyle.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, _Rborder, _borderColor);
            mainstyle.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, _Tborder, _borderColor);

            mainstyle.Alignment.WrapText = true;
            return classname;
        }

        CarlosAg_Excel_Properties_Font obj = new CarlosAg_Excel_Properties_Font();

        public void getCarlosAg_Excel_Properties()
        {

            gxlTitle_CellStyle1 = xfnCELL_STYLE(sxlbook, RandomString(), sxlTitleFont, 16, "#305496", false, "#F8F9D0", "left", 1, 1, 1, 1, "#F8F9D0").ToString();
            gxlTitle_CellStyle2 = xfnCELL_STYLE(sxlbook, RandomString(), sxlTitleFont, 16, "#305496", true, "#F8F9D0", "left", 1, 1, 1, 1, "#F8F9D0").ToString();

            gxlEMPTC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 10, "#000000", false, "#FFFFFF", "center", 0, 0, 0, 0, "").ToString();
            gxlERRMSG = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 10, "#fc0303", false, "#FCFCFC", "center", 0, 0, 0, 0, "").ToString();


            /*********************************************************************************/
            /**************************** NORMAL THEME CELL STYLE ****************************/
            /*********************************************************************************/
            gxlNLHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#EEEEEE", "left", 1, 1, 1, 1, "#BFBFBF").ToString();
            gxlNRHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#EEEEEE", "right", 1, 1, 1, 1, "#BFBFBF").ToString();
            gxlNCHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#EEEEEE", "center", 1, 1, 1, 1, "#BFBFBF").ToString();

            gxlNLC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#FFFFFF", "left", 1, 1, 1, 1, "#BFBFBF").ToString();
            gxlNRC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#FFFFFF", "right", 1, 1, 1, 1, "#BFBFBF").ToString();
            gxlNCC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#FFFFFF", "center", 1, 1, 1, 1, "#BFBFBF").ToString();

            gxlNCC_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#F7F7F7", "center", 1, 1, 1, 1, "#BFBFBF").ToString();


            gxlNLC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#FFFFFF", "left", 1, 1, 1, 1, "#BFBFBF").ToString();
            gxlNRC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#FFFFFF", "right", 1, 1, 1, 1, "#BFBFBF").ToString();
            gxlNCC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#FFFFFF", "center", 1, 1, 1, 1, "#BFBFBF").ToString();

            gxlNLC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#F7F7F7", "left", 1, 1, 1, 1, "#BFBFBF").ToString();
            gxlNRC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#F7F7F7", "right", 1, 1, 1, 1, "#BFBFBF").ToString();
            gxlNCC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#F7F7F7", "center", 1, 1, 1, 1, "#BFBFBF").ToString();


            /*********************************************************************************/
            /**************************** BLUE THEME CELL STYLE ****************************/
            /*********************************************************************************/
            gxlBLHC_sp = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#FFFFFF", true, "#2f74b5", "left", 1, 1, 1, 1, "#2f74b5").ToString();
            gxlBRHC_sp = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#FFFFFF", true, "#2f74b5", "right", 1, 1, 1, 1, "#2f74b5").ToString();
            gxlBCHC_sp = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#FFFFFF", true, "#2f74b5", "center", 1, 1, 1, 1, "#2f74b5").ToString();

            gxlBLHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#9bc2e6", "left", 1, 1, 1, 1, "#b0d4f5").ToString();
            gxlBRHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#9bc2e6", "right", 1, 1, 1, 1, "#b0d4f5").ToString();
            gxlBCHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#9bc2e6", "center", 1, 1, 1, 1, "#b0d4f5").ToString();

            gxlBLC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#ebf7ff", "left", 1, 1, 1, 1, "#a6c9e8").ToString();
            gxlBRC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#ebf7ff", "right", 1, 1, 1, 1, "#a6c9e8").ToString();
            gxlBCC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#ebf7ff", "center", 1, 1, 1, 1, "#a6c9e8").ToString();

            gxlBCC_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#d3e6f5", "center", 1, 1, 1, 1, "#a6c9e8").ToString();


            gxlBLC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#ebf7ff", "left", 1, 1, 1, 1, "#a6c9e8").ToString();
            gxlBRC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#ebf7ff", "right", 1, 1, 1, 1, "#a6c9e8").ToString();
            gxlBCC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#ebf7ff", "center", 1, 1, 1, 1, "#a6c9e8").ToString();

            gxlBLC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#F7F7F7", "left", 1, 1, 1, 1, "#a6c9e8").ToString();
            gxlBRC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#F7F7F7", "right", 1, 1, 1, 1, "#a6c9e8").ToString();
            gxlBCC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#F7F7F7", "center", 1, 1, 1, 1, "#a6c9e8").ToString();


            /*********************************************************************************/
            /**************************** GREEN THEME CELL STYLE ****************************/
            /*********************************************************************************/
            gxlGLHC_sp = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#FFFFFF", true, "#548235", "left", 1, 1, 1, 1, "#548235").ToString();
            gxlGRHC_sp = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#FFFFFF", true, "#548235", "right", 1, 1, 1, 1, "#548235").ToString();
            gxlGCHC_sp = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#FFFFFF", true, "#548235", "center", 1, 1, 1, 1, "#548235").ToString();

            gxlGLHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#aad08e", "left", 1, 1, 1, 1, "#8aec8a").ToString();
            gxlGRHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#aad08e", "right", 1, 1, 1, 1, "#8aec8a").ToString();
            gxlGCHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#aad08e", "center", 1, 1, 1, 1, "#8aec8a").ToString();

            gxlGLC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#f0ffff", "left", 1, 1, 1, 1, "#8aec8a").ToString();
            gxlGRC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#f0ffff", "right", 1, 1, 1, 1, "#8aec8a").ToString();
            gxlGCC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#f0ffff", "center", 1, 1, 1, 1, "#8aec8a").ToString();

            gxlGCC_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#bee396", "center", 1, 1, 1, 1, "#8aec8a").ToString();


            gxlGLC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#f0ffff", "left", 1, 1, 1, 1, "#8aec8a").ToString();
            gxlGRC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#f0ffff", "right", 1, 1, 1, 1, "#8aec8a").ToString();
            gxlGCC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#f0ffff", "center", 1, 1, 1, 1, "#8aec8a").ToString();

            gxlGLC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#F7F7F7", "left", 1, 1, 1, 1, "#8aec8a").ToString();
            gxlGRC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#F7F7F7", "right", 1, 1, 1, 1, "#8aec8a").ToString();
            gxlGCC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#F7F7F7", "center", 1, 1, 1, 1, "#8aec8a").ToString();

            gxlGCHC_Highlite = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 9, "#ffeb00", true, "#548235", "center", 1, 1, 1, 1, "#548235").ToString();
            gxlGCHR_Highlite = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 9, "#ffeb00", true, "#548235", "right", 1, 1, 1, 1, "#548235").ToString();
            gxlGCHL_Highlite = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 9, "#ffeb00", true, "#548235", "left", 1, 1, 1, 1, "#548235").ToString();

            /*********************************************************************************/
            /**************************** BROWN THEME CELL STYLE ****************************/
            /*********************************************************************************/
            gxlBRLHC_sp = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#FFFFFF", true, "#806000", "left", 1, 1, 1, 1, "#806000").ToString();
            gxlBRRHC_sp = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#FFFFFF", true, "#806000", "right", 1, 1, 1, 1, "#806000").ToString();
            gxlBRCHC_sp = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#FFFFFF", true, "#806000", "center", 1, 1, 1, 1, "#806000").ToString();

            gxlBRLHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#ffd966", "left", 1, 1, 1, 1, "#e8c354").ToString();
            gxlBRRHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#ffd966", "right", 1, 1, 1, 1, "#e8c354").ToString();
            gxlBRCHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#ffd966", "center", 1, 1, 1, 1, "#e8c354").ToString();

            gxlBRLC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#fff9e7", "left", 1, 1, 1, 1, "#e8c354").ToString();
            gxlBRRC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#fff9e7", "right", 1, 1, 1, 1, "#e8c354").ToString();
            gxlBRCC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#fff9e7", "center", 1, 1, 1, 1, "#e8c354").ToString();

            gxlBRCC_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#e6d195", "center", 1, 1, 1, 1, "#e8c354").ToString();


            gxlBRLC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#fff9e7", "left", 1, 1, 1, 1, "#e8c354").ToString();
            gxlBRRC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#fff9e7", "right", 1, 1, 1, 1, "#e8c354").ToString();
            gxlBRCC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#fff9e7", "center", 1, 1, 1, 1, "#e8c354").ToString();

            gxlBRLC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#e6d195", "left", 1, 1, 1, 1, "#e8c354").ToString();
            gxlBRRC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#e6d195", "right", 1, 1, 1, 1, "#e8c354").ToString();
            gxlBRCC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#e6d195", "center", 1, 1, 1, 1, "#e8c354").ToString();

            /*********************************************************************************/
            /**************************** PURPLE THEME CELL STYLE ****************************/
            /*********************************************************************************/
            gxlPULHC_sp = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#FFFFFF", true, "#640352", "left", 1, 1, 1, 1, "#640352").ToString();
            gxlPURHC_sp = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#FFFFFF", true, "#640352", "right", 1, 1, 1, 1, "#640352").ToString();
            gxlPUCHC_sp = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#FFFFFF", true, "#640352", "center", 1, 1, 1, 1, "#640352").ToString();

            gxlPULHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#db7fca", "left", 1, 1, 1, 1, "#db7fca").ToString();
            gxlPURHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#db7fca", "right", 1, 1, 1, 1, "#db7fca").ToString();
            gxlPUCHC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#db7fca", "center", 1, 1, 1, 1, "#db7fca").ToString();

            gxlPULC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#fef4fc", "left", 1, 1, 1, 1, "#de95c6").ToString();
            gxlPURC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#fef4fc", "right", 1, 1, 1, 1, "#de95c6").ToString();
            gxlPUCC = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#fef4fc", "center", 1, 1, 1, 1, "#de95c6").ToString();

            gxlPUCC_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", false, "#e6d195", "center", 1, 1, 1, 1, "#de95c6").ToString();


            gxlPULC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#fef4fc", "left", 1, 1, 1, 1, "#de95c6").ToString();
            gxlPURC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#fef4fc", "right", 1, 1, 1, 1, "#de95c6").ToString();
            gxlPUCC_bo = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#fef4fc", "center", 1, 1, 1, 1, "#de95c6").ToString();

            gxlPULC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#e695cc", "left", 1, 1, 1, 1, "#de95c6").ToString();
            gxlPURC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#e695cc", "right", 1, 1, 1, 1, "#de95c6").ToString();
            gxlPUCC_bo_cr = xfnCELL_STYLE(sxlbook, RandomString(), sxlbodyFont, 8, "#000000", true, "#e695cc", "center", 1, 1, 1, 1, "#de95c6").ToString();


        }

    }

    public class CarlosAg_Excel_Properties_Font
    {
        public Workbook sxlbook { get; set; }
        public string sxlTitleFont { get; set; }
        public string sxlTitleFontBgColor { get; set; }
        public string sxlTitleFontForeColor { get; set; }

        public string sxlbodyFont { get; set; }


        //*********************************** CELLS *****************************************///
        public string gxlTitle_CellStyle1 { get; set; }
        public string gxlTitle_CellStyle2 { get; set; }

        public string gxlEMPTC { get; set; }

        public string gxlERRMSG { get; set; }


        /*********************************************************************************/
        /**************************** NORMAL THEME CELL STYLE ****************************/
        /*********************************************************************************/
        public string gxlNLHC { get; set; }
        public string gxlNRHC { get; set; }
        public string gxlNCHC { get; set; }

        public string gxlNLC { get; set; }
        public string gxlNRC { get; set; }
        public string gxlNCC { get; set; }

        public string gxlNCC_cr { get; set; }


        public string gxlNLC_bo { get; set; }
        public string gxlNRC_bo { get; set; }
        public string gxlNCC_bo { get; set; }

        public string gxlNLC_bo_cr { get; set; }
        public string gxlNRC_bo_cr { get; set; }
        public string gxlNCC_bo_cr { get; set; }


        /*********************************************************************************/
        /**************************** BLUE THEME CELL STYLE ****************************/
        /*********************************************************************************/
        public string gxlBLHC_sp { get; set; }
        public string gxlBRHC_sp { get; set; }
        public string gxlBCHC_sp { get; set; }

        public string gxlBLHC { get; set; }
        public string gxlBRHC { get; set; }
        public string gxlBCHC { get; set; }

        public string gxlBLC { get; set; }
        public string gxlBRC { get; set; }
        public string gxlBCC { get; set; }

        public string gxlBCC_cr { get; set; }


        public string gxlBLC_bo { get; set; }
        public string gxlBRC_bo { get; set; }
        public string gxlBCC_bo { get; set; }

        public string gxlBLC_bo_cr { get; set; }
        public string gxlBRC_bo_cr { get; set; }
        public string gxlBCC_bo_cr { get; set; }

        /*********************************************************************************/
        /**************************** GREEN THEME CELL STYLE ****************************/
        /*********************************************************************************/
        public string gxlGLHC_sp { get; set; }
        public string gxlGRHC_sp { get; set; }
        public string gxlGCHC_sp { get; set; }
        public string gxlGCHC_Highlite { get; set; }
        public string gxlGCHL_Highlite { get; set; }
        public string gxlGCHR_Highlite { get; set; }

        public string gxlGLHC { get; set; }
        public string gxlGRHC { get; set; }
        public string gxlGCHC { get; set; }

        public string gxlGLC { get; set; }
        public string gxlGRC { get; set; }
        public string gxlGCC { get; set; }

        public string gxlGCC_cr { get; set; }


        public string gxlGLC_bo { get; set; }
        public string gxlGRC_bo { get; set; }
        public string gxlGCC_bo { get; set; }

        public string gxlGLC_bo_cr { get; set; }
        public string gxlGRC_bo_cr { get; set; }
        public string gxlGCC_bo_cr { get; set; }

        /*********************************************************************************/
        /**************************** BROWN THEME CELL STYLE ****************************/
        /*********************************************************************************/
        public string gxlBRLHC_sp { get; set; }
        public string gxlBRRHC_sp { get; set; }
        public string gxlBRCHC_sp { get; set; }

        public string gxlBRLHC { get; set; }
        public string gxlBRRHC { get; set; }
        public string gxlBRCHC { get; set; }

        public string gxlBRLC { get; set; }
        public string gxlBRRC { get; set; }
        public string gxlBRCC { get; set; }

        public string gxlBRCC_cr { get; set; }


        public string gxlBRLC_bo { get; set; }
        public string gxlBRRC_bo { get; set; }
        public string gxlBRCC_bo { get; set; }

        public string gxlBRLC_bo_cr { get; set; }
        public string gxlBRRC_bo_cr { get; set; }
        public string gxlBRCC_bo_cr { get; set; }

        /*********************************************************************************/
        /**************************** PURPLE THEME CELL STYLE ****************************/
        /*********************************************************************************/
        public string gxlPULHC_sp { get; set; }
        public string gxlPURHC_sp { get; set; }
        public string gxlPUCHC_sp { get; set; }

        public string gxlPULHC { get; set; }
        public string gxlPURHC { get; set; }
        public string gxlPUCHC { get; set; }

        public string gxlPULC { get; set; }
        public string gxlPURC { get; set; }
        public string gxlPUCC { get; set; }

        public string gxlPUCC_cr { get; set; }


        public string gxlPULC_bo { get; set; }
        public string gxlPURC_bo { get; set; }
        public string gxlPUCC_bo { get; set; }

        public string gxlPULC_bo_cr { get; set; }
        public string gxlPURC_bo_cr { get; set; }
        public string gxlPUCC_bo_cr { get; set; }

    }

}