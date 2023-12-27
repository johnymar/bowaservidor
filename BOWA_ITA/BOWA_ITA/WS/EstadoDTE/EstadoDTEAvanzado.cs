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
    public class EstadoDTEAvanzado
    {
        private static bool ParseResponse(string response, out EstadoDTEAvanzadoResult result)
        {
            try
            {
                XmlDocument doc = UtilidadesWS.GetDocument(response);

                result = new EstadoDTEAvanzadoResult();
                result.ResponseXml = response;

                result.Estado = int.Parse(doc.GetElementsByTagName("SII:ESTADO")[0].InnerText);
                try
                {
                    result.Glosa = doc.GetElementsByTagName("SII.GLOSA")[0].InnerText;
                }
                catch { }

                int n;
                DateTime date;

                if (UtilidadesWS.TryParseAtencionString(doc.GetElementsByTagName("NUMATENCION")[0].InnerText, out n, out date))
                {
                    result.NumeroAtencion = n;
                    result.FechaAtencion = date;
                }

                try
                {
                    result.TrackId = int.Parse(doc.GetElementsByTagName("TRACKID")[0].InnerText);
                }
                catch { }

                result.IsRecibido = doc.GetElementsByTagName("RECIBIDO")[0].InnerText == "SI";
                result.EstadoBody = doc.GetElementsByTagName("ESTADO")[0].InnerText;

                try
                {
                    result.GlosaBody = doc.GetElementsByTagName("GLOSA")[0].InnerText;
                }
                catch { }

                return true;
            }
            catch (Exception ex)
            {
                result = new EstadoDTEAvanzadoResult();                
                result.Glosa = ex.Message;
                result.ResponseXml = response;
                return false;
            }
        }

        public static EstadoDTEAvanzadoResult GetEstado(int numeroRutConsultante, string numeroVerificadorConsultante, int numeroRutEmpresa, string digitoVerificadorEmpresa, int numeroRutReceptor, string digitoVerificadorReceptor, int TipoDTE, int FolioDTE, DateTime FechaDTE, int MontoDTE, string FirmaDTE, string nombreCertificado, AmbienteEnum ambiente, string tokenFullPath, out string error)
        {
            try
            {
                string message = "";
                string token = Autorizacion.Autenticar.GetToken(nombreCertificado, ambiente, out message, tokenFullPath);
                if (!String.IsNullOrEmpty(message))
                {
                    error = "Error al recuperar el token.\n\nError: " + message;
                    return null;
                }

                string FechaDTEString = FechaDTE.ToString("ddMMyyyy");
                string response = string.Empty;

                if (ambiente == AmbienteEnum.Produccion)
                {
                    BOWA_ITA.Produccion.EstadoDTEAvanzado.QueryEstDteAvService query = new BOWA_ITA.Produccion.EstadoDTEAvanzado.QueryEstDteAvService();
                    response = query.getEstDteAv(numeroRutEmpresa.ToString(), digitoVerificadorEmpresa, numeroRutReceptor.ToString(), digitoVerificadorReceptor, TipoDTE.ToString(), FolioDTE.ToString(), FechaDTEString, MontoDTE.ToString(), FirmaDTE, token);
                }
                else
                {
                    BOWA_ITA.Certificacion.EstadoDTEAvanzado.QueryEstDteAvService query = new BOWA_ITA.Certificacion.EstadoDTEAvanzado.QueryEstDteAvService();
                    response = query.getEstDteAv(numeroRutEmpresa.ToString(), digitoVerificadorEmpresa, numeroRutReceptor.ToString(), digitoVerificadorReceptor, TipoDTE.ToString(), FolioDTE.ToString(), FechaDTEString, MontoDTE.ToString(), FirmaDTE, token);
                }


                EstadoDTEAvanzadoResult result;
                
                if (EstadoDTEAvanzado.ParseResponse(response, out result))
                    error = string.Empty;
                else
                    error = "CANT PARSE RESPONSE: " + result.Glosa;

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
