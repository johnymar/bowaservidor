using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ITA_CHILE.Security.Encrypt
{
    public static class Encrypter
    {
        private static string passPhrase =
                            "NLd6fqCsQX6xeUpbJkSG" +
                            "SavVXb851sYpbRsHZuXP" +
                            "1kwq80oVX9zPHNc7e8V5" +
                            "tjWsxlP7mWTCHRwZlJMM" +
                            "eE1XKWRGlRtfLS5Ie50a" +
                            "Y5J6piwlQXpdCURzMjAj" +
                            "tKbnpgEbgaxtWImkKspR" +
                            "mHPFonwzdndkWFyAyRNA" +
                            "DZ7CW8s7LPaMVeOctV27" +
                            "WOPu1xuLmf5bOI81mUkp";

        private static string saltValue =
                            "TcVwzsc12PihGL7xr4LO" +
                            "96lWnvJTeeguaAjGNZdD" +
                            "6toTnabd5ZEuVSYCu0Et" +
                            "TnmWegCdSd9p4u9MR7rz" +
                            "vUL5dgy8t2a6JJlvURa8" +
                            "49eFzcgkFfI8sSUGEaxh" +
                            "075iLorkI0yBgU27t8Xe" +
                            "KXU8A4L6x6FHZIWcy2ME" +
                            "YIe9GBpnRzryRP5tFaTj" +
                            "whldEhztRWHWUUsCm89T";

        private static string hashAlgorithm = "SHA1";
        private static int passwordIterations = 7;
        private static string initVector = "#$/[%Ñ$ASs435#^%";
        private static int keySize = 256;

        /// <summary>
        /// Encrypts data and save it to filePath.
        /// </summary>
        /// <param name="data">Data to be encrypted.</param>
        /// <param name="filePath">File path to save the ecrypted data.</param>
        public static void EncryptToFile(string data, string filePath)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(initVector);
            byte[] rgbSalt = Encoding.ASCII.GetBytes(saltValue);
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            byte[] rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
            RijndaelManaged managed = new RijndaelManaged();
            managed.Mode = CipherMode.CBC;
            ICryptoTransform transform = managed.CreateEncryptor(rgbKey, bytes);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            byte[] inArray = stream.ToArray();
            stream.Close();
            stream2.Close();
            File.WriteAllText(filePath, Convert.ToBase64String(inArray));
        }

        /// <summary>
        /// Decrypts filePath content.
        /// </summary>
        /// <param name="filePath">Filepath to the file that will be decrypted.</param>
        /// <returns>Decrypted data.</returns>
        public static string DecryptFileContent(string filePath)
        {
            string data = File.ReadAllText(filePath);
            byte[] bytes = Encoding.ASCII.GetBytes(initVector);
            byte[] rgbSalt = Encoding.ASCII.GetBytes(saltValue);
            byte[] buffer = Convert.FromBase64String(data);
            byte[] rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
            RijndaelManaged managed = new RijndaelManaged();
            managed.Mode = CipherMode.CBC;
            ICryptoTransform transform = managed.CreateDecryptor(rgbKey, bytes);
            MemoryStream stream = new MemoryStream(buffer);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            byte[] buffer5 = new byte[buffer.Length];
            int count = stream2.Read(buffer5, 0, buffer5.Length);
            stream.Close();
            stream2.Close();
            return Encoding.UTF8.GetString(buffer5, 0, count);
        }
    }
}
