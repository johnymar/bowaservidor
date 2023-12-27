using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static ITA_CHILE.Enum.Ambiente;

namespace ItaSystem.DTE.WS.AceptacionReclamo
{
    public static class AceptacionReclamoWS
    {
        //public static bool ParseResponse(string response, out AceptacionReclamoResult result)
        //{
        //    try
        //    {
        //        XmlDocument doc = UtilidadesWS.GetDocument(response);

        //        result = new AceptacionReclamoResult();
        //        result.ResponseXml = response;
        //        result.Estado = doc.GetElementsByTagName("ESTADO")[0].InnerText;
        //        result.GlosaEstado = doc.GetElementsByTagName("GLOSA_ESTADO")[0].InnerText;

        //        int n;
        //        DateTime date;

        //        if (UtilidadesWS.TryParseAtencionString(doc.GetElementsByTagName("NUM_ATENCION")[0].InnerText, out n, out date))
        //        {
        //            result.NumeroAtencion = n;
        //            result.FechaAtencion = date;
        //        }

        //        int estado;
        //        if (int.TryParse(result.Estado, out estado))
        //        {
        //            result.ERR_CODE = doc.GetElementsByTagName("ERR_CODE")[0].InnerText;
        //            result.GLosa_ERR_CODE = doc.GetElementsByTagName("GLOSA_ERR")[0].InnerText;
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new EstadoDTEResult();
        //        result.ResponseXml = response;
        //        result.GlosaEstado = ex.Message;
        //        return false;
        //    }
        //}

        public static string NotificarAceptacionReclamo(string rutEmisor, string dvEmisor, int tipoDoc, int folio, string accion, string nombreCertificado, AmbienteEnum ambiente, string tokenFullPath, string password = "")
        {
            try
            {
                /*
                 * ACD: Acepta Contenido del Documento
                 * RCD: Reclamo al Contenido del Documento
                 * ERM: Otorga Recibo de Mercaderías o Servicios
                 * RFP: Reclamo por Falta Parcial de Mercaderías
                 * RFT: Reclamo por Falta Total de Mercaderías
                 */
                string message = "";
                string token = ItaSystem.DTE.WS.Autorizacion.Autenticar.GetToken(nombreCertificado, ambiente, out message, tokenFullPath, password);
                if (!String.IsNullOrEmpty(message))
                {
                    // error = "Error al recuperar el token.\n\nError: " + message;
                    return null;
                }
                if (ambiente == AmbienteEnum.Produccion)
                {
                    BOWA_ITA.Produccion.RegistroReclamo.RegistroReclamoDteServiceEndpointService query = new BOWA_ITA.Produccion.RegistroReclamo.RegistroReclamoDteServiceEndpointService();
                    query.Token = token;
                    var response = query.ingresarAceptacionReclamoDoc(rutEmisor, dvEmisor, tipoDoc.ToString(), folio.ToString(), accion);
                    return response.descResp;
                }
                else
                {
                    BOWA_ITA.Certificacion.RegistroReclamoWS.RegistroReclamoDteServiceEndpointService query = new BOWA_ITA.Certificacion.RegistroReclamoWS.RegistroReclamoDteServiceEndpointService();
                    query.Token = token;
                    var response = query.ingresarAceptacionReclamoDoc(rutEmisor, dvEmisor, tipoDoc.ToString(), folio.ToString(), accion);
                    return response.descResp;
                }                
            }
            catch (Exception ex)
            {                
                return null;
            }
        }

    }
}
