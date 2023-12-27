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
using dte_local1.clases;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Data;
using System.Threading;
using static ITA_CHILE.Enum.Ambiente;
using Newtonsoft.Json;
using ItaSystem.DTE.WS.EstadoEnvio;
using ItaSystem.DTE.WS.EnvioDTE;
using Formatting = Newtonsoft.Json.Formatting;
using ItaSystem.DTE.Engine.Helpers;

namespace dte_local1
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

        public static void tabla()
        {
         
            Initialize2();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = "SELECT rut FROM cliente ORDER BY rut,id";
            cmd.Prepare();
            MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Program.rut_produccion.Add(reader.GetString(0));
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
            }
        

           public static void grabar_estatus(string rut, int a)
        {
            Initialize2();
            string connectionString = @"server=localhost;userid=root;password=12345678;database=bowa";

            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE rut_sobre SET nuevo='" + 0 + "' WHERE rut='" + rut + "'";
                cmd.Prepare();

              //  cmd.Parameters.AddWithValue("@rut", rut);
               // cmd.Parameters.AddWithValue("@nuevo", a);
                cmd.ExecuteNonQuery();
                         }
            catch (Exception ee) {
                Console.Write("error " + ee);
            }
        }
        public static MySqlConnection conexion = new MySqlConnection();
        public static void HasRows()
        {

            Initialize2();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = "SELECT * FROM rut_sobre WHERE nuevo ='1'";
            cmd.Prepare();
            MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Program.rut_produccion.Add(reader.GetString(1));
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
            }
        
        public static int existe_viejo = 0;
        public static void buscar_viejo(string rut)
        {
            Initialize2();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = "SELECT * FROM cliente_viejo WHERE rut='" + rut + "'";
            MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        existe_viejo = 1;
                    }
                }
                else
                {
                    existe_viejo = 0;
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
            
        }
    
        public static void guardar_tabla()
        {
            Initialize2();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = "SELECT rut FROM cliente ORDER BY rut,id";
            MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Program.rut_produccion.Add(reader.GetString(0));
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();

                for (int y1=0; y1 < Program.rut_produccion.Count; y1++)
                {
                    string rr= Program.rut_produccion[y1];
                    cmd.CommandText = "INSERT IGNORE INTO rut_sobre (rut, nuevo) VALUES ('"+rr+"', 0)";
                    cmd.ExecuteNonQuery();
                }
                 conexion.Close();

            
            Program.rut_produccion.Clear();


        }
        public static void leer(string rut)
        {
            Initialize2();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = "SELECT * from certificado WHERE rut_empresa=@rut_empresa";
            cmd.Parameters.AddWithValue("@rut_empresa", rut);
            cmd.Prepare();
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
                conexion.Close();
                configuracion.LeerArchivo(RutEmpresa);
                configuracion.Empresa.RutEmpresa = RutEmpresa;

                configuracion.Empresa.Giro = Giro;
                configuracion.Empresa.RazonSocial = RazonSocial;
                configuracion.Empresa.Comuna = Comuna;
                configuracion.Empresa.Direccion = Direccion;
                configuracion.Empresa.NumeroResolucion = NumeroResolucion;
                configuracion.Empresa.FechaResolucion = Convert.ToDateTime(FechaResolucion);


                configuracion.Certificado.Rut = represen_r;
                configuracion.Certificado.Nombre = represen_n;

                configuracion.GenerarArchivo(RutEmpresa);

            
        }


        public static string Initialize2()
        {
            conexion.Close();
            try
            {

                conexion.ConnectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";

                conexion.Open();
                return null;
            }
            catch(Exception ee)
            {
                return "error " + ee;
                using (StreamWriter sw = File.CreateText(@Program.ruta+"\\temp\\listo1_2_1.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                }
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

                using (StreamWriter sw = File.CreateText(@Program.ruta+"\\temp\\listo1_2_1.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
                return false;
            }
        }

        //Close connection
         public static bool CloseConnection()
        {
            try
            {
                conexion.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                using (StreamWriter sw = File.CreateText(@Program.ruta+"\\temp\\listo1_2.txt"))
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
            configuracion.LeerArchivo(Program.rutempresa);
            string messageResult = string.Empty;
            long trackID = -1;
            int i;
            Console.Write("Enviando rut: " + configuracion.Certificado.Rut+"-"+ configuracion.Certificado.Nombre);
            try
            {
                for (i = 1; i <= 5; i++)
                {
                    EnvioDTEResult responseEnvio = new EnvioDTEResult();

                    if (nuevaBoleta) responseEnvio = ItaSystem.DTE.WS.EnvioBoleta.EnvioBoleta.Enviar(configuracion.Certificado.Rut, configuracion.Empresa.RutEmpresa, filePathEnvio, configuracion.Certificado.Nombre, AmbienteEnum.Produccion, out messageResult, Program.docPath + "tkn.dat");
                    else responseEnvio = ItaSystem.DTE.WS.EnvioDTE.EnvioDTE.Enviar(configuracion.Certificado.Rut, configuracion.Empresa.RutEmpresa, filePathEnvio, configuracion.Certificado.Nombre, AmbienteEnum.Produccion, out messageResult, Program.docPath + "tkn.dat", "");

                    if (responseEnvio != null || string.IsNullOrEmpty(messageResult))
                    {
                        trackID = responseEnvio.TrackId;

                       
                        return trackID;
                    }
                }

                if (i == 5)
                    throw new Exception("SE HA ALCANZADO EL MÁXIMO NÚMERO DE INTENTOS: " + messageResult);
            }
            catch (Exception ex)
            {
                messageResult = ex.Message;
                using (StreamWriter sw = File.CreateText(@Program.ruta + "\\temp\\listo2.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
                return 0;
            }
            return 0;

        }



        public EstadoEnvioBoletaResult ConsultarEstadoEnvioBoleta(AmbienteEnum ambiente, long trackId)
        {
            leer(Program.rutempresa);
            configuracion.LeerArchivo(Program.rutempresa);
            string[] ff = Program.rutempresa.Split('-');
            int rutEmpresa = Convert.ToInt32(ff[0]);
            string rutEmpresaDigito = ff[1];

            string error = "";
            var responseEstadoEnvio = EstadoEnvio.GetEstadoEnvioBoleta(rutEmpresa, rutEmpresaDigito, trackId, configuracion.Certificado.Nombre, ambiente, out error, ".\\out\\tkn.dat",result);

            if (!String.IsNullOrEmpty(error))
                throw new Exception(error);

            return responseEstadoEnvio;
        }


       

        public long token_leer(string docpath, int paso) {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement doc = xmlDoc.DocumentElement;
            if (File.Exists(docpath))
            {
                xmlDoc.Load(docpath);
            }

            XmlNodeList contenido = xmlDoc.GetElementsByTagName("ESTADO");

            foreach (XmlNode node in contenido)
            {
                Estado = node.InnerText;

            }
            if (Estado != "-11")
            {
                contenido = xmlDoc.GetElementsByTagName("TIPO_DOCTO");

                foreach (XmlNode node in contenido)
                {
                    tipoDoc = node.InnerText;

                }


                contenido = xmlDoc.GetElementsByTagName("INFORMADOS");
                foreach (XmlNode node in contenido)
                {
                    Informados = node.InnerText;

                }
                contenido = xmlDoc.GetElementsByTagName("ACEPTADOS");

                foreach (XmlNode node in contenido)
                {
                    aceptados = node.InnerText;

                }

                contenido = xmlDoc.GetElementsByTagName("REPAROS");

                foreach (XmlNode node in contenido)
                {
                    reparos = node.InnerText;

                }
                contenido = xmlDoc.GetElementsByTagName("RECHAZADOS");

                foreach (XmlNode node in contenido)
                {
                    rechazados = node.InnerText;


                }

                contenido = xmlDoc.GetElementsByTagName("TRACKID");

                foreach (XmlNode node in contenido)
                {
                    trackid = node.InnerText;
                }

                contenido = xmlDoc.GetElementsByTagName("ESTADO");

                foreach (XmlNode node in contenido)
                {
                    Estado = node.InnerText;

                }
                contenido = xmlDoc.GetElementsByTagName("GLOSA");

                foreach (XmlNode node in contenido)
                {
                    Glosa = node.InnerText;

                }
                contenido = xmlDoc.GetElementsByTagName("NUM_ATENCION");

                foreach (XmlNode node in contenido)
                {
                    string g = node.InnerText;
                    string[] H = g.Trim(' ').Split('(');
                    Numatencion = H[0];
                    string[] x1 = H[1].Split(' ');
                    Fecha = x1[1];
                    hora = x1[2].Trim(')');

                }
                Cliente = Program.rutempresa;
                dte = Program.nombre_file;
                Initialize2();

                string connectionString = @"server=localhost;userid=root;password=12345678;database=bowa";

                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into respuestasii (Cliente, dte, trackid, Estado, Glosa, NumAtencion, Fecha, hora, tipoDoc, Informados, aceptados, rechazados, reparos ) VALUES " +
    "(@Cliente, @dte, @trackid, @Estado, @Glosa, @NumAtencion, @Fecha, @hora, @tipoDoc, @Informados, @aceptados, @rechazados, @reparos)";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@Cliente", Cliente);
                    cmd.Parameters.AddWithValue("@dte", dte);
                    cmd.Parameters.AddWithValue("@trackid", trackid);
                    cmd.Parameters.AddWithValue("@Estado", Estado);
                    cmd.Parameters.AddWithValue("@Glosa", Glosa);
                    cmd.Parameters.AddWithValue("@Numatencion", Numatencion);
                    cmd.Parameters.AddWithValue("@Fecha", Fecha);
                    cmd.Parameters.AddWithValue("@hora", hora);
                    cmd.Parameters.AddWithValue("@tipoDoc", tipoDoc);
                    cmd.Parameters.AddWithValue("@Informados", Informados);
                    cmd.Parameters.AddWithValue("@aceptados", aceptados);
                    cmd.Parameters.AddWithValue("@rechazados", rechazados);
                    cmd.Parameters.AddWithValue("@reparos", reparos);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
                long.Parse(trackid);

            }
            else
            {
                string path = @Program.ruta+"\\temp\\responsesii.xml";
                if (File.Exists(path))
                {
                    xmlDoc.Load(path);
                    File.Delete(path);
                }


                contenido = xmlDoc.GetElementsByTagName("ERROR");

                foreach (XmlNode node in contenido)
                {
                    if (node.InnerText != "")
                    {
                        error2 = node.InnerText;
                        Console.WriteLine("\n Error de envio: " + Program.nombre_file + "Intento=  "+paso + "\n" + error2);
                    }
                    }

                contenido = xmlDoc.GetElementsByTagName("TRACKID");
                string hh = "";
                foreach (XmlNode node in contenido)
                {
                    String error1 = node.InnerText;
                    hh = error1;
                }

                contenido = xmlDoc.GetElementsByTagName("ERROR");
                String error = "";
                foreach (XmlNode node in contenido)
                {
                    error = node.InnerText;

                }
                trackid = hh;

                xmlDoc = new XmlDocument();
                doc = xmlDoc.DocumentElement;
                if (File.Exists(docpath))
                {
                    xmlDoc.Load(docpath);
                }

                tipoDoc = "39";
                Informados = "0";
                aceptados = "0";
                reparos = "0";
                rechazados = "0";

                contenido = xmlDoc.GetElementsByTagName("ESTADO");

                foreach (XmlNode node in contenido)
                {
                    Estado = node.InnerText;

                }
                Glosa = error;


                contenido = xmlDoc.GetElementsByTagName("NUM_ATENCION");

                foreach (XmlNode node in contenido)
                {
                    string g = node.InnerText;
                    string[] H = g.Trim(' ').Split('(');
                    Numatencion = H[0];
                    string[] x1 = H[1].Split(' ');
                    Fecha = x1[1];
                    hora = x1[2].Trim(')');

                }
                Cliente = Program.rutempresa;
                dte = Program.nombre_file;
                Initialize2();

                string connectionString = @"server=localhost;userid=root;password=12345678;database=bowa";

                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(connectionString);
                    MySqlCommand cmd = new MySqlCommand(); 
                    connection.Open();
                    
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into respuestasii (Cliente, dte, trackid, Estado, Glosa, NumAtencion, Fecha, hora, tipoDoc, Informados, aceptados, rechazados, reparos ) VALUES " +
    "(@Cliente, @dte, @trackid, @Estado, @Glosa, @NumAtencion, @Fecha, @hora, @tipoDoc, @Informados, @aceptados, @rechazados, @reparos)";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@Cliente", Cliente);
                    cmd.Parameters.AddWithValue("@dte", dte);
                    cmd.Parameters.AddWithValue("@trackid", trackid);
                    cmd.Parameters.AddWithValue("@Estado", Estado);
                    cmd.Parameters.AddWithValue("@Glosa", Glosa);
                    cmd.Parameters.AddWithValue("@Numatencion", Numatencion);
                    cmd.Parameters.AddWithValue("@Fecha", Fecha);
                    cmd.Parameters.AddWithValue("@hora", hora);
                    cmd.Parameters.AddWithValue("@tipoDoc", tipoDoc);
                    cmd.Parameters.AddWithValue("@Informados", Informados);
                    cmd.Parameters.AddWithValue("@aceptados", aceptados);
                    cmd.Parameters.AddWithValue("@rechazados", rechazados);
                    cmd.Parameters.AddWithValue("@reparos", reparos);
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ee)
                {
                    if (connection != null)
                        connection.Close();
                    using (StreamWriter sw = File.CreateText(@Program.ruta+"\\temp\\listo1_2_1.txt"))
                    {

                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                    }
                }

            }
    
            return long.Parse(trackid);

        }



        public EstadoEnvioResult ConsultarEstadoEnvio(AmbienteEnum ambiente, long trackId)
        {
            //string signature = SIMPLE_API.Security.Firma.Firma.GetFirmaFromString(xmlEnvio);
            leer(Program.rutempresa);
            configuracion.LeerArchivo(Program.rutempresa);
            string[] ff = Program.rutempresa.Split('-');
            int rutEmpresa = Convert.ToInt32(ff[0]);
            string rutEmpresaDigito = ff[1];

            var responseEstadoEnvio = EstadoEnvio.GetEstado(rutEmpresa, rutEmpresaDigito, trackId, configuracion.Certificado.Nombre, ambiente, out string error, ".\\out\\tkn.dat");

            if (!String.IsNullOrEmpty(error))
                throw new Exception(error);

            return responseEstadoEnvio;
        }
        public static string fecha_envio="";
        public static string sobre = "";
        public EnvioBoleta GenerarEnvioBoletaDTEToSII(List<DTE> dtes, List<string> xmlDtes)
        {
            Handler.Initialize2();
            Handler.leer(Program.rutempresa);
            configuracion.LeerArchivo(Program.rutempresa);

            var EnvioSII = new ItaSystem.DTE.Engine.Envio.EnvioBoleta();
         
            EnvioSII.SetDTE = new ItaSystem.DTE.Engine.Envio.SetDTE();
            var date1 = DateTime.Now.Date;
            string v = date1.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
            EnvioSII.SetDTE.Id = "FENV010_" + v + "_" + Program.enviox;
            sobre = "FENV010_" + v + "_" + Program.enviox;
            /*Es necesario agregar en el envío, los objetos DTE como sus respectivos XML en strings*/
            int cuenta = 0;
            string nombre = "";
            int y = 0;
            int y1 = 0;
            foreach (var a in dtes)
            {
                
                EnvioSII.SetDTE.DTEs.Add(a);
            }
           
            foreach (var a in xmlDtes)
            {
                cuenta = cuenta + 1;
                EnvioSII.SetDTE.dteXmls.Add(a);
                EnvioSII.SetDTE.signedXmls.Add(a);

               
            }
           
            boletas = Convert.ToString(cuenta);
            EnvioSII.SetDTE.Caratula = new ItaSystem.DTE.Engine.Envio.Caratula();
            EnvioSII.SetDTE.Caratula.FechaEnvio = DateTime.Now;
            fecha_envio = DateTime.Now.ToString();
            /*Fecha de Resolución y Número de Resolución se averiguan en el sitio del SII según ambiente de producción o certificación*/
            EnvioSII.SetDTE.Caratula.FechaResolucion = configuracion.Empresa.FechaResolucion;
            EnvioSII.SetDTE.Caratula.NumeroResolucion = configuracion.Empresa.NumeroResolucion;

            EnvioSII.SetDTE.Caratula.RutEmisor = configuracion.Empresa.RutEmpresa;
            EnvioSII.SetDTE.Caratula.RutEnvia = configuracion.Certificado.Rut;
            EnvioSII.SetDTE.Caratula.RutReceptor = "60803000-K"; //Este es el RUT del SII
            EnvioSII.SetDTE.Caratula.SubTotalesDTE = new List<ItaSystem.DTE.Engine.Envio.SubTotalesDTE>();

            /*En la carátula del envío, se debe indicar cuantos documentos de cada tipo se están enviando*/
            var tipos = EnvioSII.SetDTE.DTEs.GroupBy(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE);
            foreach (var a in tipos)
            {
                EnvioSII.SetDTE.Caratula.SubTotalesDTE.Add(new ItaSystem.DTE.Engine.Envio.SubTotalesDTE()
                {
                    Cantidad = a.Count(),
                    TipoDTE = a.ElementAt(0).Documento.Encabezado.IdentificacionDTE.TipoDTE
                });
              
            }
            return EnvioSII;
        }
        public static string boletas = "";
        public static string boleta_inicial = "";
        public static string boletas_final = "";
        private EstadoEnvioResult result;

        public ItaSystem.DTE.Engine.RCOF.ConsumoFolios GenerarRCOF(List<DTE> dtes)
        {
            var rcof = new ItaSystem.DTE.Engine.RCOF.ConsumoFolios();
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
            List<ItaSystem.DTE.Engine.RCOF.Resumen> resumenes = new List<ItaSystem.DTE.Engine.RCOF.Resumen>();

            /*datos de boletas electrónicas afectas*/
            /* Estos datos se deben calcular, debido a que no se informa IVA en boletas electrónicas 
             */
            int totalBrutoAfecto = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica)
                        .Sum(x => x.Documento.Detalles
                        .Where(y => y.IndicadorExento == ItaSystem.DTE.Engine.Enum.IndicadorFacturacionExencion.IndicadorFacturacionExencionEnum.NotSet)
                        .Sum(y => y.MontoItem));

            int totalExento = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica)
                    .Sum(x => x.Documento.Detalles
                    .Where(y => y.IndicadorExento == ItaSystem.DTE.Engine.Enum.IndicadorFacturacionExencion.IndicadorFacturacionExencionEnum.NoAfectoOExento)
                    .Sum(y => y.MontoItem));

            int totalNeto = (int)Math.Round(totalBrutoAfecto / 1.19, 0, MidpointRounding.AwayFromZero);
            int totalIVA = (int)Math.Round(totalNeto * 0.19, 0, MidpointRounding.AwayFromZero);
            int totalTotal = totalExento + totalNeto + totalIVA;

            int rangoInicial = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Min(x => x.Documento.Encabezado.IdentificacionDTE.Folio);
            int rangoFinal = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Max(x => x.Documento.Encabezado.IdentificacionDTE.Folio);
            resumenes.Add(new ItaSystem.DTE.Engine.RCOF.Resumen
            {
                FoliosAnulados = 0,
                FoliosEmitidos = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Count(),
                FoliosUtilizados = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Count(),
                MntExento = totalExento,
                MntIva = totalIVA,
                MntNeto = totalNeto,
                MntTotal = totalTotal,
                TasaIVA = 19,
                TipoDocumento = ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica,
                RangoUtilizados = new List<ItaSystem.DTE.Engine.RCOF.RangoUtilizados>() { new ItaSystem.DTE.Engine.RCOF.RangoUtilizados() { Inicial = rangoInicial, Final = rangoFinal } }
                //RangoAnulados = new List<ItaSystem.DTE.Engine.RCOF.RangoAnulados>() { new ItaSystem.DTE.Engine.RCOF.RangoAnulados() { Final = 0, Inicial = 0 } }
            });

            /*datos de notas de credito electronicas*/
            /*datos de boletas electrónicas afectas*/
            totalNeto = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoNeto);
            totalExento = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoExento);
            totalIVA = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.IVA);
            totalTotal = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoTotal);
            rangoInicial = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Min(x => x.Documento.Encabezado.IdentificacionDTE.Folio);
            rangoFinal = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Max(x => x.Documento.Encabezado.IdentificacionDTE.Folio);
            resumenes.Add(new ItaSystem.DTE.Engine.RCOF.Resumen
            {
                FoliosAnulados = 0,
                FoliosEmitidos = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Count(),
                FoliosUtilizados = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Count(),
                MntExento = totalExento,
                MntIva = totalIVA,
                MntNeto = totalNeto,
                MntTotal = totalTotal,
                TasaIVA = 19,
                TipoDocumento = ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica,
                RangoUtilizados = new List<ItaSystem.DTE.Engine.RCOF.RangoUtilizados>() { new ItaSystem.DTE.Engine.RCOF.RangoUtilizados() { Inicial = rangoInicial, Final = rangoFinal } }
                //RangoAnulados =new List<ItaSystem.DTE.Engine.RCOF.RangoAnulados>() { new ItaSystem.DTE.Engine.RCOF.RangoAnulados() { Final = 0, Inicial = 0 } }
            });

            rcof.DocumentoConsumoFolios.Resumen = resumenes;
            return rcof;
        }

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
                using (StreamWriter sw = File.CreateText(@Program.ruta+"\\temp\\listo1_2_1.txt"))
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
            var certificadosone = ItaSystem.DTE.Engine.Utilidades.ObtenerCertificadosUsuario();
            string result = "Máquina:\n";
            foreach (var a in certificadosMaquina)
                result += a + "\n";
            result += "one:\n";
            foreach (var a in certificadosone)
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
