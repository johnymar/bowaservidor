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
        static public int servidor = 0;
        static public string enviox;
        static public string nombreconsumo;
        static List<string> AllFiles = new List<string>();
        static List<string> AllFiles1 = new List<string>();
        static List<string> AllFiles2 = new List<string>();
        static List<string> AllFiles3 = new List<string>();
        static List<string> AllFiles4 = new List<string>();
        static List<string> AllFiles6 = new List<string>();

        static List<string> AllFilesx = new List<string>();
        static int esteequipo = 0;
        static public string nombre_file = "";
        static public string fecha = "";
        static public string pathtoken = "";
        static public string envio;
        static public string rut = "0";
        private static Autorizacion aut;
        static public List<string> fecha_vacio = new List<string>();
        static public string fecha_vacio_actual = "";
        static public Configuracion configuracion = new Configuracion();
        static public List<string> rut_produccion = new List<string>();
        static public List<string> rut_ejecutado = new List<string>();
        static public string rutempresa = "0";
        static public string fecha_inicial = "";
       // public static string ruta = @"C:\Users\administrador\Documents\bowa\"; 
        //public static string ruta1 = @"C:\Users\administrador\Documents\";
        public static string ruta = @"C:\Users\Administrador\Documents\bowa\";
        public static string ruta1 = @"CC:\Users\Administrador\Documents\bowa\";
        static void Main(string[] args)
        {
            try
            {
                Thread t = new Thread(new ParameterizedThreadStart(ThreadLoop));
                t.Start((Action)CallBack);
            }
            catch (Exception ee)
            {
                using (StreamWriter sw = File.CreateText(@ruta + "\\temp\\listo1_1.txt"))
                {

                    sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                }
                //  Console.Write("Press <Enter> to exit... ");
                // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            }
        }
        private static void CallBack()
        {

            // verificar_hora();
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
                    using (StreamWriter sw = File.CreateText(@ruta + "temp\\listo1_1.txt"))
                    {

                        sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                    }
                    // Console.Write("Press <Enter> to exit... ");
                    // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                }
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
                string rutempresa_ejecutar = "0";
                string rutempresa = "0";
                for (int ir = 0; ir < cuenta_ruts; ir++)
                {
                    rutempresa = rut_produccion[ir];
                    rutempresa_ejecutar = rutempresa;

                    string[] lines = new string[2];
                    string fecha = "";


                    string path = (@ruta + "temp\\rut_mover_empa_firmado.txt");

                    /*  if (File.Exists(path))
                         {
                             // string path = (@"C:\Users\admin\Documents\bowa\temp\aviso.txt");
                             lines = File.ReadAllLines(path);
                             Console.WriteLine(String.Join(Environment.NewLine, lines));
                             if (!lines[0].Equals(null))
                             {
                                 rutempresa_ejecutar = lines[0];
                                 rutempresa = rutempresa_ejecutar;
                                 fecha = lines[1];

                                 Console.WriteLine("\nEjecutando solo para RUT:[ " + rutempresa_ejecutar + " ]");
                             }

                         }
                                       Handler.leer_rut_estatus();

                            Console.WriteLine("Leyendo rut=" + rut_produccion[pasar]);*/

                    AllFiles.Clear();
                    AllFiles1.Clear();
                    AllFiles2.Clear();
                    AllFiles3.Clear();
                    fecha_vacio.Clear();
                    Console.WriteLine("\nBorrando para RUT:[ " + rutempresa + " ]");
                    int incluir = 0;
                    string t1 = rutempresa;
                    AllFiles.Add(t1);
                    int cantidad = AllFiles.Count();
                    for (int i = 0; i < cantidad; i++)
                    {
                        if (AllFiles[i] != "temp" && AllFiles[i] != "respaldo2")
                        {
                            path = @ruta + "\\" + AllFiles[i] + "\\";
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
                                                Console.WriteLine("Directorio " + @ruta + "\\" + AllFiles[i] + "\\" + t);
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
                                using (StreamWriter sw = File.CreateText(@ruta + "temp\\listo1_1.txt"))
                                {

                                    sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                                }
                                //                  Console.WriteLine("No se encontraron nuevos xml a enviar");
                            }
                        }
                    }

                    cantidad = AllFiles2.Count();

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
                                    path = @ruta + "" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\";
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
                                                    Console.WriteLine("Directorio " + @ruta + "" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + t);
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
                                        using (StreamWriter sw = File.CreateText(@ruta + "temp\\listo1_1.txt"))
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

                                        path = @ruta + "" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\";
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
                                                            Console.WriteLine("Directorio " + @ruta + "" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + t);
                                                        }
                                                        else
                                                        {
                                                            incluir = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        AllFiles1.Add(t);
                                                        Console.WriteLine("Directorio " + @ruta + "" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + t);

                                                    }

                                                }
                                            }
                                        }
                                        catch (Exception ee)
                                        {
                                            using (StreamWriter sw = File.CreateText(@ruta + "temp\\listo1_1.txt"))
                                            {

                                                sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);
                                                Console.WriteLine("Error " + ee);
                                                //    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

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
                    fecha = "2023-06-01";
                    mover(fecha);
                  //  Console.Write("Press <Enter> to exit... ");
                  //  while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

//                    Environment.Exit(0);
                    //  Console.Clear();


                }
            }

            catch (Exception ee)
            {
                using (StreamWriter sw = File.CreateText(@ruta + "temp\\listo1_1.txt"))
                {

                    sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                }

                Process[] proc = Process.GetProcessesByName("mover_empa_firmado.exe");
                proc[0].Kill();

            }
        }


        public static string rut_error = "";
        public static void mover(string fecha)
        {
            try
            {
                try
                {

                    //si no existe la carpeta temporal la creamos

                }
                catch (Exception ee)
                {
                    using (StreamWriter sw = File.CreateText(@ruta + "temp\\listo1_1.txt"))
                    {

                        sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                    }
                }
/*                string path = (@ruta + "temp\\fecha_mover.txt");
                string[] lines = null;
                if (File.Exists(path))
                {
                    // string path = (@"C:\Users\admin\Documents\bowa\temp\aviso.txt");
                    lines = File.ReadAllLines(path);
                    Console.WriteLine(String.Join(Environment.NewLine, lines));
                    if (!lines[0].Equals(null))
                    {
                        fecha = lines[0];
                    }

                }
*/

                int cantidad = AllFiles1.Count();
                for (int i1 = 0; i1 < cantidad; i1++)
                {
                    DateTime fecha1 = Convert.ToDateTime(AllFiles1[i1]);
                    DateTime fecha2 = Convert.ToDateTime(fecha);

                    if ( fecha1>=fecha2)
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
                                            fecha = AllFiles1[i1];
                                            rutempresa = AllFiles[i];



                                            string tt = @ruta + "" + AllFiles[i] + "\\" +
                                              AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\";
                                            if (Directory.Exists(@ruta + "" + AllFiles[i] + "\\" +
                                             AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\"))
                                            {

                                                rutempresa = AllFiles[i];
                                                string[] folderPaths = Directory.GetDirectories(@ruta + "" + AllFiles[i] + "\\" +
                                                    AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\");

                                                if (AllFiles1.Count() > 0)

                                                {
                                                    try
                                                    {

                                                        string dpaths = (@ruta + "" + AllFiles[i] + "\\"
                                        + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_empaquetado\\");

                                                        if (Directory.Exists(dpaths))
                                                        {
                                                       //     Console.Write("\n Moviendo a firmado");
                                                            string[] pathFilesss = System.IO.Directory.GetFiles(dpaths, "*.xml");
                                                            Console.WriteLine("Copiado empaqueado a firnado en: "+ AllFiles[i] + "\\"
                                        + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] );
                                                            foreach (string pathFilex1 in pathFilesss)
                                                            {
                                                                System.IO.File.Copy(pathFilex1, @ruta + "\\" + AllFiles[i] + "\\"
                                           + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_firmado\\" + Path.GetFileName(pathFilex1), true);
                                                            //    Console.WriteLine("Copiando Boleta " + Path.GetFileName(pathFilex1));
                                                            }
                                                        }
                                                    }

                                                    catch (Exception ee)
                                                    {
                                                        using (StreamWriter sw = File.CreateText(@ruta + "temp\\listo1_1.txt"))
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
                }
            }
            catch (Exception ee)
            {
            }
        }
    }
}
         