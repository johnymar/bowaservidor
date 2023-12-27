using ItaSystem.DTE.Engine.Documento;
using ItaSystem.DTE.Engine.Enum;
using ItaSystem.DTE.Engine.Envio;
using ItaSystem.DTE.Engine.RespuestaEnvio;
using ItaSystem.DTE.WS.EstadoDTE;
using ITA_CHILE.Security.Firma;
using System.Security.Cryptography;

using ItaSystem.DTE.WS.EstadoEnvio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Globalization;

using MySql.Data.MySqlClient;
using System.Xml;
using System.Data;
using System.Threading;
using static ITA_CHILE.Enum.Ambiente;
using Newtonsoft.Json;
using ItaSystem.DTE.WS.EstadoEnvio;
using ItaSystem.DTE.WS.EnvioDTE;
using Formatting = Newtonsoft.Json.Formatting;
using revision_boletas.clases;

namespace Consulta_dte
{
    public class Handler
    {
        public String error2 = "";
        private static Configuracion configuracion;

        public Handler()
        {
            configuracion = new Configuracion();
        }


        static string RutEmpresa = "";
        static string Giro = "";
        static string RazonSocial = "";
        static string Comuna = "";
        static string Direccion = "";
        static int NumeroResolucion = 0;
        static string FechaResolucion = "";
        static string represen_r = "";
        static string represen_n = "";
        static public Boolean produccion = true;


        public static void leer(string rut)
        {
            string query = "SELECT * from certificado WHERE rut_empresa=@rut_empresa";
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, cusuarioxion);
                cmd.Parameters.AddWithValue("@rut_empresa", rut);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RutEmpresa = reader.GetString(0);
                    Giro = reader.GetString(1);
                    RazonSocial = reader.GetString(2);
                    Comuna = reader.GetString(3);
                    Direccion = reader.GetString(4);
                    NumeroResolucion = reader.GetInt32(5);
                    FechaResolucion = reader.GetString(6);

                    represen_r = reader.GetString(7);
                    represen_n = reader.GetString(8);
                    produccion = reader.GetBoolean(9);


                }
                cusuarioxion.Close();
                configuracion.LeerArchivo();
                configuracion.Empresa.RutEmpresa = RutEmpresa;

                configuracion.Empresa.Giro = Giro;
                configuracion.Empresa.RazonSocial = RazonSocial;
                configuracion.Empresa.Comuna = Comuna;
                configuracion.Empresa.Direccion = Direccion;
                configuracion.Empresa.NumeroResolucion = NumeroResolucion;
                configuracion.Empresa.FechaResolucion = Convert.ToDateTime(FechaResolucion);


                configuracion.Certificado.Rut = represen_r;
                configuracion.Certificado.Nombre = represen_n;

                configuracion.GenerarArchivo();

            }
        }


        public static string Initialize2()
        {
            cusuarioxion.Close();
            try
            {

                cusuarioxion.ConnectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";

                cusuarioxion.Open();
                return null;
            }
            catch (Exception ee)
            {
                return "error " + ee;
                using (StreamWriter sw = File.CreateText(@Program.ruta + "\\Documents\\bowa\temp\\listo1_4.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                }
            }
        }
        //open connection to database
        private static bool OpenConnection()
        {
            cusuarioxion.Close();
            try
            {
                cusuarioxion.Open(); //// lo debo cambiar para que se caiga
                return true;

            }
            catch (MySqlException ex)
            {

                using (StreamWriter sw = File.CreateText(@Program.ruta + "\\Documents\\bowa\temp\\listo1_4.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
                return false;
            }
        }

        //Close connection
        public static MySqlConnection cusuarioxion = new MySqlConnection();
        public static bool CloseConnection()
        {
            try
            {
                cusuarioxion.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                using (StreamWriter sw = File.CreateText(@Program.ruta + "\\Documents\\bowa\temp\\listo1_4.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
                Console.Write(ex.Message);
                return false;
            }

        }

        public static string Cliente = "";
        public static string trackid = "";
        public static string Estado = "";
        public static string Glosa = "";
        public static string Numatencion = "";
        public static string Fecha = "";
        public static string hora = "";
        public static string dte = "";
        public static string tipoDoc = "";
        public static string Informados = "";
        public static string aceptados = "";
        public static string rechazados = "";
        public static string reparos = "";
        public long EnviarEnvioDTEToSII(string filePathEnvio, AmbienteEnum ambiente, string pp, bool nuevaBoleta = false)
        {


            Initialize2();
            leer(Program.rutempresa);
            configuracion.LeerArchivo();
            string messageResult = string.Empty;
            long trackID = -1;
            int i;
            try
            {
                for (i = 1; i <= 5; i++)
                {
                    EnvioDTEResult responseEnvio = new EnvioDTEResult();

                    if (nuevaBoleta) responseEnvio = ItaSystem.DTE.WS.EnvioBoleta.EnvioBoleta.Enviar(configuracion.Certificado.Rut, configuracion.Empresa.RutEmpresa, filePathEnvio, configuracion.Certificado.Nombre, ambiente, out messageResult, ".\\out\\tkn.dat");
                    else responseEnvio = ItaSystem.DTE.WS.EnvioDTE.EnvioDTE.Enviar(configuracion.Certificado.Rut, configuracion.Empresa.RutEmpresa, filePathEnvio, configuracion.Certificado.Nombre, ambiente, out messageResult, ".\\out\\tkn.dat", "");

                    if (responseEnvio != null || string.IsNullOrEmpty(messageResult))
                    {
                        trackID = responseEnvio.TrackId;

                        /*Aquí pueden obtener todos los datos de la respuesta, tal como:
                         * Estado
                         * Fecha
                         * Archivo
                         * Glosa
                         * XML
                         * Entre otros*/
                        Thread.Sleep(500);
                        try
                        {
                            leer(Program.rutempresa);
                            configuracion.LeerArchivo();
                            Console.WriteLine("buscando respuesta sii para Tracking " + trackID);
                            if (produccion)
                            {
                                Console.WriteLine("Esperando...");
                                //var responseEstadoDTE = ConsultarEstadoEnvioBoleta(AmbienteEnum.Produccion, trackID);
                                //string respuesta = JsonConvert.SerializeObject(responseEstadoDTE, Formatting.Indented);

                                var responseEstadoDTE = ConsultarEstadoEnvio(AmbienteEnum.Produccion, trackID);

                                string respuesta = responseEstadoDTE.ResponseXml;
                                string archivo_respuesta = filePathEnvio + "respuesta_" + trackID + ".txt";
                                TextWriter tw = new StreamWriter(archivo_respuesta, true);
                                tw.WriteLine(respuesta);
                                tw.Close();
                                Console.WriteLine("Respuesta Sii:" + respuesta);
                                //  Program.ReadMatrixFromFile(archivo_respuesta);


                            }

                        }
                        catch (Exception ex)
                        {

                        }

                        return trackID;
                    }
                }

                if (i == 5)
                    throw new Exception("SE HA ALCANZADO EL MÁXIMO NÚMERO DE INTENTOS: " + messageResult);
            }
            catch (Exception ex)
            {
                messageResult = ex.Message;
                using (StreamWriter sw = File.CreateText(@Program.ruta + "\\Documents\\bowa\temp\\listo1_4.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
                return 0;
            }
            return 0;

        }


        public EstadoEnvioBoletaResult ConsultarEstadoEnvioBoleta(AmbienteEnum ambiente, long trackId, EstadoEnvioResult result)
        {
            leer(Program.rutempresa);
            configuracion.LeerArchivo();
            string[] ff = Program.rutempresa.Split('-');
            int rutEmpresa = Convert.ToInt32(ff[0]);
            string rutEmpresaDigito = ff[1];

            string error = "";
            var responseEstadoEnvio = EstadoEnvio.GetEstadoEnvioBoleta(rutEmpresa, rutEmpresaDigito, trackId, configuracion.Certificado.Nombre, ambiente, out error, ".\\out\\tkn.dat",result);

            if (!String.IsNullOrEmpty(error))
                throw new Exception(error);

            return responseEstadoEnvio;
        }


        public EstadoEnvioResult ConsultarEstadoEnvio(AmbienteEnum ambiente, long trackId)
        {
            //string signature = SIMPLE_API.Security.Firma.Firma.GetFirmaFromString(xmlEnvio);
            string[] ff = Program.rutempresa.Split('-');
            int rutEmpresa = Convert.ToInt32(ff[0]);
            string rutEmpresaDigito = ff[1];

            var responseEstadoEnvio = EstadoEnvio.GetEstado(rutEmpresa, rutEmpresaDigito, trackId, Program.nombre, ambiente, out string error, ".\\out\\tkn.dat", "", 1);
            Thread.Sleep(500);
            if (!String.IsNullOrEmpty(error))
                throw new Exception(error);

            return responseEstadoEnvio;
        }


        public static string fecha_envio = "";
        public static string sobre = "";

        public static string boletas = "";
        public static string boleta_inicial = "";
        public static string boletas_final = "";

        //public bool ValidateEnvio(string filePath, ItaSystem.DTE.Security.Firma.Firma.TipoXML tipo)
        //{
        //    string messageResult = string.Empty;
        //    if (ItaSystem.DTE.Engine.XML.XmlHandler.ValidateWithSchema(filePath, out messageResult, ItaSystem.DTE.Engine.XML.Schemas.EnvioDTE))
        //        if (ItaSystem.DTE.Security.Firma.Firma.VerificarFirma(filePath, tipo))
        //            return true;
        //        else
        //            throw new Exception("NO SE HA PODIDO VERIFICAR LA FIRMA DEL ENVÍO");
        //    throw new Exception(messageResult);
        //}


        public bool Validate(string filePath, Firma.TipoXML tipo, string schema)
        {
            string messageResult = string.Empty;
            if (ItaSystem.DTE.Engine.XML.XmlHandler.ValidateWithSchema(filePath, out messageResult, schema))
            {
                if (ITA_CHILE.Security.Firma.Firma.VerificarFirma(filePath, tipo, out string messageOutFirma))
                    return true;
                else
                    return false;
                //MessageBox.Show("Error al validar firma electrónica: " + messageResult + "", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //  MessageBox.Show("Error: " + messageResult + ". Verifique que contiene la carpeta XML con los XSD para validación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                return false;
            }
        }

        private string EnsureExists(int tipoDTE, int folio, string pathCaf)
        {
            var cafFile = string.Empty;
            try
            {

                foreach (var file in System.IO.Directory.GetFiles(pathCaf))
                    if (ParseName((new FileInfo(file)).Name, tipoDTE, folio))
                        cafFile = file;
                if (string.IsNullOrEmpty(cafFile))
                    throw new Exception("NO HAY UN CÓDIGO DE AUTORIZACIÓN DE FOLIOS (CAF) ASIGNADO PARA ESTE TIPO DE DOCUMENTO (" + tipoDTE + ") QUE INCLUYA EL FOLIO REQUERIDO (" + folio + ").");

            }
            catch (Exception ex)
            {
                //MessageBox.Show(""+ ex);
                Console.WriteLine("" + ex);
                using (StreamWriter sw = File.CreateText(@Program.ruta + "\\Documents\\bowa\temp\\listo1_4.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
            }
            return cafFile;
        }

        private static bool ParseName(string name, int tipoDTE, int folio)
        {
            try
            {
                var values = name.Substring(0, name.IndexOf('.')).Split('_');
                int tipo = Convert.ToInt32(values[0]);
                int desde = Convert.ToInt32(values[1]);
                int hasta = Convert.ToInt32(values[2]);

                return tipoDTE == tipo && desde <= folio && folio <= hasta;
            }
            catch { return false; }
        }


        public string ObtenerCertificados()
        {
            var certificadosMaquina = ItaSystem.DTE.Engine.Utilidades.ObtenerCertificadosMaquinas();
            var certificadosusuario = ItaSystem.DTE.Engine.Utilidades.ObtenerCertificadosUsuario();
            string result = "Máquina:\n";
            foreach (var a in certificadosMaquina)
                result += a + "\n";
            result += "usuario:\n";
            foreach (var a in certificadosusuario)
                result += a + "\n";
            return result;
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public static string TipoDTEString(TipoDTE.DTEType tipo)
        {
            switch (tipo)
            {
                case ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.FacturaCompraElectronica: return "FACTURA DE COMPRA ELECTRÓNICA";
                case ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.FacturaElectronica: return "FACTURA ELECTRÓNICA";
                case ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.FacturaElectronicaExenta: return "FACTURA ELECTRÓNICA EXENTA";
                case ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.GuiaDespachoElectronica: return "GUIA DE DESPACHO ELECTRÓNICA";
                case ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica: return "NOTA DE CRÉDITO ELECTRÓNICA";
                case ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaDebitoElectronica: return "NOTA DE DÉBITO ELECTRÓNICA";
                case ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.Factura: return "FACTURA MANUAL";
                case ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCredito: return "NOTA DE CRÉDITO MANUAL";
            }
            return "Not Set";
        }

    }
}
