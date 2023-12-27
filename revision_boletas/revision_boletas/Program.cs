using revision_boletas.clases;

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
using ItaSystem.DTE.Engine.Helpers;
using System.Collections;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using ItaSystem.DTE.WS.EstadoEnvio;
using Ubiety.Dns.Core;
using System.Security.Cryptography;
using System.Threading;
using System.Linq.Expressions;

namespace Consulta_dte
{
    class Program
    {
        static ItaSystem.DTE.WS.EstadoEnvio.EstadoEnvioResult result;
        static public List<string> rut_produccion = new List<string>();
        static public List<string> tracking = new List<string>();
        static public List<string> sucursal = new List<string>();
        static public List<string> equipo = new List<string>();
        static public List<string> fecha = new List<string>();
        static public string folio = "";

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
        static public string ruta = @"C:\Users\Administrador\Documents\bowa\";
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

                using (StreamWriter sw = File.CreateText(@ruta + "\\temp\\listo1_4.txt"))
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
            catch (Exception ee)
            {
                return "error " + ee;
                using (StreamWriter sw = File.CreateText(@ruta + "\\temp\\listo1_4.txt"))
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
            try
            {
                Thread t = new Thread(new ParameterizedThreadStart(ThreadLoop));
                t.Start((Action)CallBack);
            }
            catch (Exception ee)
            {
            }
        }
        private static void CallBack()
        {

            principal();

        }

        public static void ThreadLoop(object callback)
        {

            while (true)


            {
                try
                {
                    ((Delegate)callback).DynamicInvoke(null);
                    Thread.Sleep(1000);
                }
                catch (Exception ee)
                {
                }
            }
        }

        static void principal()
        {
            string tracking = "0";
            string path = (@ruta + "temp\\revision_boleta_ejecutar.txt");
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                Console.WriteLine(String.Join(Environment.NewLine, lines));
                if (!lines[0].Equals(null))
                {
                    string[] xx = lines[0].Split(':');
                    string[] xx1 = lines[1].Split(':');
                    rutempresa = xx[1];
                    tracking = xx1[1];
                    //                fecha = lines[1];
                }

                track = tracking;
                if ((track != "") && track != "0" && track != "-999999")
                {


                    Handler.Initialize2();
                    try
                    {
                        var responseEstadoDTE = handler.ConsultarEstadoEnvioBoleta(AmbienteEnum.Produccion, Convert.ToInt64(track), result);
                        string respuesta = JsonConvert.SerializeObject(responseEstadoDTE, Newtonsoft.Json.Formatting.Indented);
                        //var responseEstadoDTE = handler.ConsultarEstadoEnvio(AmbienteEnum.Produccion, Convert.ToInt64(track));


                        //string respuesta = responseEstadoDTE.ResponseXml;

                        using (StreamWriter sw = File.CreateText(@ruta + "\\temp\\respuesta_consumo_" + track + ".txt"))
                        {

                            sw.WriteLine(respuesta);
                            Console.WriteLine(respuesta);
                        }


                       
                    }
                    catch (Exception e)
                    {
                        var jObject1 = JObject.Parse(e.Message);
                        Console.Write("Error: " + jObject1);
                       
                    }
                }
            }
            else {
                principal1();
            }
        }
    
        static void principal1()
        {

            string envio = "";
            rut_produccion.Clear();
            HasRows();

            // int cuenta_ruts = rut_produccion.Count();
            int cuenta_ruts = rut_produccion.Count();

            for (int pasar = 0; pasar < cuenta_ruts; pasar++)
            {
                string query = "";
                try
                {
                    tracking.Clear();
                    sucursal.Clear();
                    equipo.Clear();
                    fecha.Clear();
                    rutempresa = rut_produccion[pasar];

                   

                    Console.WriteLine("Buscando envios pendientes por revisar en rut: " + rutempresa);
                    query = "SELECT * from envioboletas WHERE Cliente=@rut_empresa and revision<>'" + 1 + "'";
                    if (OpenConnection() == true)
                    {
                        //Create Command
                        MySqlCommand cmd = new MySqlCommand(query, cusuarioxion);
                        cmd.Parameters.AddWithValue("@rut_empresa", rutempresa);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            //  Console.WriteLine(reader.GetString(3));
                            tracking.Add(reader.GetString(3));
                            fecha.Add(reader.GetString(7));

                            sucursal.Add(reader.GetString(10));
                            equipo.Add(reader.GetString(11));
                        }
                        reader.Close();
                    }
                    if (tracking.Count() == 0)
                    {
                    }
                    else
                    {
                        
                        
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
                        string sucu = "";
                            string equi = "";
                        string fe = "";
                        string respuesta = "";
                        cusuarioxion.Close();
                        int g = tracking.Count();
                        for (int r = 0; r < g; r++)
                        {
                            envio = "";
                            string status = "";
                            string glosa = "";
                            track = tracking[r];
                            sucu = sucursal[r];
                            equi = equipo[r];
                            fe = fecha[r];
                            String path = "";
                            if ((track != "") && track != "0" && track != "-999999")
                            {
                                Handler.Initialize2();
                                Console.WriteLine("Esperando...por Tracking " + track + " del rut " + rutempresa);
                                try
                                {
                                    var responseEstadoDTE = handler.ConsultarEstadoEnvioBoleta(AmbienteEnum.Produccion, Convert.ToInt64(track), result);
                                    respuesta = JsonConvert.SerializeObject(responseEstadoDTE, Newtonsoft.Json.Formatting.Indented);
                                    //var responseEstadoDTE = handler.ConsultarEstadoEnvio(AmbienteEnum.Produccion, Convert.ToInt64(track));


                                    //string respuesta = responseEstadoDTE.ResponseXml;
                                    path = @ruta + "\\" + rutempresa + "\\" + sucu + "\\" + sucu + "\\" + fe + "\\sobre_enviado\\respuesta_consumo_"+track+".txt";

                                    using (StreamWriter sw = File.CreateText(path))
                                    {

                                        sw.WriteLine(respuesta); Console.WriteLine(respuesta);
                                    }





                                    //string respuesta = responseEstadoDTE.ResponseXml;


                                    folio = "0"; status = ""; glosa = "";

                                    using (StreamReader fielRead = new StreamReader(path))
                                    {

                                        String line;
                                        String fecha_emision = "";
                                        String descripcion = "";
                                        int Codigo = 0;
                                        string folio = "0";
                                        while ((line = fielRead.ReadLine()) != null)
                                        {
                                            if (line.Contains("Folio") && Codigo == 0)
                                            {
                                                string[] h = line.Split(':');
                                                folio = h[1];
                                            }
                                            if (line.Contains("fecha_recepcion") && Codigo == 0)
                                            {
                                                string[] h = line.Split(':');
                                                string[] hh = h[1].Split(' ');
                                                fecha_emision = hh[1];
                                            }
                                            if (line.Contains("Estado") && Codigo == 0)
                                            {
                                                string[] h = line.Split(':');
                                                status = h[1];
                                            }
                                            if (line.Contains("Descripcion") && Codigo == 0)
                                            {
                                                string[] h = line.Split(':');
                                                glosa = h[1];
                                            }
                                            if (line.Contains("Codigo") && Codigo == 0)
                                            {
                                                // string[] h = line.Split(':');
                                                Codigo = 1;
                                            }
                                            if (line.Contains("Detalle") || line.Contains("DTE") || line.Contains("Error") && Codigo == 1)
                                            {
                                                // string[] h = line.Split(':');
                                                descripcion = descripcion +line;
                                             
                                            }

                                        
                                        }
                                        if (!status.Contains("EPR") && !status.Contains("RPR") && !status.Contains("RLV") && glosa != "" && fecha_emision != "" && descripcion != "")
                                        {
                                            envio = "1";
                                            Codigo = 0;
                                            glosa = glosa.Replace(',', ' ');
                                            glosa = glosa.ToString().Replace(((char)34).ToString(), "");
                                            status = status.Replace(',', ' ');
                                            status = status.ToString().Replace('"', ' ').Trim();
                                            folio = folio.Replace(',', ' ');
                                            folio = folio.ToString().Replace('"', ' ').Trim();
                                            try
                                            {
                                                Convert.ToInt32(folio);
                                            }
                                            catch (Exception ee)
                                            {
                                                folio = "0";
                                            }
                                            fecha_emision = fecha_emision.Replace(',', ' ');
                                            fecha_emision = fe;
                                            descripcion = descripcion.Replace(',', ' ');
                                            descripcion = descripcion.ToString().Replace('"', ' ').Trim();
                                            try
                                            {
                                                //#String connectionString = @"server=sh-pro12.hostgator.cl;userid=posfacto;password=eD1[P4q4kZw+0M;database=posfacto_bowa";
                                                //MySqlConnection connection = new MySqlConnection(connectionString);
                                                //connection.Open();
                                                // MySqlCommand cmd = new MySqlCommand();
                                                //cmd.Connection = connection;

                                                query = "INSERT INTO `boletas_rechazadas` (`rut`, `folio`, `status`, `glosa`, `fecha_emision`, `descripcion`, `sucursal`) VALUES (@rut_empresa,@folio,@status,@glosa,@fecha_emision,@descripcion,@sucursal)";
                                                if (OpenConnection() == true)
                                                {

                                                    MySqlCommand cmd = new MySqlCommand(query, cusuarioxion);


                                                    cmd.Parameters.AddWithValue("@rut_empresa", rutempresa);
                                                    cmd.Parameters.AddWithValue("@status", status);
                                                    cmd.Parameters.AddWithValue("@glosa", glosa);
                                                    cmd.Parameters.AddWithValue("@folio", Convert.ToInt32(folio));
                                                    cmd.Parameters.AddWithValue("@fecha_emision", fecha_emision);
                                                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                                                    cmd.Parameters.AddWithValue("@sucursal", sucu);

                                                    cmd.Prepare();

                                                    cmd.ExecuteNonQuery();
                                                }
                                                cusuarioxion.Close();
                                                folio = ""; glosa = ""; descripcion = ""; fecha_emision = ""; status = "";
                                                Console.WriteLine("Grabando boleta " + folio + " del rut: " + rutempresa + " status: " + status);

                                            }
                                            catch (Exception ee)
                                            {
                                                Console.Write("Press <Enter> to exit... ");
                                                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                                            }

                                        }
                                        query = "UPDATE envioboletas SET revision=1  WHERE Cliente=@rut_empresa && trackid=@trackid";
                                        if (OpenConnection() == true)
                                        {
                                            //Create Command
                                            MySqlCommand cmd = new MySqlCommand(query, cusuarioxion);
                                            cmd.Parameters.AddWithValue("@rut_empresa", rutempresa);
                                            cmd.Parameters.AddWithValue("@trackid", track);
                                            cmd.ExecuteNonQuery();
                                        }
                                        // Console.WriteLine("Actualizado Envioboleta ");

                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Write("Press <Enter> to exit... ");
                                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                                    var jObject1 = JObject.Parse(respuesta);
                                    Console.Write("Error: " + jObject1);

                                    using (StreamWriter sw = File.CreateText(@ruta + "\\temp\\log_error_consulta.txt"))
                                    {

                                        sw.WriteLine(ex);

                                    }
                                }

                            }
                        }
                        if (envio == "1")
                        {
                            Console.WriteLine("Envio NO-ok rut" + rutempresa);
                            envio = "";
                        }
                        else
                        {
                            Console.WriteLine("Envio ok rut" + rutempresa);
                        }
                    }
                }
                catch { }
                          
                Thread.Sleep(1000);

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
            // grabar_consulta(archivo, track, fecha_rec, tipo, informados, aceptados, rechazados, reparos);
            File.Delete(archivo);
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


                string connectionString = @"server = 45.7.230.91; userid = root1; password = 12345678; database = bowa";

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
