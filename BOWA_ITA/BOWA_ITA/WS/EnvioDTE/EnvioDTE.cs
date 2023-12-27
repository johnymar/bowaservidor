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

namespace ItaSystem.DTE.WS.EnvioDTE
{
    public class EnvioDTE
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
                
                result.Estado = doc.GetElementsByTagName("STATUS")[0].InnerText;

                foreach (XmlNode e in doc.GetElementsByTagName("TRACKID"))
                    result.Errores += e.InnerText + Environment.NewLine;

                try
                {
                    result.TrackId = long.Parse(doc.GetElementsByTagName("TRACKID")[0].InnerText);
                }
                catch { }
                try 
                {
                    result.Fecha = DateTime.ParseExact(doc.GetElementsByTagName("TIMESTAMP")[0].InnerText, "yyyy-MM-dd HH:mm:ss", null);
                }
                catch 
                {
                }
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

        public static EnvioDTEResult Enviar(string rutEnvia, string rutEmpresa, string fileName, string nombreCertificado, AmbienteEnum ambiente, out string error, string tokenFullPath, string password="")
        {
            #region Autorizacion
            string message = "";
            string token = Autorizacion.Autenticar.GetToken(nombreCertificado, ambiente, out message, tokenFullPath, password);

            if (!String.IsNullOrEmpty(message))
            {
                token = string.Empty;
                error = "Error al recuperar el token." + Environment.NewLine + Environment.NewLine + "Error: " + message;
                return null;
            }

            #endregion

            #region PREPARACION

            // Lea el documento xml que se va a enviar al SII
            XDocument xdocument = XDocument.Load(fileName, LoadOptions.PreserveWhitespace);     
            StringBuilder secuencia = new StringBuilder();

            var numeroRutEmisor = int.Parse(rutEnvia.Substring(0, rutEnvia.Length - 2));
            var digitoVerificadorEmisor = rutEnvia.Substring(rutEnvia.Length - 1).ToUpper();
            var numeroRutEmpresa = int.Parse(rutEmpresa.Substring(0, rutEmpresa.Length - 2));
            var digitoVerificadorEmpresa = rutEmpresa.Substring(rutEmpresa.Length - 1);

            secuencia.Append("--7d23e2a11301c4\r\n");
            secuencia.Append("Content-Disposition: form-data; name=\"rutSender\"\r\n");
            secuencia.Append("\r\n");
            secuencia.Append(numeroRutEmisor + "\r\n");
            secuencia.Append("--7d23e2a11301c4\r\n");
            secuencia.Append("Content-Disposition: form-data; name=\"dvSender\"\r\n");
            secuencia.Append("\r\n");
            secuencia.Append(digitoVerificadorEmisor.ToUpper() + "\r\n");
            secuencia.Append("--7d23e2a11301c4\r\n");
            secuencia.Append("Content-Disposition: form-data; name=\"rutCompany\"\r\n");
            secuencia.Append("\r\n");
            secuencia.Append(numeroRutEmpresa + "\r\n");
            secuencia.Append("--7d23e2a11301c4\r\n");
            secuencia.Append("Content-Disposition: form-data; name=\"dvCompany\"\r\n");
            secuencia.Append("\r\n");
            secuencia.Append(digitoVerificadorEmpresa + "\r\n");
            secuencia.Append("--7d23e2a11301c4\r\n");
            secuencia.Append("Content-Disposition: form-data; name=\"archivo\"; filename=\"" + fileName + "\"\r\n");
            secuencia.Append("Content-Type: text/xml\r\n");
            secuencia.Append("\r\n");

            secuencia.Append("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
            secuencia.Append(xdocument.ToString(SaveOptions.DisableFormatting));
            secuencia.Append("\r\n");
            secuencia.Append("--7d23e2a11301c4--\r\n");
            #endregion

            #region CONFIGURACION DE REQUEST

            // Defina que ambiente utilizar.
            // Certificacion "https://maullin.sii.cl/cgi_dte/UPL/DTEUpload";
            string pUrl = string.Empty;

            if(ambiente == AmbienteEnum.Produccion)
                pUrl = "https://palena.sii.cl/cgi_dte/UPL/DTEUpload";
            else
                pUrl = "https://maullin.sii.cl/cgi_dte/UPL/DTEUpload";
            // Cree los parametros del header.
            // Token debe ser el valor asignado por el SII
            string pMethod = "POST";
            string pAccept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg,application/vnd.ms-powerpoint, application/ms-excel,application/msword, */*";
            string pReferer = "";
            string pToken = "TOKEN={0}";

            // Cree un nuevo request para iniciar el proceso
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pUrl);
            request.Method = pMethod;
            request.Accept = pAccept;
            request.Referer = pReferer;

            // Agregar el content-type
            request.ContentType = "multipart/form-data: boundary=7d23e2a11301c4";
            request.ContentLength = secuencia.Length;

            // Defina manualmente los headers del request
            request.Headers.Add("Accept-Language", "es-cl");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Cookie", string.Format(pToken, token));

            // Defina el user agent
            request.UserAgent = "Mozilla/4.0 (compatible; PROG 1.0; Windows NT 5.0; YComp 5.0.2.4)";
            request.KeepAlive = true;

            #endregion

            #region ESCRIBE LA DATA NECESARIA

            try
            {
                using (StreamWriter sw = new StreamWriter(request.GetRequestStream(), Encoding.GetEncoding("ISO-8859-1")))
                { 
                    sw.Write(secuencia.ToString());
                    //File.WriteAllText("envio.txt", secuencia.ToString());
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }

            #endregion

            #region ENVIA Y SOLICITA RESPUESTA

            string respuestaSii = string.Empty;

            try
            {                
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        respuestaSii = sr.ReadToEnd().Trim();
                    }

                }

                if (string.IsNullOrEmpty(respuestaSii))
                    throw new ArgumentNullException("Respuesta del SII es null");

                //System.IO.File.WriteAllText("responseEnvio.xml", respuestaSii);                
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }

            #endregion

            EnvioDTEResult result;
            if (EnvioDTE.ParseRespuesta(respuestaSii, out result))
            {
                error = string.Empty;
            }
            else
                error = "CANT PARSE RESPONSE: " + result.Errores + "\n" + result.ResponseXml;

            return result;
        }
    }
}
