using Consulta_dte.clases;
using dte_local1;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static ITA_CHILE.Enum.Ambiente;
using Formatting = Newtonsoft.Json.Formatting;

namespace Consulta_dte
{
    class Program
    {
        static public List<string> rut_produccion = new List<string>();
        static public List<string> tracking = new List<string>();
        static public List<string> status_lleno= new List<string>();
        public static string ruta = @"C:/Users/Administrador/Documents/bowa/";

        static public string rutempresa = "";
        static string track = "";
        static string fecha_rec = "";
        static string estado = "";
        static string informados = "";
        static string aceptados = "";
        static string rechazados = "";
        static string reparos = "";
        static string tipo = "";
        static string archivo = "";
        static public string nombre = "";
        static Handler handler = new Handler();
        static public Configuracion configuracion = new Configuracion();

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

                using (StreamWriter sw = File.CreateText(@"C:\Users\usuario\Documents\bowa\temp\listo1.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
                return false;
            }
        }
        public static MySqlConnection cusuarioxion = new MySqlConnection();

        public static string Initialize2()
        {
            cusuarioxion.Close();
            try
            {

                cusuarioxion.ConnectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";

                cusuarioxion.Open();
                return null;
            }
            catch(Exception ee)
            {
                return "error " + ee;
                using (StreamWriter sw = File.CreateText(@ruta + "temp/listo1.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                }
            }
        }
        public static void HasRows()
        {
            Initialize2();
            string query = "SELECT rut FROM cliente ORDER BY rut,id";
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, cusuarioxion);
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
        }
        static void Main(string[] args)
        {
          /*  string path=@"C:\Users\usuario\Documents\bowa\temp\tracking.txt";
            if (File.Exists(path))
            {
                string[] lines = new string[1];
                lines = File.ReadAllLines(path);
                Console.WriteLine(String.Join(Environment.NewLine, lines));
                if (!lines[0].Equals(null))
                {
                    string[] hh = lines[0].Split('_');
                    rutempresa = hh[0];
                    track = hh[1];
                    //                fecha = lines[1];
                    Console.WriteLine("\nInicio de Firma de Boletas para RUT:[ " + rutempresa + " ]");
                }
            }
            Console.WriteLine("Esperando...");
            try
            {
                //var responseEstadoDTE = handler.ConsultarEstadoEnvioBoleta(AmbienteEnum.Produccion, Convert.ToInt64(track));
                //string respuesta = JsonConvert.SerializeObject(responseEstadoDTE, Newtonsoft.Json.Formatting.Indented);
                var responseEstadoDTE = handler.ConsultarEstadoEnvio(AmbienteEnum.Produccion, Convert.ToInt64(track));


                //string respuesta = responseEstadoDTE.ResponseXml;

                //var responseEstadoDTE = handler.ConsultarEstadoEnvioBoleta(AmbienteEnum.Produccion, Convert.ToInt64(track));
                string respuesta = JsonConvert.SerializeObject(responseEstadoDTE, Formatting.Indented);
                   Console.WriteLine("Respuesta Sii:" + respuesta);
                using (StreamWriter sw = File.CreateText(@"C:\Users\usuario\Documents\bowa\temp\respuesta_consumo.txt"))
                {

                    sw.WriteLine(respuesta);

                }
            }catch(Exception ee) { }

                Console.Write("Press <Enter> to exit... ");
               while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
               Environment.Exit(0);*/
                Initialize2();
            rut_produccion.Clear();
            HasRows();
            
            int cuenta_ruts = rut_produccion.Count();
            for (int pasar = 0; pasar < cuenta_ruts; pasar++)
            {
                rutempresa= rut_produccion[pasar];
                char c= rutempresa[0];
                if (c != '7' )
                {
                    Console.WriteLine("Revisando tracking consumo Rut: [" + cuenta_ruts + ":" + pasar + "] " + rut_produccion[pasar]);
                    tracking.Clear();
                    status_lleno.Clear();
                    rutempresa = rut_produccion[pasar];
                    string query = "SELECT * from envioconsumo WHERE Cliente=@rut_empresa";
                    if (OpenConnection() == true)
                    {
                        //Create Command
                        MySqlCommand cmd = new MySqlCommand(query, cusuarioxion);
                        cmd.Parameters.AddWithValue("@rut_empresa", rutempresa);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            tracking.Add(reader.GetString(2));
                            status_lleno.Add(reader.GetString(20));
                        }
                        reader.Close();
                    }
                    query = "SELECT nombre_representante from certificado WHERE rut_empresa=@rut_empresa";
                    if (OpenConnection() == true)
                    {
                        //Create Command
                        MySqlCommand cmd = new MySqlCommand(query, cusuarioxion);
                        cmd.Parameters.AddWithValue("@rut_empresa", rutempresa);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            nombre = reader.GetString(0);
                        }
                        reader.Close();
                    }

                    cusuarioxion.Close();
                    int g = tracking.Count();
                    for (int r = 0; r < g; r++)
                    {
                        string status = "";
                        string glosa = "";


                        track = tracking[r];
                        if ((track != "" || track != "0") && (status_lleno[r] == "" || status_lleno[r] == null || !status_lleno[r].Contains("EPR")))
                        {



                            Console.WriteLine("Esperando...");
                            try
                            {
                                //var responseEstadoDTE = handler.ConsultarEstadoEnvioBoleta(AmbienteEnum.Produccion,Convert.ToInt64(track));
                                //string respuesta = JsonConvert.SerializeObject(responseEstadoDTE, Newtonsoft.Json.Formatting.Indented);
                                var responseEstadoDTE = handler.ConsultarEstadoEnvio(AmbienteEnum.Produccion, Convert.ToInt64(track));


                                string respuesta = responseEstadoDTE.ResponseXml;

                                using (StreamWriter sw = File.CreateText(@ruta + "temp\respuesta_consumo.txt"))
                                {

                                    sw.WriteLine(respuesta);

                                }


                                using (StreamReader fielRead = new StreamReader(@ruta + "temp\respuesta_consumo.txt"))
                                {
                                    String line;

                                    while ((line = fielRead.ReadLine()) != null)
                                    {
                                        string[] datos = line.Split(new char[] { ',' });
                                        string d = datos[0];
                                        if (datos[0].Contains("<ESTADO"))
                                        {
                                            string[] h = datos[0].Split('>');
                                            string[] hh = h[1].Split('<');
                                            status = hh[0];
                                        }
                                        if (datos[0].Contains("<GLOSA"))
                                        {
                                            string[] h = datos[0].Split('>');
                                            string[] hh = h[1].Split('<');
                                            glosa = hh[0];
                                        }

                                    }
                                }
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine("Error " + ee);
                                using (StreamWriter sw = File.CreateText(@ruta + "temp/log_error_consulta.txt"))
                                {

                                    sw.WriteLine(ee);

                                }

                            }

                            try
                            {
                                Console.WriteLine("\nStatus =" + status + " Glosa: " + glosa);
                            
                              string connectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
                                MySqlConnection connection = null; 
                                connection = new MySqlConnection(connectionString);
                                connection.Open();
                                MySqlCommand cmd = new MySqlCommand();
                                cmd.Connection = connection;

                                
                                    cmd.CommandText = "UPDATE envioconsumo SET status=@status, Glosa=@glosa WHERE Cliente = @rutempresa and trackid= @track";
                                    cmd.Prepare();

                                                                 cmd.Parameters.AddWithValue("@track", track);
                                                                  cmd.Parameters.AddWithValue("@rutempresa", rutempresa);
                                                               cmd.Parameters.AddWithValue("@status", status);
                                    cmd.Parameters.AddWithValue("@glosa", glosa);
                                    cmd.ExecuteNonQuery();

                                cusuarioxion.Close();

                                connectionString = @"server=sh-pro12.hostgator.cl;userid=posfacto;password=eD1[P4q4kZw+0M;database=posfacto_bowa";
                                connection = null;
                                connection = new MySqlConnection(connectionString);
                                connection.Open();
                                cmd = new MySqlCommand();
                                cmd.Connection = connection;

                                cmd.CommandText = "UPDATE envioconsumo SET status=@status WHERE Cliente = @rutempresa and trackid= @track";
                                cmd.Prepare();

                                cmd.Parameters.AddWithValue("@track", track);
                                cmd.Parameters.AddWithValue("@rutempresa", rutempresa);
                                cmd.Parameters.AddWithValue("@status", status);
                                cmd.ExecuteNonQuery();
                                Console.WriteLine("Grabando en posfactory rut" + rutempresa + " status" + status);

                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine("Error " + ee);
                                using (StreamWriter sw = File.CreateText(@ruta + "temp/log_error_consulta.txt"))
                                {

                                    sw.WriteLine(ee);

                                }

                            }


                            /*            if (File.Exists(path))
                                        {
                                            string[] lines = new string[1];
                                            lines = File.ReadAllLines(path);
                                            Console.WriteLine(String.Join(Environment.NewLine, lines));
                                            if (!lines[0].Equals(null))
                                            {
                                                string[] hh = lines[0].Split('_');
                                                rutempresa = hh[0];
                                                track = hh[1];
                                                //                fecha = lines[1];
                                                Console.WriteLine("\nInicio de Firma de Boletas para RUT:[ " + rutempresa + " ]");
                                            }*/


                            //   Program.ReadMatrixFromFile(archivo_respuesta);

                        }
                    }
                }
            }
        }
        static void WritMatrixToFile(int[][] matrix, string outputFile)
        {
            using (var writer = new StreamWriter(outputFile))
            {
                for (int row = 0; row < matrix.Length; row++)
                {
                    writer.WriteLine(string.Join(",", matrix[row]));
                }
            }
        }
        static List<string> GetLines(string inputFile)
        {
            List<string> lines = new List<string>();
            using (var reader = new StreamReader(inputFile))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        lines.Add(line);
                    }
                }
            }
            return lines;
        }



        static public void ReadMatrixFromFile(string inputFile)
        {
            track = "0";
            fecha_rec = DateTime.Now.ToString();
            estado = "error";
            tipo = "39";
            informados = "0";
            rechazados = "0";
            aceptados = "0";
            reparos = "0";
            var lines = GetLines(inputFile);
            int y = 0;
            for (int i = 0; i < lines.Count - 1; i++)
            {
                var line = lines[i];
                if (line.Length == 1) { }
                else
                {

                    string[] p = line.Split(':');
                    if (p.Length == 1) { }
                    else
                    {
                        string r = p[0].Replace('"', ' ');
                        string r1 = p[1].Replace(',', ' ');
                        if (r.Contains("TrackId"))
                        {
                            track = r1;
                        }
                        if (r.Contains("fecha_recepcion"))
                        {
                            string[] h = r1.Split(' ');
                            fecha_rec = h[1];
                            fecha_rec = fecha_rec.Replace('"', ' ');
                            fecha_rec = fecha_rec.Trim();
                        }
                        if (r.Contains("Estado"))
                        {
                            estado = r1;
                        }
                        if (r.Contains("Tipo"))
                        {
                            tipo = r1;
                        }
                        if (r.Contains("Informados"))
                        {
                            informados = r1;
                        }
                        if (r.Contains("Aceptados"))
                        {
                            aceptados = r1;
                        }
                        if (r.Contains("Rechazados"))
                        {
                            rechazados = r1;
                        }
                        if (r.Contains("Reparos"))
                        {
                            reparos = r1;
                        }

                    }
                }
            }
            grabar_consulta(archivo, track, fecha_rec, tipo, informados, aceptados, rechazados, reparos);
            File.Delete(archivo);
            Console.Write("Press <Enter> to exit... ");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }
        public static void grabar_consulta(string archivo, string track, string fecha_rec, string tipo, string informados, string acpetados, string rechazados, string reparos)
        {
            try
            {
                byte[] rawData = File.ReadAllBytes(archivo);
                FileInfo info = new FileInfo(archivo);
                Handler.Initialize2();
                Handler.leer(Program.rutempresa);
                configuracion.LeerArchivo();

                Handler.Cliente = Program.rutempresa;


                string connectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";

                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into respuestasii (Trackid, Estado, Fecha, tipoDoc, Informados," +
                        "aceptados, rechazados, reparos,cliente, respuesta_txt) VALUES " + "(@Trackid, @Estado, @Fecha, @tipoDoc," +
                        "@Informados, @aceptados,@rechazados,@reparos,@cliente, @respuesta_txt) ON DUPLICATE KEY UPDATE Estado = @Estado, Fecha = @Fecha, tipoDoc = @tipoDoc, Informados = @Informados," +
                        "aceptados = @aceptados, rechazados = @rechazados, reparos = @reparos,cliente = @cliente, respuesta_txt= @respuesta_txt";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@Trackid", track);
                    cmd.Parameters.AddWithValue("@Estado", estado);
                    cmd.Parameters.AddWithValue("@Fecha", fecha_rec);
                    cmd.Parameters.AddWithValue("@tipoDoc", tipo);
                    cmd.Parameters.AddWithValue("@Informados", informados);
                    cmd.Parameters.AddWithValue("@aceptados", aceptados);
                    cmd.Parameters.AddWithValue("@rechazados", rechazados);
                    cmd.Parameters.AddWithValue("@reparos", reparos);
                    cmd.Parameters.AddWithValue("@cliente", rutempresa); 
                   cmd.Parameters.AddWithValue("@respuesta_txt", rawData);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
            }
            catch (Exception ee) { }
        }

    }
}
