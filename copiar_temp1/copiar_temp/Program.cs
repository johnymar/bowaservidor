using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using static System.Net.Mime.MediaTypeNames;

namespace copiar_temp
{
    class Program
    {
        static public List<string> rut_produccion = new List<string>();
        static public List<string> boletas = new List<string>();
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
        public static MySqlConnection conexion = new MySqlConnection();
        public static string ruta = "C:/Users/Administrador/Documents/bowa/";
        static void Main(string[] args)
        {
            try
            {
                Thread t = new Thread(new ParameterizedThreadStart(ThreadLoop));
                t.Start((Action)CallBack);
            }
            catch (Exception ee)
            {

                //  Console.Write("Press <Enter> to exit... ");
                // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
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
                    Thread.Sleep(5000);
                }
                catch (Exception ee)
                {

                    // Console.Write("Press <Enter> to exit... ");
                    // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                }
            }
        }

        public static void HasRows()
        {


            try
            {
                Initialize2();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "SELECT rut FROM cliente where status='0' order by rut desc ";
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
                conexion.Close();
            }
            catch (Exception ee)
            {

            }
        }
        public static string Initialize2()
        {
            conexion.Close();
            try
            {

                //  conexion.ConnectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
                conexion.ConnectionString = @"server=localhost;userid=root1;password=12345678;database=bowa";

                conexion.Open();
                return null;
            }
            catch (Exception ee)
            {
                return "error " + ee;

            }
        }

        static void principal()
        {
            int cc = 0;
            while (true)
            {
                rut_produccion.Clear();
                HasRows();
                int cuenta_ruts = rut_produccion.Count();
                for (int pasar = 0; pasar < cuenta_ruts; pasar++)
                {
                    rutempresa = rut_produccion[pasar];
                    char c = rutempresa[0];
                    if (c != ' ')
                    {
                        Console.WriteLine("Revisando Rut: " + pasar + ":" + cuenta_ruts + " " + rutempresa);
                        try
                        {
                            Initialize2();
                            MySqlCommand cmd = new MySqlCommand();
                            cmd.Connection = conexion;
                            cmd.CommandText = "SELECT * from log_descarga WHERE rut=@rut_empresa && copiada='0'&& descargada='1' && clave not like '%vacio%'";
                            cmd.Parameters.AddWithValue("@rut_empresa", rutempresa);
                            cmd.Prepare();

                            MySqlDataReader reader = cmd.ExecuteReader();


                            while (reader.Read())
                            {
                                boletas.Add(reader.GetString(1));
                            }
                            reader.Close();

                        }
                        catch (Exception ee)
                        {

                        }
                    }
                    conexion.Close();
                    double numero_boletas = boletas.Count();

                    for (int i = 0; i < numero_boletas; i++)
                    {
                        string[] h = boletas[i].Split('/');
                        string path = @ruta + "\\temp\\" + h[0] + "/" + h[1] + "/" + h[2] + "/" + h[3] + "/";
                        try
                        {
                            if (Directory.Exists(path))
                            {

                            }
                            else
                            {
                                Directory.CreateDirectory(path);
                                Console.WriteLine("\nGenerando Directorio " + path);

                            }
                        }

                        catch (Exception ee)

                        {
                            System.Diagnostics.Process.Start(
                            Environment.GetCommandLineArgs()[0],
                            Environment.GetCommandLineArgs()[1]);
                            conexion.Close();
                            // close current process
                            Environment.Exit(0);
                        }


                        try
                        {
                            Console.WriteLine("Copiando " + pasar + ":" + cuenta_ruts + " " + boletas[i]);

                            string path1 = @ruta + "\\respaldo2\\" + boletas[i];
                            string[] nb = h[4].Split('.');
                            string numero = nb[0];
                            if (File.Exists(path1))
                            {
                                System.IO.File.Copy(@ruta + "\\respaldo2\\" + boletas[i], @ruta + "\\temp\\" + boletas[i], true);

                            }
                            else
                            {
                                string path2 = @ruta + "\\bowa\\faltantes\\" + rutempresa + "\\" + h[1] + "\\" + h[2] + "\\";

                                try
                                {
                                    if (Directory.Exists(path2))
                                    {
                                        string path3 = @ruta + "\\bowa\\faltantes\\" + rutempresa + "\\" + h[1] + "\\" + h[2] + "\\falt-" + rutempresa + ".txt";
                                        StreamWriter stream = null;
                                        try
                                        {
                                            stream = File.AppendText(path3);
                                            stream.WriteLine(numero);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                        finally
                                        {
                                            if (stream != null)
                                                stream.Close();
                                        }
                                    }
                                    else

                                    {
                                        Directory.CreateDirectory(path2);
                                        Console.WriteLine("\nGenerando Directorio " + path2);

                                    }
                                }
                                catch (Exception ex) { }
                            }
                            try
                            {
                                Initialize2();
                                MySqlCommand cmd = new MySqlCommand();
                                cmd.Connection = conexion;
                                cmd.CommandText = "UPDATE  log_descarga SET copiada='1' WHERE clave like'" + boletas[i] + "'";
                                cmd.Parameters.AddWithValue("@rut_empresa", rutempresa);
                                cmd.Prepare();
                                cmd.ExecuteNonQuery();

                                conexion.Close();
                            }
                            catch (Exception ee) { }



                        }
                        catch (Exception ee)
                        {
                            conexion.Close();
                            // close current process

                        }

                        cc++;
                        if (cc > 10000)
                        {

                        }
                    }



                    if (numero_boletas > 0)
                    {

                        /*              try
                                      {

                                          Initialize2();
                                          MySqlCommand cmd = new MySqlCommand();
                                          cmd.Connection = conexion;
                                          cmd.CommandText = "UPDATE rut_declarar SET nuevo='1' WHERE rut=@rut_empresa";
                                          cmd.Parameters.AddWithValue("@rut_empresa", rutempresa);
                                          cmd.Prepare();
                                          cmd.ExecuteNonQuery();



                                      }
                                      catch (Exception ee)
                                      {
                                          // Application.Restart();
                                          Console.Write("Press <Enter> to exit... " + ee);
                                          System.Diagnostics.Process.Start(
                     Environment.GetCommandLineArgs()[0],
                      Environment.GetCommandLineArgs()[1]);
                                          conexion.Close();
                                          // close current process

                                      }
              */
                    }
                    conexion.Close();
                    boletas.Clear();
                    try
                    {
                        Initialize2();
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conexion;
                        cmd.CommandText = "UPDATE  cliente SET status='1' WHERE rut=@rut_empresa";
                        cmd.Parameters.AddWithValue("@rut_empresa", rutempresa);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();

                        conexion.Close();
                    }
                    catch (Exception ee) { }
                }
                Console.Clear();
                try
                {
                    Initialize2();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conexion;
                    cmd.CommandText = "UPDATE  cliente SET status='0' WHERE 1";
                    cmd.Parameters.AddWithValue("@rut_empresa", rutempresa);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    conexion.Close();
                }
                catch (Exception ee) { }
            }
//            Environment.Exit(0);
            //  _ = System.Diagnostics.Process.Start("copiar1.exe");


        }
    }
}
