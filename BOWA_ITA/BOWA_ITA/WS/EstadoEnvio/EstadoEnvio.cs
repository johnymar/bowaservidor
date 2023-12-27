using BOWA_ITA.Certificacion.RegistroReclamoWS;
using BOWA_ITA.Produccion.RegistroReclamo;
using ITA_CHILE.WS;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static ITA_CHILE.Enum.Ambiente;

namespace ItaSystem.DTE.WS.EstadoEnvio
{
    public class EstadoEnvio
    {
        public static bool ParseResponse(string response, out EstadoEnvioResult result, int tipo)
        {
            try
            {
                XmlDocument doc = ITA_CHILE.WS.UtilidadesWS.GetDocument(response);

                result = new EstadoEnvioResult();
                result.ResponseXml = response;                
                result.Estado = doc.GetElementsByTagName("ESTADO")[0].InnerText;
                try
                {
                    result.TrackId = int.Parse(doc.GetElementsByTagName("TRACKID")[0].InnerText);
                }
                catch { }

                int n;
                DateTime date;

                if(ITA_CHILE.WS.UtilidadesWS.TryParseAtencionString(doc.GetElementsByTagName("NUM_ATENCION")[0].InnerText, out n, out date))
                {
                    result.NumeroAtencion = n;
                    result.FechaAtencion = date;
                }

                if (result.Estado != "-11")
                {
                    result.Glosa = doc.GetElementsByTagName("GLOSA")[0].InnerText;
                    if(result.Estado == "EPR" && tipo!=1)
                    {
                        result.TipoDocumento = int.Parse(doc.GetElementsByTagName("TIPO_DOCTO")[0].InnerText);
                        result.CantidadInformados = int.Parse(doc.GetElementsByTagName("INFORMADOS")[0].InnerText);
                        result.CantidadAceptados = int.Parse(doc.GetElementsByTagName("ACEPTADOS")[0].InnerText);
                        result.CantidadRechazados = int.Parse(doc.GetElementsByTagName("RECHAZADOS")[0].InnerText);
                        result.CantidadReparos = int.Parse(doc.GetElementsByTagName("REPAROS")[0].InnerText);
                    }
                }
                else
                {
                    result.SRV_CODE = doc.GetElementsByTagName("SRV_CODE")[0].InnerText;
                    result.ERR_CODE = doc.GetElementsByTagName("ERR_CODE")[0].InnerText;
                    result.SQL_CODE = doc.GetElementsByTagName("SQL_CODE")[0].InnerText;
                }
                return true;
            }
            catch(Exception ex) 
            {
                result = new EstadoEnvioResult();
                result.ResponseXml = response;
                result.Glosa = ex.Message;
                return false;
            }
        }

        public static EstadoEnvioResult GetEstado(int numeroRut, string numeroVerificador, long trackId, string nombreCertificado, AmbienteEnum ambiente, out string error, string tokenFullPath, string password = "",int tipo=0)
        {
            numeroVerificador = numeroVerificador.ToLower();
            try
            {
                string message = "";
                string token = Autorizacion.Autenticar.GetToken(nombreCertificado, ambiente, out message, tokenFullPath, password);
                if(!String.IsNullOrEmpty(message))
                {
                    error = message;
                    return null;
                }
                string response = string.Empty;
                if (ambiente == AmbienteEnum.Produccion)
                {
                    BOWA_ITA.Produccion.EstadoEnvio.QueryEstUpService query = new BOWA_ITA.Produccion.EstadoEnvio.QueryEstUpService();
                    response = query.getEstUp(numeroRut.ToString(), numeroVerificador, trackId.ToString(), token);
                }
                else
                {
                    BOWA_ITA.Certificacion.EstadoEnvio.QueryEstUpService query = new BOWA_ITA.Certificacion.EstadoEnvio.QueryEstUpService();
                    response = query.getEstUp(numeroRut.ToString(), numeroVerificador, trackId.ToString(), token);
                }

                EstadoEnvioResult result;
                if (EstadoEnvio.ParseResponse(response, out result, tipo))
                    error = string.Empty;
                else
                    error = "CANT PARSE RESPONSE: " + result.Glosa;
                return result;
            }
            catch(Exception ex) 
            {
                error = ex.Message;
                return null;
            }
        }

        public static EstadoEnvioBoletaResult GetEstadoEnvioBoleta(int numeroRut, string numeroVerificador, long trackId, string nombreCertificado, AmbienteEnum ambiente, out string error, string tokenFullPath, EstadoEnvioResult result, string password = "")
        {
            error = null;
            string response = "";
            EstadoEnvioBoletaResult respuesta=new EstadoEnvioBoletaResult();

            try
            {
                HttpServices httpServices = new HttpServices();
                response = httpServices.WSGetEstadoEnvio(numeroRut, numeroVerificador, trackId, ambiente, tokenFullPath, nombreCertificado, string.Empty, out error);
               respuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<EstadoEnvioBoletaResult>(response);
               return respuesta;
            }
            catch (Exception ex)
            {

                return respuesta;
            }
        }
    }
}
