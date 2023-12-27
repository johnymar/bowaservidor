using ItaSystem.DTE.Engine.Documento;
using ItaSystem.DTE.Engine.Envio;
using ItaSystem.DTE.Engine.XML;
using boletas.clases;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static ITA_CHILE.Enum.Ambiente;
using System.Xml;
using System.Xml.Linq;
using System.Data;

namespace boletas
{
    class Program
    {
        static int esteequipo = 1;
        static string track = "";
        static string fecha_rec = "";
        static string estado = "";
        static string informados = "";
        static string aceptados = "";
        static string rechazados = "";
        static string reparos = "";
        static string tipo = "";
        static public List<string> rut_produccion = new List<string>();


        static readonly string[][] emptyArray = new string[0][];
        static List<string> AllFiles = new List<string>();
        static List<string> AllFiles1 = new List<string>();
        static List<string> AllFiles2 = new List<string>();
        static List<string> AllFiles3 = new List<string>();
        static public string docPath = "";
        static public string nombre_file = "";
        static public string fecha = "";
        private static string fecha_rcof;
        static public string pathtoken = "";
        static public string envio;
        static public string enviox;
        private static Autorizacion aut;
        static public Configuracion configuracion = new Configuracion();
        static List<string> AllFilesx = new List<string>();
        static public string ruta1 = "C:\\Users\\administrador\\Documents\\bowa\\respaldo2\\";
        static public string ruta = "C:\\Users\\administrador\\Documents\\bowa\\";


        public static void enviar_sobrex(String rut, String fecha)
        {

            string t1 = rut;
            AllFiles2.Clear();
            AllFiles3.Clear();
            Console.WriteLine("" + rut);
            int incluir = 0;
            AllFiles.Add(t1);
            string path = "";
            int cantidad = AllFiles.Count();
            for (int i = 0; i < cantidad; i++)
            {
                if (AllFiles[i] != "temp" && AllFiles[i] != "respaldo" && AllFiles[i] != "rcof" && AllFiles[i] != "caf")
                {
                    path = @ruta + rut + "\\";
                    try
                    {
                        string[] folderPaths = Directory.GetDirectories(path);
                        foreach (string s in folderPaths)
                        {
                            string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                            if (t != rut && t != "caf" && t != "rcof")
                            {
                                if (AllFiles2.Count > 0)
                                {
                                    for (int tr = 0; tr <= AllFiles2.Count - 1; tr++)
                                    {
                                        if (incluir == 0)
                                        {
                                            if (AllFiles2[tr] != t)
                                            {
                                                incluir = 0;

                                            }
                                            else
                                            {
                                                incluir = 1;
                                            }
                                        }
                                    }
                                    if (incluir == 0)
                                    {
                                        AllFiles2.Add(t);
                                    }
                                    else
                                    {
                                        incluir = 0;
                                    }
                                }


                                else
                                {
                                    AllFiles2.Add(t);
                                }
                            }
                        }
                    }

                    catch (Exception ee)

                    {
                    }
                }
            }

            cantidad = AllFiles2.Count();
            for (int i = 0; i < cantidad; i++)
            {

                path = @ruta + rut + "\\" + AllFiles2[i] + "\\" + AllFiles2[i] + "\\";
                try
                {
                    string[] folderPaths = Directory.GetDirectories(path);
                    foreach (string s in folderPaths)
                    {
                        string t = s.Remove(0, s.LastIndexOf('\\') + 1);

                        if (AllFiles3.Count > 0 )
                        {
                            for (int tr = 0; tr <= AllFiles3.Count - 1; tr++)
                            {
                                if (incluir == 0)
                                {
                                    if (AllFiles3[tr] != t)
                                    {
                                        incluir = 0;

                                    }
                                    else
                                    {
                                        incluir = 1;
                                    }
                                }
                            }
                            if (incluir == 0)
                            {
                                AllFiles3.Add(t);
                            }
                            else
                            {
                                incluir = 0;
                            }
                        }

                        else
                        {
                            AllFiles3.Add(t);
                        }
                    }
                }

                catch (Exception ee)

                {
                }
            }
            cantidad = AllFiles3.Count();
            for (int i = 0; i < cantidad; i++)
            {



                string dpath1 = (@ruta + rut + "\\1\\1\\" + AllFiles3[i] + "\\");
                string dpath2 = (@ruta + rut + "\\2\\2\\" + AllFiles3[i] + "\\");
                string dpath3 = (@ruta + rut + "\\3\\3\\" + AllFiles3[i] + "\\");
                string dpath4 = (@ruta + rut + "\\4\\4\\" + AllFiles3[i] + "\\");
                string dpath5 = (@ruta + rut + "\\5\\5\\" + AllFiles3[i] + "\\");
                string mes_reenvio = "0";
                string dia_reenvio = "0";
                if (AllFiles3[i] != "1" && AllFiles3[i] != "2" && AllFiles3[i] != "3" && AllFiles3[i] != "4" && AllFiles3[i] != "5" && AllFiles3[i] != "INFORMACION")
                {
                    string año_reenvio = "0";

                    string[] liness = new string[2];

                    string[] ll = fecha.Split('-');
                    if (!fecha.Equals(null))
                    {
                        mes_reenvio = ll[1];
                        año_reenvio = ll[0];
                        dia_reenvio = ll[2];
                        //                fecha = lines[1];

                    }
                }

                if (AllFiles3[i] != "INFORMACION")
                {
                    string[] mm = AllFiles3[i].Split('-');
                    string mx = mm[1];
                    string ma = mm[0];
                    string dx = mm[2];


                    if ((Directory.Exists(dpath1) && Convert.ToInt32(mx) <= Convert.ToInt32(mes_reenvio)) || ma != "2023") // && Convert.ToInt32(dia_reenvio) !=0 && Convert.ToInt32(dx) == Convert.ToInt32(dia_reenvio))
                    {

                        if (Directory.Exists(dpath1))
                        {
                            try
                            {

                                Directory.Delete(dpath1, recursive: true);    //throws if directory doesn't exist. 
                                Console.WriteLine("Borrando " + dpath1);
                            }
                            catch
                            {
                                Thread.Sleep(2000);  //wait 2 seconds 
                                Directory.Delete(dpath1, recursive: true);
                            }

                        }

                    }
                    if ((Directory.Exists(dpath2) && Convert.ToInt32(mx) <= Convert.ToInt32(mes_reenvio)) || ma != "2023") // && Convert.ToInt32(dia_reenvio) !=0 && Convert.ToInt32(dx) == Convert.ToInt32(dia_reenvio))
                    {

                        if (Directory.Exists(dpath2))
                        {
                            try
                            {

                                Directory.Delete(dpath2, recursive: true);    //throws if directory doesn't exist. 
                                Console.WriteLine("Borrando " + dpath2);
                            }
                            catch
                            {
                                Thread.Sleep(2000);  //wait 2 seconds 
                                Directory.Delete(dpath2, recursive: true);
                            }

                        }
                    }
                    if ((Directory.Exists(dpath3) && Convert.ToInt32(mx) <= Convert.ToInt32(mes_reenvio)) || ma != "2023") // && Convert.ToInt32(dia_reenvio) !=0 && Convert.ToInt32(dx) == Convert.ToInt32(dia_reenvio))
                    {


                        if (Directory.Exists(dpath3))
                        {
                            try
                            {

                                Directory.Delete(dpath3, recursive: true);    //throws if directory doesn't exist. 
                                Console.WriteLine("Borrando " + dpath3);
                            }
                            catch
                            {
                                Thread.Sleep(2000);  //wait 2 seconds 
                                Directory.Delete(dpath3, recursive: true);
                            }

                        }


                        if ((Directory.Exists(dpath4) && Convert.ToInt32(mx) <= Convert.ToInt32(mes_reenvio)) || ma != "2023") // && Convert.ToInt32(dia_reenvio) !=0 && Convert.ToInt32(dx) == Convert.ToInt32(dia_reenvio))
                        {


                            if (Directory.Exists(dpath4))
                            {
                                try
                                {

                                    Directory.Delete(dpath4, recursive: true);    //throws if directory doesn't exist. 
                                    Console.WriteLine("Borrando " + dpath4);
                                }
                                catch
                                {
                                    Thread.Sleep(2000);  //wait 2 seconds 
                                    Directory.Delete(dpath4, recursive: true);
                                }

                            }

                        }
                    }
                }
            }
        }
        public static MySqlConnection conexion = new MySqlConnection();
        public static void HasRows()
        {


            try
            {
                Initialize2();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "SELECT rut FROM cliente ORDER BY rut";
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

                conexion.ConnectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";

                conexion.Open();
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
        static void Main(string[] args)
        {
            // leer_log();1
            int consumo = 0;
            int boletas = 0;
            string paths = (@ruta +"temp\\setting_borrado_boletas.txt");
            string rutempresa = "";

            if (File.Exists(paths))
            {
                string[] slines = new string[2];
                slines = File.ReadAllLines(paths);
             
                Console.WriteLine(String.Join(Environment.NewLine, slines));
                string[] ll1 = slines[1].Split(':');

                if (!slines[0].Equals(null))
                {
                    Console.WriteLine("\nEjecutar borrado de boletas por fecha ");
                    if (ll1[1] != null)
                    {

                      
                        fecha = ll1[1];
                    }


                
                 
                }
            }
           
                rut_produccion.Clear();
                HasRows();
                int cuenta_ruts = rut_produccion.Count();
                for (int pasar = 0; pasar < cuenta_ruts; pasar++)
                {
                    rutempresa = rut_produccion[pasar];
                Console.WriteLine("Borrando boletas para rut[" + rutempresa + "] para la fecha menores a: " + fecha);
                enviar_sobrex(rutempresa, fecha);
            }


        }
    }
}




