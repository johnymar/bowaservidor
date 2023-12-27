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
using dte_local2.clases;
using ItaSystem.DTE.WS.EstadoEnvio;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Data;
using ItaSystem.DTE.WS.EnvioDTE;
using static ITA_CHILE.Enum.Ambiente;

namespace dte_local2
{
    public class Handler
    {
        public String error2 = "";
        private static Configuracion configuracion;

        public Handler()
        {
            configuracion = new Configuracion();
        }

        static Boolean TipoBoleta = false;

        static string RutEmpresa = "";
        static string Giro = "";
        static string RazonSocial = "";
        static string Comuna = "";
        static string Direccion = "";
        static int NumeroResolucion = 0;
        static string FechaResolucion = "";
        static string represen_r = "";
        public static string represen_n = "";

        static public Boolean produccion = false;

        public static void grabar_estatus(string rut, string a)
        {
            //Initialize2();
            String ConnectionString ="";
            if (Program.servidor == 0)
            {
                 ConnectionString = @"server=localhost;userid=root;password=12345678;database=bowa";
            }
            else
            {
                 ConnectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
            }
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(ConnectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "Insert into rut_ejecutado (rut, estatus) VALUES " +
             "(@rut, @estatus) ON DUPLICATE KEY UPDATE estatus=@estatus";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@rut", rut);
                cmd.Parameters.AddWithValue("@estatus", a);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ee) {
                Console.WriteLine("error " + ee);
            }
        }


        public static void leer_rut_estatus()
        {
            Initialize2();
            String ConnectionString = "";
            if (Program.servidor == 0)
            {
                ConnectionString = @"server=localhost;userid=root;password=12345678;database=bowa";
            }
            else
            {
                ConnectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
            }
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(ConnectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT rut FROM rut_ejecutado ORDER BY rut";
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Program.rut_ejecutado.Add(reader.GetString(0));
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception ee)
            {
                Console.Write("Error " + ee);
            }

        }

        public static int buscar_rut_ngr(string rut)
        {
            int salir = 0;
            Initialize2();
            String ConnectionString = "";
            if (Program.servidor == 0)
            {
                ConnectionString = @"server=localhost;userid=root;password=12345678;database=bowa";
            }
            else
            {
                ConnectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
            }

            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(ConnectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT rut FROM cliente_ngr WHERE rut='" + rut + "'";
                MySqlDataReader reader = cmd.ExecuteReader();
                salir = 0;
                while (reader.Read())
                {
                    salir = 1;
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception ee)
            {
                Console.Write("Error " + ee);
            }
            return salir;
        }

        public static void leer_fecha(String rut)
        {
            Initialize2();
            String ConnectionString = "";
            if (Program.servidor == 0)
            {
                ConnectionString = @"server=localhost;userid=root;password=12345678;database=bowa";
            }
            else
            {
                ConnectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
            }

            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(ConnectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT fecha FROM fecha_activacion WHERE rut='"+ rut+"'";
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Program.fecha_inicial=reader.GetString(0);
                }
                reader.Close();
                connection.Close();
            }

            catch (Exception ee)
            {
                Console.Write("Error " + ee);
            }

        }
        public static void HasRows()
        {
            Initialize2();
            string query = "SELECT rut FROM cliente ORDER BY rut,id";
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conexion);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Program.rut_produccion.Add(reader.GetString(0));
                        Console.WriteLine("Adicionando Cliente"+ reader.GetString(0));

                    }

                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
               
            }
        }
        public static void leer(string rut)
        {
            Console.WriteLine("leyendo rut");
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
                    produccion = reader.GetBoolean(9);

                    TipoBoleta = reader.GetBoolean(11);




                }
                reader.Close();
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
                Console.WriteLine("Saliendo leyendo rut");

            }
        }


        public static string Initialize2()
        {
            string ConnectionString = "";

            conexion.Close();
            try
            {
                Console.WriteLine("Conectado a servidor =" + Program.servidor);
                if (Program.servidor == 0)
                {
                    conexion.ConnectionString = @"server=localhost;userid=root;password=12345678;database=bowa";
                }
                else
                {
                    conexion.ConnectionString= @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
                }
                Console.WriteLine("conectada");

                conexion.Open();
                Console.WriteLine("Base datos conectada");
                return null;
            }
            catch (Exception ee)
            {
                using (StreamWriter sw = File.CreateText(@Program.ruta + "temp\\listo2.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);
                    Console.WriteLine("error " + ee);
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                }
                return "error " + ee;
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

                using (StreamWriter sw = File.CreateText(@Program.ruta+"temp\\listo2.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
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
                using (StreamWriter sw = File.CreateText(@Program.ruta+"temp\\listo2.txt"))
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
            //configuracion.LeerArchivo();
            string messageResult = string.Empty;
            long trackID = -1;
            int i;
            try
            {
                for (i = 1; i <= 5; i++)
                {
                    EnvioDTEResult responseEnvio = new EnvioDTEResult();

                    Console.WriteLine("Enviando rcof para rut" + Program.rutempresa);
                    Console.WriteLine(configuracion.Certificado.Nombre);
                    Console.WriteLine("\nrut certificado" +configuracion.Certificado.Rut);

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
                        return trackID;
                    }
                }

                if (i == 5)
                    throw new Exception("SE HA ALCANZADO EL MÁXIMO NÚMERO DE INTENTOS: " + messageResult);
            }
            catch (Exception ex)
            {
                messageResult = ex.Message;
                using (StreamWriter sw = File.CreateText(@Program.ruta+"temp\\listo2.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
                return 0;
            }
            return 0;

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

                String ConnectionString = "";
                if (Program.servidor == 0)
                {
                    ConnectionString = @"server=localhost;userid=root;password=12345678;database=bowa";
                }
                else
                {
                    ConnectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
                }

                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(ConnectionString);
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
                string path = @Program.ruta+"temp\\responsesii.xml";
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
                        Console.WriteLine("\n Error de envio: " + Program.nombre_file + "Intento=  " + paso + "\n" + error2);
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

                String ConnectionString = "";
                if (Program.servidor == 0)
                {
                    ConnectionString = @"server=localhost;userid=root;password=12345678;database=bowa";
                }
                else
                {
                    ConnectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
                }

                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(ConnectionString);
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
                catch (Exception ee)
                {
                    using (StreamWriter sw = File.CreateText(@Program.ruta + "temp\\listo2.txt"))
                    {

                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                    }
                    if (connection != null)
                        connection.Close();
                }

            }

            return long.Parse(trackid);

        }



        public EstadoEnvioResult ConsultarEstadoEnvio(AmbienteEnum ambiente, long trackId)
        {
            Initialize2();
            //string signature = SIMPLE_API.Security.Firma.Firma.GetFirmaFromString(xmlEnvio);
            leer(Program.rutempresa);
            configuracion.LeerArchivo();

            int rutEmpresa = configuracion.Empresa.RutCuerpo;
            string rutEmpresaDigito = configuracion.Empresa.DV;


            string error = "";
            EstadoEnvioResult responseEstadoEnvio = EstadoEnvio.GetEstado(rutEmpresa, rutEmpresaDigito, trackId, configuracion.Certificado.Nombre, ambiente, out error, ".\\out\\tkn.dat", "", 1);


            if (!String.IsNullOrEmpty(error))
                throw new Exception(error);

            return responseEstadoEnvio;
        }

        public EnvioBoleta GenerarEnvioBoletaDTEToSII(List<DTE> dtes, List<string> xmlDtes)
        {
            Handler.Initialize2();
            Handler.leer(Program.rutempresa);
            configuracion.LeerArchivo();
            var EnvioSII = new ItaSystem.DTE.Engine.Envio.EnvioBoleta();
            EnvioSII.SetDTE = new ItaSystem.DTE.Engine.Envio.SetDTE();
            var date1 = DateTime.Now.Date;
            string v = date1.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
            EnvioSII.SetDTE.Id = "FENV010_" + v + "_" + Program.envio;
            /*Es necesario agregar en el envío, los objetos DTE como sus respectivos XML en strings*/
            foreach (var a in dtes)
                EnvioSII.SetDTE.DTEs.Add(a);
            foreach (var a in xmlDtes)
            {
                EnvioSII.SetDTE.dteXmls.Add(a);
                EnvioSII.SetDTE.signedXmls.Add(a);
            }

            EnvioSII.SetDTE.Caratula = new ItaSystem.DTE.Engine.Envio.Caratula();
            EnvioSII.SetDTE.Caratula.FechaEnvio = DateTime.Now;
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
        public ItaSystem.DTE.Engine.RCOF.ConsumoFolios GenerarRCOF(List<DTE> dtes, string ruta)
        {
            Initialize2();
            Handler.leer(Program.rutempresa);




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
            rcof.DocumentoConsumoFolios.Caratula.SecEnvio = Program.envio;
            rcof.DocumentoConsumoFolios.Caratula.FechaEnvio = DateTime.Now;
            List<ItaSystem.DTE.Engine.RCOF.Resumen> resumenes = new List<ItaSystem.DTE.Engine.RCOF.Resumen>();

            /*datos de boletas electrónicas afectas*/
            /* Estos datos se deben calcular, debido a que no se informa IVA en boletas electrónicas 
             */

            if (TipoBoleta == false)
            {

                int rut_ngr = Handler.buscar_rut_ngr(Program.rutempresa);
                Console.WriteLine("Rut " + Program.rutempresa + " ngr=" + rut_ngr);
                // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                int totalBrutoAfecto = 0;
                int totalExento = 0;
                int totalIVA = 0;
                int totalNeto = 0;
               
                //  if (rut_ngr == 1)
                // {
                totalBrutoAfecto = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica)
                         .Sum(x => x.Documento.Encabezado.Totales.MontoTotal);

                totalExento = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica)
                        .Sum(x => x.Documento.Encabezado.Totales.MontoExento);

                totalIVA = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica)
                        .Sum(x => x.Documento.Encabezado.Totales.IVA);

                totalNeto = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica)
                        .Sum(x => x.Documento.Encabezado.Totales.MontoNeto);

                //                    totalNeto = (int)Math.Round(totalBrutoAfecto / 1.19, 0, MidpointRounding.AwayFromZero);
                //                  totalIVA = (int)Math.Round(totalNeto * 0.19, 0, MidpointRounding.AwayFromZero);

                /*    else
                    {
                        totalBrutoAfecto = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica)
                               .Sum(x => x.Documento.Detalles
                               .Where(y => y.IndicadorExento == ItaSystem.DTE.Engine.Enum.IndicadorFacturacionexencion.IndicadorFacturacionexencionenum.NotSet)
                               .Sum(y => y.MontoItem));

                        totalExento = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoExento);



                        totalNeto = (int)Math.Round(totalBrutoAfecto / 1.19, 0, MidpointRounding.AwayFromZero);
                        totalIVA = (int)Math.Round(totalNeto * 0.19, 0, MidpointRounding.AwayFromZero);
                    }*/

               // totalNeto = (totalBrutoAfecto - totalExento)/1.19;
              //  totalIVA = totalBrutoAfecto - totalExento- totalNeto;


                Console.WriteLine("Total " + totalBrutoAfecto);

                /*Se calculan todos los rangos según el array de DTEs*/
                var resultRangos = new List<ItaSystem.DTE.Engine.RCOF.RangoUtilizados>();
                List<int> lst = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == TipoDTE.DTEType.BoletaElectronica).Select(x => x.Documento.Encabezado.IdentificacionDTE.Folio).ToList();
                var minBoundaries = lst.Where(i => !lst.Contains(i - 1)).OrderBy(x => x).ToList();
                var maxBoundaries = lst.Where(i => !lst.Contains(i + 1)).OrderBy(x => x).ToList();
                for (int i = 0; i < maxBoundaries.Count; i++)
                {
                    resultRangos.Add(new ItaSystem.DTE.Engine.RCOF.RangoUtilizados() { Inicial = minBoundaries[i], Final = maxBoundaries[i] });
                }


                resumenes.Add(new ItaSystem.DTE.Engine.RCOF.Resumen
                {
                    FoliosAnulados = 0,
                    FoliosEmitidos = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Count(),
                    FoliosUtilizados = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Count(),
                    MntExento = totalExento,
                    MntIva = totalIVA,
                    MntNeto = totalNeto,
                    MntTotal = totalBrutoAfecto,
                    TasaIVA = 19,
                    TipoDocumento = ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica,
                    RangoUtilizados = resultRangos
                    //RangoAnulados = new List<ChileSystems.DTE.Engine.RCOF.RangoAnulados>() { new ChileSystems.DTE.Engine.RCOF.RangoAnulados() { Final = 0, Inicial = 0 } }
                });

                /*datos de notas de credito electronicas*/
                /*datos de boletas electrónicas afectas*/
                if (dtes.Any(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == TipoDTE.DTEType.NotaCreditoElectronica))
                {
                    totalNeto = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoNeto);
                    totalExento = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoExento);
                    totalIVA = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.IVA);
                    totalBrutoAfecto = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoTotal);

                    resultRangos = new List<ItaSystem.DTE.Engine.RCOF.RangoUtilizados>();
                    lst = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == TipoDTE.DTEType.NotaCreditoElectronica).Select(x => x.Documento.Encabezado.IdentificacionDTE.Folio).ToList();
                    minBoundaries = lst.Where(i => !lst.Contains(i - 1)).OrderBy(x => x).ToList();
                    maxBoundaries = lst.Where(i => !lst.Contains(i + 1)).OrderBy(x => x).ToList();
                    for (int i = 0; i < maxBoundaries.Count; i++)
                    {
                        resultRangos.Add(new ItaSystem.DTE.Engine.RCOF.RangoUtilizados() { Inicial = minBoundaries[i], Final = maxBoundaries[i] });
                    }

                    resumenes.Add(new ItaSystem.DTE.Engine.RCOF.Resumen
                    {
                        FoliosAnulados = 0,
                        FoliosEmitidos = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Count(),
                        FoliosUtilizados = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Count(),
                        MntExento = totalExento,
                        MntIva = totalIVA,
                        MntNeto = totalNeto,
                        MntTotal =totalBrutoAfecto,
                        TasaIVA = 19,
                        TipoDocumento = ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica,
                        RangoUtilizados = resultRangos
                        //RangoAnulados =new List<ChileSystems.DTE.Engine.RCOF.RangoAnulados>() { new ChileSystems.DTE.Engine.RCOF.RangoAnulados() { Final = 0, Inicial = 0 } }
                    });
                }
            }

            else
            {
                int totalBrutoAfecto = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronicaExenta)
                            .Sum(x => x.Documento.Detalles
                            .Sum(y => y.MontoItem));

                int totalExento = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronicaExenta)
                        .Sum(x => x.Documento.Detalles
                        .Sum(y => y.MontoItem));

                int totalNeto = (int)Math.Round(totalBrutoAfecto / 1.0, 0, MidpointRounding.AwayFromZero);
                int totalIVA = 0;
                int totalTotal = totalNeto;

                /*Se calculan todos los rangos según el array de DTEs*/
                var resultRangos = new List<ItaSystem.DTE.Engine.RCOF.RangoUtilizados>();
                List<int> lst = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronicaExenta).Select(x => x.Documento.Encabezado.IdentificacionDTE.Folio).ToList();
                var minBoundaries = lst.Where(i => !lst.Contains(i - 1)).OrderBy(x => x).ToList();
                var maxBoundaries = lst.Where(i => !lst.Contains(i + 1)).OrderBy(x => x).ToList();
                for (int i = 0; i < maxBoundaries.Count; i++)
                {
                    resultRangos.Add(new ItaSystem.DTE.Engine.RCOF.RangoUtilizados() { Inicial = minBoundaries[i], Final = maxBoundaries[i] });
                }


                resumenes.Add(new ItaSystem.DTE.Engine.RCOF.Resumen
                {
                    FoliosAnulados = 0,
                    FoliosEmitidos = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronicaExenta).Count(),
                    FoliosUtilizados = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronicaExenta).Count(),
                    MntExento = totalExento,
                    MntIva = totalIVA,
                    MntNeto = totalNeto,
                    MntTotal = totalTotal,
                    TasaIVA = 19,
                    TipoDocumento = ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronicaExenta,
                    RangoUtilizados = resultRangos
                    //RangoAnulados = new List<ChileSystems.DTE.Engine.RCOF.RangoAnulados>() { new ChileSystems.DTE.Engine.RCOF.RangoAnulados() { Final = 0, Inicial = 0 } }
                });

                /*datos de notas de credito electronicas*/
                /*datos de boletas electrónicas afectas*/
                if (dtes.Any(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == TipoDTE.DTEType.NotaCreditoElectronica))
                {
                    totalNeto = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoNeto);
                    totalExento = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoExento);
                    totalIVA = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.IVA);
                    totalTotal = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoTotal);

                    resultRangos = new List<ItaSystem.DTE.Engine.RCOF.RangoUtilizados>();
                    lst = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == TipoDTE.DTEType.NotaCreditoElectronica).Select(x => x.Documento.Encabezado.IdentificacionDTE.Folio).ToList();
                    minBoundaries = lst.Where(i => !lst.Contains(i - 1)).OrderBy(x => x).ToList();
                    maxBoundaries = lst.Where(i => !lst.Contains(i + 1)).OrderBy(x => x).ToList();
                    for (int i = 0; i < maxBoundaries.Count; i++)
                    {
                        resultRangos.Add(new ItaSystem.DTE.Engine.RCOF.RangoUtilizados() { Inicial = minBoundaries[i], Final = maxBoundaries[i] });
                    }

                    resumenes.Add(new ItaSystem.DTE.Engine.RCOF.Resumen
                    {
                        FoliosAnulados = 0,
                        FoliosEmitidos = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Count(),
                        FoliosUtilizados = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Count(),
                        MntExento = totalExento,
                        MntIva = totalIVA,
                        MntNeto = totalNeto,
                        MntTotal = totalBrutoAfecto,
                        TasaIVA = 19,
                        TipoDocumento = ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica,
                        RangoUtilizados = resultRangos
                        //RangoAnulados =new List<ChileSystems.DTE.Engine.RCOF.RangoAnulados>() { new ChileSystems.DTE.Engine.RCOF.RangoAnulados() { Final = 0, Inicial = 0 } }
                    });
                }
            }

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

        public EnvioBoleta GenerarEnvioBoletaDTEToSII1(List<DTE> dtes, List<string> xmlDtes)
        {
            Handler.Initialize2();
            Handler.leer(Program.rutempresa);
            configuracion.LeerArchivo();
            var EnvioSII = new ItaSystem.DTE.Engine.Envio.EnvioBoleta();
            EnvioSII.SetDTE = new ItaSystem.DTE.Engine.Envio.SetDTE();
            var date1 = DateTime.Now.Date;
            string v = date1.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
            EnvioSII.SetDTE.Id = "FENV010_" + v + "_" + Program.envio;
            /*Es necesario agregar en el envío, los objetos DTE como sus respectivos XML en strings*/
            foreach (var a in dtes)
                EnvioSII.SetDTE.DTEs.Add(a);
            foreach (var a in xmlDtes)
            {
                EnvioSII.SetDTE.dteXmls.Add(a);
                EnvioSII.SetDTE.signedXmls.Add(a);
            }

            EnvioSII.SetDTE.Caratula = new ItaSystem.DTE.Engine.Envio.Caratula();
            EnvioSII.SetDTE.Caratula.FechaEnvio = DateTime.Now;
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
                using (StreamWriter sw = File.CreateText(@Program.ruta+"temp\\listo2.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
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
