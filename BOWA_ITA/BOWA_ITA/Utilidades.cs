using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ItaSystem.DTE.Engine
{
    public static class Utilidades
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);

        public static string BreakTextEveryNCharacters(string text, int characterPerLine = 76)
        {
            return text;
            //return System.Text.RegularExpressions.Regex.Replace(text, "(.{" + characterPerLine + "})", "$1" + Environment.NewLine);
        }

        

        public static List<string> ObtenerCertificadosMaquinas()
        {
            X509Store store = new X509Store(StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var retorno = new List<string>();
            foreach (X509Certificate2 c in store.Certificates)
            {
                retorno.Add(c.FriendlyName);
            }
            return retorno;
        }

        public static List<string> ObtenerCertificadosUsuario()
        {
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var retorno = new List<string>();
            foreach (X509Certificate2 c in store.Certificates)
            {
                retorno.Add(c.FriendlyName);
            }
            return retorno;
        }

        public static string RandomString(int size, bool digitsOnly)
        {
            if (digitsOnly)
            {
                return (Math.Floor((random.NextDouble() * (Math.Pow(10, size))))).ToString().PadLeft(size, '0');
            }
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public static string ConvertEncode(string text)
        {
            //var str = 
            //        Encoding.GetEncoding("ISO-8859-1")
            //        .GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("ISO-8859-1"), Encoding.UTF8.GetBytes(text)));
            //return str;
            return System.Security.SecurityElement.Escape(text);
        }

        public static string EncodeToISO88581(string text)
        {
            Encoding iso8859 = Encoding.GetEncoding("ISO-8859-1");
            Encoding unicode = Encoding.Unicode;
            byte[] srcTextBytes = iso8859.GetBytes(text);
            byte[] destTextBytes = Encoding.Convert(iso8859, unicode, srcTextBytes);
            char[] destChars = new char[unicode.GetCharCount(destTextBytes, 0, destTextBytes.Length)];
            unicode.GetChars(destTextBytes, 0, destTextBytes.Length, destChars, 0);
            StringBuilder result = new StringBuilder(text.Length + (int)(text.Length * 0.1));
            foreach (char c in destChars)
            {
                int value = Convert.ToInt32(c);
                if (value == 34)
                    result.AppendFormat("&quot;");
                else if (value == 38)
                    result.AppendFormat("&amp;");
                else if (value == 39)
                    result.AppendFormat("&apos;");
                else if (value == 60)
                    result.AppendFormat("&lt;");
                else if (value == 62)
                    result.AppendFormat("&gt;");
                else
                    result.Append(c);
            }
            return result.ToString();     
        }
    }
}
