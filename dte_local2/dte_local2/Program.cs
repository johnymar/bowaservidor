using ItaSystem.DTE.Engine.Documento;
using ItaSystem.DTE.Engine.Envio;
using ItaSystem.DTE.Engine.XML;
using dte_local2.clases;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using static ITA_CHILE.Enum.Ambiente;

namespace dte_local2
{
    class Program

    {
        static public string enviox;
        static public string nombreconsumo;
        static List<string> AllFiles = new List<string>();
        static List<string> AllFiles1 = new List<string>();
        static List<string> AllFiles2 = new List<string>();
        static List<string> AllFiles3 = new List<string>();
        static List<string> AllFiles4 = new List<string>();
        static List<string> AllFiles6 = new List<string>();
        static List<string> Allrut = new List<string>();

        static List<string> AllFilesx = new List<string>();
        static int esteequipo = 0;
        static public string nombre_file = "";
        static public string fecha = "";
        static public string pathtoken = "";
        static public string envio;
        static public string rut = "0";
        private static Autorizacion aut;
        static Handler handler = new Handler();
        static public List<string> fecha_vacio = new List<string>();
        static public string fecha_vacio_actual = "";
        static public Configuracion configuracion = new Configuracion();
        static public List<string> rut_produccion = new List<string>();
        static public List<string> rut_ejecutado = new List<string>();
        static public string rutempresa="0";
        static public string fecha_inicial = "";
        public static string ruta = @"C:/Users/Administrador/Documents/";
        static void Main(string[] args)
        {
            try
            {
                Thread t = new Thread(new ParameterizedThreadStart(ThreadLoop));
                t.Start((Action)CallBack);
            }
            catch (Exception ee)
            {
                using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                {

                    sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                }
                //  Console.Write("Press <Enter> to exit... ");
                // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            }
        }
        private static void CallBack()
        {

            verificar_hora();

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
                    using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                    {

                        sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                    }
                    // Console.Write("Press <Enter> to exit... ");
                    // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                }
            }
        }

        public static string dia = "";
        static void verificar_hora()
        {
          
            try
            {
                string path = (@ruta+"bowa\\temp\\ultimo_rcof.txt");

                if (File.Exists(path))
                {
                    string[] lines = File.ReadAllLines(path);
                    Console.WriteLine("Ultimo rcof ejecutado Dia: " + String.Join(Environment.NewLine, lines));
                    if (!lines[0].Equals(null))
                    {
                        dia = lines[0];
                    }
                }

                string[] fecha1 = DateTime.Today.ToString().Split(' ');
                string fecha = fecha1[0].Replace("/", "-");

                DateTime scheduledRun = DateTime.Today.AddHours(Convert.ToInt32(1)); // runs today at 3am. 

                if (DateTime.Now > scheduledRun)//&& fecha != dia)
                {

                    fecha1 = DateTime.Today.ToString().Split(' ');
                    string dia = fecha1[0].Replace("/", "-");

                    using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\ultimo_rcof.txt"))
                    {
                        sw.WriteLine(dia);
                    }

                    doStuff();
                }
            }
            catch (Exception ee)
            {
                using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                {

                    sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                }
                //   Console.Write("Press <Enter> to exit... ");
                //    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            }
        }

        static void doStuff()
        {
            try
            {
                principal();
            }
            catch (Exception ee)
            {
                using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                {

                    sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                }
                // Console.Write("Press <Enter> to exit... ");
                // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            }
        }
        static void principal()
        {
            try
            {
                rut_ejecutado.Clear();
                rut_produccion.Clear();
                Handler.HasRows();
                int cuenta_ruts = rut_produccion.Count();

                string[] lines = new string[2];

                /*string path = (@ruta+"bowa\temp\aviso2.txt");
                if (File.Exists(path))
                {
                    // string path = (@"C:\Users\admin\Documents\bowa\temp\aviso.txt");
                    lines = File.ReadAllLines(path);
                    Console.WriteLine(String.Join(Environment.NewLine, lines));
                    if (!lines[0].Equals(null))
                    {
                        rutempresa = lines[0];
                        //                fecha = lines[1];
                    }
                }*/
                string rutempresa = "0";
                string rutempresa_ejecutar = "0";
                string path = (@ruta + "bowa\\temp\\rut_rcof_ejecutar.txt");
                if (File.Exists(path))
                {
                    // string path = (@"C:\Users\admin\Documents\bowa\temp\aviso.txt");
                    lines = File.ReadAllLines(path);
                //    Console.WriteLine(String.Join(Environment.NewLine, lines));
                    if (!lines[0].Equals(null))
                    {
                        rutempresa_ejecutar = lines[0];
                        rutempresa = rutempresa_ejecutar;
                        //                fecha = lines[1];
                        Console.WriteLine("\nEjecutando solo para RUT:[ " + rutempresa_ejecutar + " ]");
                    }

                }
                Handler.leer_rut_estatus();

                for (int pasar = 0; pasar < cuenta_ruts; pasar++)
                {

                    AllFiles.Clear();
                    AllFiles1.Clear();
                    AllFiles2.Clear();
                    AllFiles3.Clear();
                    fecha_vacio.Clear();
                    rut = "0";

                    rutempresa = rut_produccion[pasar];
                    char c = rutempresa[pasar];


                    if (c != '7' || c == '7')
                    {
                        //   Console.WriteLine("\nInicio de Firma de Boletas para RUT:[ " + rutempresa + " ]");
                        rutempresa = rut_produccion[pasar];
                        int rut_existe = 0;
                        /*
                            for (int u = 0; u < rut_produccion.Count(); u++)
                            {
                                if (rutempresa == rut_produccion[u])
                                {
                                    rut_existe = 1;
                                }
                            }
                        */
                        if (File.Exists(path))
                        {
                            // string path = (@"C:\Users\admin\Documents\bowa\temp\aviso.txt");
                            lines = File.ReadAllLines(path);
                          //  Console.WriteLine(String.Join(Environment.NewLine, lines));
                            if (!lines[0].Equals(null))
                            {
                                rutempresa_ejecutar = lines[0];
                                rutempresa = rutempresa_ejecutar;
                                //                fecha = lines[1];
                            //    Console.WriteLine("\nEjecutando solo para RUT:[ " + rutempresa_ejecutar + " ]");
                            }

                        }

                        if (((rut_existe == 0) && rutempresa_ejecutar == "0") || (rut_existe == 0 && rutempresa == rutempresa_ejecutar && rutempresa_ejecutar != "0"))
                        {
                            string rut_aviso = rutempresa;
                            using (StreamWriter sw = File.CreateText(@ruta + "bowa\\temp\\aviso.txt"))
                            {



                                //   sw.WriteLine(rut_aviso);


                            }
                            using (StreamWriter sw = File.CreateText(@ruta + "bowa\\temp\\aviso1.txt"))
                            {



                                sw.WriteLine(rut_aviso);


                            }
                            using (StreamWriter sw = File.CreateText(@ruta + "bowa\\temp\\aviso2.txt"))
                            {



                                sw.WriteLine(rut_aviso);


                            }
                            using (StreamWriter sw = File.CreateText(@ruta + "bowa\\temp\\aviso3.txt"))
                            {



                                sw.WriteLine(rut_aviso);



                            }
                            // Process.Start(@ruta+"dte\dte_local1\dte_local1\bin\Debug\dte_local1.exe");
                            /*Process[] instancia = Process.GetProcessesByName("dte_local1");
                             if (instancia.Length == 1)
                             {
                                 Console.Write("Esperando cierre proceso dte_local1.exe");
                                 while (instancia.Length == 1)
                                 {
                                     instancia = Process.GetProcessesByName("dte_local1");
                                 }
                                 Console.Write("Proceso dte_local1.exe finalizado");
                             }
                             else
                             {
                             }*/


                            Handler.grabar_estatus(rutempresa, "0");
                            //   grabar_tracking(0, @"C:\\Users\\one\\Documents\\bowa\\15835677-5\\2021-1-5\\rcof\\ConsumoFolios_15835677-5_05012021.xml");
                           // Console.Write("rut empresa actual: [" + pasar + ":" + cuenta_ruts + "]" + rutempresa);
                            Thread.Sleep(1500);

                            // leer_estado_rcof(5196993742, "");
                            path = ruta + "\\bowa\\";
                            /*try
                            {
                                String[] filePaths = Directory.GetFiles(@ruta+"\\bowa\\");
                                string[] folderPaths = Directory.GetDirectories(path);
                                foreach (string s in folderPaths)
                                {
                                    string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                                    AllFiles.Add(t);

                                }
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine("No se encontraron nuevos xml a enviar");
                            }*/
                            int incluir = 0;
                            string t1 = rutempresa;
                            AllFiles.Add(t1);
                            int cantidad = AllFiles.Count();
                            for (int i = 0; i < cantidad; i++)
                            {
                                if (AllFiles[i] != "temp" && AllFiles[i] != "respaldo")
                                {
                                    path = ruta + "bowa\\" + AllFiles[i] + "\\";
                                    try
                                    {
                                        string[] folderPaths = Directory.GetDirectories(path);
                                        foreach (string s in folderPaths)
                                        {
                                            string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                                            if (t != "rcof" && t != "pdf")
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
                                        using (StreamWriter sw = File.CreateText(@ruta + "bowa\\temp\\listo1.txt"))
                                        {

                                            sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                                        }
                                        //                  Console.WriteLine("No se encontraron nuevos xml a enviar");
                                    }
                                }
                            }

                            cantidad = AllFiles2.Count();
                            if (cantidad > 1)
                            {
                                Console.Write("buscando sucursales");
                                using (StreamWriter sw = File.CreateText(@ruta + "bowa\\temp\\rut_rcof_sucu.txt"))
                                {

                                    sw.WriteLine(rutempresa);

                                }
                                Process.Start(@ruta + "\\dte\\dte_local_sucu\\dte_local_sucu\\bin\\Debug\\dte_local_sucu.exe");
                                Process[] instancia1 = Process.GetProcessesByName("dte_local_sucu");
                                if (instancia1.Length == 1)
                                {
                                    Console.Write("Esperando cierre proceso dte_local_sucu.exe");
                                    while (instancia1.Length == 1)
                                    {
                                        instancia1 = Process.GetProcessesByName("dte_local_sucu");
                                    }
                                    Console.Write("Proceso dte_local_sucu.exe finalizado");
                                }
                                else
                                {
                                }
                            }
                            for (int i = 0; i < cantidad; i++)
                            {
                                string ver = AllFiles2[i];
                                int x = 0;
                            }

                            cantidad = AllFiles.Count();
                            for (int i = 0; i < cantidad; i++)
                            {

                                if (AllFiles[i] != "temp" && AllFiles[i] != "respaldo")
                                {
                                    int cantidad2 = AllFiles2.Count();
                                    for (int i2 = 0; i2 < cantidad2; i2++)
                                    {
                                        if (AllFiles2[i] != "caf" && AllFiles2[i] != "rcof" && AllFiles2[i2] != "pdf")
                                        {
                                            path = @ruta + "\\bowa\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\";
                                            try
                                            {
                                                string[] folderPaths = Directory.GetDirectories(path);
                                                foreach (string s in folderPaths)
                                                {
                                                    string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                                                    if (AllFiles3.Count > 0)
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
                                                using (StreamWriter sw = File.CreateText(@ruta + "bowa\\temp\\listo1.txt"))
                                                {

                                                    sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                                                }
                                                //                        Console.WriteLine("No se encontraron nuevos xml a enviar");
                                            }
                                        }
                                    }
                                }
                            }
                            AllFiles1.Clear();
                            cantidad = AllFiles3.Count();
                            for (int i = 0; i < cantidad; i++)
                            {
                                string ver = AllFiles3[i];

                                int x = 0;
                            }
                            cantidad = AllFiles.Count();
                            for (int i = 0; i < cantidad; i++)
                            {
                                if (AllFiles[i] != "temp" && AllFiles[i] != "respaldo")
                                {

                                    int cantidad2 = AllFiles2.Count();
                                    for (int i2 = 0; i2 < cantidad2; i2++)
                                    {
                                        if (AllFiles2[i2] != "caf" && AllFiles2[i2] != "rcof" && AllFiles2[i2] != "f")
                                        {
                                            int cantidad3 = AllFiles3.Count();
                                            for (int i3 = 0; i3 < cantidad3; i3++)
                                            {

                                                string hhh1 = AllFiles[i];
                                                string hhh2 = AllFiles2[i2];
                                                string hhh = AllFiles3[i3];

                                                path = @ruta + "\\bowa\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\";
                                                try
                                                {
                                                    if (Directory.Exists(path))
                                                    {

                                                        string[] folderPaths = Directory.GetDirectories(path);
                                                        foreach (string s in folderPaths)
                                                        {
                                                            string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                                                            if (AllFiles1.Count > 0)
                                                            {
                                                                for (int tr = 0; tr <= AllFiles1.Count - 1; tr++)
                                                                {
                                                                    if (incluir == 0)
                                                                    {
                                                                        if (AllFiles1[tr] != t)
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
                                                                    AllFiles1.Add(t);
                                                                }
                                                                else
                                                                {
                                                                    incluir = 0;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                AllFiles1.Add(t);
                                                            }

                                                        }
                                                    }
                                                }
                                                catch (Exception ee)
                                                {
                                                    using (StreamWriter sw = File.CreateText(@ruta + "bowa\\temp\\listo1.txt"))
                                                    {

                                                        sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                                                    }
                                                    //    Console.WriteLine("No se encontraron nuevos xml a enviar");
                                                }
                                            }
                                        }

                                    }
                                }
                            }

                            cantidad = AllFiles1.Count();
                            for (int i = 0; i < cantidad; i++)
                            {
                                string ver = AllFiles1[i];
                                int x = 0;
                            }

                            string dd = rutempresa;
                            fechas(rutempresa);

                          enviar_sobre();
                         //  enviar_sobre_0(rutempresa);
                           Console.WriteLine("rut: " + rutempresa);
                       //  while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                         //   enviar_sobre1();
                            //   enviar_sobre();
                            //    Console.Write("Press <Enter> to exit... ");
                            //   while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                            Handler.grabar_estatus(rutempresa, "1");


                        //    Console.Clear();
                        }
                    }
                }

                string connectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    string dia = DateTime.Now.Day.ToString();
                    int dian = Convert.ToInt16(dia);
                    if (dian < 10) { dia = "0" + dia; }
                    string mes = DateTime.Now.Month.ToString();
                    int mesn = Convert.ToInt16(mes);
                    if (mesn < 10) { mes = "0" + mes; }
                    string ano = DateTime.Now.Year.ToString();
                    cmd.CommandText = "Delete from envioconsumo WHERE fecha_consumo>@fech";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@fech", ano + "-" + mes + "-" + dia);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "Delete from envioconsumo WHERE fecha_consumo<'2021-01-01'";
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    Console.Write("Borrando tabla rut_ejecutado");

                //    cmd.CommandText = "TRUNCATE TABLE rut_ejecutado";
                 //   cmd.Prepare();
                 //   cmd.ExecuteNonQuery();
                 //   Console.Write("\nTabla rut_ejecutado borrada");

                    if (connection != null)
                        connection.Close();
                    Thread.Sleep(1000);


                    connectionString = @"server=sh-pro12.hostgator.cl;userid=posfacto;password=eD1[P4q4kZw+0M;database=posfacto_bowa";
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Delete from envioconsumo WHERE fecha_consumo>@fech";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@fech", ano + "-" + mes + "-" + dia);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "Delete from envioconsumo WHERE fecha_consumo<'2021-01-01'";
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    if (connection != null)
                        connection.Close();

                }
                catch (Exception ee)
                {
                    Console.Write("Press <Enter> to exit... " + ee);
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
                
                 /*  Process.Start(@ruta + "dte/Consulta_dte/Consulta_dte/Consulta_dte/bin/Debug/Co.exe");
                    Process[] instancia = Process.GetProcessesByName("dte_local_sucu");
                    if (instancia.Length == 1)
                    {
                        Console.Write("Esperando cierre proceso dte_local_sucu.exe");
                        while (instancia.Length == 1)
                        {
                            instancia = Process.GetProcessesByName("dte_local_sucu");
                        }
                        Console.Write("Proceso dte_local_sucu.exe finalizado");
                    }
                */
         

            }

            catch (Exception ee)
            {
                using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                {

                    sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                }
                //                Console.Write("Press <Enter> to exit... ");
                //              while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            }
        }

        public static string rut_error = "";



        
        public static string fecha_rcof = "";

        public static void fechas(string rut)
        {
            int entro = 0;
            List<string> fechas_llenas = new List<string>();
            fechas_llenas.Clear();
            string fecha_llena_sucursal_max = "";
            string fecha_llena_sucursal_min = "";
          //  Console.Write("\n Iniciando verificacion de fechas de transmision");
            int cantidad = AllFiles1.Count();
            for (int i1 = 0; i1 < cantidad; i1++)
            {

                int cantidad2 = AllFiles.Count();
                for (int i = 0; i < cantidad2; i++)
                {
                    if (AllFiles[i] != "temp")
                    {

                        int cantidad3 = AllFiles2.Count();
                        for (int i2 = 0; i2 < cantidad3; i2++)
                        {
                            if (AllFiles2[i2] != "caf" && AllFiles2[i2] != "rcof")
                            {

                                int cantidad1 = AllFiles3.Count();

                                for (int i3 = 0; i3 < cantidad1; i3++)
                                {

                                    DirectoryInfo di = new DirectoryInfo(@ruta + "bowa\\" + AllFiles[i] + "\\"
                                   + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_empaquetado\\");

                                    if (Directory.Exists(@ruta + "bowa\\" + AllFiles[i] + "\\"
                    + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_empaquetado\\"))
                                    {
                                        // Console.Write("\n Dierctorio localizado, Buscando xml");
                                        string[] pathFilesss = System.IO.Directory.GetFiles(@ruta + "bowa\\" + AllFiles[i] + "\\"
                        + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_empaquetado\\", "*.xml");
                                        int su = 0;
                                        int eq = 0;
                                        rut = AllFiles[i];
                                        if (pathFilesss.Length != 0)
                                        {
                                            /*if (entro == 1 && su != Convert.ToInt16(AllFiles3[i3]) && eq != Convert.ToInt16(AllFiles2[i2]))
                                            {
                                                entro = 0;
                                                fecha_llena_sucursal_min = (AllFiles1[i1] + "_" + AllFiles2[i2] + "_" + AllFiles3[i3] + "_" + "min");
                                            }
                                            if (entro == 0)
                                            {
                                                su = Convert.ToInt16(AllFiles3[i3]); eq = Convert.ToInt16(AllFiles2[i2]);
                                                entro = 1;
                                            }*/
                                            fecha_llena_sucursal_max = AllFiles1[i1] + "_" + AllFiles2[i2] + "_" + AllFiles3[i3] + "_" + "max";
                                        }
                                        else
                                        {
                                        }
                                        fechas_llenas.Add(fecha_llena_sucursal_max);
                                    }
                                    //fechas_llenas.Add(fecha_llena_sucursal_min);
                                }

                            }
                        }
                    }
                }
            }
            string fecha_max_1 = "";
            string fecha_min_1 = "";
            string fecha_max_2 = "";
            string fecha_min_2 = "";
            string fecha_max_3 = "";
            string fecha_min_3 = "";
            string fecha_max_4 = "";
            string fecha_min_4 = "";
            string fecha_max_5 = "";
            string fecha_min_5 = "";
            List<string> fechas_llenas_1 = new List<string>();
            fechas_llenas_1.Clear();
            List<string> fechas_llenas_2 = new List<string>();
            fechas_llenas_2.Clear();
            List<string> fechas_llenas_3 = new List<string>();
            fechas_llenas_3.Clear();
            List<string> fechas_llenas_4 = new List<string>();
            fechas_llenas_4.Clear();
            List<string> fechas_llenas_5 = new List<string>();
            fechas_llenas_5.Clear();


            for (int r = 0; r < fechas_llenas.Count(); r++)
            {
                string jgjgj = fechas_llenas[r];
                if (fechas_llenas[r] != "")
                {
                    string[] gtr = fechas_llenas[r].Split('_');

                    if (gtr[1] == "1")
                    {
                        string[] ff = gtr[0].Split('-');
                        if (ff[1].Length == 1)
                        {
                            ff[1] = "0" + ff[1];
                        }
                        if (ff[2].Length == 1)
                        {
                            ff[2] = "0" + ff[2];
                        }

                        fechas_llenas_1.Add(ff[0] + "-" + ff[1] + "-" + ff[2]);
                    }

                    if (gtr[1] == "2")
                    {
                        string[] ff = gtr[0].Split('-');
                        if (ff[1].Length == 1)
                        {
                            ff[1] = "0" + ff[1];
                        }
                        if (ff[2].Length == 1)
                        {
                            ff[2] = "0" + ff[2];
                        }

                        fechas_llenas_2.Add(ff[0] + "-" + ff[1] + "-" + ff[2]);
                    }

                    if (gtr[1] == "3")
                    {
                        string[] ff = gtr[0].Split('-');
                        if (ff[1].Length == 1)
                        {
                            ff[1] = "0" + ff[1];
                        }
                        if (ff[2].Length == 1)
                        {
                            ff[2] = "0" + ff[2];
                        }

                        fechas_llenas_3.Add(ff[0] + "-" + ff[1] + "-" + ff[2]);
                    }

                    if (gtr[1] == "4")
                    {
                        string[] ff = gtr[0].Split('-');
                        if (ff[1].Length == 1)
                        {
                            ff[1] = "0" + ff[1];
                        }
                        if (ff[2].Length == 1)
                        {
                            ff[2] = "0" + ff[2];
                        }

                        fechas_llenas_4.Add(ff[0] + "-" + ff[1] + "-" + ff[2]);
                    }

                    if (gtr[1] == "5")
                    {
                        string[] ff = gtr[0].Split('-');
                        if (ff[1].Length == 1)
                        {
                            ff[1] = "0" + ff[1];
                        }
                        if (ff[2].Length == 1)
                        {
                            ff[2] = "0" + ff[2];
                        }

                        fechas_llenas_5.Add(ff[0] + "-" + ff[1] + "-" + ff[2]);
                    }
                }
            }

            string menor1 = "2023/01/01";
            string mayor1 = "2023/01/01";

            string menor2 = "2023/01/01";
            string mayor2 = "2023/01/01";

            string menor3 = "2023/01/01";
            string mayor3 = "2023/01/01";

            string menor4 = "2023/01/01";
            string mayor4 = "2023/01/01";


            string menor5 = "2023/01/01";
            string mayor5 = "2023/01/01";

            mayor1 = fechas_llenas_1.Max();
            menor1 = fechas_llenas_1.Min();
            mayor2 = fechas_llenas_2.Max();
            menor2 = fechas_llenas_2.Min();
            mayor3 = fechas_llenas_3.Max();
            menor3 = fechas_llenas_3.Min();
            mayor4 = fechas_llenas_4.Max();
            menor4 = fechas_llenas_4.Min();
            mayor5 = fechas_llenas_5.Max();
            menor5 = fechas_llenas_5.Min();
            if (menor1 != "2023/01/01" && mayor1 != "2023/01/01" && menor1 != null && mayor1 != null)
            {
                grabar_fechas(menor1, mayor1, "1", "1", rut);

                if (menor2 != "2023/01/01" && mayor2 != "2023/01/01" && menor2 != null && mayor2 != null)
                {
                    grabar_fechas(menor2, mayor2, "2", "2", rut);
                }
                if (menor3 != "2023/01/01" && mayor3 != "2023/01/01" && menor3 != null && mayor3 != null)
                {
                    grabar_fechas(menor3, mayor3, "3", "3", rut);
                }
                if (menor4 != "2023/01/01" && mayor4 != "2023/01/01" && menor4 != null && mayor4 != null)
                {
                    grabar_fechas(menor4, mayor4, "4", "4", rut);
                }
                if (menor5 != "2023/01/01" && mayor5 != "2023/01/01" && menor5 != null && mayor5 != null)
                {
                    grabar_fechas(menor5, mayor5, "5", "5", rut);
                }
            }
            else
            {
                //   Console.Write("mes actual " + rut);
                error1(rut);
                // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

            }

        }
        public static void error1(string error)
        {
            string dia = DateTime.Now.ToString("dd/MM/yyyy");
            string hora = DateTime.Now.ToString("hh:mm:ss");
            string docPath = @ruta + "\\bowa\\temp\\";
            TextWriter tw = new StreamWriter(docPath + "log_error.txt", true);
            tw.WriteLine(error); tw.Close();
            tw.Close();
        }
        public static bool dia_actual = false;
        public static void enviar_sobre_0(string rut)
        {
            rutempresa = rut;
            int rut_ngr=Handler.buscar_rut_ngr(rut);

            if (rut_ngr == 1)
            {
                Console.Write("RUT NGR ENCONTRADO... ");
           //     while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            }
            if (rut_ngr == 0)
            {
                int aqui = 0;
                int generar_rcof = 0;
                string filePathArchivo = "";
                string[] linesx = new string[2];
                List<ItaSystem.DTE.Engine.Documento.DTE> dtes = new List<ItaSystem.DTE.Engine.Documento.DTE>();
                List<string> xmlDtes = new List<string>();
                string fff = "";
                int entro = 1;
                int vaciox = 1;
                int generar_vacio = 1; int y = 0;
                string xmlString = "";
                fecha = "";
                fecha_rcof = "";

                try
                {

                    //si no existe la carpeta temporal la creamos

                }
                catch (Exception ee)
                {
                    using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                    {

                        sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                    }
                }

                Console.Write("\n Iniciando busqueda de xml");
                int encontro_xml_enviar = 0;
                int cantidad = AllFiles1.Count();
                Console.Write("llamado ");




                int cantidad2 = AllFiles.Count();
                //Console.Write("aqui1 ");
               // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                for (int i = 0; i < cantidad2; i++)
                {



                    if (AllFiles[i] != "temp")
                    {

                        int cantidad3 = AllFiles2.Count();
                        for (int i2 = 0; i2 < cantidad3; i2++)
                        {
                            Console.Write("Cantidad3 " + i2);
                          //  while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                            if (AllFiles2[i2] != "caf" && AllFiles2[i2] != "rcof")
                            {

                                int cantidad1 = AllFiles3.Count();

                                aqui = 0;
                                if (AllFiles1.Count() > 0)

                                {
                                    try
                                    {

                                        string dpaths = (@ruta+"\\bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\");

                                        try
                                        {
                                            //si no existe la carpeta temporal la creamos
                                            if (!(Directory.Exists(dpaths)))
                                            {
                                                Directory.CreateDirectory(dpaths);

                                            }


                                        }
                                        catch (Exception ee)
                                        {
                                            using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                                            {

                                                sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                                            }
                                        }

                                        try
                                        {
                                            string[] lines = new string[100000];

                                            string[] hh = null;

                                            Handler.leer_fecha(rutempresa);
                                            Console.Write("\nLeyendo " + fecha_inicial);
                                            //while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                                            string fh = fecha_inicial;
                                            if (fh == "" || fh == null)
                                            {
                                                fh = DateTime.Today.ToString();
                                            }
                                            if (esteequipo == 1)
                                            {
                                                hh = (fh.Split('/'));
                                            }
                                            else
                                            {
                                                hh = (fh.Split('-'));
                                            }
                                            int[] ggg1 = new int[3];
                                            ggg1[0] = Convert.ToInt32(hh[2]);
                                            ggg1[1] = Convert.ToInt32(hh[1]);
                                            ggg1[2] = Convert.ToInt32(hh[0]);

                                            if (ggg1[0].ToString().Length < 4)
                                            {
                                                ggg1[0] = 2000 + ggg1[0];
                                            }

                                            fecha = ggg1[0] + "-" + ggg1[1] + "-" + ggg1[2];


                                            int ee = DateTime.Now.Month-1;
                                            if (ee == 0)
                                            {
                                                ee = 12;
                                            }
                                            Console.Write("mes actual " + ee);
                                            //while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                                            
                                            while (ee <DateTime.Now.Month)
                                            {
                                                int dias = DateTime.DaysInMonth(2021, ee);
                                                int eee = 1;
                                                for (eee = 1; eee <= dias; eee++)
                                                {

                                                    int verificado = 0;


                                                    if (Convert.ToString(ee).Length < 2 && Convert.ToString(eee).Length < 2)
                                                    {
                                                        fecha = "2023" + "-0" + ee + "-0" + eee;

                                                    }
                                                    if (Convert.ToString(ee).Length < 2 && Convert.ToString(eee).Length == 2)
                                                    {
                                                        fecha = "2023" + "-0" + ee + "-" + eee;

                                                    }
                                                    if (Convert.ToString(ee).Length == 2 && Convert.ToString(eee).Length < 2)
                                                    {
                                                        fecha = "2023" + "-" + ee + "-0" + eee;

                                                    }
                                                    if (Convert.ToString(ee).Length == 2 && Convert.ToString(eee).Length == 2)
                                                    {
                                                        fecha = "2023" + "-" + ee + "-" + eee;
                                                    }

                                                    Console.WriteLine("\n Leyendo:" + @ruta+"bowa\\" + rutempresa + "\\"
                                         + AllFiles2[i2] + "\\" + AllFiles2[i2] + "\\" + fecha + "\\xml_empaquetado\\");

                                                    // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                                                    if ((Directory.Exists(@ruta+"bowa\\" + rutempresa + "\\"
                                         + AllFiles2[i2] + "\\" + AllFiles2[i2] + "\\" + fecha + "\\xml_empaquetado\\")))
                                                    {


                                                        string[] pathFilesss = System.IO.Directory.GetFiles(@ruta+"bowa\\" + rutempresa + "\\"
                                             + AllFiles2[i2] + "\\" + AllFiles2[i2] + "\\" + fecha + "\\xml_empaquetado\\", "*.xml");
                                                        if (pathFilesss.Length != 0)
                                                        {
                                                            verificado = 1;
                                                        }
                                                        //Console.Write("\n aqui1");
                                                        //while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                                                        pathFilesss = System.IO.Directory.GetFiles(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\", "*.xml");
                                                        if (pathFilesss.Length != 0)
                                                        {
                                                            Console.WriteLine("\n Chequeada:" + fecha + " Llena");
                                                            //while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                                                            verificado = 1;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        if (verificado == 0)
                                                        {
                                                            if (!(Directory.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\")))
                                                            {
                                                                Directory.CreateDirectory(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\");


                                                            }
                                                            else
                                                            {

                                                                Console.Write("/nrevisando \\rcof\\" + fecha);
                                                                string[] pathFilesss = System.IO.Directory.GetFiles(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\", "*.xml");
                                                                if (pathFilesss.Length != 0)
                                                                {
                                                                    verificado = 1;
                                                                    Console.Write("/n ya existe \rcof\\" + fecha);
                                                                }
                                                            }

                                                        }

                                                       // Console.Write("leyyy" + verificado);
                                                        Console.Write(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\envio.txt");
                                                        // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                                                        if (verificado == 0 && File.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\envio.txt"))
                                                        {

                                                            string line = "";
                                                            System.IO.StreamReader file =
                                                               new System.IO.StreamReader(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\envio.txt");

                                                            while ((line = file.ReadLine()) != null)
                                                            {

                                                                envio = line;
                                                            }
                                                            int enviox = 0;
                                                            file.Close();
                                                            enviox = Convert.ToInt32(envio) + 50;
                                                            envio = "" + enviox;
                                                            System.IO.StreamWriter ficheroTemporal =
                                                        new System.IO.StreamWriter(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\envio.txt");
                                                            ficheroTemporal.WriteLine(envio);
                                                            ficheroTemporal.Close();
                                                            file.Close();


                                                        }
                                                        else
                                                        {
                                                            if (verificado == 0)
                                                            {
                                                                System.IO.StreamWriter ficheroTemporal =
                                                            new System.IO.StreamWriter(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\envio.txt");
                                                                ficheroTemporal.WriteLine("1");
                                                                ficheroTemporal.Close();
                                                                envio = "1";
                                                            }
                                                        }
                                                    }
                                                    if (verificado == 0 && (Directory.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\")))
                                                    {

                                                        //  Console.WriteLine(fecha);
                                                        fecha_rcof = fecha;
                                                        var rcof = handler.GenerarRCOF_vacio(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\");
                                                        rcof.DocumentoConsumoFolios.Id = "RCOF_" + fecha_rcof.Replace("-", "");
                                                        xmlString = "";
                                                        Handler.Initialize2();
                                                        Handler.leer(rutempresa);

                                                        filePathArchivo = rcof.Firmar(Handler.represen_n, out xmlString, @ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\", "");
                                                        Console.WriteLine("RCOF de fecha:  " + fecha_rcof + " rut:" + rutempresa + " Generado " + filePathArchivo);
                                                        string docPath = @ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\";
                                                        TextWriter tw = new StreamWriter(docPath + "log_" + fecha + ".txt", true);
                                                        tw.WriteLine("RCOF de fecha:  " + fecha_rcof + " rut:" + rutempresa + " Generado " + filePathArchivo);
                                                        tw.Close();
                                                        fecha_rcof = "";

                                                        long trackId = handler.EnviarEnvioDTEToSII(filePathArchivo, AmbienteEnum.Produccion, @ruta+"bowa\\" + rutempresa + "\\rcof\\" + filePathArchivo + "\\");
                                                        error("RCOF de fecha:  " + filePathArchivo + " rut:" + rutempresa + " Enviado al SII con tracking: " + trackId);
                                                        Console.WriteLine("RCOF de fecha:  " + filePathArchivo + " rut:" + rutempresa + " Enviado al SII con tracking: " + trackId + " \n nombre del archivo: " + filePathArchivo);
                                                        //   grabar_tracking(trackId, filePathArchivo);

                                                    }
                                                }

                                                Console.Write("pausa");
                                                //while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                                                ee = ee + 1;
                                            }



                                        }
                                        catch (Exception ee)
                                        {
                                            Console.Write("error" + ee);
                                            //        while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                                        }

                                    }
                                    catch (Exception ee)
                                    {
                                        Console.Write("error" + ee);
                                        //   while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                                    }
                                }
                            }
                        }
                    }
                }

            }  
        }


        public static void enviar_sobre()
        {
            int aqui = 0;
            int generar_rcof = 0;
            string filePathArchivo = "";
            string[] linesx = new string[2];
            List<ItaSystem.DTE.Engine.Documento.DTE> dtes = new List<ItaSystem.DTE.Engine.Documento.DTE>();
            List<string> xmlDtes = new List<string>();
            string fff = "";
            int entro = 1;
            int vaciox = 1;
            int generar_vacio = 1; int y = 0;
            string xmlString = "";
            fecha = "";
            fecha_rcof = "";
            try
            {
                try
                {

                    //si no existe la carpeta temporal la creamos

                }
                catch (Exception ee)
                {
                    using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                    {

                        sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                    }
                }

                Console.Write("\n Iniciando busqueda de xml");
                int encontro_xml_enviar = 0;
                int cantidad = AllFiles1.Count();
                for (int i1 = 0; i1 < cantidad; i1++)
                {
                    Console.Write("\n Fecha: " + fecha);

                    if (generar_rcof == 1)
                    {


                        Console.Write("\n revisando si existe envio para la fecha del recof");
                        if (File.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\envio.txt"))
                        {
                            string line = "";
                            System.IO.StreamReader file =
                               new System.IO.StreamReader(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\envio.txt");

                            while ((line = file.ReadLine()) != null)
                            {

                                envio = line;
                            }
                            int enviox = 2;
                            file.Close();
                            enviox = Convert.ToInt32(envio) + 50;
                            envio = "" + enviox;
                            System.IO.StreamWriter ficheroTemporal =
                        new System.IO.StreamWriter(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\envio.txt");
                            ficheroTemporal.WriteLine(envio);
                            ficheroTemporal.Close();
                            file.Close();
                            Console.Write("\n Generando envio numero: " + envio);


                        }
                        else
                        {
                            System.IO.StreamWriter ficheroTemporal =
                        new System.IO.StreamWriter(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\envio.txt");
                            ficheroTemporal.WriteLine("1");
                            ficheroTemporal.Close();
                            envio = "1";
                            Console.Write("\n Generando envio numero: " + envio);


                        }

                        var rcof = handler.GenerarRCOF(dtes, "\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\");
                        rcof.DocumentoConsumoFolios.Id = "RCOF_" + fecha.Replace("-", "") + "_" + envio;

                        Handler.Initialize2();
                        Handler.leer(rutempresa);


                        filePathArchivo = rcof.Firmar(Handler.represen_n, out xmlString, @ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\", "");
                        Console.WriteLine("RCOF de fecha:  " + fecha_rcof + " rut:" + rutempresa + " Generado " + filePathArchivo);
                        string docPath = @ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\";
                        TextWriter tw = new StreamWriter(docPath + "log_" + fecha_rcof + ".txt", true);
                        tw.WriteLine("RCOF de fecha:  " + fecha_rcof + " rut:" + rutempresa + " Generado " + filePathArchivo);
                        tw.Close();
                        fecha_rcof = "";
                        generar_rcof = 0; dtes.Clear();
                        xmlDtes.Clear();


                    }



                    int cantidad2 = AllFiles.Count();
                    for (int i = 0; i < cantidad2; i++)
                    {
                        if (AllFiles[i] != "temp")
                        {

                            int cantidad3 = AllFiles2.Count();
                            for (int i2 = 0; i2 < cantidad3; i2++)
                            {
                                if (AllFiles2[i2] != "caf" && AllFiles2[i2] != "rcof")
                                {

                                    int cantidad1 = AllFiles3.Count();

                                    aqui = 0;
                                    for (int i3 = 0; i3 < cantidad1; i3++)
                                    {
                                        fecha = AllFiles1[i1];
                                        entro = 1;
                                        rutempresa = AllFiles[i];



                                        string tt = @ruta+"\\bowa\\" + AllFiles[i] + "\\" +
                                          AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\";
                                        if (Directory.Exists(@ruta+"\\bowa\\" + AllFiles[i] + "\\" +
                                         AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\"))
                                        {

                                            rutempresa = AllFiles[i];
                                            string[] folderPaths = Directory.GetDirectories(@ruta+"\\bowa\\" + AllFiles[i] + "\\" +
                                                AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\");

                                            if (AllFiles1.Count() > 0)

                                            {
                                                try
                                                {

                                                    string dpaths = (@ruta+"\\bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\");

                                                    try
                                                    {
                                                        //si no existe la carpeta temporal la creamos
                                                        if (!(Directory.Exists(dpaths)))
                                                        {
                                                            Directory.CreateDirectory(dpaths);

                                                        }


                                                    }
                                                    catch (Exception ee)
                                                    {
                                                        using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                                                        {

                                                            sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                                                        }
                                                    }

                                                    string f = AllFiles1[i1];
                                                    string[] lines = new string[100000];
                                                    Console.Write("\n Buscando en: " + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_empaquetado\\");

                                                    DirectoryInfo di = new DirectoryInfo(@ruta+"bowa\\" + AllFiles[i] + "\\"
                                    + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_empaquetado\\");

                                                    DateTime fecha_busqueda = DateTime.Parse("01-05-2023");
                                                  DateTime fecha_busqueda1 = (DateTime.Parse(AllFiles1[i1]));

                                                    if (Directory.Exists(dpaths) && fecha_busqueda<= fecha_busqueda1)
                                                    {
                                                        Console.Write("\n Dierctorio localizado, Buscando xml");
                                                        string[] pathFilesss = System.IO.Directory.GetFiles(dpaths, "*.xml");
                                                        if (pathFilesss.Length != 0)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            if (AllFiles[i] != "temp" && AllFiles[i] != "respaldo")
                                                            {
                                                                if (AllFiles2[i2] != "caf" && AllFiles2[i2] != "rcof" && AllFiles2[i2] != "pdf")
                                                                {

                                                                    string rutatt = @ruta+"bowa\\" + AllFiles[i] + "\\"
                                            + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\rcof_enviado\\";

                                                                    if (Directory.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha))
                                                                    {
                                                                        string[] pathFilesx = System.IO.Directory.GetFiles(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha, "*.xml");

                                                                        foreach (string pathFilex1 in pathFilesx)
                                                                        {
                                                                            entro = 0;
                                                                        }
                                                                    }
                                                                    if (Directory.Exists(@ruta+"bowa\\" + AllFiles[i] + "\\"
                                           + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_empaquetado\\"))
                                                                    {
                                                                        string[] pathFiles = System.IO.Directory.GetFiles(@ruta+"bowa\\" + AllFiles[i] + "\\"
                                               + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_empaquetado\\", "*.xml");

                                                                        string fs = @ruta+"bowa\\" + AllFiles[i] + "\\"
                                               + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_empaquetado\\";
                                                                        int vv = 1;
                                                                        if (entro == 1)
                                                                        {


                                                                            entro = 0;
                                                                            foreach (string pathFilex1 in pathFiles)
                                                                            {
                                                                                entro = 1;
                                                                                fecha = AllFiles1[i1];
                                                                            }
                                                                        }
                                                                        if (entro == 1 && ((fecha == fecha_rcof) || fecha_rcof == ""))
                                                                        {
                                                                            if (aqui == 0)
                                                                            {
                                                                                Console.WriteLine("Buscando xml en rut: [" + AllFiles[i] + "] en Sucursal [" + AllFiles2[i2] + "] Equipo [" + AllFiles3[i3] + "] fecha " + AllFiles1[i1]);

                                                                                aqui = 1;
                                                                            }
                                                                            int rcof_existe = 0;
                                                                            if (fecha_rcof == "")
                                                                            {
                                                                                if (Directory.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof))
                                                                                {
                                                                                    string[] pathFiles1x = System.IO.Directory.GetFiles(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "//", "*.xml");

                                                                                    foreach (string pathFilex1 in pathFiles1x)
                                                                                    {
                                                                                        rcof_existe = 1;
                                                                                    }
                                                                                }
                                                                            }

                                                                            string gg1 = AllFiles1[i1];
                                                                            string[] hh = (AllFiles1[i1].Split('-'));
                                                                            int[] ggg = new int[3];
                                                                            ggg[0] = Convert.ToInt32(hh[0]);
                                                                            ggg[1] = Convert.ToInt32(hh[1]);
                                                                            ggg[2] = Convert.ToInt32(hh[2]);
                                                                            int suma = ggg[0] + ggg[1] + ggg[2];
                                                                            //  Console.WriteLine(DateTime.Today.Date.ToString());
                                                                            string g = DateTime.Today.Date.ToString();
                                                                            string fh = DateTime.Today.ToShortDateString();
                                                                            if (esteequipo == 1)
                                                                            {
                                                                                hh = (fh.Split('/'));
                                                                            }
                                                                            else
                                                                            {
                                                                                hh = (fh.Split('-'));
                                                                            }
                                                                            int[] ggg1 = new int[3];
                                                                            ggg1[0] = Convert.ToInt32(hh[2]);
                                                                            ggg1[1] = Convert.ToInt32(hh[1]);
                                                                            ggg1[2] = Convert.ToInt32(hh[0]);

                                                                            if (ggg1[0].ToString().Length < 4)
                                                                            {
                                                                                ggg1[0] = 2000 + ggg1[0];
                                                                            }

                                                                            //int suma1 = ggg1[0] + ggg1[1] + ggg1[2];

                                                                            bool dia_actual = false;
                                                                            if ((ggg1[1] == ggg[1] && ggg1[2] == ggg[2] && ggg1[0] == ggg[0]))
                                                                            {
                                                                                dia_actual = true;
                                                                            }
                                                                            if (rcof_existe == 0 && !dia_actual)
                                                                            {
                                                                                generar_rcof = 1;
                                                                                Console.WriteLine("Buscando xml firmados en rut: [" + AllFiles[i] + "] en Sucursal [" + AllFiles2[i2] + "] Equipo [" + AllFiles3[i3] + "] fecha " + AllFiles1[i1]);
                                                                                fff = fecha;
                                                                                foreach (string pathFile1 in pathFiles)
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        fecha_rcof = AllFiles1[i1];
                                                                                        vaciox = 0;
                                                                                        string xml = File.ReadAllText(pathFile1, Encoding.GetEncoding("ISO-8859-1"));

                                                                                        Console.Write("Guardando Boleta " + pathFile1 + "\n");
                                                                                        List<int> numbers = new List<int>();

                                                                                        XmlDocument xmlDoc = new XmlDocument();
                                                                                        XmlElement doc = xmlDoc.DocumentElement;
                                                                                        xmlDoc.LoadXml(xml);
                                                                                        XmlNodeList contenido = xmlDoc.GetElementsByTagName("Folio");
                                                                                        string numero_folio = "";
                                                                                        string fecha_emision = "";
                                                                                        string neto = "0";
                                                                                        string exento = "0";
                                                                                        string iva = "0";
                                                                                        string total = "0";
                                                                                        string tipo = "39";

                                                                                        int wy = 0;
                                                                                        foreach (XmlNode node in contenido)
                                                                                        {
                                                                                            string dd = node.InnerText;
                                                                                            numero_folio = dd;
                                                                                            dd = dd.Replace('"', ' ');
                                                                                            int d = Convert.ToInt32(dd);
                                                                                            numbers.Add(Convert.ToInt32(d));
                                                                                            wy++;

                                                                                        }

                                                                                        contenido = xmlDoc.GetElementsByTagName("FchEmis");
                                                                                        foreach (XmlNode node in contenido)
                                                                                        {
                                                                                            fecha_emision = node.InnerText;

                                                                                        }
                                                                                        contenido = xmlDoc.GetElementsByTagName("TipoDTE");
                                                                                        foreach (XmlNode node in contenido)
                                                                                        {
                                                                                            tipo = node.InnerText;

                                                                                        }
                                                                                        contenido = xmlDoc.GetElementsByTagName("MntNeto");
                                                                                        foreach (XmlNode node in contenido)
                                                                                        {
                                                                                            neto = node.InnerText;

                                                                                        }
                                                                                        contenido = xmlDoc.GetElementsByTagName("MntExe");
                                                                                        foreach (XmlNode node in contenido)
                                                                                        {
                                                                                            exento = node.InnerText;

                                                                                        }
                                                                                        contenido = xmlDoc.GetElementsByTagName("IVA");
                                                                                        foreach (XmlNode node in contenido)
                                                                                        {
                                                                                            iva = node.InnerText;

                                                                                        }
                                                                                        contenido = xmlDoc.GetElementsByTagName("MntTotal");
                                                                                        foreach (XmlNode node in contenido)
                                                                                        {
                                                                                            total = node.InnerText;

                                                                                        }

                                                                                        grabar_boleta(AllFiles[i], tipo, AllFiles2[i2], AllFiles3[i3], numero_folio, fecha_emision, neto, exento, iva, total, pathFile1);




                                                                                        var dte = ItaSystem.DTE.Engine.XML.XmlHandler.DeserializeFromString<ItaSystem.DTE.Engine.Documento.DTE>(xml);
                                                                                        dtes.Add(dte);
                                                                                        xmlDtes.Add(xml);
                                                                                        lines[y] = DateTime.Now.ToString() + " " + Path.GetFileName(pathFile1) + " " + "incluida_en RCOF";
                                                                                        Console.WriteLine(lines[y]);

                                                                                        string docPath = @ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\";
                                                                                        TextWriter tw = new StreamWriter(docPath + "log_" + fecha_rcof + ".txt", true);
                                                                                        tw.WriteLine(lines[y]);
                                                                                        tw.Close();
                                                                                    }
                                                                                    catch (Exception ee)
                                                                                    {
                                                                                        using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                                                                                        {

                                                                                            sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                                                                                        }
                                                                                        error("" + ee);
                                                                                    }


                                                                                    y++;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                catch (Exception ee)
                                                {
                                                    using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                                                    {
                                                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + rutempresa);
                                                        rut_error = rutempresa;
                                                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);
                                                        continue;
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Console.Write("\nAccion RCOF= " + generar_rcof + "\n");

                if (generar_rcof == 1)
                {

                    Console.Write("\n Generando RCOF");
                    if (File.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\envio.txt"))
                    {
                        string line = "";
                        System.IO.StreamReader file =
                           new System.IO.StreamReader(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\envio.txt");

                        while ((line = file.ReadLine()) != null)
                        {

                            envio = line;
                        }
                        int enviox = 0;
                        file.Close();
                        enviox = Convert.ToInt32(envio) + 50;
                        envio = "" + enviox;
                        System.IO.StreamWriter ficheroTemporal =
                    new System.IO.StreamWriter(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\envio.txt");
                        ficheroTemporal.WriteLine(envio);
                        ficheroTemporal.Close();
                        file.Close();


                    }
                    else
                    {
                        System.IO.StreamWriter ficheroTemporal =
                    new System.IO.StreamWriter(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\envio.txt");
                        ficheroTemporal.WriteLine("5");
                        ficheroTemporal.Close();
                        envio = "1";

                    }

                    var rcof = handler.GenerarRCOF(dtes, "\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\");
                    rcof.DocumentoConsumoFolios.Id = "RCOF_" + fecha.Replace("-", "") + "_" + envio;

                    Handler.Initialize2();
                    Handler.leer(rutempresa);

                    filePathArchivo = rcof.Firmar(Handler.represen_n, out xmlString, @ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\", "");
                    Console.WriteLine("RCOF de fecha:  " + fecha_rcof + " rut:" + rutempresa + " Generado " + filePathArchivo);
                    string docPath = @ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\";
                    TextWriter tw = new StreamWriter(docPath + "log_" + fecha_rcof + ".txt", true);
                    tw.WriteLine("RCOF de fecha:  " + fecha_rcof + " rut:" + rutempresa + " Generado " + filePathArchivo);
                    tw.Close();
                    fecha_rcof = "";
                    generar_rcof = 0;
                    dtes.Clear();


                }


                AllFilesx.Clear();
                entro = 0;
                int incluir = 0;

                string path = @ruta+"bowa\\" + rutempresa + "\\rcof\\";
                try
                {
                    string[] folderPaths = Directory.GetDirectories(path);
                    foreach (string s in folderPaths)
                    {
                        string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                        if (t != "1" && t != "2" && t != "3" && t != "4" && t != "5")
                            if (AllFiles1.Count > 0)
                            {
                                for (int tr = 0; tr <= AllFilesx.Count - 1; tr++)
                                {
                                    if (incluir == 0)
                                    {
                                        if (AllFilesx[tr] != t)
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
                                    AllFilesx.Add(t);
                                }
                                else
                                {
                                    incluir = 0;
                                }
                            }
                            else
                            {
                                AllFilesx.Add(t);
                            }

                    }


                }
                catch (Exception ee)
                {
                    using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                    {

                        sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                    }
                    //    Console.WriteLine("No se encontraron nuevos xml a enviar");
                }

                int cuentax = AllFilesx.Count();

                for (int ig = 0; ig < AllFilesx.Count(); ig++)
                {
                    if (Directory.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + AllFilesx[ig] + "\\"))
                    {

                        string ttt = @ruta+"bowa\\" + rutempresa + "\\rcof\\" + AllFilesx[ig] + "//";
                        string[] pathFiles1x = System.IO.Directory.GetFiles(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + AllFilesx[ig] + "//", "*.xml");
                        fecha_rcof = AllFilesx[ig];

                        foreach (string pathFilex1 in pathFiles1x)
                        {
                            if (!File.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + AllFilesx[ig] + "\\rcof_enviado.txt"))
                            {

                                filePathArchivo = pathFilex1;
                                if (File.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + AllFilesx[ig] + "\\rcof_enviado.txt"))
                                {


                                }
                                else
                                {
                                    System.IO.StreamWriter ficheroTemporal =
                                new System.IO.StreamWriter(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + AllFilesx[ig] + "\\rcof_enviado.txt");
                                    ficheroTemporal.WriteLine(DateTime.Now.ToString());
                                    ficheroTemporal.Close();


                                    entro = 1;
                                    long trackId = 0;


                                    xmlString = string.Empty;

                                    if (Handler.produccion == false)
                                    {
                                        trackId = handler.EnviarEnvioDTEToSII(pathFilex1, AmbienteEnum.Certificacion, @ruta+"bowa\\" + rutempresa + "\\rcof\\" + AllFilesx[ig] + "\\");
                                        error("RCOF de fecha:  " + AllFilesx[ig] + " rut:" + rutempresa + " Enviado al SII con tracking: " + trackId);

                                        Console.WriteLine("RCOF de fecha:  " + AllFilesx[ig] + " rut:" + rutempresa + " Enviado al SII con tracking: " + trackId);
                                        grabar_tracking(trackId, filePathArchivo);

                                    }
                                    else
                                    {
                                        Thread.Sleep(1000);
                                        trackId = handler.EnviarEnvioDTEToSII(pathFilex1, AmbienteEnum.Produccion, @ruta+"bowa\\" + rutempresa + "\\rcof\\" + AllFilesx[ig] + "\\");
                                        error("RCOF de fecha:  " + AllFilesx[ig] + " rut:" + rutempresa + " Enviado al SII con tracking: " + trackId);
                                        Console.WriteLine("RCOF de fecha:  " + AllFilesx[ig] + " rut:" + rutempresa + " Enviado al SII con tracking: " + trackId + " \n nombre del archivo: " + filePathArchivo);
                                        grabar_tracking(trackId, filePathArchivo);
                                        //wait 2 seconds 
                                    }
                                    if (Convert.ToInt64(trackId.ToString()) < 0)
                                    {
                                        Console.WriteLine(pathFilex1 + " error_envio " + trackId.ToString());
                                        Console.WriteLine("Error Archivo RCOF no recibido " + AllFilesx[ig] + " rut: " + rutempresa + " " + DateTime.Now.ToString());
                                        error("Error Archivo RCOF no recibido " + AllFilesx[ig] + " rut: " + rutempresa + " " + DateTime.Now.ToString());

                                        // Console.WriteLine("Error Archivo RCOF no recibido " + fecha_rcof + " rut: " + rutempresa + " " + DateTime.Now.ToString());
                                        //grabar_tracking(trackId, filePathArchivo);

                                    }
                                    else
                                    {

                                        //Console.WriteLine("RCOF Archivo enviado exitosamente del dia: " + fecha_rcof + " rut:" + rutempresa + " " + filePathArchivo + " Tracking:  " + trackId.ToString() + " fecha:" + DateTime.Now.ToString());
                                        // leer_estado_rcof(trackId, filePathArchivo);
                                        error("RCOF Archivo enviado exitosamente del dia: " + AllFilesx[ig] + " rut:" + rutempresa + " " + filePathArchivo + " Tracking:  " + trackId.ToString() + " fecha:" + DateTime.Now.ToString());

                                        //grabar_tracking(trackId, filePathArchivo);
                                        if (File.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + AllFilesx[ig] + "\\" + pathFilex1))
                                        {
                                            //   String rr = @ruta+"bowa\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\rcof_enviado\\" + fi.Name.Replace(".xml", "") + "_" + AllFiles2[i2] + "_" + AllFiles3[i3] + ".xml";
                                            //status = ItaSystem.DTE.Engine.CAFHandler.CAFHandler.subir_rcof(AllFiles[i], Convert.ToInt32(AllFiles2[i2]), Convert.ToInt32(AllFiles3[i3]), AllFiles1[i1], @ruta+"bowa\" + AllFiles[i] + "\\"
                                            //   + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\rcof_enviado\\", fi.Name);
                                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost//script//email_consumo_folios.php?rut=" + rutempresa + "&ruta=" + pathFilex1 + "&fecha=" + AllFilesx[ig]);
                                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                            String ver = response.ProtocolVersion.ToString();
                                            StreamReader reader = new StreamReader(response.GetResponseStream());
                                            string str = reader.ReadLine();
                                            while (str != null)
                                            {
                                                Console.WriteLine(str);
                                                str = reader.ReadLine();
                                            }
                                        }
                                        Thread.Sleep(1000);  //wait 2 seconds 


                                    }

                                }
                            }
                        }
                    }
                }

            }



            catch (Exception ee)
            {
                using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                {

                    sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                }
            }



            try
            {


                int ixx = 0;


                nombreconsumo = filePathArchivo;
                Console.WriteLine("Busqueda de Sobres por enviar Finalizada");
                using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo2.txt"))
                {
                    linesx[1] = "Barrido terminado";
                    Console.Write(linesx[1]);
                    foreach (string line in linesx)
                        if (line != "")
                        {
                            sw.WriteLine(line);
                        }
                }

                string path1xx = (@ruta+"bowa\\temp\\aviso2.txt");
                if (File.Exists(path1xx))
                {
                    //    File.Delete(path1xx);
                }
                path1xx = (@ruta+"bowa\\rutempresa\\rcof_cero.txt");
                if (File.Exists(path1xx))
                {
                    File.Delete(path1xx);
                }
            }
            catch (Exception ee)
            {
                using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                {
                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + rutempresa);
                    rut_error = rutempresa;
                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                }
            }

            //   Console.Write("Press <Enter> to exit... ");
            //  while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

        }

        //   Console.Write("Press <Enter> to exit... ");
        public static void error(string error)
        {
            string docPath = @ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\";
            TextWriter tw = new StreamWriter(docPath + "log_" + fecha_rcof + ".txt", true);
            tw.WriteLine(error); tw.Close();
            tw.Close();
        }
        public static void leer_estado_rcof(long trackid, string archivo)
        {
            try
            {

                var responseEstadoDTE = handler.ConsultarEstadoEnvio(AmbienteEnum.Produccion, trackid);
                string respuesta = responseEstadoDTE.ResponseXml;

                XmlDocument xmlDoc = new XmlDocument();
                XmlElement doc = xmlDoc.DocumentElement;
                xmlDoc.LoadXml(respuesta);
                XmlNodeList contenido = xmlDoc.GetElementsByTagName("ESTADO");

                foreach (XmlNode node in contenido)
                {
                    Handler.Estado = node.InnerText;

                }
                if (Handler.Estado != "-11")
                {
                    contenido = xmlDoc.GetElementsByTagName("TIPO_DOCTO");

                    foreach (XmlNode node in contenido)
                    {
                        Handler.tipoDoc = node.InnerText;

                    }


                    contenido = xmlDoc.GetElementsByTagName("INFORMADOS");
                    foreach (XmlNode node in contenido)
                    {
                        Handler.Informados = node.InnerText;

                    }
                    contenido = xmlDoc.GetElementsByTagName("ACEPTADOS");

                    foreach (XmlNode node in contenido)
                    {
                        Handler.aceptados = node.InnerText;

                    }

                    contenido = xmlDoc.GetElementsByTagName("REPAROS");

                    foreach (XmlNode node in contenido)
                    {
                        Handler.reparos = node.InnerText;

                    }
                    contenido = xmlDoc.GetElementsByTagName("RECHAZADOS");

                    foreach (XmlNode node in contenido)
                    {
                        Handler.rechazados = node.InnerText;


                    }

                    contenido = xmlDoc.GetElementsByTagName("TRACKID");

                    foreach (XmlNode node in contenido)
                    {
                        Handler.trackid = node.InnerText;
                    }

                    contenido = xmlDoc.GetElementsByTagName("ESTADO");

                    foreach (XmlNode node in contenido)
                    {
                        Handler.Estado = node.InnerText;

                    }
                    contenido = xmlDoc.GetElementsByTagName("GLOSA");

                    foreach (XmlNode node in contenido)
                    {
                        Handler.Glosa = node.InnerText;

                    }
                    contenido = xmlDoc.GetElementsByTagName("NUM_ATENCION");

                    foreach (XmlNode node in contenido)
                    {
                        string g = node.InnerText;
                        string[] H = g.Trim(' ').Split('(');
                        Handler.Numatencion = H[0];
                        string[] x1 = H[1].Split(' ');
                        Handler.Fecha = x1[1];
                        Handler.hora = x1[2].Trim(')');

                    }
                    Handler.Cliente = Program.rutempresa;
                    Handler.dte = archivo;
                    Handler.Initialize2();

                    string connectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";

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

                        cmd.Parameters.AddWithValue("@Cliente", Handler.Cliente);
                        cmd.Parameters.AddWithValue("@dte", Handler.dte);
                        cmd.Parameters.AddWithValue("@trackid", Handler.trackid);
                        cmd.Parameters.AddWithValue("@Estado", Handler.Estado);
                        cmd.Parameters.AddWithValue("@Glosa", Handler.Glosa);
                        cmd.Parameters.AddWithValue("@Numatencion", Handler.Numatencion);
                        cmd.Parameters.AddWithValue("@Fecha", Handler.Fecha);
                        cmd.Parameters.AddWithValue("@hora", Handler.hora);
                        cmd.Parameters.AddWithValue("@tipoDoc", Handler.tipoDoc);
                        cmd.Parameters.AddWithValue("@Informados", Handler.Informados);
                        cmd.Parameters.AddWithValue("@aceptados", Handler.aceptados);
                        cmd.Parameters.AddWithValue("@rechazados", Handler.rechazados);
                        cmd.Parameters.AddWithValue("@reparos", Handler.reparos);
                        cmd.ExecuteNonQuery();

                        connectionString = @"server=sh-pro12.hostgator.cl;userid=posfacto;password=eD1[P4q4kZw+0M;database=posfacto_bowa";
                        cmd = new MySqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = "Insert into respuestasii (Cliente, dte, trackid, Estado, Glosa, NumAtencion, Fecha, hora, tipoDoc, Informados, aceptados, rechazados, reparos ) VALUES " +
        "(@Cliente, @dte, @trackid, @Estado, @Glosa, @NumAtencion, @Fecha, @hora, @tipoDoc, @Informados, @aceptados, @rechazados, @reparos)";
                        cmd.Prepare();

                        cmd.Parameters.AddWithValue("@Cliente", Handler.Cliente);
                        cmd.Parameters.AddWithValue("@dte", Handler.dte);
                        cmd.Parameters.AddWithValue("@trackid", Handler.trackid);
                        cmd.Parameters.AddWithValue("@Estado", Handler.Estado);
                        cmd.Parameters.AddWithValue("@Glosa", Handler.Glosa);
                        cmd.Parameters.AddWithValue("@Numatencion", Handler.Numatencion);
                        cmd.Parameters.AddWithValue("@Fecha", Handler.Fecha);
                        cmd.Parameters.AddWithValue("@hora", Handler.hora);
                        cmd.Parameters.AddWithValue("@tipoDoc", Handler.tipoDoc);
                        cmd.Parameters.AddWithValue("@Informados", Handler.Informados);
                        cmd.Parameters.AddWithValue("@aceptados", Handler.aceptados);
                        cmd.Parameters.AddWithValue("@rechazados", Handler.rechazados);
                        cmd.Parameters.AddWithValue("@reparos", Handler.reparos);
                        cmd.ExecuteNonQuery();


                    }
                    finally
                    {
                        if (connection != null)
                            connection.Close();
                    }


                }



            }
            catch (Exception ee)
            {
                Console.WriteLine("Ha ocurrido un error:" + ee);
                error("Ha ocurrido un error:" + ee);
                using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                }
            }
        }

        public static void grabar_tracking(long trackid, string archivo)
        {

            Handler.Initialize2();
            Handler.leer(rutempresa);
            configuracion.LeerArchivo();

            byte[] rawData = File.ReadAllBytes(archivo);
            FileInfo info = new FileInfo(archivo);

            try
            {
                string fecha_consumo = "";
                string neto = "0";
                string iva = "0";
                string total = "0";
                string numero_folios = "0";
                string rango1 = "0";
                string rango2 = "0";
                string rango3 = "0";
                string rango4 = "0";
                string rango5 = "0";

                string rangof1 = "0";
                string rangof2 = "0";
                string rangof3 = "0";
                string rangof4 = "0";
                string rangof5 = "0";


                XmlDocument xmlDoc = new XmlDocument();
                XmlElement doc = xmlDoc.DocumentElement;
                xmlDoc.Load(archivo);
                XmlNodeList contenido = xmlDoc.GetElementsByTagName("FchInicio");

                foreach (XmlNode node in contenido)
                {
                    fecha_consumo = node.InnerText;

                }


                contenido = xmlDoc.GetElementsByTagName("MntNeto");

                foreach (XmlNode node in contenido)
                {
                    neto = node.InnerText;
                }

                contenido = xmlDoc.GetElementsByTagName("MntIva");

                foreach (XmlNode node in contenido)
                {
                    iva = node.InnerText;

                }
                contenido = xmlDoc.GetElementsByTagName("MntTotal");

                foreach (XmlNode node in contenido)
                {
                    total = node.InnerText;

                }


                contenido = xmlDoc.GetElementsByTagName("FoliosEmitidos");

                foreach (XmlNode node in contenido)
                {
                    numero_folios = node.InnerText;

                }

                int f = 0;
                contenido = xmlDoc.GetElementsByTagName("Inicial");

                foreach (XmlNode node in contenido)
                {
                    if (f == 0)
                    {
                        rango1 = node.InnerText;
                    }
                    if (f == 1)
                    {
                        rango2 = node.InnerText;
                    }
                    if (f == 2)
                    {
                        rango3 = node.InnerText;
                    }
                    if (f == 3)
                    {
                        rango4 = node.InnerText;
                    }
                    if (f == 4)
                    {
                        rango5 = node.InnerText;
                    }
                    f = f + 1;
                }
                f = 0;
                contenido = xmlDoc.GetElementsByTagName("Final");

                foreach (XmlNode node in contenido)
                {
                    if (f == 0)
                    {
                        rangof1 = node.InnerText;
                    }
                    if (f == 1)
                    {
                        rangof2 = node.InnerText;
                    }
                    if (f == 2)
                    {
                        rangof3 = node.InnerText;
                    }
                    if (f == 3)
                    {
                        rangof4 = node.InnerText;
                    }
                    if (f == 4)
                    {
                        rangof5 = node.InnerText;
                    }
                    f = f + 1;
                }

                Handler.Initialize2();
                Handler.leer(Program.rutempresa);
                configuracion.LeerArchivo();

                Handler.Cliente = Program.rutempresa;
                Handler.dte = archivo;


                if (rango1 == "")
                {
                    rango1 = "0";
                }
                if (rango2 == "")
                {
                    rango2 = "0";
                }
                if (rango3 == "")
                {
                    rango3 = "0";
                }
                if (rango4 == "")
                {
                    rango4 = "0";
                }
                if (rango5 == "")
                {
                    rango5 = "0";
                }


                if (rangof1 == "")
                {
                    rangof1 = "0";
                }
                if (rangof2 == "")
                {
                    rangof2 = "0";
                }
                if (rangof3 == "")
                {
                    rangof3 = "0";
                }
                if (rangof4 == "")
                {
                    rangof4 = "0";
                }
                if (rangof5 == "")
                {
                    rangof5 = "0";
                }
                string connectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
                string[] dias = DateTime.Now.ToString().Split(' ');
                string dia = dias[0];
                string clave_consumo = rutempresa + fecha_consumo;
                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into envioconsumo (Cliente, dte, trackid, fechaDeclaracion,NumeroFolios, Neto, Iva, Total," +
                        "fecha_consumo,rango_inicial1,rango_final1,rango_inicial2,rango_final2,rango_inicial3,rango_final3,rango_inicial4," +
                        "rango_final4,rango_inicial5,rango_final5,clave_consumo) VALUES " +
    "(@Cliente, @dte, @trackid, @fechaDeclaracion,@NumeroFolios, @Neto, @Iva, @Total," +
                        "@fecha_consumo,@rango_inicial1,@rango_final1,@rango_inicial2,@rango_final2,@rango_inicial3,@rango_final3,@rango_inicial4," +
                        "@rango_final4,@rango_inicial5,@rango_final5,@clave_consumo) ON DUPLICATE KEY UPDATE Cliente=@Cliente, dte=@dte, trackid=@trackid, fechaDeclaracion=@fechaDeclaracion, " +
                        "NumeroFolios = @NumeroFolios, Neto = @Neto, Iva=@Iva, Total=@Total, rango_inicial1 = @rango_inicial1, rango_final1 = @rango_final1," +
                        "rango_inicial2 = @rango_inicial2, rango_final2 = @rango_final2, rango_inicial3 = @rango_inicial3, rango_final3 = @rango_final3," +
                        "rango_inicial4 = @rango_inicial4, rango_final4 = @rango_final4, rango_inicial5= @rango_inicial5, rango_final5 = @rango_final5";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@Cliente", Handler.Cliente);
                    cmd.Parameters.AddWithValue("@dte", Handler.dte);
                    cmd.Parameters.AddWithValue("@trackid", trackid);
                    cmd.Parameters.AddWithValue("@fechaDeclaracion", dia);
                    cmd.Parameters.AddWithValue("@NumeroFolios", numero_folios);
                    cmd.Parameters.AddWithValue("@Neto", neto);
                    cmd.Parameters.AddWithValue("@Iva", iva);
                    cmd.Parameters.AddWithValue("@Total", total);
                    cmd.Parameters.AddWithValue("@fecha_consumo", fecha_consumo);
                    cmd.Parameters.AddWithValue("@rango_inicial1", rango1);
                    cmd.Parameters.AddWithValue("@rango_final1", rangof1);
                    cmd.Parameters.AddWithValue("@rango_inicial2", rango2);
                    cmd.Parameters.AddWithValue("@rango_final2", rangof2);
                    cmd.Parameters.AddWithValue("@rango_inicial3", rango3);
                    cmd.Parameters.AddWithValue("@rango_final3", rangof3);
                    cmd.Parameters.AddWithValue("@rango_inicial4", rango4);
                    cmd.Parameters.AddWithValue("@rango_final4", rangof4);
                    cmd.Parameters.AddWithValue("@rango_inicial5", rango5);
                    cmd.Parameters.AddWithValue("@rango_final5", rangof5);
                    cmd.Parameters.AddWithValue("@clave_consumo", clave_consumo);
                    cmd.ExecuteNonQuery();

                    if (connection != null)
                        connection.Close();
                    Thread.Sleep(1000);


                    connectionString = @"server=sh-pro12.hostgator.cl;userid=posfacto;password=eD1[P4q4kZw+0M;database=posfacto_bowa";
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into envioconsumo (Cliente, dte, trackid, fechaDeclaracion,NumeroFolios, Neto, Iva, Total," +
                        "fecha_consumo,rango_inicial1,rango_final1,rango_inicial2,rango_final2,rango_inicial3,rango_final3,rango_inicial4," +
                        "rango_final4,rango_inicial5,rango_final5,clave_consumo) VALUES " +
    "(@Cliente, @dte, @trackid, @fechaDeclaracion,@NumeroFolios, @Neto, @Iva, @Total," +
                        "@fecha_consumo,@rango_inicial1,@rango_final1,@rango_inicial2,@rango_final2,@rango_inicial3,@rango_final3,@rango_inicial4," +
                        "@rango_final4,@rango_inicial5,@rango_final5,@clave_consumo) ON DUPLICATE KEY UPDATE Cliente=@Cliente, dte=@dte, trackid=@trackid, fechaDeclaracion=@fechaDeclaracion, " +
                        "NumeroFolios = @NumeroFolios, Neto = @Neto, Iva=@Iva, Total=@Total, rango_inicial1 = @rango_inicial1, rango_final1 = @rango_final1," +
                        "rango_inicial2 = @rango_inicial2, rango_final2 = @rango_final2, rango_inicial3 = @rango_inicial3, rango_final3 = @rango_final3," +
                        "rango_inicial4 = @rango_inicial4, rango_final4 = @rango_final4, rango_inicial5= @rango_inicial5, rango_final5 = @rango_final5";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@Cliente", Handler.Cliente);
                    cmd.Parameters.AddWithValue("@dte", Handler.dte);
                    cmd.Parameters.AddWithValue("@trackid", trackid);
                    cmd.Parameters.AddWithValue("@fechaDeclaracion", dia);
                    cmd.Parameters.AddWithValue("@NumeroFolios", numero_folios);
                    cmd.Parameters.AddWithValue("@Neto", neto);
                    cmd.Parameters.AddWithValue("@Iva", iva);
                    cmd.Parameters.AddWithValue("@Total", total);
                    cmd.Parameters.AddWithValue("@fecha_consumo", fecha_consumo);
                    cmd.Parameters.AddWithValue("@rango_inicial1", rango1);
                    cmd.Parameters.AddWithValue("@rango_final1", rangof1);
                    cmd.Parameters.AddWithValue("@rango_inicial2", rango2);
                    cmd.Parameters.AddWithValue("@rango_final2", rangof2);
                    cmd.Parameters.AddWithValue("@rango_inicial3", rango3);
                    cmd.Parameters.AddWithValue("@rango_final3", rangof3);
                    cmd.Parameters.AddWithValue("@rango_inicial4", rango4);
                    cmd.Parameters.AddWithValue("@rango_final4", rangof4);
                    cmd.Parameters.AddWithValue("@rango_inicial5", rango5);
                    cmd.Parameters.AddWithValue("@rango_final5", rangof5);
                    cmd.Parameters.AddWithValue("@clave_consumo", clave_consumo);
                    cmd.ExecuteNonQuery();
                    if (connection != null)
                        connection.Close();

                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine("ERROR " + ee);
                Thread.Sleep(10000);
                //   Console.WriteLine("Ha ocurrido un error:" + ex);
                //  error("Ha ocurrido un error:" + ex);
                using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                }
            }
        }


        public static void grabar_boleta(string rut, string tipo, string sucu, string equipo, string folio, string fecha, string neto, string exento, string iva, string total, string documento)
        {
            try
            {
                Handler.Initialize2();
                Handler.leer(Program.rutempresa);
                configuracion.LeerArchivo();
                string claveboleta = rut + "_" + folio;


                string connectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";

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
                    //  cmd.Parameters.AddWithValue("@boleta", rawData);
                    cmd.Parameters.AddWithValue("@claveboleta", claveboleta);
                    cmd.ExecuteNonQuery();



                    Handler.Cliente = Program.rutempresa;
                    byte[] rawData = File.ReadAllBytes(documento);


                    cmd.CommandText = "Insert into boleta_archivo(clave_boleta,boleta) VALUES " + "(@claveboleta1,@boleta) ON " +
                        "DUPLICATE KEY UPDATE boleta=@boleta";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@claveboleta1", claveboleta);
                    cmd.Parameters.AddWithValue("@boleta", rawData);
                    cmd.ExecuteNonQuery();


                    connectionString = @"server=sh-pro12.hostgator.cl;userid=posfacto;password=eD1[P4q4kZw+0M;database=posfacto_bowa"; connection = new MySqlConnection(connectionString);
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
                    //  cmd.Parameters.AddWithValue("@boleta", rawData);
                    cmd.Parameters.AddWithValue("@claveboleta", claveboleta);
                    cmd.ExecuteNonQuery();


                    connectionString = @"server=162.241.61.53;userid=posfacto_bowa;password=bowa123.;database=posfacto_bowa";
                    cmd.CommandText = "Insert into boleta_archivo(clave_boleta,boleta) VALUES " + "(@claveboleta1,@boleta) ON " +
                        "DUPLICATE KEY UPDATE boleta=@boleta";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@claveboleta1", claveboleta);
                    cmd.Parameters.AddWithValue("@boleta", rawData);
                    cmd.ExecuteNonQuery();


                }

                finally
                {
                    if (connection != null)
                        connection.Close();
                }






            }
            catch (Exception ex)
            {
                //   Console.WriteLine("Ha ocurrido un error:" + ex);
                //  error("Ha ocurrido un error:" + ex);
            }




        }




        public static void grabar_fechas(string fecha_inicial, string fecha_final, string sucursal, string equipo, string rut)
        {
            
            Console.WriteLine("\nRut " + rut + " fecha Inicial= " + fecha_inicial + " fecha final: " + fecha_final);
           // Console.Write("Press <Enter> to exit... ");
           // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            Handler.Initialize2();
            Handler.leer(rut);
            configuracion.LeerArchivo();
            try
            {
                string connectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    string dis = "srx10k";
                    cmd.CommandText = "SELECT distribuidor FROM certificado WHERE rut_empresa= '"+rut+"'";
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        dis = (reader.GetString(0));
                    }
                    if (dis == "0") {
                        dis = "srx10k";
                    }
                    reader.Close();

                    cmd.CommandText = "Insert into fechas_activas (rut, fecha_inicial, fecha_final, sucursal, equipo, clave_fecha, distribuidor) VALUES " +
    "(@rut, @fecha_inicial, @fecha_final, @sucursal,@equipo, @clave_fecha, @distribuidor) ON DUPLICATE KEY UPDATE fecha_inicial=@fecha_inicial," +
    "fecha_final= @fecha_final, distribuidor=@distribuidor";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@rut", rut);
                    cmd.Parameters.AddWithValue("@fecha_inicial", fecha_inicial);
                    cmd.Parameters.AddWithValue("@fecha_final", fecha_final);
                    cmd.Parameters.AddWithValue("@sucursal", sucursal);
                    cmd.Parameters.AddWithValue("@equipo", equipo);
                    cmd.Parameters.AddWithValue("@clave_fecha", rut + "_" + sucursal + "_" + equipo);
                    cmd.Parameters.AddWithValue("@distribuidor", dis);
                    cmd.ExecuteNonQuery();
                    if (connection != null)
                        connection.Close();
                    Thread.Sleep(1000);
                    
                        connectionString = @"server=sh-pro12.hostgator.cl;userid=posfacto;password=eD1[P4q4kZw+0M;database=posfacto_bowa";
                        connection = new MySqlConnection(connectionString);
                        connection.Open();
                        cmd = new MySqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = "Insert into fechas_activas (rut, fecha_inicial, fecha_final, sucursal, equipo, clave_fecha, distribuidor) VALUES " +
        "(@rut, @fecha_inicial, @fecha_final, @sucursal,@equipo, @clave_fecha, @distribuidor) ON DUPLICATE KEY UPDATE fecha_inicial=@fecha_inicial," +
        "fecha_final= @fecha_final, distribuidor = @distribuidor";
                        cmd.Prepare();

                        cmd.Parameters.AddWithValue("@rut", rut);
                        cmd.Parameters.AddWithValue("@fecha_inicial", fecha_inicial);
                        cmd.Parameters.AddWithValue("@fecha_final", fecha_final);
                        cmd.Parameters.AddWithValue("@sucursal", sucursal);
                        cmd.Parameters.AddWithValue("@equipo", equipo);
                        cmd.Parameters.AddWithValue("@clave_fecha", rut + "_" + sucursal + "_" + equipo);
                    cmd.Parameters.AddWithValue("@distribuidor", dis);
                    cmd.ExecuteNonQuery();
                        if (connection != null)
                            connection.Close();

                    
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }

            }
            catch (Exception ee)
            {
                    Console.Write("Press <Enter> to exit... ");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                // Console.WriteLine("Ha ocurrido un error:" + ee);
                error("Ha ocurrido un error:" + ee);
                Thread.Sleep(5000);
                using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                }
            }
        }

        public static int encontro_xml_enviar = 0;
        public static int espera = 100;
        public static void enviar_sobre1()
        {
            string[] linesx = new string[2];

            string ruta1 = @ruta+"\\bowa\\";

            String path = @"" + ruta1 + rutempresa + "\\";
            string sucu = "0";
            string equi = "0";
            AllFiles.Clear();
            try
            {
                if (Directory.Exists(@"" + ruta1 + rutempresa + "\\"))
                {
                    String[] filePaths = Directory.GetFiles(@"" + ruta1 + rutempresa + "\\");

                    string[] folderPaths = Directory.GetDirectories(path);
                    AllFiles.Clear();
                    foreach (string s in folderPaths)
                    {
                        string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                        if (t != "caf" && t != "rcof" && t != "bowa" && t != "pdf")
                        {
                            AllFiles.Add(t);
                            sucu = t;
                        }

                    }
                }
            }
            catch (Exception ee)

            {
                using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                }
                //Console.WriteLine("\nNo se encontraron nuevos xml a firmar");
            }
            int incluir = 0;
            AllFiles2.Clear();

            int cantidad = AllFiles.Count();
            for (int i = 0; i < cantidad; i++)
            {

                path = @"" + ruta1 + rutempresa + "\\" + AllFiles[i] + "\\";
                try
                {
                    string[] folderPaths = Directory.GetDirectories(path);
                    foreach (string s in folderPaths)
                    {
                        string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                        if (t != "bowa" && t != "rcof" && t != "pdf")
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
                                    equi = t;
                                }
                                else
                                {
                                    incluir = 0;
                                }
                            }
                            else
                            {
                                AllFiles2.Add(t);
                                equi = t;
                            }



                        }


                    }
                }

                catch (Exception ee)
                {
                    //          Console.WriteLine("\nNo se encontraron nuevos xml a firmar");
                    using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                    {

                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                    }
                }

            }

            int gh = 0;
            string[] fecha1 = DateTime.Today.ToString().Split(' ');
            string fecha = fecha1[0];
            int dia_con_ventas = 0;
            AllFiles3.Clear();
            cantidad = AllFiles.Count();
            for (int i = 0; i < cantidad; i++)
            {

                int cantidad2 = AllFiles2.Count();
                for (int i2 = 0; i2 < cantidad2; i2++)
                {

                    // Console.WriteLine(ruta1 + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] );
                    path = @"" + ruta1 + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\";
                    String pathx = @"" + ruta1 + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\";
                    try
                    {
                        if (Directory.Exists(path))
                        {
                            string[] folderPaths = Directory.GetDirectories(path);

                            foreach (string s in folderPaths)
                            {
                                gh = 1;

                                string t = s.Replace("/", "");
                                t = s.Remove(0, s.LastIndexOf('\\') + 1);
                                if (Directory.Exists(pathx + t))
                                {
                                    if (AllFiles3.Count > 0)
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
                        }
                    }

                    catch (Exception ee)
                    {
                        using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                        {

                            sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                        }
                        //                Console.WriteLine("\nNo se encontraron nuevos xml a firmar");
                    }
                }


            }

            int cuentas = AllFiles3.Count();
            for (int y = 0; y < cuentas; y++)
            {
                string gg1 = AllFiles3[y];
                string[] hh = (AllFiles3[y].Split('-'));
                int[] ggg = new int[3];
                ggg[0] = Convert.ToInt32(hh[0]);
                ggg[1] = Convert.ToInt32(hh[1]);
                ggg[2] = Convert.ToInt32(hh[2]);
                int suma = ggg[0] + ggg[1] + ggg[2];
                //  Console.WriteLine(DateTime.Today.Date.ToString());
                string g = DateTime.Today.Date.ToString();
                string[] fh = DateTime.Today.ToString().Split(' ');
                hh = (fh[0].Split('-'));
                if (esteequipo == 1)
                {
                    hh = (fh[0].Split('/'));
                }

                if (Convert.ToInt32(hh[0]) > 1)
                {
                    ggg[0] = Convert.ToInt32(hh[0]) - 1;
                    ggg[1] = Convert.ToInt32(hh[1]);
                    ggg[2] = Convert.ToInt32(hh[2]);
                }
                else
                {
                    if (Convert.ToInt32(hh[1]) == 2)
                    {
                        ggg[0] = 31;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 3)
                    {
                        ggg[0] = 28;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 4)
                    {
                        ggg[0] = 31;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 5)
                    {
                        ggg[0] = 30;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }

                    if (Convert.ToInt32(hh[1]) == 6)
                    {
                        ggg[0] = 31;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 7)
                    {
                        ggg[0] = 30;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 8)
                    {
                        ggg[0] = 31;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 9)
                    {
                        ggg[0] = 31;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 10)
                    {
                        ggg[0] = 30;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 11)
                    {
                        ggg[0] = 31;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);
                    }
                    if (Convert.ToInt32(hh[1]) == 12)
                    {
                        ggg[0] = 30;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);
                    }
                    if (Convert.ToInt32(hh[1]) == 1)
                    {
                        ggg[0] = 31;
                        ggg[1] = 12;
                        ggg[2] = Convert.ToInt32(hh[2]) - 1;
                    }

                }

                string s1 = ggg[0].ToString();
                string s2 = ggg[1].ToString();
                string s3 = ggg[2].ToString();

                int suma1 = ggg[0] + ggg[1] + ggg[2];
                if (suma == suma1 && dia_con_ventas == 0)
                {
                    dia_con_ventas = 1;

                }


            }

            path = (ruta1 + rutempresa + "\\rcof_cero.txt");
            string[] lines1 = new string[100];
            AllFiles4.Clear();
            if (File.Exists(path))
            {
                lines1 = File.ReadAllLines(path);
                int cuen = lines1.Count();
                for (int f = 0; f < cuen; f++)
                {
                    string ffecha = lines1[f];
                    ffecha = ffecha.Replace("/", "-");
                    if (ffecha != "")
                    {
                        AllFiles6.Add(ffecha);
                        path = (@"" + ruta1 + rutempresa + "\\" + sucu + "\\" + equi + "\\" + ffecha + "\\");
                        try
                        {
                            //si no existe la carpeta temporal la creamos
                            if (!(Directory.Exists(path)))
                            {
                                Directory.CreateDirectory(path);

                            }


                        }
                        catch (Exception ee)
                        {
                            using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                            {

                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                            }
                        }


                        path = (@"" + ruta1 + rutempresa + "\\rcof\\" + ffecha + "\\");
                        try
                        {
                            //si no existe la carpeta temporal la creamos
                            if (!(Directory.Exists(path)))
                            {
                                Directory.CreateDirectory(path);

                            }


                        }
                        catch (Exception ee)
                        {
                            using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                            {

                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                            }
                        }


                        path = (@"" + ruta1 + rutempresa + "\\" + sucu + "\\" + equi + "\\" + ffecha + "\\xml_empaquetado\\");
                        try
                        {
                            //si no existe la carpeta temporal la creamos
                            if (!(Directory.Exists(path)))
                            {
                                Directory.CreateDirectory(path);

                            }


                        }
                        catch (Exception ee)
                        {
                            using (StreamWriter sw = File.CreateText(@ruta+"bowa\temp\\listo1.txt"))
                            {

                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                            }
                        }
                        path = (@"" + ruta1 + rutempresa + "\\" + sucu + "\\" + equi + "\\" + ffecha + "\\xml_firmado\\");
                        try
                        {
                            //si no existe la carpeta temporal la creamos
                            if (!(Directory.Exists(path)))
                            {
                                Directory.CreateDirectory(path);

                            }


                        }
                        catch (Exception ee)
                        {
                            using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                            {

                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                            }
                        }

                        path = (@"" + ruta1 + rutempresa + "\\" + sucu + "\\" + equi + "\\" + ffecha + "\\sobre_preparado\\");
                        try
                        {
                            //si no existe la carpeta temporal la creamos
                            if (!(Directory.Exists(path)))
                            {
                                Directory.CreateDirectory(path);

                            }


                        }
                        catch (Exception ee)
                        {
                            using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                            {

                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                            }
                        }

                        path = (@"" + ruta1 + rutempresa + "\\" + sucu + "\\" + equi + "\\" + ffecha + "\\sobre_enviado\\");
                        try
                        {
                            //si no existe la carpeta temporal la creamos
                            if (!(Directory.Exists(path)))
                            {
                                Directory.CreateDirectory(path);

                            }


                        }
                        catch (Exception ee)
                        {
                            using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                            {

                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);
                               //     Console.Write("Press <Enter> to exit... ");
                                 // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                            }
                        }


                    }
                }
            }


            if (dia_con_ventas == 0)
            {
                string[] hh = (fecha.Split('-'));
                if (esteequipo == 1)
                {
                    hh = (fecha.Split('/'));
                }
                int[] ggg = new int[3];
                if (Convert.ToInt32(hh[0]) > 1)
                {
                    ggg[0] = Convert.ToInt32(hh[0]) - 1;
                    ggg[1] = Convert.ToInt32(hh[1]);
                    ggg[2] = Convert.ToInt32(hh[2]);
                }
                else
                {
                    if (Convert.ToInt32(hh[1]) == 2)
                    {
                        ggg[0] = 31;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 3)
                    {
                        ggg[0] = 28;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 4)
                    {
                        ggg[0] = 31;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 5)
                    {
                        ggg[0] = 30;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }

                    if (Convert.ToInt32(hh[1]) == 6)
                    {
                        ggg[0] = 31;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 7)
                    {
                        ggg[0] = 30;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 8)
                    {
                        ggg[0] = 31;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 9)
                    {
                        ggg[0] = 31;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 10)
                    {
                        ggg[0] = 30;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);

                    }
                    if (Convert.ToInt32(hh[1]) == 11)
                    {
                        ggg[0] = 31;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);
                    }
                    if (Convert.ToInt32(hh[1]) == 12)
                    {
                        ggg[0] = 30;
                        ggg[1] = Convert.ToInt32(hh[1]) - 1;
                        ggg[2] = Convert.ToInt32(hh[2]);
                    }
                    if (Convert.ToInt32(hh[1]) == 1)
                    {
                        ggg[0] = 31;
                        ggg[1] = 12;
                        ggg[2] = Convert.ToInt32(hh[2]) - 1;
                    }


                }
                string dia = "" + ggg[0];
                if (dia.Length == 1)
                {
                    dia = "0" + dia;
                }
                string mes = "" + ggg[1];
                if (mes.Length == 1)
                {
                    mes = "0" + mes;
                }
                fecha = ggg[2] + "-" + mes + "-" + dia;

                path = (@"" + ruta1 + rutempresa + "\\" + sucu + "\\" + equi + "\\" + fecha + "\\");
                try
                {
                    //si no existe la carpeta temporal la creamos
                    if (!(Directory.Exists(path)))
                    {
                        Directory.CreateDirectory(path);

                    }


                }
                catch (Exception ee)
                {
                    using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                    {

                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                    }
                }


                path = (@"" + ruta1 + rutempresa + "\\rcof\\" + fecha + "\\");
                try
                {
                    //si no existe la carpeta temporal la creamos
                    if (!(Directory.Exists(path)))
                    {
                        Directory.CreateDirectory(path);

                    }


                }
                catch (Exception ee)
                {
                    using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                    {

                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                    }
                }


                path = (@"" + ruta1 + rutempresa + "\\" + sucu + "\\" + equi + "\\" + fecha + "\\xml_empaquetado\\");
                try
                {
                    //si no existe la carpeta temporal la creamos
                    if (!(Directory.Exists(path)))
                    {
                        Directory.CreateDirectory(path);

                    }


                }
                catch (Exception ee)
                {
                    using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                    {

                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                    }
                }
                path = (@"" + ruta1 + rutempresa + "\\" + sucu + "\\" + equi + "\\" + fecha + "\\xml_firmado\\");
                try
                {
                    //si no existe la carpeta temporal la creamos
                    if (!(Directory.Exists(path)))
                    {
                        Directory.CreateDirectory(path);

                    }


                }
                catch (Exception ee)
                {
                    using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                    {

                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                    }
                }

                path = (@"" + ruta1 + rutempresa + "\\" + sucu + "\\" + equi + "\\" + fecha + "\\sobre_preparado\\");
                try
                {
                    //si no existe la carpeta temporal la creamos
                    if (!(Directory.Exists(path)))
                    {
                        Directory.CreateDirectory(path);

                    }


                }
                catch (Exception ee)
                {
                    using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                    {

                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                    }
                }

                path = (@"" + ruta1 + rutempresa + "\\" + sucu + "\\" + equi + "\\" + fecha + "\\sobre_enviado\\");
                try
                {
                    //si no existe la carpeta temporal la creamos
                    if (!(Directory.Exists(path)))
                    {
                        Directory.CreateDirectory(path);

                    }


                }
                catch (Exception ee)
                {
                    using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                    {

                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                    }
                }

                if (File.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\"))

                    if (File.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\envio.txt"))
                {
                    string line = "";
                    System.IO.StreamReader file =
                       new System.IO.StreamReader(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\envio.txt");

                    while ((line = file.ReadLine()) != null)
                    {

                        envio = line;
                    }
                    int enviox = 0;
                    file.Close();
                    enviox = Convert.ToInt32(envio) + 50;
                    envio = "" + enviox;
                    System.IO.StreamWriter ficheroTemporal =
                new System.IO.StreamWriter(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\envio.txt");
                    ficheroTemporal.WriteLine(envio);
                    ficheroTemporal.Close();
                    file.Close();


                }
                else
                {
                    System.IO.StreamWriter ficheroTemporal =
                new System.IO.StreamWriter(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha + "\\envio.txt");
                    ficheroTemporal.WriteLine("1");
                    ficheroTemporal.Close();
                    envio = "1";

                }
                //  Console.WriteLine(fecha);
                fecha_rcof = fecha;
                var rcof = handler.GenerarRCOF_vacio(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\");
                rcof.DocumentoConsumoFolios.Id = "RCOF_" + fecha_rcof.Replace("-", "");
                string xmlString = "";
                Handler.Initialize2();
                Handler.leer(rutempresa);

                var filePathArchivo = rcof.Firmar(Handler.represen_n, out xmlString, @ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\", "");
                Console.WriteLine("RCOF de fecha:  " + fecha_rcof + " rut:" + rutempresa + " Generado " + filePathArchivo);
                string docPath = @ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\";
                TextWriter tw = new StreamWriter(docPath + "log_" + fecha + ".txt", true);
                tw.WriteLine("RCOF de fecha:  " + fecha_rcof + " rut:" + rutempresa + " Generado " + filePathArchivo);
                tw.Close();
                fecha_rcof = "";


                DirectoryInfo dii = new DirectoryInfo(ruta1 + rutempresa + "\\rcof\\" + fecha_rcof + "\\");
                foreach (var fi in dii.GetFiles("*.xml"))
                {
                    enviox = fi.Name;
                    string pathFile = fi.Name;
                    encontro_xml_enviar = 1;

                    nombre_file = fi.Name;
                    long trackId = handler.EnviarEnvioDTEToSII(dii + fi.Name, AmbienteEnum.Certificacion, ruta1 + rutempresa + "\\rcof\\" + fecha_rcof + "\\");
                    if (Convert.ToInt32(trackId.ToString()) <= 0)
                    {
                        Console.WriteLine(fi.Name + " error_envio " + trackId.ToString());
                    }

                    else
                    {


                        Console.WriteLine(fi.Name + " enviado " + trackId.ToString());

                        using (StreamWriter sw = File.CreateText(ruta1 + rutempresa + "\\rcof\\" + fecha_rcof + "\\envio_rcof.txt"))
                        {
                            string[] fecha1x = DateTime.Today.ToString().Split(' ');
                            string fechax = fecha1[0];
                            sw.WriteLine(fechax);
                        }
                        if (File.Exists(ruta1 + rutempresa + "\\rcof" + fecha_rcof + "\\" + fi.Name))
                        {
                            try
                            {
                                /* HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost//script//email_consumo_folios.php?rut=" + rutempresa + "\\rcof\\"  + fecha_vacio_actual + "\\rcof\\" + fi.Name + "&fecha=" + fecha_vacio_actual);
                                 HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                 String ver = response.ProtocolVersion.ToString();
                                 StreamReader reader = new StreamReader(response.GetResponseStream());
                                 string str = reader.ReadLine();
                                 while (str != null)
                                 {
                                     Console.WriteLine(str);
                                     str = reader.ReadLine();
                                 }*/
                            }
                            catch (Exception ee)
                            {
                                using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                                {

                                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                                }
                            }
                        }

                        break;
                    }

                }


            }

            cuentas = AllFiles6.Count();
            for (int rr = 0; rr < cuentas; rr++)
            {
                fecha_rcof = AllFiles6[rr];
                //  Console.WriteLine(fecha_rcof);
                if (File.Exists(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\envio.txt"))
                {
                    string line = "";
                    System.IO.StreamReader file =
                       new System.IO.StreamReader(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\envio.txt");

                    while ((line = file.ReadLine()) != null)
                    {

                        envio = line;
                    }
                    int enviox = 0;
                    file.Close();
                    enviox = Convert.ToInt32(envio) + 50;
                    envio = "" + enviox;
                    System.IO.StreamWriter ficheroTemporal =
                new System.IO.StreamWriter(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\envio.txt");
                    ficheroTemporal.WriteLine(envio);
                    ficheroTemporal.Close();
                    file.Close();


                }
                else
                {
                    System.IO.StreamWriter ficheroTemporal =
                new System.IO.StreamWriter(@ruta+"bowa\\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\envio.txt");
                    ficheroTemporal.WriteLine("1");
                    ficheroTemporal.Close();
                    envio = "1";

                }

                /*var rcof = handler.GenerarRCOF_vacio(@ruta+"bowa\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\");
                rcof.DocumentoConsumoFolios.Id = "RCOF_" + fecha_rcof.Replace("-", "");
                string xmlString = "";
                Handler.Initialize2();
                Handler.leer(rutempresa);

                var filePathArchivo = rcof.Firmar(Handler.represen_n, out xmlString, @ruta+"bowa\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\", "");
                Console.WriteLine("RCOF de fecha:  " + fecha_rcof + " rut:" + rutempresa + " Generado " + filePathArchivo);
                string docPath = @"C: \Users\one\Documents\bowa\" + rutempresa + "\\rcof\\" + fecha_rcof + "\\";
                TextWriter tw = new StreamWriter(docPath + "log_" + fecha_rcof + ".txt", true);
                tw.WriteLine("RCOF de fecha:  " + fecha_rcof + " rut:" + rutempresa + " Generado " + filePathArchivo);
                tw.Close();
                fecha_rcof = "";

                */




                DirectoryInfo dii = new DirectoryInfo(ruta1 + rutempresa + "\\rcof\\" + fecha_vacio_actual + "\\");
                foreach (var fi in dii.GetFiles("*.xml"))
                {
                    enviox = fi.Name;
                    string pathFile = fi.Name;
                    encontro_xml_enviar = 1;
                    Thread.Sleep(espera);
                    nombre_file = fi.Name;
                    long trackId = handler.EnviarEnvioDTEToSII(dii + fi.Name, AmbienteEnum.Certificacion, ruta1 + rutempresa + "\\rcof\\" + fecha_vacio_actual + "\\");
                    grabar_tracking(trackId, ruta1 + rutempresa + "\\rcof\\" + fecha_vacio_actual + "\\" + fi.ToString());
                    if (Convert.ToInt32(trackId.ToString()) <= 0)
                    {
                        Console.WriteLine(fi.Name + " error_envio " + trackId.ToString());
                    }
                    else
                    {
                        Console.WriteLine(fi.Name + " enviado " + trackId.ToString());
                        try
                        {
                            /*  HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost//script//email_consumo_folios.php?rut=" + rutempresa + "\\rcof\\"+ fecha_vacio_actual +  fi.Name.Replace(".xml", "") + "_" + sucu + "_" + equi + ".xml" + "&fecha=" + fecha_vacio_actual);
                              HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                              String ver = response.ProtocolVersion.ToString();
                              StreamReader reader = new StreamReader(response.GetResponseStream());
                              string str = reader.ReadLine();
                              while (str != null)
                              {
                                  Console.WriteLine(str);
                                  str = reader.ReadLine();
                              }*/
                        }
                        catch (Exception ee)
                        {

                            using (StreamWriter sw = File.CreateText(@ruta+"bowa\\temp\\listo1.txt"))
                            {

                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                            }



                        }



                        break;
                    }
                }

            }
            if (File.Exists(@"C:\bowa\rcof_cero.txt"))
            {
                File.Delete(@"C:\bowa\rcof_cero.txt");
                using (StreamWriter sw = File.CreateText(@"C:\bowa\rcof_cero.txt"))
                {

                    sw.WriteLine("");

                }
            }
            try
            {
                // enviar_sobre();
            }
            catch (Exception ee)
            {
                linesx[0] = "Error " + ee;
                using (StreamWriter sw = File.CreateText(@ruta+"bowa\\listo2.txt"))
                {
                    Console.Write(linesx[0]);
                    foreach (string line in linesx)
                        if (line != "")
                        {
                            sw.WriteLine(line);
                        }
                }


                Console.WriteLine("Error " + ee);
            }

            using (StreamWriter sw = File.CreateText(@ruta+"bowa\\listo2.txt"))
            {
                linesx[1] = "Barrido terminado";
                Console.Write(linesx[1]);
                foreach (string line in linesx)
                    if (line != "")
                    {
                        sw.WriteLine(line);
                    }
            }

            string path1xx = (@"C:\bowa\temp\aviso2.txt");
            if (File.Exists(path1xx))
            {
                File.Delete(path1xx);
            }
            path1xx = (@"C:\bowa\temp\aviso_vacio.txt");
            if (File.Exists(path1xx))
            {
                File.Delete(path1xx);
            }

            //    Console.Write("Press <Enter> to exit... ");
            //  while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }

    }



}

