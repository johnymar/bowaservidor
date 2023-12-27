using ItaSystem.DTE.Engine.Documento;
using ItaSystem.DTE.Engine.XML;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ItaSystem.DTE.Engine.CAFHandler
{
    public static class CAFHandler
    {
        //public static string CAFFOLDERPATH { get { return Config.Resources.CAFFOLDERPATH; } }

        public static CAF GetCAF(string _CAFFilePath)
        {            
            //var aut = XmlHandler.DeserializeFromString<Autorizacion>(_CAFFilePath);
            var aut = XmlHandler.DeserializeRaw<Autorizacion>(_CAFFilePath);
            //aut.CAF.cafString = File.ReadAllText(_CAFFilePath);
            ////aut.CAF.Datos.RazonSocialEmisor = aut.CAF.Datos.RazonSosdfcialEmisor.Replace("\"", "&quot;");
            ////aut.CAF.Datos.RazonSocialEmisor = SecurityElement.Escape(aut.CAF.Datos.RazonSocialEmisor);

            ////string s = GetBetween(aut.CAF.cafString, "<CAF version=\"1.0\">", "</CAF>");
            //string s = GetBetween(aut.CAF.cafString, "<AUTORIZACION>", "<RSASK>");
            //string ss = s.Substring(1, s.Length - 2);
            ////aut.CAF.Datos.RazonSocialEmisor = SecurityElement.Escape(aut.CAF.Datos.RazonSocialEmisor);

            //aut.CAF.cafString = ss;

            //XmlDocument xml = new XmlDocument();
            //xml.LoadXml(ss);


            aut.CAF.IdCAF = 1;
            return aut.CAF;
        }

        private static string GetBetween(string content, string startString, string endString)
        {
            int Start = 0, End = 0;
            if (content.Contains(startString) && content.Contains(endString))
            {
                Start = content.IndexOf(startString, 0) + startString.Length;
                End = content.IndexOf(endString, Start);
                return content.Substring(Start, End - Start);
            }
            else
                return string.Empty;
        }

        public static string GetPrivateKey(string _CAFFilePath)
        {
            var aut = XmlHandler.DeserializeRaw<Autorizacion>(_CAFFilePath);
            return aut.ClavePrivadaRSA;
        }
    }
}
