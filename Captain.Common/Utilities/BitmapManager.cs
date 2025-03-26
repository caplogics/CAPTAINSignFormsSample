/************************************************************************************
* Class Name    : BitmapManager
* Author        : 
* Created Date  : 
* Version       : 1.0
* Description   : This class file has bitmap related methods
* 
*****************************************ReviewLog***********************************
* Author Version Date Description
*************************************************************************************
*
*************************************************************************************/

using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
using Captain.Common.Exceptions;

namespace Captain.Common.Utilities
{
    public class BitmapManager
    {
        public BitmapManager()
        {
        }

        public Image Copy(Image source, Rectangle section)
        {
            // Create the new bitmap and associated graphics object
            Image bmp = (Image)new Bitmap(section.Width, section.Height);
            Graphics g = Graphics.FromImage(bmp);

            // Draw the specified section of the source bitmap to the new one
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            // Clean up
            g.Dispose();

            // Return the bitmap
            return bmp;
        }

        public Bitmap BytesToBitmap(byte[] Bytes)
        {
            MemoryStream ms = new MemoryStream(Bytes);
            Bitmap bmp = (Bitmap)Bitmap.FromStream(ms);

            // Do NOT close the stream!
            return bmp;
        }

        public byte[] BitmapToBytes(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            byte[] bmpBytes = ms.GetBuffer();
            bitmap.Dispose();
            ms.Close();

            return bmpBytes;
        }

        public static string ImageToHex(Image image)
        {
            try
            {
                return Encodings.BytesToHex(Encodings.ImageToByteArray(image, ImageFormat.Gif));
            }
            catch (Exception ex)
            {
                StackFrame stackframe = new StackFrame(1, true);
                ExceptionLogger.LogException(stackframe, ex, ExceptionSeverityLevel.Low);
            }

            return "";
        }

        public static Image HexToImage(string hexData)
        {
            try
            {
                byte[] bytes = Encodings.HexToBytes(hexData);
                if (bytes.Length > 0)
                {
                    return Encodings.ByteArrayToImage(bytes);
                }
            }
            catch (Exception ex)
            {
                StackFrame stackframe = new StackFrame(1, true);
                ExceptionLogger.LogException(stackframe, ex, ExceptionSeverityLevel.Low);
            }

            return null;
        }
    }
}