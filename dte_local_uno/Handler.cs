using ItaSystem.DTE.Engine.Documento;
using ItaSystem.DTE.Engine.Enum;
using ItaSystem.DTE.Engine.Envio;
using ItaSystem.DTE.Engine.RespuestaEnvio;
using ItaSystem.DTE.WS.EstadoDTE;
using ITA_CHILE.Security.Firma;
using System.Security.Cryptography;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Globalization;
using dte_local.clases;
using ItaSystem.DTE.WS.EstadoEnvio;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ItaSystem.DTE.Engine.Helpers;

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
        static Boolean produccion = false;
        static public Boolean TipoBoleta = false;
        static public String vencido = "0";

        static public int tipoboleta = 39;
        public static void grabar_estatus(string rut, int a)
        {/*
            Initialize2();
           // string connectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
            string connectionString = @"server=localhost;userid=root1;password=12345678;database=bowa";


            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "Insert into rut_declarar (rut, nuevo) VALUES " +
             "(@rut, @nuevo) ON DUPLICATE KEY UPDATE nuevo=@nuevo";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@rut", rut);
                cmd.Parameters.AddWithValue("@nuevo", a);

                    cmd.ExecuteNonQuery();
               
            }
            catch (Exception ee) {
                Console.Write("Error conexion: " + ee);
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

            }*/
        }

        private static X509Certificate2 ObtenerCertificado(string nombreCertificado, string password = "")
        {
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            string[] x1 = null;
            string[] x2 = null;
            x1 = nombreCertificado.Split(' ');
            int igual = 0;
            X509Certificate2Collection certCollection = store.Certificates;
            X509Certificate2 cert = null;
            foreach (X509Certificate2 c in certCollection)
            {

                x2 = c.FriendlyName.ToString().Split(' ');
                for (int y = 0; y < x1.Length - 1 && y < x2.Length - 1; y++)
                {
                    string t = x2[y];
                    string t1 = x1[y];

                    if (x1[y].Contains(x2[y]))
                    {
                        igual = igual + 1;
                    }
                    else
                    {
                        igual = 0;
                    }
                }
                if (igual >= x1.Length - 1)
                {
                    cert = c;
                    return cert;
                }
            }

            X509Store store2 = new X509Store(StoreLocation.LocalMachine);
            store2.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certCollection2 = store2.Certificates;


            x1 = nombreCertificado.Split(' ');
            igual = 0;
            foreach (X509Certificate2 c in certCollection2)
            {
                x2 = c.FriendlyName.ToString().Split(' ');
                for (int y = 0; y < x1.Length - 1 && y < x2.Length - 1; y++)
                {
                    string t = x2[y];
                    string t1 = x1[y];
                    if (x1[y].Contains(x2[y]))
                    {
                        igual = igual + 1;
                    }
                    else
                    {
                        igual = 0;
                    }
                }
                if (igual >= x1.Length - 1)
                {
                    cert = c;
                    return cert;
                }
            }

            /*Intenta obtener el certificado desde un archivo y password*/
            if (cert == null && !string.IsNullOrEmpty(password))
            {
                X509Certificate2Collection certCollection3 = new X509Certificate2Collection();
                certCollection3.Import(nombreCertificado, password, X509KeyStorageFlags.PersistKeySet);
                cert = certCollection3[0];
            }

            return cert;
        }

        public static void grabar_boleta(string ruta, string rut, string tipo, string sucu, string equipo, string folio, string fecha, string neto, string exento, string iva, string total, string documento)
        {
            try
            {
                Initialize2();
                leer(Program.rutempresa);
                configuracion.LeerArchivo();
                string h = configuracion.Empresa.RutEmpresa; 
               // string boleta = File.ReadAllText(ruta, Encoding.GetEncoding("ISO-8859-1"));

                // string aa= @"C:/Users/one/Documents/bowa/" + rut + "/pdf/"+folio+".pdf";
                //   byte[] pdf_boleta = File.ReadAllBytes(aa);
                //FileInfo info = new FileInfo(aa);

                
              //  byte[] rawData = File.ReadAllBytes("");
                string claveboleta = rut + "_" + folio;

                string connectionString = @"server=localhost;userid=root;password=12345678;database=bowa";

                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into boletas(tipo,rut, sucursal, equipo, folio, fecha_emision," +
                        "neto, exento, iva,total, claveboleta) VALUES " + "(@tipo,@rut, @sucursal, @equipo, @folio," +
                        "@fecha_emision, @neto,@exento,@iva,@total,@claveboleta) ON DUPLICATE KEY UPDATE fecha_emision=@fecha_emision, neto=@neto," +
                        "exento=@exento,iva=@iva,total=@total,claveboleta=@claveboleta";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@rut", rut);
                    cmd.Parameters.AddWithValue("@sucursal", sucu);
                    cmd.Parameters.AddWithValue("@equipo", equipo);
                    cmd.Parameters.AddWithValue("@folio", folio);
                    cmd.Parameters.AddWithValue("@fecha_emision", fecha);
                    cmd.Parameters.AddWithValue("@neto", neto);
                    cmd.Parameters.AddWithValue("@exento", exento);
                    cmd.Parameters.AddWithValue("@iva", iva);
                    cmd.Parameters.AddWithValue("@total", total);

                    cmd.Parameters.AddWithValue("@claveboleta", claveboleta);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "Insert into boleta_archivo(clave_boleta,boleta_text) VALUES " + "(@claveboleta,@boleta) ON DUPLICATE KEY UPDATE boleta_text=@boleta";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@boleta", "");
                    cmd.ExecuteNonQuery();
                    connection.Close();


                    connection = null;
                    connectionString = @"server=sh-pro12.hostgator.cl;userid=posfacto;password=eD1[P4q4kZw+0M;database=posfacto_bowa";
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into boletas(tipo,rut, sucursal, equipo, folio, fecha_emision," +
                        "neto, exento, iva,total, claveboleta) VALUES " + "(@tipo,@rut, @sucursal, @equipo, @folio," +
                        "@fecha_emision, @neto,@exento,@iva,@total,@claveboleta) ON DUPLICATE KEY UPDATE fecha_emision=@fecha_emision, neto=@neto," +
                        "exento=@exento,iva=@iva,total=@total,claveboleta=@claveboleta";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@rut", rut);
                    cmd.Parameters.AddWithValue("@sucursal", sucu);
                    cmd.Parameters.AddWithValue("@equipo", equipo);
                    cmd.Parameters.AddWithValue("@folio", folio);
                    cmd.Parameters.AddWithValue("@fecha_emision", fecha);
                    cmd.Parameters.AddWithValue("@neto", neto);
                    cmd.Parameters.AddWithValue("@exento", exento);
                    cmd.Parameters.AddWithValue("@iva", iva);
                    cmd.Parameters.AddWithValue("@total", total);

                    cmd.Parameters.AddWithValue("@claveboleta", claveboleta);
                    cmd.ExecuteNonQuery();
                    Console.Write("\nBOLETA GRABADA");

                    cmd.CommandText = "Insert into boleta_archivo(clave_boleta,boleta) VALUES " + "(@claveboleta,@boleta) ON DUPLICATE KEY UPDATE boleta=@boleta";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@boleta", "");
                    cmd.ExecuteNonQuery();
                    /*
                    connection = null;
                    connectionString = @"server=162.241.61.53;userid=posfacto_bowa;password=bowa123.;database=posfacto_bowa";
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into pdf_archivo(rut, boleta, pdf_boleta) VALUES " + "(@rut,@boleta, @pdf_boleta)" +
                        " ON DUPLICATE KEY UPDATE pdf_boleta=@pdf_boleta";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@rut", rut);
                    cmd.Parameters.AddWithValue("@boleta", folio);
                    cmd.Parameters.AddWithValue("@pdf_boleta", pdf_boleta);
                    cmd.ExecuteNonQuery();

                    */
                    connection.Close();

                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }






            }
            catch (Exception ex)
            {
               
                Console.WriteLine("Ha ocurrido un error:" + ex);

            }
        }

        public static void grabar_sobre(string rut, int a)
        {
            Initialize2();
          //  string connectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
           string connectionString = @"server=localhost;userid=root1;password=12345678;database=bowa";

            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "Insert into rut_sobre (rut, nuevo) VALUES " +
             "(@rut, @nuevo) ON DUPLICATE KEY UPDATE nuevo=@nuevo";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@rut", rut);
                cmd.Parameters.AddWithValue("@nuevo", a);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ee) {
                Console.Write("Error conexion: " + ee);
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            }

            string query = "SELECT * FROM rut_sobre_gerson WHERE rut ='" + rut + "'";
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conexion);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.Write("Rut gerson encontrado: " + rut);
                        try
                        {
                            connection = new MySqlConnection(connectionString);
                            connection.Open();
                            cmd = new MySqlCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = "UPDATE `rut_sobre_gerson` SET nuevo=@nuevo WHERE rut=@rut";
                            cmd.Prepare();

                            cmd.Parameters.AddWithValue("@rut", rut);
                            cmd.Parameters.AddWithValue("@nuevo", a);
                            cmd.ExecuteNonQuery();

                            Console.Write("Cliente Gerson en activado: " + rut);
                        
                            cmd.CommandText = "Insert into rut_sobre (rut, nuevo) VALUES " +
                        "(@rut, @nuevo) ON DUPLICATE KEY UPDATE nuevo='"+"0"+"'";
                            cmd.Prepare();

                            cmd.Parameters.AddWithValue("@rut", rut);
                            cmd.Parameters.AddWithValue("@nuevo", a);
                            cmd.ExecuteNonQuery();
                           

                        }
                        catch (Exception ee) {
                            Console.Write("Error conexion: " + ee);
                            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
            }
            connection.Close();
            conexion.Close();
        }

        public static void HasRows()
        {
            Initialize2();
            string query = "SELECT * FROM cliente_uno where 1";
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conexion);
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
                conexion.Close();
            }
        }
        public static int existe_viejo = 0;
        public static void buscar_viejo(string rut)
        {
            Initialize2();
            string query = "SELECT * FROM cliente_viejo WHERE rut='" + rut + "'";
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conexion);
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
                conexion.Close();
            }
        }

        public static void guardar_tabla()
        {/*
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
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
            }
            Initialize2();
            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conexion);
                for (int y1=0; y1 < Program.rut_produccion.Count; y1++)
                {
                    string rr= Program.rut_produccion[y1];
                    cmd.CommandText = "INSERT IGNORE INTO rut_declarar (rut, nuevo) VALUES ('"+rr+"', 0)";
                    cmd.ExecuteNonQuery();
                }
                conexion.Close();
                

            }
            Program.rut_produccion.Clear();
            */

        }
        public static void leer(string rut)
        {
            Initialize2();
            string query = "SELECT * from certificado WHERE rut_empresa=@rut_empresa";
           // Console.WriteLine("\nentro");
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
                if (TipoBoleta)
                {
                    tipoboleta = 41;
                }
                else { tipoboleta = 39; }
            }
            conexion.Close();
        }


        public static string Initialize2()
        {
            try
            {
                conexion.Close();
              //  conexion.ConnectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
                conexion.ConnectionString = "SERVER=" + "localhost" + ";" + "USER=root1;" + "DATABASE=bowa;" + "PASSWORD=12345678;";

                conexion.Open();
                return null;
            }
            catch(Exception ex)
            {
                Console.Write("Error conexion: " + ex);
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                return "error";
                using (StreamWriter sw = File.CreateText(@Program.ruta+"\\temp\\listo_3.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
            }
        }
        public static string Initialize3()
        {
            try
            {

                conexion.ConnectionString = "SERVER=" + "sh-pro12.hostgator.cl" + ";" + "user=posfacto;" + "DATABASE=posfacto_bowa;" + "PASSWORD=eD1[P4q4kZw+0M;";
                conexion.Open();
                return null;
            }
            catch (Exception ex)
            {
                Console.Write("Error conexion: " + ex);
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                return "error";
                using (StreamWriter sw = File.CreateText(@Program.ruta + "\\temp\\listo_3.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
            }
        }
        //open connection to database
        public static bool OpenConnection()
        {
            conexion.Close();
            try
            {
                conexion.Open(); //// lo debo cambiar para que se caiga
                return true;

            }
            catch (MySqlException ex)

            {
                Console.Write("Error conexion: " + ex);
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                using (StreamWriter sw = File.CreateText(@Program.ruta + "\\temp\\listo_3.txt"))
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
                Console.Write("Error conexion: " + ex);
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                Console.Write(ex.Message);
                return false;
                using (StreamWriter sw = File.CreateText(@Program.ruta + "\\temp\\listo_3.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
            }
        }

       

       
        public string TimbrarYFirmarXMLDTE(string time,DTE dte, string pathResult, string pathCaf, string archivo, string @ruta)
        {
            /*En primer lugar, el documento debe timbrarse con el CAF que descargas desde el SII, es simular
             * cuando antes debías ir con las facturas en papel para que te las timbraran */
            string messageOut = string.Empty;
            X509Certificate cer = ObtenerCertificado(configuracion.Certificado.Nombre, "");
            String fecha_vencimiento = "0";
            try
            {
                fecha_vencimiento = cer.GetExpirationDateString();
            }
            catch(Exception ee)
            {
                fecha_vencimiento = "0";
            }

            DateTime datetime;
            
            DateTime.TryParse(fecha_vencimiento, out datetime);
            if (DateTime.Compare(datetime,DateTime.Today)>0) 
            {
                vencido = "0";
            }
            else
            {
                vencido = "1";
               
        }
            if (vencido=="0")
            {
              
            try
                {
                    rev_caf = 0;
               
                dte.Documento.Timbrar(time,
                    EnsureExists((int)dte.Documento.Encabezado.IdentificacionDTE.TipoDTE, dte.Documento.Encabezado.IdentificacionDTE.Folio, pathCaf),
                    out messageOut);
            }
            catch (Exception ee) {
                string docPath = @ruta;
                Console.Write("Error conexion: " + ee);
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                // Write the string array to a new file named "WriteLines.txt".
                TextWriter tw = new StreamWriter(docPath + "log.txt", true);
                tw.WriteLine("Error Archivo xml original vacio "+archivo);
                tw.Close();
            }
            }

            /*El documento timbrado se guarda en la variable pathResult*/

            /*Finalmente, el documento timbrado debe firmarse con el certificado digital*/
            /*Se debe entregar en el argumento del método Firmar, el "FriendlyName" o Nombre descriptivo del certificado*/
            /*Retorna el filePath donde estará el archivo XML timbrado y firmado, listo para ser enviado al SII*/
            Initialize2();
            leer(Program.rutempresa);
            configuracion.LeerArchivo();
            string h = configuracion.Empresa.RutEmpresa;
            // Console.WriteLine("\npaso1" + configuracion.Empresa.RutEmpresa);
            string h1 = configuracion.Certificado.Nombre;

            Console.WriteLine("\nusando certificado: " + configuracion.Certificado.Nombre);
            Console.WriteLine("\npath result: " + pathResult);

            var c = dte.Firmar(configuracion.Certificado.Nombre, out messageOut, pathResult, "", "");
            //Console.WriteLine("\npaso2" + c);

            if (c.Equals(""))
            {
                // MessageBox.Show("Error - aplicacion no activa", "Error", MessageBoxButtons.OK);
                Console.WriteLine("Error - aplicacion no activa");
            }
            Console.WriteLine("@ruta: " + c);
            return c;
        }

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

        public EnvioBoleta GenerarEnvioBoletaDTEToSII(List<DTE> dtes, List<string> xmlDtes)
        {
            leer(Program.rutempresa);
            configuracion.LeerArchivo();
            var EnvioSII = new ItaSystem.DTE.Engine.Envio.EnvioBoleta();
            EnvioSII.SetDTE = new ItaSystem.DTE.Engine.Envio.SetDTE();
            var date1 = DateTime.Now.Date;
            string v = date1.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
            EnvioSII.SetDTE.Id = "FENV010_" + Program.envio;
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
            else
            {
                return false;
            }
        }
        public static int rev_caf = 0;
        private string EnsureExists(int tipoDTE, int folio, string pathCaf)
        {
            var cafFile = string.Empty;
            try
            {

                foreach (var file in System.IO.Directory.GetFiles(pathCaf))
                {
                    if (ParseName((new FileInfo(file)).Name, tipoDTE, folio) && rev_caf == 1)
                    {
                        Console.WriteLine(file);
                        cafFile = file;
                        Console.WriteLine(cafFile + " rev" + rev_caf);
                        return cafFile;
                    }
                }
                if (string.IsNullOrEmpty(cafFile) && rev_caf == 0)
                    Console.WriteLine(cafFile);
                throw new Exception("NO HAY UN CÓDIGO DE AUTORIZACIÓN DE FOLIOS (CAF) ASIGNADO PARA ESTE TIPO DE DOCUMENTO (" + tipoDTE + ") QUE INCLUYA EL FOLIO REQUERIDO (" + folio + ").");
               
                Console.Write("Error no se consiguio CAF");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            }
            catch (Exception ex)
            {
                Console.Write("Error conexion: " + ex);
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                //MessageBox.Show(""+ ex);
                Console.WriteLine("" + ex);
                using (StreamWriter sw = File.CreateText(@Program.ruta + "\\temp\\listo.txt"))
                {
                    
                            sw.WriteLine(DateTime.Now.ToString()+" Error: "+ex);
                        
                }
            }
            Console.Write("caf " + cafFile);
            return cafFile;
        }

        private static bool ParseName(string name, int tipoDTE, int folio)
        {
            try
            {
                bool f = false;
                if (rev_caf == 0)
                {
                    Console.WriteLine("nombre " + name + " tipoDTE " + tipoDTE + " folio" + folio);
                    var values = name.Substring(0, name.IndexOf('.')).Split('_');
                    double tipo = Convert.ToInt64(values[0]);
                    double desde = Convert.ToInt64(values[1]);
                    double hasta = Convert.ToInt64(values[2]);
                    Console.WriteLine("tipo" + tipo + " desde" + desde + " hasta" + hasta);
                    if (folio >= desde && folio <= hasta)
                    {
                        f = true;
                        rev_caf = 1;
                    }
                    else { 
                    f = false;
                    rev_caf = 0;
                }
            }
                return f;
            }
            catch (Exception ee){
                Console.Write("Error conexion: " + ee);
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                return false;
            }
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
