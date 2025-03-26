/************************************************************************************
* Class Name    : EncryptionManager
* Author        : 
* Created Date  : 
* Version       : 1.0
* Description   : This class has  encrypted & decrypted methods
*************************************************************************************/

#region Using

using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml; 

#endregion

namespace Captain.Common.Utilities
{
    public class EncryptionManager
    {
        private RijndaelManaged _cipher = null;
        private static string _secretPhrase = "Some secret key phrase";

        private string _data = "";
        private string _iv = "";
        private string _key = "";

        public EncryptionManager()
        {
            _cipher = new RijndaelManaged();
            _cipher.KeySize = 256;
            _cipher.BlockSize = 256;
            _cipher.Mode = CipherMode.CBC;
            _cipher.Padding = PaddingMode.PKCS7;
        }

        public string GenerateKey()
        {
            _cipher.GenerateKey();
            _key = ToString2(_cipher.Key);

            return _key;
        }

        public string GenerateIV()
        {
            _cipher.GenerateIV();
            _iv = ToString2(_cipher.Key);
            return _iv;
        }

        /// <summary>
        /// DES Encryption method - used to encryp password for the java.
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string EncryptData(string plainText)
        {
            DES des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;

            des.Key = Encoding.UTF8.GetBytes(_secretPhrase.Substring(0, 8));
            des.IV = Encoding.UTF8.GetBytes(_secretPhrase.Substring(0, 8));

            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            byte[] resultBytes = des.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);

            return Convert.ToBase64String(resultBytes);
        }

        /// <summary>
        /// DES Decryption method - used the decrypt password encrypted in java
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public string DecryptData(string encryptedText)
        {
            DES des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;
            des.Key = Encoding.UTF8.GetBytes(_secretPhrase.Substring(0, 8));
            des.IV = System.Text.Encoding.UTF8.GetBytes(_secretPhrase.Substring(0, 8));

            byte[] bytes = Convert.FromBase64String(encryptedText);
            byte[] resultBytes = des.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length);

            return Encoding.UTF8.GetString(resultBytes);
        }

        public string EncryptMessage(string message, string key, string iv)
        {
            byte[] lkey = GetBytes(key);
            byte[] liv = GetBytes(iv);
            ICryptoTransform transform = _cipher.CreateEncryptor(lkey, liv);
            byte[] plainText = Encoding.Unicode.GetBytes(message);
            byte[] cipherText = transform.TransformFinalBlock(plainText, 0, plainText.Length);

            _data = ToString2(cipherText);

            return _data;
        }

        public string DecryptMessage(string message, string key, string iv)
        {
            byte[] lkey = GetBytes(key);
            byte[] liv = GetBytes(iv);
            byte[] cipherText = GetBytes(message);
            ICryptoTransform transform = _cipher.CreateDecryptor(lkey, liv);
            byte[] plainText = transform.TransformFinalBlock(cipherText, 0, cipherText.Length);

            return Encoding.Unicode.GetString(plainText, 0, plainText.Length);
        }

        public string EncryptMessage(string message)
        {
            _cipher.GenerateKey();

            _cipher.GenerateIV();

            ICryptoTransform transform = _cipher.CreateEncryptor();

            byte[] plainText = Encoding.Unicode.GetBytes(message);

            byte[] cipherText = transform.TransformFinalBlock(plainText, 0, plainText.Length);

            _data = ToString2(cipherText);
            _iv = ToString2(_cipher.IV);
            _key = ToString2(_cipher.Key);

            return _key + "-" + _data + "-" + _iv;
        }

        public string DecryptMessage(string message)
        {
            string[] temp = message.Split('-');

            _cipher.Key = GetBytes(temp[0]);

            byte[] cipherText = GetBytes(temp[1]);

            _cipher.IV = GetBytes(temp[2]);

            ICryptoTransform transform = _cipher.CreateDecryptor();

            byte[] plainText = transform.TransformFinalBlock(cipherText, 0, cipherText.Length);

            return Encoding.Unicode.GetString(plainText);
        }

        private int GetByteCount(string hexString)
        {
            int numHexChars = 0;
            char c;

            for (int iByteIndex = 0; iByteIndex <= hexString.Length - 1; iByteIndex++)
            {
                c = hexString[iByteIndex];
                if (IsHexDigit(c))
                {
                    numHexChars++;
                }
            }

            if ((numHexChars % 2) != 0)
            {
                numHexChars--;
            }
            return numHexChars / 2;
        }

        private byte[] GetBytes(string hexString)
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

        private string ToString2(byte[] bytes)
        {
            int bytesLenth = bytes.Length;

            StringBuilder hexString = new StringBuilder(bytesLenth);
            for (int byteIndex = 0; byteIndex < bytesLenth; byteIndex++)
            {
                hexString.Append(bytes[byteIndex].ToString("X2"));
            }
            return hexString.ToString();
        }

        private bool InHexFormat(string hexString)
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

        private bool IsHexDigit(char c)
        {
            int numChar = 0;
            int numA = Convert.ToInt32('A');
            int num1 = Convert.ToInt32('0');
            c = char.ToUpper(c);
            numChar = Convert.ToInt32(c);
            if (numChar >= numA && numChar < (numA + 6))
            {
                return true;
            }
            if (numChar >= num1 && numChar < (num1 + 10))
            {
                return true;
            }
            return false;
        }

        private byte HexToByte(string hex)
        {
            if (hex.Length > 2 || hex.Length <= 0)
            {
                throw (new ArgumentException("hex must be 1 or 2 characters in length"));
            }
            byte newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return newByte;
        }

    }
}
