using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain.Common.Utilities
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters;
    using System.Xml.Serialization;
    using System.Xml;

    public class SerializeArray
    {
        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private static string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        private static Byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        } 

        /// <summary>
        /// Serializes an array to an xml file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectArray"></param>
        public static string SerializeObject<T>(T objectArray)
        {
            try
            {
                string xmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(typeof(T));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                xs.Serialize(xmlTextWriter, objectArray);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                xmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());

                return xmlizedString;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// De-serializes an xml string to an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T DeSerializeObject<T>(string xmlString)
        {
            T objectArray;

            XmlSerializer xs = new XmlSerializer(typeof(T));
            
            using (MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xmlString)))
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                objectArray = (T)xs.Deserialize(memoryStream);

                memoryStream.Close();
            }

            return objectArray;
        }
    }

}
