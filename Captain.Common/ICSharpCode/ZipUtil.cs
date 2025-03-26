using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace ViewPoint.Common.ICSharpCode
{
    public static class ZipUtil
    {
        /// <summary>
        /// Zips a list of files.
        /// </summary>
        /// <param name="files">The list of files to zip.</param>
        /// <param name="zipFilePathName">The full file path of the zip to create.</param>
        /// <param name="password">The password of the zip. Set to null if no password.</param>
        public static void ZipFiles(List<String> files, string zipFilePathName, string password)
        {
            using (FileStream fileStream = File.Create(zipFilePathName))
            {
                using (ZipOutputStream zipStream = new ZipOutputStream(fileStream))
                {
                    zipStream.SetLevel(0); //0-9, 9 being the highest level of compression

                    zipStream.Password = password;	// optional. Null is the same as not setting.

                    foreach (string filename in files)
                    {
                        FileInfo fileInfo = new FileInfo(filename);
                        long fileSize = fileInfo.Length;
                        string entryName = fileInfo.Name; 

                        fileInfo = null;

                        ZipEntry newEntry = new ZipEntry(entryName);
                        newEntry.DateTime = DateTime.Now;

                        // Specifying this to encrypt the file. Allowable values are 0 (off), 128 or 256.
                        newEntry.AESKeySize = 0;

                        // Use this to make it compatible with other zip utilities.
                        // zipStream.UseZip64 = UseZip64.Off;
                        newEntry.Size = fileSize;

                        zipStream.PutNextEntry(newEntry);

                        // Zip the file in buffered chunks
                        // the "using" will close the stream even if an exception occurs
                        byte[] buffer = new byte[4096];
                        using (FileStream streamReader = File.OpenRead(filename))
                        {
                            StreamUtils.Copy(streamReader, zipStream, buffer);
                        }
                        zipStream.CloseEntry();
                    }
                    zipStream.IsStreamOwner = true;	// Makes the Close also Close the underlying stream
                    zipStream.Close();
                }
            }
        }

        /// <summary>
        /// Extracts files from a zip to a folder.
        /// </summary>
        /// <param name="zipFilePathName">The full path to the zip file</param>
        /// <param name="outputFolder">The full path where to extract the files.</param>
        /// <param name="password">The password of the zip file.</param>
        public static void ExtractFiles(string zipFilePathName, string outputFolder, string password)
        {
            ZipFile zipFile = null;
            try
            {
                using (FileStream fileStream = File.OpenRead(zipFilePathName))
                {
                    zipFile = new ZipFile(fileStream);
                    if (!String.IsNullOrEmpty(password))
                    {
                        zipFile.Password = password;		// AES encrypted entries are handled automatically
                    }
                    foreach (ZipEntry zipEntry in zipFile)
                    {
                        if (!zipEntry.IsFile)
                        {
                            continue;			// Ignore directories
                        }
                        String entryFileName = zipEntry.Name;
                        // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                        // Optionally match entrynames against a selection list here to skip as desired.
                        // The unpacked length is available in the zipEntry.Size property.

                        byte[] buffer = new byte[4096];		// 4K is optimum
                        Stream zipStream = zipFile.GetInputStream(zipEntry);

                        // Manipulate the output filename here as desired.
                        String fullZipToPath = Path.Combine(outputFolder, entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0) { Directory.CreateDirectory(directoryName); }

                        // Unzip file in buffered chunks to conserve memory
                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                    }
                    fileStream.Close();
                }
            }
            finally
            {
                if (zipFile != null)
                {
                    zipFile.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zipFile.Close(); // Ensure we release resources
                }
            }
        }
    }
}
