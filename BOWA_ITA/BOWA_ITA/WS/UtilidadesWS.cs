using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ITA_CHILE.WS
{
    public static class UtilidadesWS
    {
        internal static XmlDocument GetDocument(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.LoadXml(xml);

            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("SII", doc.DocumentElement.NamespaceURI);

            return doc;
        }

        internal static bool TryParseAtencionString(string text, out int numeroAtencion, out DateTime fechaAtencion)
        {
            try
            {
                text = System.Text.RegularExpressions.Regex.Replace(text.Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Trim(), "[ ]{2,}", " ");

                numeroAtencion = int.Parse((text.Substring(0, text.IndexOf(" ") + 1).Trim()));
                try { fechaAtencion = DateTime.ParseExact((text.Substring(text.IndexOf(" ") + 1).Trim()), "yyyy/MM/dd HH:mm:ss", null); }
                catch { fechaAtencion = DateTime.ParseExact((text.Substring(text.IndexOf(" ") + 1).Trim()), "dd/MM/yyyy HH:mm:ss", null); }
                return true;
            }
            catch
            {
                numeroAtencion = -1;
                fechaAtencion = DateTime.MinValue;
                return false;
            }
        }
    }
}
