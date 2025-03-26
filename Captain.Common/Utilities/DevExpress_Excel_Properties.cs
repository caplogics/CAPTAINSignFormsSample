using DevExpress.CodeParser;
using DevExpress.Spreadsheet;
using NPOI.SS.Formula.Functions;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;


public class DevExpress_Excel_Properties : Devexpress_Excel_Properties_Font
{

    private Random random = new Random();
    private string RandomString()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 10)
          .Select(s => s[random.Next(s.Length)]).ToArray());

    }

    public Style xfnCELL_STYLE(Workbook sxlbook, string _FontName, int _FontSize, string _FontColor, bool FontBold, string _BackColor, string _Aligenment,
       int _border, int _Tborder, int _Rborder, int _Bborder, int _Lborder, string _borderStyle, string _borderColor, string _cellType)
    {

        if (_FontName == "")
            _FontName = sxlbodyFont;

        string classname = RandomString();//Styleclassname; // RandomString();
        Style mainstyle = sxlbook.Styles.Add(classname);
        mainstyle.Font.Name = _FontName;
        mainstyle.Font.Size = _FontSize;
        mainstyle.Font.Bold = FontBold;
        mainstyle.Font.Color = System.Drawing.ColorTranslator.FromHtml(_FontColor);
        mainstyle.Fill.BackgroundColor = System.Drawing.ColorTranslator.FromHtml(_BackColor);
        //mainstyle.Interior.Pattern = StyleInteriorPattern.Solid;


        //ALIGNMENT
        if (_Aligenment.ToLower() == "left")
            mainstyle.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
        else if (_Aligenment.ToLower() == "right")
            mainstyle.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
        else
            mainstyle.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;

        mainstyle.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

        if (_borderColor == "")
            _borderColor = "#FFFFFF";

        // BORDER STYLE
        //mainstyle.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, _Bborder, _borderColor);
        //mainstyle.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, _Lborder, _borderColor);
        //mainstyle.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, _Rborder, _borderColor);
        //mainstyle.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, _Tborder, _borderColor);

        if (_border == 1)
        {
            mainstyle.Borders.SetAllBorders(System.Drawing.ColorTranslator.FromHtml(_borderColor), BorderLineStyle.None);
            switch (_borderStyle)
            {

                case "Thin":
                    //mainstyle.Borders.SetAllBorders(System.Drawing.ColorTranslator.FromHtml(_borderColor), BorderLineStyle.Thin);
                    if (_Tborder == 1)
                        mainstyle.Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                    if (_Rborder == 1)
                        mainstyle.Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                    if (_Bborder == 1)
                        mainstyle.Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                    if (_Lborder == 1)
                        mainstyle.Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                    break;

                case "Thick":
                    //mainstyle.Borders.SetAllBorders(System.Drawing.ColorTranslator.FromHtml(_borderColor), BorderLineStyle.Thick);
                    if (_Tborder == 1)
                        mainstyle.Borders.TopBorder.LineStyle = BorderLineStyle.Thick;
                    if (_Rborder == 1)
                        mainstyle.Borders.RightBorder.LineStyle = BorderLineStyle.Thick;
                    if (_Bborder == 1)
                        mainstyle.Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                    if (_Lborder == 1)
                        mainstyle.Borders.LeftBorder.LineStyle = BorderLineStyle.Thick;
                    break;

                case "Medium":
                    //mainstyle.Borders.SetAllBorders(System.Drawing.ColorTranslator.FromHtml(_borderColor), BorderLineStyle.Medium);
                    if (_Tborder == 1)
                        mainstyle.Borders.TopBorder.LineStyle = BorderLineStyle.Medium;
                    if (_Rborder == 1)
                        mainstyle.Borders.RightBorder.LineStyle = BorderLineStyle.Medium;
                    if (_Bborder == 1)
                        mainstyle.Borders.BottomBorder.LineStyle = BorderLineStyle.Medium;
                    if (_Lborder == 1)
                        mainstyle.Borders.LeftBorder.LineStyle = BorderLineStyle.Medium;
                    break;

                case "Hair":
                    //mainstyle.Borders.SetAllBorders(System.Drawing.ColorTranslator.FromHtml(_borderColor), BorderLineStyle.Hair);
                    if (_Tborder == 1)
                        mainstyle.Borders.TopBorder.LineStyle = BorderLineStyle.Hair;
                    if (_Rborder == 1)
                        mainstyle.Borders.RightBorder.LineStyle = BorderLineStyle.Hair;
                    if (_Bborder == 1)
                        mainstyle.Borders.BottomBorder.LineStyle = BorderLineStyle.Hair;
                    if (_Lborder == 1)
                        mainstyle.Borders.LeftBorder.LineStyle = BorderLineStyle.Hair;
                    break;
            }

        }
        else
        {
            mainstyle.Borders.SetAllBorders(System.Drawing.ColorTranslator.FromHtml(_borderColor), BorderLineStyle.None);
        }


        if (_cellType == "N")
            mainstyle.NumberFormat = "0";
        if (_cellType == "D")
            mainstyle.NumberFormat = "#0.00";

        mainstyle.Alignment.WrapText = true;
        return mainstyle;
    }

    Devexpress_Excel_Properties_Font obj = new Devexpress_Excel_Properties_Font();

    public void getDevexpress_Excel_Properties()
    {

        gxlTitle_CellStyle1 = xfnCELL_STYLE(sxlbook, sxlTitleFont, 16, "#305496", false, "#F8F9D0", "left", 1, 1, 1, 1, 1, "Thin", "#F8F9D0", "");
        gxlTitle_CellStyle2 = xfnCELL_STYLE(sxlbook, sxlTitleFont, 16, "#305496", true, "#F8F9D0", "left", 1, 1, 1, 1, 1, "Thin", "#F8F9D0", "");

        gxlEMPTC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 10, "#000000", false, "#FFFFFF", "center", 0, 0, 0, 0, 0, "", "", "");
        gxlEMPTL = xfnCELL_STYLE(sxlbook, sxlbodyFont, 10, "#000000", false, "#FFFFFF", "left", 0, 0, 0, 0, 0, "", "", "");
        gxlEMPTR = xfnCELL_STYLE(sxlbook, sxlbodyFont, 10, "#000000", false, "#FFFFFF", "right", 0, 0, 0, 0, 0, "", "", "");
        gxlERRMSG = xfnCELL_STYLE(sxlbook, sxlbodyFont, 10, "#fc0303", false, "#FCFCFC", "center", 0, 0, 0, 0, 0, "", "", "");


        /*********************************************************************************/
        /**************************** NORMAL THEME CELL STYLE ****************************/
        /*********************************************************************************/
        gxlNLHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#EEEEEE", "left", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
        gxlNRHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#EEEEEE", "right", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
        gxlNCHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#EEEEEE", "center", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");

        gxlNLC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
        gxlNRC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
        gxlNumb_NRC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "N");
        gxlDeci_NRC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "D");
        gxlDeci_NRC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "D");
        gxlNCC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");

        gxlNumb_NCC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "N");
        gxlDeci_NCC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "D");
        //gxlDate_NCC = xfnDATECELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");

        gxlNCC_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#F7F7F7", "center", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");

        gxlNumb_NRC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "N");

        gxlNLC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
        gxlNRC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
        gxlNCC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");

        gxlNLC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#F7F7F7", "left", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
        gxlNRC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#F7F7F7", "right", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
        gxlNCC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#F7F7F7", "center", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");


        /*********************************************************************************/
        /**************************** BLUE THEME CELL STYLE ****************************/
        /*********************************************************************************/
        gxlBLHC_sp = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#FFFFFF", true, "#2f74b5", "left", 1, 1, 1, 1, 1, "Thin", "#2f74b5", "");
        gxlBRHC_sp = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#FFFFFF", true, "#2f74b5", "right", 1, 1, 1, 1, 1, "Thin", "#2f74b5", "");
        gxlBCHC_sp = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#FFFFFF", true, "#2f74b5", "center", 1, 1, 1, 1, 1, "Thin", "#2f74b5", "");

        gxlBLHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#9bc2e6", "left", 1, 1, 1, 1, 1, "Thin", "#b0d4f5", "");
        gxlBRHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#9bc2e6", "right", 1, 1, 1, 1, 1, "Thin", "#b0d4f5", "");
        gxlBCHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#9bc2e6", "center", 1, 1, 1, 1, 1, "Thin", "#b0d4f5", "");

        gxlBLC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#ebf7ff", "left", 1, 1, 1, 1, 1, "Thin", "#a6c9e8", "");
        gxlBRC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#ebf7ff", "right", 1, 1, 1, 1, 1, "Thin", "#a6c9e8", "");
        gxlNumb_BRC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#ebf7ff", "right", 1, 1, 1, 1, 1, "Thin", "#a6c9e8", "N");
        gxlDeci_BRC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#ebf7ff", "right", 1, 1, 1, 1, 1, "Thin", "#a6c9e8", "D");


        gxlBCC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#ebf7ff", "center", 1, 1, 1, 1, 1, "Thin", "#a6c9e8", "");

        gxlBCC_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#d3e6f5", "center", 1, 1, 1, 1, 1, "Thin", "#a6c9e8", "");


        gxlBLC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#ebf7ff", "left", 1, 1, 1, 1, 1, "Thin", "#a6c9e8", "");
        gxlBRC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#ebf7ff", "right", 1, 1, 1, 1, 1, "Thin", "#a6c9e8", "");
        gxlBCC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#ebf7ff", "center", 1, 1, 1, 1, 1, "Thin", "#a6c9e8", "");

        gxlBLC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#F7F7F7", "left", 1, 1, 1, 1, 1, "Thin", "#a6c9e8", "");
        gxlBRC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#F7F7F7", "right", 1, 1, 1, 1, 1, "Thin", "#a6c9e8", "");
        gxlBCC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#F7F7F7", "center", 1, 1, 1, 1, 1, "Thin", "#a6c9e8", "");


        /*********************************************************************************/
        /**************************** GREEN THEME CELL STYLE ****************************/
        /*********************************************************************************/
        gxlGLHC_sp = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#FFFFFF", true, "#548235", "left", 1, 1, 1, 1, 1, "Thin", "#548235", "");
        gxlGRHC_sp = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#FFFFFF", true, "#548235", "right", 1, 1, 1, 1, 1, "Thin", "#548235", "");
        gxlGCHC_sp = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#FFFFFF", true, "#548235", "center", 1, 1, 1, 1, 1, "Thin", "#548235", "");

        gxlGLHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#aad08e", "left", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");
        gxlGRHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#aad08e", "right", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");
        gxlGCHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#aad08e", "center", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");

        gxlGLC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#f0ffff", "left", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");
        gxlGRC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#f0ffff", "right", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");
        gxlDeci_GRC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#f0ffff", "right", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "D");
        gxlNumb_GRC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#f0ffff", "right", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "N");

        gxlGCC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#f0ffff", "center", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");
        gxlNumb_GCC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#f0ffff", "center", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "N");
        gxlDeci_GCC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#f0ffff", "center", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "D");
        gxlGCC_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#bee396", "center", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");
        //  gxlNumbGCC_cr = xfnNUMBCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#bee396", "center", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");


        gxlGLC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#f0ffff", "left", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");
        gxlGRC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#f0ffff", "right", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");
        gxlGCC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#f0ffff", "center", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");

        gxlGLC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#F7F7F7", "left", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");
        gxlGRC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#F7F7F7", "right", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");
        gxlGCC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#F7F7F7", "center", 1, 1, 1, 1, 1, "Thin", "#8aec8a", "");

        gxlGCHC_Highlite = xfnCELL_STYLE(sxlbook, sxlbodyFont, 9, "#ffeb00", true, "#548235", "center", 1, 1, 1, 1, 1, "Thin", "#548235", "");
        gxlGCHR_Highlite = xfnCELL_STYLE(sxlbook, sxlbodyFont, 9, "#ffeb00", true, "#548235", "right", 1, 1, 1, 1, 1, "Thin", "#548235", "");
        gxlGCHL_Highlite = xfnCELL_STYLE(sxlbook, sxlbodyFont, 9, "#ffeb00", true, "#548235", "left", 1, 1, 1, 1, 1, "Thin", "#548235", "");

        /*********************************************************************************/
        /**************************** BROWN THEME CELL STYLE ****************************/
        /*********************************************************************************/
        gxlBRLHC_sp = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#FFFFFF", true, "#806000", "left", 1, 1, 1, 1, 1, "Thin", "#806000", "");
        gxlBRRHC_sp = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#FFFFFF", true, "#806000", "right", 1, 1, 1, 1, 1, "Thin", "#806000", "");
        gxlBRCHC_sp = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#FFFFFF", true, "#806000", "center", 1, 1, 1, 1, 1, "Thin", "#806000", "");

        gxlBRLHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#ffd966", "left", 1, 1, 1, 1, 1, "Thin", "#e8c354", "");
        gxlBRRHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#ffd966", "right", 1, 1, 1, 1, 1, "Thin", "#e8c354", "");
        gxlBRCHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#ffd966", "center", 1, 1, 1, 1, 1, "Thin", "#e8c354", "");

        gxlBRLC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#fff9e7", "left", 1, 1, 1, 1, 1, "Thin", "#e8c354", "");
        gxlBRRC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#fff9e7", "right", 1, 1, 1, 1, 1, "Thin", "#e8c354", "");
        gxlBRCC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#fff9e7", "center", 1, 1, 1, 1, 1, "Thin", "#e8c354", "");

        gxlBRCC_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#e6d195", "center", 1, 1, 1, 1, 1, "Thin", "#e8c354", "");


        gxlBRLC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#fff9e7", "left", 1, 1, 1, 1, 1, "Thin", "#e8c354", "");
        gxlBRRC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#fff9e7", "right", 1, 1, 1, 1, 1, "Thin", "#e8c354", "");
        gxlBRCC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#fff9e7", "center", 1, 1, 1, 1, 1, "Thin", "#e8c354", "");

        gxlBRLC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#e6d195", "left", 1, 1, 1, 1, 1, "Thin", "#e8c354", "");
        gxlBRRC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#e6d195", "right", 1, 1, 1, 1, 1, "Thin", "#e8c354", "");
        gxlBRCC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#e6d195", "center", 1, 1, 1, 1, 1, "Thin", "#e8c354", "");

        /*********************************************************************************/
        /**************************** PURPLE THEME CELL STYLE ****************************/
        /*********************************************************************************/
        gxlPULHC_sp = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#FFFFFF", true, "#640352", "left", 1, 1, 1, 1, 1, "Thin", "#640352", "");
        gxlPURHC_sp = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#FFFFFF", true, "#640352", "right", 1, 1, 1, 1, 1, "Thin", "#640352", "");
        gxlPUCHC_sp = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#FFFFFF", true, "#640352", "center", 1, 1, 1, 1, 1, "Thin", "#640352", "");

        gxlPULHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#db7fca", "left", 1, 1, 1, 1, 1, "Thin", "#db7fca", "");
        gxlPURHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#db7fca", "right", 1, 1, 1, 1, 1, "Thin", "#db7fca", "");
        gxlPUCHC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#db7fca", "center", 1, 1, 1, 1, 1, "Thin", "#db7fca", "");

        gxlPULC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#fef4fc", "left", 1, 1, 1, 1, 1, "Thin", "#de95c6", "");
        gxlPURC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#fef4fc", "right", 1, 1, 1, 1, 1, "Thin", "#de95c6", "");
        gxlPUCC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#fef4fc", "center", 1, 1, 1, 1, 1, "Thin", "#de95c6", "");

        gxlPUCC_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#e6d195", "center", 1, 1, 1, 1, 1, "Thin", "#de95c6", "");


        gxlPULC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#fef4fc", "left", 1, 1, 1, 1, 1, "Thin", "#de95c6", "");
        gxlPURC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#fef4fc", "right", 1, 1, 1, 1, 1, "Thin", "#de95c6", "");
        gxlPUCC_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#fef4fc", "center", 1, 1, 1, 1, 1, "Thin", "#de95c6", "");

        gxlPULC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#e695cc", "left", 1, 1, 1, 1, 1, "Thin", "#de95c6", "");
        gxlPURC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#e695cc", "right", 1, 1, 1, 1, 1, "Thin", "#de95c6", "");
        gxlPUCC_bo_cr = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", true, "#e695cc", "center", 1, 1, 1, 1, 1, "Thin", "#de95c6", "");


        /*********************************************************************************/
        /**************************** Report Data Styles ***************************/
        /*********************************************************************************/
        #region Report Data Frame Styles

        gxlFrameStyleL = xfnCELL_STYLE(sxlbook, sxlbodyFont, 12, "#FFFFFF", false, "#1A4777", "left", 1, 1, 1, 1, 1, "Thin", "#1A4777", "");
        gxlFrameStyleR = xfnCELL_STYLE(sxlbook, sxlbodyFont, 12, "#FFFFFF", false, "#1A4777", "right", 1, 1, 1, 1, 1, "Thin", "#1A4777", "");
        gxlFrameStyleC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 12, "#FFFFFF", false, "#1A4777", "center", 1, 1, 1, 1, 1, "Thin", "#1A4777", "");

        gxlFrameBlockStyleC = xfnCELL_STYLE(sxlbook, sxlbodyFont, 12, "#1A4777", true, "#b8d9ff", "center", 1, 1, 1, 1, 1, "Thin", "#b8d9ff", "");
        gxlFrameBlockStyleL = xfnCELL_STYLE(sxlbook, sxlbodyFont, 12, "#1A4777", true, "#b8d9ff", "left", 1, 1, 1, 1, 1, "Thin", "#b8d9ff", "");
        gxlFrameBlockStyleR = xfnCELL_STYLE(sxlbook, sxlbodyFont, 12, "#1A4777", true, "#b8d9ff", "right", 1, 1, 1, 1, 1, "Thin", "#b8d9ff", "");

        gxlFrameFooterStyle = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#FFFFFF", false, "#1A4777", "right", 1, 1, 1, 1, 1, "Thin", "#1A4777", "");

        #endregion

        #region Cell Styles

        gxlDBCC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
        gxlDBLC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
        gxlDBRC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");

        gxlDBCC_Blue_Borders_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
        gxlDBLC_Blue_Borders_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
        gxlDBRC_Blue_Borders_bo = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");

        gxlRCC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
        gxlRLC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
        gxlRRC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");

        gxlRCC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
        gxlRLC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
        gxlRRC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");

        gxlNumb_DBCC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");
        gxlNumb_DBLC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");
        gxlNumb_DBRC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");

        gxlNumb_DBCC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");
        gxlNumb_DBLC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");
        gxlNumb_DBRC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");

        gxlNumb_RCC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");
        gxlNumb_RLC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");
        gxlNumb_RRC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");

        gxlNumb_RCC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");
        gxlNumb_RLC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");
        gxlNumb_RRC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");

        gxlDeci_DBCC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "D");
        gxlDeci_DBLC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "D");
        gxlDeci_DBRC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "D");

        gxlDeci_DBCC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "D");
        gxlDeci_DBLC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "D");
        gxlDeci_DBRC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "D");

        gxlDeci_RCC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "D");
        gxlDeci_RLC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "D");
        gxlDeci_RRC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", false, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "D");

        gxlDeci_RCC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "D");
        gxlDeci_RLC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "D");
        gxlDeci_RRC_Blue_Borders = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#1A4777", true, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "D");

        #endregion
        /*********************************************************************************/


        /*********************************************************************************/
        /**************************** Parameters Page Cell Styles ************************/
        /*********************************************************************************/
        #region Parameters Page Cell Styles

        xTitleCellstyle = xfnCELL_STYLE(sxlbook, sxlTitleFont, 20, "#002060", true, "#ffffff", "center", 0, 0, 0, 0, 0, "Thin", "#F8F9D0", "");
        xTitleCellstyle2 = xfnCELL_STYLE(sxlbook, sxlbodyFont, 14, "#0070C0", true, "#EEF5FC", "center", 1, 1, 1, 1, 1, "Thin", "#FFFFFF", "");
        xsubTitleintakeCellstyle = xfnCELL_STYLE(sxlbook, sxlbodyFont, 10, "#0070C0", true, "#F8FBFE", "center", 1, 1, 1, 1, 1, "Thin", "#F8F9D0", "");
        xPageTitleCellstyle = xfnCELL_STYLE(sxlbook, sxlTitleFont, 16, "#002060", true, "#ffffff", "center", 1, 1, 1, 1, 1, "Thin", "#F8F9D0", "");
        reportNameStyle = xfnCELL_STYLE(sxlbook, sxlTitleFont, 16, "#002060", true, "#CFE6F9", "center", 1, 1, 1, 1, 1, "Thin", "#FFFFFF", "");
        paramsCellStyle = xfnCELL_STYLE(sxlbook, sxlbodyFont, 8, "#000000", false, "#EEF5FC", "left", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
        gxlGenerate_lr = xfnCELL_STYLE(sxlbook, sxlTitleFont, 10, "#0070C0", false, "#FFFFFF", "left", 0, 1, 1, 1, 1, "Thin", "#d3e6f5", "");

        #endregion
        /*********************************************************************************/
    }

    public void xlRowsMerge(DevExpress.Spreadsheet.Worksheet worksheet, int Rowindex, int ColumnIndex, int numberOfColumns)
    {
        DevExpress.Spreadsheet.CellRange range = worksheet.Range.FromLTRB(ColumnIndex, Rowindex, ColumnIndex + numberOfColumns - 1, Rowindex);
        worksheet.MergeCells(range);
        range.Alignment.WrapText = true;
        
    }
    public void xlRowsMerge(DevExpress.Spreadsheet.Worksheet worksheet, int Rowindex, int ColumnIndex, int numberOfColumns, DevExpress.Spreadsheet.Style _cellStyle)
    {
        DevExpress.Spreadsheet.CellRange range = worksheet.Range.FromLTRB(ColumnIndex, Rowindex, ColumnIndex + numberOfColumns - 1, Rowindex);
        worksheet.MergeCells(range);
        range.Alignment.WrapText = true;
        range.Style = _cellStyle;
    }
    public void xlRowsMerge(DevExpress.Spreadsheet.Worksheet worksheet, int Rowindex, int ColumnIndex, int numberOfColumns, DevExpress.Spreadsheet.Style _cellStyle, string AutoFit)
    {
        DevExpress.Spreadsheet.CellRange range = worksheet.Range.FromLTRB(ColumnIndex, Rowindex, ColumnIndex + numberOfColumns - 1, Rowindex);
        worksheet.MergeCells(range);
        range.Alignment.WrapText = true;
        range.Style = _cellStyle;
        if (AutoFit == "Y")
        {
            //worksheet.Columns.AutoFit(range.LeftColumnIndex, range.RightColumnIndex);
            worksheet.Rows.AutoFit(range.TopRowIndex, range.BottomRowIndex);
        }
    }
    public void xlColsMerge(DevExpress.Spreadsheet.Worksheet worksheet, int startRowindex, int ColumnIndex, int endRowIndex)
    {
        //CellRange mergeRange = worksheet.Range.FromLTRB(0, Rowindex - 1, 0, numberOfRows - 2);
        DevExpress.Spreadsheet.CellRange mergeRange = worksheet.Range.FromLTRB(ColumnIndex, startRowindex, ColumnIndex, endRowIndex - 1);
        worksheet.MergeCells(mergeRange);
    }

    public void xlColsMerge(DevExpress.Spreadsheet.Worksheet worksheet, int startRowindex, int ColumnIndex, int endRowIndex, DevExpress.Spreadsheet.Style _cellStyle)
    {
        //CellRange mergeRange = worksheet.Range.FromLTRB(0, Rowindex - 1, 0, numberOfRows - 2);
        DevExpress.Spreadsheet.CellRange mergeRange = worksheet.Range.FromLTRB(ColumnIndex, startRowindex, ColumnIndex, endRowIndex - 1);
        worksheet.MergeCells(mergeRange);
        mergeRange.Alignment.WrapText = true;
        mergeRange.Style = _cellStyle;
    }

    public void xlColsMerge(DevExpress.Spreadsheet.Worksheet worksheet, int startRowindex,int LeftColIndex, int RightColumnIndex, int endRowIndex, DevExpress.Spreadsheet.Style _cellStyle)
    {
        //CellRange mergeRange = worksheet.Range.FromLTRB(0, Rowindex - 1, 0, numberOfRows - 2);
        DevExpress.Spreadsheet.CellRange mergeRange = worksheet.Range.FromLTRB(LeftColIndex, startRowindex, RightColumnIndex, endRowIndex - 1);
        worksheet.MergeCells(mergeRange);
        mergeRange.Alignment.WrapText = true;
        mergeRange.Style = _cellStyle;
    }

    public void AutoFitRows(DevExpress.Spreadsheet.Worksheet worksheet, int rowRange)
    {
        //var worksheet = _spreadsheetControl.Document.Worksheets[worksheetName];
        worksheet.Rows[rowRange].AutoFit();
    }
}

public class Devexpress_Excel_Properties_Font
{
    public Workbook sxlbook { get; set; }
    public string sxlTitleFont { get; set; }
    public string sxlTitleFontBgColor { get; set; }
    public string sxlTitleFontForeColor { get; set; }

    public string sxlbodyFont { get; set; }


    //*********************************** CELLS *****************************************///
    public Style gxlTitle_CellStyle1 { get; set; }
    public Style gxlTitle_CellStyle2 { get; set; }

    public Style gxlEMPTC { get; set; }

    public Style gxlEMPTL
    {
        get; set;
    }

    public Style gxlEMPTR
    {
        get; set;
    }

    public Style gxlERRMSG { get; set; }


    /*********************************************************************************/
    /**************************** NORMAL THEME CELL STYLE ****************************/
    /*********************************************************************************/
    public Style gxlNLHC { get; set; }
    public Style gxlNRHC { get; set; }
    public Style gxlNCHC { get; set; }

    public Style gxlNLC { get; set; }
    public Style gxlNRC { get; set; }

    public Style gxlNumb_NRC { get; set; }
    public Style gxlNumb_NRC_bo { get; set; }
    public Style gxlDeci_NRC { get; set; }
    public Style gxlDeci_NRC_bo
    {
        get; set;
    }
    public Style gxlNCC { get; set; }
    public Style gxlNumb_NCC { get; set; }
    public Style gxlDeci_NCC { get; set; }
    public Style gxlDate_NCC { get; set; }

    public Style gxlNCC_cr { get; set; }


    public Style gxlNLC_bo { get; set; }
    public Style gxlNRC_bo { get; set; }
    public Style gxlNCC_bo { get; set; }

    public Style gxlNLC_bo_cr { get; set; }
    public Style gxlNRC_bo_cr { get; set; }
    public Style gxlNCC_bo_cr { get; set; }


    /*********************************************************************************/
    /**************************** BLUE THEME CELL STYLE ****************************/
    /*********************************************************************************/
    public Style gxlBLHC_sp { get; set; }
    public Style gxlBRHC_sp { get; set; }
    public Style gxlBCHC_sp { get; set; }

    public Style gxlBLHC { get; set; }
    public Style gxlBRHC { get; set; }
    public Style gxlBCHC { get; set; }

    public Style gxlBLC { get; set; }
    public Style gxlBRC { get; set; }
    public Style gxlNumb_BRC { get; set; }
    public Style gxlDeci_BRC { get; set; }
    public Style gxlBCC { get; set; }

    public Style gxlBCC_cr { get; set; }


    public Style gxlBLC_bo { get; set; }
    public Style gxlBRC_bo { get; set; }
    public Style gxlBCC_bo { get; set; }

    public Style gxlBLC_bo_cr { get; set; }
    public Style gxlBRC_bo_cr { get; set; }
    public Style gxlBCC_bo_cr { get; set; }

    /*********************************************************************************/
    /**************************** GREEN THEME CELL STYLE ****************************/
    /*********************************************************************************/
    public Style gxlGLHC_sp { get; set; }
    public Style gxlGRHC_sp { get; set; }
    public Style gxlGCHC_sp { get; set; }
    public Style gxlGCHC_Highlite { get; set; }
    public Style gxlGCHL_Highlite { get; set; }
    public Style gxlGCHR_Highlite { get; set; }

    public Style gxlGLHC { get; set; }
    public Style gxlGRHC { get; set; }
    public Style gxlGCHC { get; set; }

    public Style gxlGLC { get; set; }
    public Style gxlGRC { get; set; }
    public Style gxlGCC { get; set; }
    public Style gxlNumb_GCC { get; set; }
    public Style gxlDeci_GCC { get; set; }
    public Style gxlDeci_GRC { get; set; }
    public Style gxlNumb_GRC { get; set; }
    
    public Style gxlGCC_cr { get; set; }
    public Style gxlNumbGCC_cr { get; set; }


    public Style gxlGLC_bo { get; set; }
    public Style gxlGRC_bo { get; set; }
    public Style gxlGCC_bo { get; set; }

    public Style gxlGLC_bo_cr { get; set; }
    public Style gxlGRC_bo_cr { get; set; }
    public Style gxlGCC_bo_cr { get; set; }

    /*********************************************************************************/
    /**************************** BROWN THEME CELL STYLE ****************************/
    /*********************************************************************************/
    public Style gxlBRLHC_sp { get; set; }
    public Style gxlBRRHC_sp { get; set; }
    public Style gxlBRCHC_sp { get; set; }

    public Style gxlBRLHC { get; set; }
    public Style gxlBRRHC { get; set; }
    public Style gxlBRCHC { get; set; }

    public Style gxlBRLC { get; set; }
    public Style gxlBRRC { get; set; }
    public Style gxlBRCC { get; set; }

    public Style gxlBRCC_cr { get; set; }


    public Style gxlBRLC_bo { get; set; }
    public Style gxlBRRC_bo { get; set; }
    public Style gxlBRCC_bo { get; set; }

    public Style gxlBRLC_bo_cr { get; set; }
    public Style gxlBRRC_bo_cr { get; set; }
    public Style gxlBRCC_bo_cr { get; set; }

    /*********************************************************************************/
    /**************************** PURPLE THEME CELL STYLE ****************************/
    /*********************************************************************************/
    public Style gxlPULHC_sp { get; set; }
    public Style gxlPURHC_sp { get; set; }
    public Style gxlPUCHC_sp { get; set; }

    public Style gxlPULHC { get; set; }
    public Style gxlPURHC { get; set; }
    public Style gxlPUCHC { get; set; }

    public Style gxlPULC { get; set; }
    public Style gxlPURC { get; set; }
    public Style gxlPUCC { get; set; }

    public Style gxlPUCC_cr { get; set; }


    public Style gxlPULC_bo { get; set; }
    public Style gxlPURC_bo { get; set; }
    public Style gxlPUCC_bo { get; set; }

    public Style gxlPULC_bo_cr { get; set; }
    public Style gxlPURC_bo_cr { get; set; }
    public Style gxlPUCC_bo_cr { get; set; }

    /*********************************************************************************/
    /**************************** Report Data Frame Styles ****************************/
    /*********************************************************************************/
    public Style gxlFrameStyleL
    {
        get; set;
    }
    public Style gxlFrameStyleR
    {
        get; set;
    }
    public Style gxlFrameStyleC
    {
        get; set;
    }

    public Style gxlFrameBlockStyleC
    {
        get; set;
    }
    public Style gxlFrameBlockStyleL
    {
        get; set;
    }
    public Style gxlFrameBlockStyleR
    {
        get; set;
    }


    public Style gxlFrameFooterStyle
    {
        get; set;
    }
    public Style gxlDBCC_Blue_Borders
    {
        get; set;
    }
    public Style gxlDBLC_Blue_Borders
    {
        get; set;
    }
    public Style gxlDBRC_Blue_Borders
    {
        get; set;
    }
    public Style gxlRCC_Blue_Borders
    {
        get; set;
    }
    public Style gxlDBLC_Blue_Borders_bo
    {
        get; set;
    }
    public Style gxlDBRC_Blue_Borders_bo
    {
        get; set;
    }
    public Style gxlDBCC_Blue_Borders_bo
    {
        get; set;
    }
    public Style gxlRLC_Blue_Borders
    {
        get; set;
    }
    public Style gxlRRC_Blue_Borders
    {
        get; set;
    }
    public Style gxlNumb_DBCC_Blue_Borders
    {
        get; set;
    }
    public Style gxlNumb_DBLC_Blue_Borders
    {
        get; set;
    }
    public Style gxlNumb_DBRC_Blue_Borders
    {
        get; set;
    }
    public Style gxlNumb_RCC_Blue_Borders
    {
        get; set;
    }
    public Style gxlNumb_RLC_Blue_Borders
    {
        get; set;
    }
    public Style gxlNumb_RRC_Blue_Borders
    {
        get; set;
    }
    public Style gxlDeci_DBCC_Blue_Borders
    {
        get; set;
    }
    public Style gxlDeci_DBLC_Blue_Borders
    {
        get; set;
    }
    public Style gxlDeci_DBRC_Blue_Borders
    {
        get; set;
    }
    public Style gxlDeci_RCC_Blue_Borders
    {
        get; set;
    }
    public Style gxlDeci_RLC_Blue_Borders
    {
        get; set;
    }
    public Style gxlDeci_RRC_Blue_Borders
    {
        get; set;
    }


    /*********************************************************************************/
    /**************************** Parameters Page Cell Styles ************************/
    /*********************************************************************************/
    public Style xTitleCellstyle
    {
        get; set;
    }
    public Style xTitleCellstyle2
    {
        get; set;
    }
    public Style xsubTitleintakeCellstyle
    {
        get; set;
    }

    public Style xPageTitleCellstyle
    {
        get; set;
    }
    public Style reportNameStyle
    {
        get; set;
    }
    public Style paramsCellStyle
    {
        get; set;
    }
    public Style gxlGenerate_lr
    {
        get; set;
    }
}

