using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace vita.Utils
{
    public class FileIO
    {
        public static string GetFileInBas64(string filePath)
        {
            string encodedContent = String.Empty;
            if (new FileInfo(filePath).Length > 0)
            {
                try
                {
                    var fileContent = File.ReadAllBytes(filePath);
                    var memoryStream = new MemoryStream(fileContent, 0, fileContent.Length, writable: false, publiclyVisible: true);
                    encodedContent = Base64Conversion.Encode(Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return encodedContent;

        }
        public static string GetSha256FileHash(string filePath)
        {
            string hash = String.Empty;
            if (new FileInfo(filePath).Length > 0)
            {
                try
                {
                    var fileContent = File.ReadAllBytes(filePath);
                    var memoryStream = new MemoryStream(fileContent, 0, fileContent.Length, writable: false, publiclyVisible: true);
                    hash = GetSha256StringHash(Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return hash;

        }
        public static string GetSha256StringHash(string text)
        {
            string hashString = string.Empty;
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            using (var myHash = new SHA256Managed())
            {
                byte[] hash = myHash.ComputeHash(bytes);
                foreach (byte x in hash)
                {
                    hashString += String.Format("{0:x2}", x);//hexadecimal
                }
            }
            return hashString;
        }
    }
}
