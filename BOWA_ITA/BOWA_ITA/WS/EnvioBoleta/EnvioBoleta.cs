using ItaSystem.DTE.WS.EnvioDTE;
using Newtonsoft.Json;
using ITA_CHILE.WS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static ITA_CHILE.Enum.Ambiente;
using System.util;

namespace ItaSystem.DTE.WS.EnvioBoleta
{
    public class EnvioBoleta
    {
        public static bool ParseRespuesta(string response, out EnvioDTEResult result)
        {
            try
            {
                result = new EnvioDTEResult();
                XmlDocument doc = UtilidadesWS.GetDocument(response);

                result.ResponseXml = response;
                result.RutEnvia = doc.GetElementsByTagName("RUTSENDER")[0].InnerText;
                result.RutEmpresa = doc.GetElementsByTagName("RUTCOMPANY")[0].InnerText;
                result.File = doc.GetElementsByTagName("FILE")[0].InnerText;
                result.Fecha = DateTime.ParseExact(doc.GetElementsByTagName("TIMESTAMP")[0].InnerText, "yyyy-MM-dd HH:mm:ss", null);
                result.Estado = doc.GetElementsByTagName("STATUS")[0].InnerText;

                foreach (XmlNode e in doc.GetElementsByTagName("TRACKID"))
                    result.Errores += e.InnerText + Environment.NewLine;

                try
                {
                    result.TrackId = long.Parse(doc.GetElementsByTagName("TRACKID")[0].InnerText);
                }
                catch { }
                return true;
            }
            catch (Exception ex)
            {
                result = new EnvioDTEResult();
                result.Estado = "ex";
                result.Errores = ex.Message;
                result.ResponseXml = response;
                return false;
            }
        }

        public static EnvioDTEResult Enviar(string rutEmisor, string rutEmpresa, string filePath, string nombreCertificado, AmbienteEnum ambiente, out string error, string tokenFullPath, string password = "")
        {
            #region Autorizacion
            HttpServices httpServices = new HttpServices();
            string message = "";
            string token = Autorizacion.AutenticarRest.GetToken(nombreCertificado, ambiente, tokenFullPath, out message, password);

            if (!String.IsNullOrEmpty(message))
            {
                token = string.Empty;
                error = "Error al recuperar el token." + Environment.NewLine + Environment.NewLine + "Error: " + message;
                return null;
            }

            #endregion
            error = "";

            // Lea el documento xml que se va a enviar al SII
            XDocument xdocument = XDocument.Load(filePath, LoadOptions.PreserveWhitespace);            
            
            try
            {
                var archivoSplitted = filePath.Split('\\');

                var body = new BodyEnvioBoleta() {
                    rutSender = int.Parse(rutEmisor.Substring(0, rutEmisor.Length - 2)),
                    dvSender = rutEmisor.Substring(rutEmisor.Length - 1).ToUpper(), 
                    rutCompany = int.Parse(rutEmpresa.Substring(0, rutEmpresa.Length - 2)),
                    dvCompany = (rutEmpresa.Substring(rutEmpresa.Length - 1)),
                    archivo = archivoSplitted[archivoSplitted.Length - 1]
                };
               
                var respuestaSii = httpServices.WSPostEnvio(body, token, filePath, out error, ambiente);               
                try
                {
                    var objeto = Newtonsoft.Json.JsonConvert.DeserializeObject<RespuestaSIIBoleta>(respuestaSii);
                    return new EnvioDTEResult()
                    {
                        RutEnvia = objeto.rut_envia,
                        RutEmpresa = objeto.rut_emisor,
                        TrackId = objeto.trackid,
                        Fecha = objeto.fecha_recepcion,
                        Estado = objeto.estado,
                        File = objeto.file
                    };

                }
                catch (Exception ex)
                {
                    error += "Error: " + ex.Message;
                    return new EnvioDTEResult()
                    {
                        Errores = ex.Message + ex.StackTrace,
                        Estado = "ERROR",
                        ResponseXml = respuestaSii
                    };
                }
            }
            catch (Exception ex)
            {
                return new EnvioDTEResult() { 
                    Errores = ex.Message,
                    Estado = "ERROR"                    
                };
            }
        }
    }
    public class RespuestaSIIBoleta
    {
        public string rut_envia { get; set; }
        public string rut_emisor { get; set; }
        public long trackid { get; set; }
        public DateTime fecha_recepcion { get; set; }
        public string estado { get; set; }
        public string file { get; set; }
    }
    public class BodyEnvioBoleta
    { 
        public int rutSender { get; set; }
        public string dvSender { get; set; }

        public int rutCompany { get; set; }

        public string dvCompany { get; set; }

        public string archivo { get; set; }
    }
    public class BodyDetail
    {
        public string ApiKey { get; set; }
        public int Cantidad { get; set; }
        public string RutEmpresa { get; set; }
        public string TipoEnvio { get; set; }

    }
}
