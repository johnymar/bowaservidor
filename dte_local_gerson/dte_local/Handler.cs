using bowuaSystem.DTE.Engine.Documento;
using bowuaSystem.DTE.Engine.Enum;
using bowuaSystem.DTE.Engine.Envio;
using bowuaSystem.DTE.Engine.RespuestaEnvio;
using bowuaSystem.DTE.WS.EstadoDTE;
using BOWUA_CHILE.Security.Firma;
using System.Security.Cryptography;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Globalization;
using dte_local.clases;
using bowuaSystem.DTE.WS.EstadoEnvio;
using MySql.Data.MySqlClient;

namespace dte_local
{
    public class Handler
    {

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


        public static void leer(string rut)
        {
            Initialize2();
            string query = "SELECT * from certificado WHERE rut_empresa=@rut_empresa";
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conexion);
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



                }
                conexion.Close();
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
            try
            {

                conexion.ConnectionString = "SERVER=" + "localhost" + ";" + "user=root;" + "DATABASE=bowa;" + "port=3306;" + "PASSWORD=12345678;";

                conexion.Open();
                return null;
            }
            catch
            {
                return "error";
            }
        }
        //open connection to database
        private static bool OpenConnection()
        {
            conexion.Close();
            try
            {
                conexion.Open(); //// lo debo cambiar para que se caiga
                return true;

            }
            catch (MySqlException ex)
            {


                return false;
            }
        }

        //Close connection
        public static MySqlConnection conexion = new MySqlConnection();
        public static bool CloseConnection()
        {
            try
            {
                conexion.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        public long EnviarEnvioDTEToSII(string filePathEnvio, bool produccion)
        {
            string messageResult = string.Empty;
            long trackID = -1;
            int i;
            try
            {
                for (i = 1; i <= 5; i++)
                {
                    string rutEmisorNumero = configuracion.Empresa.RutEmpresa.Substring(0, configuracion.Empresa.RutEmpresa.Length - 2);
                    string rutEmisorDigito = configuracion.Empresa.RutEmpresa.Substring(configuracion.Empresa.RutEmpresa.Length - 1);
                    string rutEmpresaNumero = configuracion.Empresa.RutEmpresa.Substring(0, configuracion.Empresa.RutEmpresa.Length - 2);
                    string rutEmpresaDigito = configuracion.Empresa.RutEmpresa.Substring(configuracion.Empresa.RutEmpresa.Length - 1);
                    var responseEnvio = bowuaSystem.DTE.WS.EnvioDTE.EnvioDTE.Enviar(rutEmisorNumero, rutEmisorDigito, rutEmpresaNumero, rutEmpresaDigito, filePathEnvio, filePathEnvio, configuracion.Certificado.Nombre, produccion, out messageResult, ".\\out\\tkn.dat");

                    if (responseEnvio != null || string.IsNullOrEmpty(messageResult))
                    {
                        trackID = responseEnvio.TrackId;
                        string line = trackID.ToString();

                        // Set a variable to the Documents path.
                
                    string docPath = ".//out//";
                       
                        // Write the string array to a new file named "WriteLines.txt".
                        TextWriter tw = new StreamWriter(docPath+"envios.txt", true);
                        tw.WriteLine(line);
                        tw.Close();
                        
                    

                    /*Aquí pueden obtener todos los datos de la respuesta, tal como:
                     * Estado
                     * Fecha
                     * Archivo
                     * Glosa
                     * XML
                     * Entre otros*/
                    return trackID;
                    }
                }

                if (i == 5)
                    throw new Exception("SE HA ALCANZADO EL MÁXIMO NÚMERO DE INTENTOS: " + messageResult);
            }
            catch (Exception ex)
            {
                messageResult = ex.Message;
                return 0;
            }
            return 0;
        }

        public EstadoEnvioResult ConsultarEstadoEnvio(bool produccion, long trackId)
        {
            //string signature = SIMPLE_API.Security.Firma.Firma.GetFirmaFromString(xmlEnvio);
            int rutEmpresa = configuracion.Empresa.RutCuerpo;
            string rutEmpresaDigito = configuracion.Empresa.DV;

            var responseEstadoEnvio = EstadoEnvio.GetEstado(rutEmpresa, rutEmpresaDigito, trackId, configuracion.Certificado.Nombre, produccion, out string error, ".\\out\\tkn.dat");

            if (!String.IsNullOrEmpty(error))
                throw new Exception(error);

            return responseEstadoEnvio;
        }

        public string TimbrarYFirmarXMLDTE(DTE dte, string pathResult, string pathCaf)
        {
            /*En primer lugar, el documento debe timbrarse con el CAF que descargas desde el SII, es simular
             * cuando antes debías ir con las facturas en papel para que te las timbraran */
            string messageOut = string.Empty;
            dte.Documento.Timbrar(
                EnsureExists((int)dte.Documento.Encabezado.IdentificacionDTE.TipoDTE, dte.Documento.Encabezado.IdentificacionDTE.Folio, pathCaf),
                out messageOut);

            /*El documento timbrado se guarda en la variable pathResult*/

            /*Finalmente, el documento timbrado debe firmarse con el certificado digital*/
            /*Se debe entregar en el argumento del método Firmar, el "FriendlyName" o Nombre descriptivo del certificado*/
            /*Retorna el filePath donde estará el archivo XML timbrado y firmado, listo para ser enviado al SII*/
            leer(Program.rutempresa);
            configuracion.LeerArchivo();
            string h = configuracion.Empresa.RutEmpresa;
            string h1 = configuracion.Certificado.Nombre;


            String c = dte.Firmar(configuracion.Certificado.Nombre, out messageOut, pathResult, "","",key_manager.key, configuracion.Empresa.RutEmpresa);
            if (c.Equals(""))
            {
                // MessageBox.Show("Error - aplicacion no activa", "Error", MessageBoxButtons.OK);
                Console.WriteLine("Error - aplicacion no activa");
            }

            return c;
        }

        public bowuaSystem.DTE.Engine.RCOF.ConsumoFolios GenerarRCOF(List<DTE> dtes)
        {
            var rcof = new bowuaSystem.DTE.Engine.RCOF.ConsumoFolios();
            //preparo los datos segun los DTE seleccionados
            DateTime fechaInicio = dtes.Min(x => x.Documento.Encabezado.IdentificacionDTE.FechaEmision);
            DateTime fechaFinal = dtes.Max(x => x.Documento.Encabezado.IdentificacionDTE.FechaEmision);

            rcof.DocumentoConsumoFolios.Caratula.FechaFinal = fechaInicio;
            rcof.DocumentoConsumoFolios.Caratula.FechaInicio = fechaFinal;
            rcof.DocumentoConsumoFolios.Caratula.FechaResolucion = configuracion.Empresa.FechaResolucion;
            rcof.DocumentoConsumoFolios.Caratula.NroResol = configuracion.Empresa.NumeroResolucion;
            rcof.DocumentoConsumoFolios.Caratula.RutEmisor = configuracion.Empresa.RutEmpresa;
            rcof.DocumentoConsumoFolios.Caratula.RutEnvia = configuracion.Certificado.Rut;
            rcof.DocumentoConsumoFolios.Caratula.SecEnvio = key_manager.secuencia_envio;
            rcof.DocumentoConsumoFolios.Caratula.FechaEnvio = DateTime.Now;
            List<bowuaSystem.DTE.Engine.RCOF.Resumen> resumenes = new List<bowuaSystem.DTE.Engine.RCOF.Resumen>();

            /*datos de boletas electrónicas afectas*/
            /* Estos datos se deben calcular, debido a que no se informa IVA en boletas electrónicas 
             */
            int totalBrutoAfecto = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica)
                        .Sum(x => x.Documento.Detalles
                        .Where(y => y.IndicadorExento == bowuaSystem.DTE.Engine.Enum.IndicadorFacturacionExencion.IndicadorFacturacionExencionEnum.NotSet)
                        .Sum(y => y.MontoItem));

            int totalExento = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica)
                    .Sum(x => x.Documento.Detalles
                    .Where(y => y.IndicadorExento == bowuaSystem.DTE.Engine.Enum.IndicadorFacturacionExencion.IndicadorFacturacionExencionEnum.NoAfectoOExento)
                    .Sum(y => y.MontoItem));

            int totalNeto = (int)Math.Round(totalBrutoAfecto / 1.19, 0, MidpointRounding.AwayFromZero);
            int totalIVA = (int)Math.Round(totalNeto * 0.19, 0, MidpointRounding.AwayFromZero);
            int totalTotal = totalExento + totalNeto + totalIVA;

            int rangoInicial = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Min(x => x.Documento.Encabezado.IdentificacionDTE.Folio);
            int rangoFinal = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Max(x => x.Documento.Encabezado.IdentificacionDTE.Folio);
            resumenes.Add(new bowuaSystem.DTE.Engine.RCOF.Resumen
            {
                FoliosAnulados = 0,
                FoliosEmitidos = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Count(),
                FoliosUtilizados = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Count(),
                MntExento = totalExento,
                MntIva = totalIVA,
                MntNeto = totalNeto,
                MntTotal = totalTotal,
                TasaIVA = 19,
                TipoDocumento = bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica,
                RangoUtilizados = new List<bowuaSystem.DTE.Engine.RCOF.RangoUtilizados>() { new bowuaSystem.DTE.Engine.RCOF.RangoUtilizados() { Inicial = rangoInicial, Final = rangoFinal } }
                //RangoAnulados = new List<bowuaSystem.DTE.Engine.RCOF.RangoAnulados>() { new bowuaSystem.DTE.Engine.RCOF.RangoAnulados() { Final = 0, Inicial = 0 } }
            });

            /*datos de notas de credito electronicas*/
            /*datos de boletas electrónicas afectas*/
            totalNeto = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoNeto);
            totalExento = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoExento);
            totalIVA = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.IVA);
            totalTotal = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoTotal);
            rangoInicial = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Min(x => x.Documento.Encabezado.IdentificacionDTE.Folio);
            rangoFinal = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Max(x => x.Documento.Encabezado.IdentificacionDTE.Folio);
            resumenes.Add(new bowuaSystem.DTE.Engine.RCOF.Resumen
            {
                FoliosAnulados = 0,
                FoliosEmitidos = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Count(),
                FoliosUtilizados = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Count(),
                MntExento = totalExento,
                MntIva = totalIVA,
                MntNeto = totalNeto,
                MntTotal = totalTotal,
                TasaIVA = 19,
                TipoDocumento = bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica,
                RangoUtilizados = new List<bowuaSystem.DTE.Engine.RCOF.RangoUtilizados>() { new bowuaSystem.DTE.Engine.RCOF.RangoUtilizados() { Inicial = rangoInicial, Final = rangoFinal } }
                //RangoAnulados =new List<bowuaSystem.DTE.Engine.RCOF.RangoAnulados>() { new bowuaSystem.DTE.Engine.RCOF.RangoAnulados() { Final = 0, Inicial = 0 } }
            });

            rcof.DocumentoConsumoFolios.Resumen = resumenes;
            return rcof;
        }

        //public bool ValidateEnvio(string filePath, bowuaSystem.DTE.Security.Firma.Firma.TipoXML tipo)
        //{
        //    string messageResult = string.Empty;
        //    if (bowuaSystem.DTE.Engine.XML.XmlHandler.ValidateWithSchema(filePath, out messageResult, bowuaSystem.DTE.Engine.XML.Schemas.EnvioDTE))
        //        if (bowuaSystem.DTE.Security.Firma.Firma.VerificarFirma(filePath, tipo))
        //            return true;
        //        else
        //            throw new Exception("NO SE HA PODIDO VERIFICAR LA FIRMA DEL ENVÍO");
        //    throw new Exception(messageResult);
        //}

        public EnvioBoleta GenerarEnvioBoletaDTEToSII(List<DTE> dtes, List<string> xmlDtes)
        {
            leer(Program.rutempresa);
            configuracion.LeerArchivo();
            var EnvioSII = new bowuaSystem.DTE.Engine.Envio.EnvioBoleta();
            EnvioSII.SetDTE = new bowuaSystem.DTE.Engine.Envio.SetDTE();
            var date1 = DateTime.Now.Date;
            string v =date1.ToString("ddMMyyyy",CultureInfo.InvariantCulture);
            EnvioSII.SetDTE.Id = "FENV010_" + Program.envio;
            /*Es necesario agregar en el envío, los objetos DTE como sus respectivos XML en strings*/
            foreach (var a in dtes)
                EnvioSII.SetDTE.DTEs.Add(a);
            foreach (var a in xmlDtes)
            {
                EnvioSII.SetDTE.dteXmls.Add(a);
                EnvioSII.SetDTE.signedXmls.Add(a);
            }

            EnvioSII.SetDTE.Caratula = new bowuaSystem.DTE.Engine.Envio.Caratula();
            EnvioSII.SetDTE.Caratula.FechaEnvio = DateTime.Now;
            /*Fecha de Resolución y Número de Resolución se averiguan en el sitio del SII según ambiente de producción o certificación*/
            EnvioSII.SetDTE.Caratula.FechaResolucion = configuracion.Empresa.FechaResolucion;
            EnvioSII.SetDTE.Caratula.NumeroResolucion = configuracion.Empresa.NumeroResolucion;

            EnvioSII.SetDTE.Caratula.RutEmisor = configuracion.Empresa.RutEmpresa;
            EnvioSII.SetDTE.Caratula.RutEnvia = configuracion.Certificado.Rut;
            EnvioSII.SetDTE.Caratula.RutReceptor = "60803000-K"; //Este es el RUT del SII
            EnvioSII.SetDTE.Caratula.SubTotalesDTE = new List<bowuaSystem.DTE.Engine.Envio.SubTotalesDTE>();

            /*En la carátula del envío, se debe indicar cuantos documentos de cada tipo se están enviando*/
            var tipos = EnvioSII.SetDTE.DTEs.GroupBy(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE);
            foreach (var a in tipos)
            {
                EnvioSII.SetDTE.Caratula.SubTotalesDTE.Add(new bowuaSystem.DTE.Engine.Envio.SubTotalesDTE()
                {
                    Cantidad = a.Count(),
                    TipoDTE = a.ElementAt(0).Documento.Encabezado.IdentificacionDTE.TipoDTE
                });
            }
            return EnvioSII;
        }
        public bool Validate(string filePath, Firma.TipoXML tipo, string schema)
        {
            string messageResult = string.Empty;
            if (bowuaSystem.DTE.Engine.XML.XmlHandler.ValidateWithSchema(filePath, out messageResult, schema))
            {
                if (BOWUA_CHILE.Security.Firma.Firma.VerificarFirma(filePath, tipo, out string messageOutFirma))
                    return true;
                else
                    return false;
                //MessageBox.Show("Error al validar firma electrónica: " + messageResult + "", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //  MessageBox.Show("Error: " + messageResult + ". Verifique que contiene la carpeta XML con los XSD para validación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
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
            catch (Exception ex) {
                //MessageBox.Show(""+ ex);
                Console.WriteLine("" + ex);
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
            var certificadosMaquina = bowuaSystem.DTE.Engine.Utilidades.ObtenerCertificadosMaquinas();
            var certificadosUsuario = bowuaSystem.DTE.Engine.Utilidades.ObtenerCertificadosUsuario();
            string result = "Máquina:\n";
            foreach (var a in certificadosMaquina)
                result += a + "\n";
            result += "Usuario:\n";
            foreach (var a in certificadosUsuario)
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
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.FacturaCompraElectronica: return "FACTURA DE COMPRA ELECTRÓNICA";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.FacturaElectronica: return "FACTURA ELECTRÓNICA";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.FacturaElectronicaExenta: return "FACTURA ELECTRÓNICA EXENTA";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.GuiaDespachoElectronica: return "GUIA DE DESPACHO ELECTRÓNICA";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica: return "NOTA DE CRÉDITO ELECTRÓNICA";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaDebitoElectronica: return "NOTA DE DÉBITO ELECTRÓNICA";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.Factura: return "FACTURA MANUAL";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCredito: return "NOTA DE CRÉDITO MANUAL";
            }
            return "Not Set";
        }

    }
}
