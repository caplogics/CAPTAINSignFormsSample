/************************************************************************************
* Class Name    : Encodings
* Author        : 
* Created Date  : 
* Version       : 1.0
* Description   : This class has  data type conversion methods
* 
*****************************************ReviewLog***********************************
* Author Version Date Description
*************************************************************************************
*
*************************************************************************************/

#region Using

using System;
using System.IO;
using System.Drawing;
using System.Text; 

#endregion

namespace Captain.Common.Utilities
{
    /// <summary>
    /// Summary description for HexEncoding.
    /// </summary>
    public class Encodings
    {
        static char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public Encodings()
        {
        }

        /// <summary>
        /// Get the lenth of bytes in a hex string.
        /// </summary>
        /// <param name="hexString">The hex string to get the count from.</param>
        /// <returns>Returns an integer.</returns>
        public static int GetByteCount(string hexString)
        {
            int numHexChars = 0;
            char tempChar;
            for (int iBytes = 0; iBytes < hexString.Length; iBytes++)
            {
                tempChar = hexString[iBytes];
                if (IsHexDigit(tempChar)) numHexChars++;
            }

            if (numHexChars % 2 != 0)
            {
                numHexChars--;
            }

            return numHexChars / 2;
        }

        /// <summary>
        /// Converts an image to bytes.
        /// </summary>
        /// <param name="imageIn">The image to convert to bytes.</param>
        /// <param name="imageFormat">The format of the image.</param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(System.Drawing.Image imageIn, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, imageFormat);
            return ms.ToArray();
        }

        /// <summary>
        /// Converts bytes to image.
        /// </summary>
        /// <param name="byteArrayIn">The array of bytes</param>
        /// <returns>Returns an image</returns>
        public static System.Drawing.Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        /// <summary>
        /// Creates a byte array from the hexadecimal string. Each two characters are combined
        /// to create one byte. First two hexadecimal characters become first byte in returned array.
        /// Non-hexadecimal characters are ignored. 
        /// </summary>
        /// <param name="hexString">string to convert to byte array</param>
        /// <returns>byte array, in the same left-to-right order as the hexString</returns>
        public static byte[] HexToBytes(string hexString)
        {
            int byteLength = hexString.Length / 2;
            byte[] bytes = new byte[byteLength];
            string hex;
            int jHexIndex = 0;
            for (int iByteIndex = 0; iByteIndex <= bytes.Length - 1; iByteIndex++)
            {
                hex = new string(new char[] { hexString[jHexIndex], hexString[jHexIndex + 1] });
                bytes[iByteIndex] = HexToByte(hex);
                jHexIndex = jHexIndex + 2;
            }
            return bytes;
        }

        /// <summary>
        /// Creates a byte array from a normal string.
        /// </summary>
        /// <param name="normalString">string to convert to byte array</param>
        /// <returns>byte array, in the same left-to-right order as the NormalString</returns>
        public static byte[] StringToBytes(string normalString)
        {
            return System.Text.Encoding.ASCII.GetBytes(normalString);
        }

        /// <summary>
        /// Creates a string from a byte array
        /// </summary>
        /// <param name="bytes">bytes to convert to string</param>
        /// <returns>string representation of bytes.</returns>
        public static string BytesToString(byte[] bytes)
        {
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Creates a hex string from a byte array
        /// </summary>
        /// <param name="bytes">bytes to convert to hex string</param>
        /// <returns>hex string representation of bytes.</returns>
        public static string BytesToHex(byte[] bytes)
        {
            int bytesLenth = bytes.Length;

            StringBuilder hexString = new StringBuilder(bytesLenth);
            for (int i = 0; i < bytesLenth; i++)
            {
                hexString.Append(bytes[i].ToString("X2"));
            }
            return hexString.ToString();
        }

        /// <summary>
        /// Determines if given string is in proper hexadecimal string format
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static bool InHexFormat(string hexString)
        {
            bool hexFormat = true;

            foreach (char digit in hexString)
            {
                if (!IsHexDigit(digit))
                {
                    hexFormat = false;
                    break;
                }
            }
            return hexFormat;
        }

        /// <summary>
        /// Returns true is tempChar is a hexadecimal digit (A-F, a-f, 0-9)
        /// </summary>
        /// <param name="tempChar">Character to test</param>
        /// <returns>true if hex digit, false if not</returns>
        public static bool IsHexDigit(Char tempChar)
        {
            int numChar;
            int numA = Convert.ToInt32('A');
            int num1 = Convert.ToInt32('0');
            tempChar = Char.ToUpper(tempChar);
            numChar = Convert.ToInt32(tempChar);
            if (numChar >= numA && numChar < (numA + 6)) { return true; }
            if (numChar >= num1 && numChar < (num1 + 10)) { return true; }

            return false;
        }

        /// <summary>
        /// Converts 1 or 2 character string into equivalant byte value
        /// </summary>
        /// <param name="hex">1 or 2 character string</param>
        /// <returns>byte</returns>
        private static byte HexToByte(string hex)
        {
            if (hex.Length > 2 || hex.Length <= 0) { throw new ArgumentException("hex must be 1 or 2 characters in length"); }
            byte newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);

            return newByte;
        }

        /// <summary>
        /// Convert a .NET Color to a hex string.
        /// </summary>
        /// <returns>ex: "FFFFFF", "AB12E9"</returns>
        public static string ColorToHexString(Color color)
        {
            byte[] bytes = new byte[3];
            bytes[0] = color.R;
            bytes[1] = color.G;
            bytes[2] = color.B;
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }
    }
}