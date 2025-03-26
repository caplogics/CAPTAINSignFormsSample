/************************************************************************************
* Class Name    : ImageGateway
* Author        : 
* Created Date  : 
* Version       : 1.0
* Description   : 
* 
*****************************************ReviewLog***********************************
* Author Version Date Description
*************************************************************************************
*
*************************************************************************************/

#region Using

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System;
using System.Drawing.Drawing2D;

#endregion

namespace Captain.Common.Utilities
{
    public class ImageGateway //: IGatewayHandler
    {
        private List<Image> _images = new List<Image>() ;
        private List<ImageTypes> _imageTypes = new List<ImageTypes>();

        public ImageGateway()
        {
        }

        //TODO: Wisej.... (Alaa: I don't think this needs migration?)
        //public void ProcessGatewayRequest(IContext objContext, IRegisteredComponent objComponent)
        //{
        //    string[] reqPath = objContext.HttpContext.Request.CurrentExecutionFilePath.Split('.');
        //    int imageIndex = int.Parse(reqPath[reqPath.Length - 2]);
        //    objContext.HttpContext.Response.ContentType = "image/gif";
        //    _images[imageIndex].Save(objContext.HttpContext.Response.OutputStream, ImageFormat.Gif);
        //    switch (_imageTypes[imageIndex])
        //    {
        //        case ImageTypes.Gif:
        //            HttpContext.Current.Response.ContentType = "image/gif";
        //            this._images[imageIndex].Save(HttpContext.Current.Response.OutputStream, ImageFormat.Gif);
        //            break;
        //        case ImageTypes.Jpg:
        //            HttpContext.Current.Response.ContentType = "image/jpeg";
        //            this._images[imageIndex].Save(HttpContext.Current.Response.OutputStream, ImageFormat.Jpeg);
        //            break;
        //        case ImageTypes.Png:
        //            HttpContext.Current.Response.ContentType = "image/png";
        //            this._images[imageIndex].Save(HttpContext.Current.Response.OutputStream, ImageFormat.Png);
        //            break;
        //    }
        //}

        public void GenerateImages(List<string> dynamicHexs, ImageTypes imageType)
        {
            this._images.Clear();
            this._imageTypes.Clear();
            foreach (string dynamicHex in dynamicHexs)
            {
                byte[] bytes = Encodings.HexToBytes(dynamicHex);
                if (bytes.Length > 0)
                {
                    this._images.Add(Encodings.ByteArrayToImage(bytes));
                    this._imageTypes.Add(imageType);
                }
            }
        }

        /// <summary>
        /// Creates a single image from multiple images
        /// </summary>
        /// <param name="imagesToMerge">List<string> imagesToMerge</param>
        /// <param name="imageType">ImageTypes</param>
        public void GenerateMergedImage(List<string> imagesToMerge, ImageTypes imageType)
        {
            System.Drawing.Graphics myGraphic = null;
            Image mainImage = null;
            if (imagesToMerge.Count >= 0)
            {
                mainImage = Image.FromFile(imagesToMerge[0]);
                Graphics.FromImage(mainImage);

                this._images.Clear();
                this._imageTypes.Clear();

                int _x = 0;

                foreach (string imageFile in imagesToMerge)
                {
                    Image img = Image.FromFile(imageFile);
                    if (!img.Equals(mainImage))
                    {
                        myGraphic.DrawImageUnscaled(img, _x, 0);
                        _x += img.Width;
                    }
                }
                myGraphic.Save();
                this._images.Add(mainImage);
                this._imageTypes.Add(imageType);
            }
        }
    }
}

