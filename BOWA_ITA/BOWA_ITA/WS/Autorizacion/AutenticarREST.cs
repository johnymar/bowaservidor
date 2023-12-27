using ITA_CHILE.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static ITA_CHILE.Enum.Ambiente;

namespace ItaSystem.DTE.WS.Autorizacion
{
    public static class AutenticarRest
    {
        /// <summary>
        /// Obtiene el valor del token desde el string XML de respuesta del SII.
        /// </summary>
        /// <param name="tokenXml"></param>
        /// <returns></returns>
        private static bool ParseToken(string tokenXml, out string token)
        {
            XmlDocument doc = UtilidadesWS.GetDocument(tokenXml);

            int e = int.Parse(doc.GetElementsByTagName("ESTADO")[0].InnerText);

            if (e == 0)
            {
                token = doc.GetElementsByTagName("TOKEN")[0].InnerText;
                return true;
            }
            else
            {
                token = doc.GetElementsByTagName("GLOSA")[0].InnerText;
                return false;
            }
        }

        /// <summary>
        /// Obtiene el valor de la semilla desde la respueta XML del servicio GetSeed del SII.
        /// </summary>
        /// <param name="seed">String XMl respueta del servicio GetSeed del SII.</param>
        /// <returns>Valor de la semilla.</returns>
        private static bool ParseSeed(string seedXml, out string seed)
        {
            XmlDocument doc = UtilidadesWS.GetDocument(seedXml);

            int e = int.Parse(doc.GetElementsByTagName("ESTADO")[0].InnerText);

            if (e == 0)
            {
                var s = double.Parse(doc.GetElementsByTagName("SEMILLA")[0].InnerText);
                string resultado = string.Empty;
                string body = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?><getToken><item><Semilla>{0}</Semilla></item></getToken>", s);
                seed = body;
                return true;
            }
            else
            {
                seed = doc.GetElementsByTagName("GLOSA")[0].InnerText;
                return false;
            }
        }

        //private static string tokenFilePath = @"C:\Users\TH\Documents\Visual Studio 2013\Projects\DTE.DesktopClient\DTE.DesktopClient\bin\Debug\out\data\tkn.dat";

        private static bool ActiveTokenExists(out string activeToken, string fullFilePath)
        {
            try
            {
                string[] data = System.IO.File.ReadAllLines(fullFilePath);

                DateTime date = DateTime.Parse(data[0]);
                if ((DateTime.Now - date).TotalMinutes <= 3)
                {
                    activeToken = data[1];
                    return true;
                }
                else
                {
                    activeToken = string.Empty;
                    activeToken = "ERROR: MAS DE 3 MINUTOS";
                    return false;
                }

            }
            catch (Exception ex)
            {
                activeToken = "ERROR: " + ex.Message;
                return false;
            }
        }

        private static bool SaveToken(string token, string fullFileToken)
        {
            try
            {
                List<string> lines = new List<string>();
                lines.Add(DateTime.Now.ToString());
                lines.Add(token);
                System.IO.File.AppendAllLines(fullFileToken, lines);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private const int TRY_NUM = 3;
        public static string GetToken(string nombreCertificado, AmbienteEnum ambiente, string fullTokenPath, out string message, string password = "")
        {
            message = string.Empty;
            HttpServices httpServices = new HttpServices();
            for (int i = 0; i < TRY_NUM; i++)
            {
                try
                {
                    string activeToken;
                    if (ActiveTokenExists(out activeToken, fullTokenPath))
                    {
                        return activeToken;
                    }

                    string s = httpServices.WSGetSemilla(ambiente).Result;

                    if (!String.IsNullOrEmpty(s))
                    {
                        string formattedSeed = string.Empty;
                        if (!ParseSeed(s, out formattedSeed))
                        {
                            message += formattedSeed;
                            //return null;
                        }
                        else
                        {
                            string signedSeedXml = ITA_CHILE.Security.Firma.Firma.firmarDocumentoSemilla(formattedSeed, nombreCertificado, password);

                            string t = httpServices.WSGetToken(signedSeedXml, ambiente).Result;

                            string formattedToken = string.Empty;
                            if (!ParseToken(t, out formattedToken))
                            {
                                message += formattedToken;
                            }
                            else
                            {
                                if (SaveToken(formattedToken, fullTokenPath))
                                {
                                    message = string.Empty;
                                    return formattedToken;
                                }
                                else
                                {
                                    message += "Error al guardar Token: " + formattedToken;
                                    return null;
                                }
                            }
                        }
                    }
                    message += "SEED ITS EMPTY. (" + i + ")";
                    //return null;
                }
                catch (Exception ex)
                {
                    message += ex.Message;
                    //return null;
                }
            }
            message += "MAXIMO NUMERO DE INTENTOS DE OBTENER EL TOKEN ALCANZADOS";
            return null;
        }

    }
}
