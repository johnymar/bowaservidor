using ITA_CHILE.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static ITA_CHILE.Enum.Ambiente;

namespace ItaSystem.DTE.WS.EstadoDTE
{
    public static class EstadoDTE
    {
        public static bool ParseResponse(string response, out EstadoDTEResult result)
        {
            try
            {
                XmlDocument doc = UtilidadesWS.GetDocument(response);

                result = new EstadoDTEResult();
                result.ResponseXml = response;
                result.Estado = doc.GetElementsByTagName("ESTADO")[0].InnerText;
                result.GlosaEstado = doc.GetElementsByTagName("GLOSA_ESTADO")[0].InnerText;
                int n;
                DateTime date;

                if (UtilidadesWS.TryParseAtencionString(doc.GetElementsByTagName("NUM_ATENCION")[0].InnerText, out n, out date))
                {
                    result.NumeroAtencion = n;
                    result.FechaAtencion = date;
                }

                int estado;
                if (int.TryParse(result.Estado, out estado))
                {                    
                    result.ERR_CODE = doc.GetElementsByTagName("ERR_CODE")[0].InnerText;
                    result.GLosa_ERR_CODE = doc.GetElementsByTagName("GLOSA_ERR")[0].InnerText;
                }

                return true;
            }
            catch (Exception ex)
            {
                result = new EstadoDTEResult();
                result.ResponseXml = response;
                result.GlosaEstado = ex.Message;
                return false;
            }
        }

        public static EstadoDTEResult GetEstado(int rutConsultante, string dvConsultante, int rutEmpresa, string dvEmpresa, int rutReceptor, string dvReceptor, int TipoDTE, int FolioDTE, DateTime FechaDTE, int MontoDTE, string nombreCertificado, AmbienteEnum ambiente, string tokenFullPath, out string error, string password = "")
        {
            try
            {
                string message = "";
                string token = Autorizacion.Autenticar.GetToken(nombreCertificado, ambiente, out message, tokenFullPath, password);
                if (!String.IsNullOrEmpty(message))
                {
                    error ="Error al recuperar el token.\n\nError: " + message;
                    return null;
                }

                string FechaDTEString = FechaDTE.ToString("ddMMyyyy");
                string response = string.Empty;
                if (ambiente == AmbienteEnum.Produccion)
                {
                    BOWA_ITA.Produccion.EstadoDTE.QueryEstDteService query = new BOWA_ITA.Produccion.EstadoDTE.QueryEstDteService();
                    response = query.getEstDte(rutConsultante.ToString(),
                        dvConsultante,
                        rutEmpresa.ToString(),
                        dvEmpresa,
                        rutReceptor.ToString(),
                        dvReceptor, 
                        TipoDTE.ToString(), 
                        FolioDTE.ToString(), 
                        FechaDTE.ToString("ddMMyyyy"), 
                        MontoDTE.ToString(), token);
                }
                else
                {
                    BOWA_ITA.Certificacion.EstadoDTE.QueryEstDteService query = new BOWA_ITA.Certificacion.EstadoDTE.QueryEstDteService();
                    response = query.getEstDte(rutConsultante.ToString(),
                        dvConsultante,
                        rutEmpresa.ToString(),
                        dvEmpresa,
                        rutReceptor.ToString(),
                        dvReceptor,
                        TipoDTE.ToString(), 
                        FolioDTE.ToString(), 
                        FechaDTE.ToString("ddMMyyyy"), 
                        MontoDTE.ToString(), 
                        token);
                }

                EstadoDTEResult result;
                if (ParseResponse(response, out result))
                    error = string.Empty;
                else
                    error = "CANT PARSE RESPONSE: " + result.GlosaEstado;
                
                return result;
            }
            catch (Exception ex) 
            {
                error = ex.Message;
                return null; 
            }
        }

        public static EstadoDTEResult GetEstadoBoleta(int rutEmpresa, string dvEmpresa, int rutReceptor, string dvReceptor, int TipoDTE, int FolioDTE, DateTime FechaDTE, int MontoDTE, string nombreCertificado, AmbienteEnum ambiente, string tokenFullPath, out string error, string password = "")
        {
            try
            {
                string message = "";
                string token = Autorizacion.AutenticarRest.GetToken(nombreCertificado, ambiente, tokenFullPath, out message, password);
                if (!String.IsNullOrEmpty(message))
                {
                    error = "Error al recuperar el token.\n\nError: " + message;
                    return null;
                }

                string FechaDTEString = FechaDTE.ToString("ddMMyyyy");

                HttpServices httpServices = new HttpServices();
                string response = httpServices.WSGetEstadoDTE(rutEmpresa, dvEmpresa, rutReceptor, dvReceptor,
                   TipoDTE, FolioDTE, FechaDTE, MontoDTE, ambiente, ".\\out\\tkn.dat", nombreCertificado, string.Empty, out error);
                dynamic respuesta = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                var result = new EstadoDTEResult()
                {
                    GLosa_ERR_CODE = respuesta.codigo,
                    Estado = respuesta.codigo,
                    GlosaEstado = respuesta.descripcion,
                    ResponseXml = respuesta.ToString()
                };

                return result;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

    }
}
