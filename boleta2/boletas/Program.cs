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
using ItaSystem.DTE.Engine.Helpers;

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
        static public string ruta = "C:\\Users\\administrador\\Documents\\bowa\\respaldo2\\";
        static public string ruta1 = "C:\\Users\\administrador\\Documents\\bowa\\";
        static public string ruta2 = "C:\\Users\\administrador\\Documents\\bowa\\temp\\";


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

                string dpath1_1 = (@ruta1 + rut + "\\1\\1\\" + AllFiles3[i] + "\\xml_empaquetado\\");
                string dpath2_1 = (@ruta1 + rut + "\\2\\2\\" + AllFiles3[i] + "\\xml_empaquetado\\");
                string dpath3_1 = (@ruta1 + rut + "\\3\\3\\" + AllFiles3[i] + "\\xml_empaquetado\\");
                string dpath4_1 = (@ruta1 + rut + "\\4\\4\\" + AllFiles3[i] + "\\xml_empaquetado\\");
                string dpath5_1 = (@ruta1 + rut + "\\5\\5\\" + AllFiles3[i] + "\\xml_empaquetado\\");
                string dpath1_2 = (@ruta2 + rut + "\\1\\1\\" + AllFiles3[i] );
                string dpath2_2 = (@ruta2 + rut + "\\2\\2\\" + AllFiles3[i] );
                string dpath3_2 = (@ruta2 + rut + "\\3\\3\\" + AllFiles3[i] );
                string dpath4_2 = (@ruta2 + rut + "\\4\\4\\" + AllFiles3[i] );
                string dpath5_2 = (@ruta2 + rut + "\\5\\5\\" + AllFiles3[i] );
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


                    if ((Directory.Exists(dpath1) && Convert.ToInt32(mx) >= Convert.ToInt32(mes_reenvio)) || ma != "2023") // && Convert.ToInt32(dia_reenvio) !=0 && Convert.ToInt32(dx) == Convert.ToInt32(dia_reenvio))
                    {

                        if (Directory.Exists(dpath1))
                        {
                            try
                            {

                                Contar_archivos(dpath1,dpath1_1, dpath1_2, rut, AllFiles3[i],"1");
                            }
                            catch
                            {
                             
                            }

                        }

                    }
                    if ((Directory.Exists(dpath2) && Convert.ToInt32(mx) <= Convert.ToInt32(mes_reenvio)) || ma != "2023") // && Convert.ToInt32(dia_reenvio) !=0 && Convert.ToInt32(dx) == Convert.ToInt32(dia_reenvio))
                    {

                        if (Directory.Exists(dpath2))
                        {
                            try
                            {

                                Contar_archivos(dpath2, dpath2_1, dpath2_2,rut, AllFiles3[i], "2");
                         
                            }
                            catch
                            {
                          
                            }

                        }
                    }
                    if ((Directory.Exists(dpath3) && Convert.ToInt32(mx) >= Convert.ToInt32(mes_reenvio)) || ma != "2023") // && Convert.ToInt32(dia_reenvio) !=0 && Convert.ToInt32(dx) == Convert.ToInt32(dia_reenvio))
                    {


                        if (Directory.Exists(dpath3))
                        {
                            try
                            {

                                Contar_archivos(dpath3, dpath3_1, dpath3_2, rut, AllFiles3[i], "3");
                            }
                            catch
                            {
                              
                            }

                        }


                        if ((Directory.Exists(dpath4) && Convert.ToInt32(mx) <= Convert.ToInt32(mes_reenvio)) || ma != "2023") // && Convert.ToInt32(dia_reenvio) !=0 && Convert.ToInt32(dx) == Convert.ToInt32(dia_reenvio))
                        {


                            if (Directory.Exists(dpath4))
                            {
                                try
                                {
                                    Contar_archivos(dpath4, dpath4_1, dpath4_2, rut, AllFiles3[i], "4");
                                }
                                catch
                                {
                                
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
        public static void Contar_archivos(string directorio, string directorio2, string directorio3, string rut, string fecha, string sucursal)
        {
            string[] array2 = Directory.GetFiles(directorio, "*.xml");
            int cantidad = array2.Count();
            string[] array3 = Directory.GetFiles(directorio2, "*.xml");
            int cantidad1 = array3.Count();
            string archivo = "";
            if (rut== "10034580-3")
            {
                int x = 1;
            }
            for (int i = 0; i < cantidad; i++)
            {
                int encontrado = 0;
                for (int y = 0; y < cantidad1; y++)
                {

                    FileInfo f1 = new FileInfo(array2.GetValue(i).ToString());
                    FileInfo f2 = new FileInfo(array3.GetValue(y).ToString());
                    
                    string[] nr = f1.Name.ToString().Split('.');
                    string[] nb = f2.Name.ToString().Split('_');
                    string[] nb1 = nb[3].Split('.');
                    if (nr[0] == nb[3])
                    {
                        archivo = f1.Name.ToString();
                        encontrado = 1;
                        break;
                    }
                    
                }
                if (encontrado == 0)
                {
                    string path1 = directorio3;
                    if (File.Exists(path1))
                    {
                        FileInfo f1 = new FileInfo(array2.GetValue(i).ToString());
                        archivo = f1.Name.ToString();
                        System.IO.File.Copy(directorio + "//" + archivo, directorio3 + "//" + archivo, true);

                    }
                    else

                    {
                        FileInfo f1 = new FileInfo(array2.GetValue(i).ToString());
                        archivo = f1.Name.ToString();
                        Directory.CreateDirectory(path1);
                        Console.WriteLine("\nGenerando Directorio " + path1);
                        System.IO.File.Copy(directorio+"//" + archivo, directorio3 +"//"+ archivo, true);


                    }
                }
            }
            
                string connectionString = @"server=localhost;userid=root1;password=12345678;database=bowa";
                // string connectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into revision_copiar_boletas (rut, fecha, sucursal, respaldo2, bowa, clave) VALUES " +
    "(@rut, @fecha, @sucursal,@respaldo2, @bowa, @clave) ON DUPLICATE KEY UPDATE respaldo2=@respaldo2," + "bowa= @bowa";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@rut", rut);
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.Parameters.AddWithValue("@sucursal", sucursal);
                    cmd.Parameters.AddWithValue("@respaldo2", cantidad);
                    cmd.Parameters.AddWithValue("@bowa", cantidad1);
                    cmd.Parameters.AddWithValue("@clave", rut + "_" + sucursal + "_" + fecha);
                    cmd.ExecuteNonQuery();
                    if (connection != null)
                        connection.Close();
                    //  Thread.Sleep(1000);
                    if (connection != null)
                        connection.Close();


                }
                catch (Exception e) { }
            
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
            string paths = (@ruta1+"temp\\revisar_cuenta_boletas_rut.txt");
            string rutempresa = "";

            if (File.Exists(paths))
            {
                string[] slines = new string[2];
                slines = File.ReadAllLines(paths);
             
                Console.WriteLine(String.Join(Environment.NewLine, slines));
                string[] ll1 = slines[1].Split(':');

                string[] ll = slines[0].Split(':');

                if (ll[1] == "")
                {
                    Console.WriteLine("\nEjecutar revision copia de boletas por fecha ");
                    if (ll1[1] != null)
                    {


                        fecha = ll1[1];
                    }




                    rut_produccion.Clear();
                    HasRows();
                    int cuenta_ruts = rut_produccion.Count();
                    for (int pasar = 0; pasar < cuenta_ruts; pasar++)
                    {
                        rutempresa = rut_produccion[pasar];
                        Console.WriteLine("revisando copia para rut[" + rutempresa + "] para la fecha mayores a: " + fecha);
                        enviar_sobrex(rutempresa, fecha);
                    }
                }
                else
                {
                   
                    rutempresa = ll[1];
                    Console.WriteLine("\nEjecutar revision copia de boletas para "+ rutempresa +" por fecha ");
                    if (ll1[1] != null)
                    {


                        fecha = ll1[1];
                    }
                    Console.WriteLine("revisando copia para rut[" + rutempresa + "] para la fecha mayores a: " + fecha);
                    enviar_sobrex(rutempresa, fecha);
                }
            }


        }
    }
}




