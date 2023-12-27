using ItaSystem.DTE.Engine.Documento;
using ItaSystem.DTE.Engine.Envio;
using ItaSystem.DTE.Engine.XML;
using dte_local.clases;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using ItaSystem.DTE.Engine.Helpers;
using Renci.SshNet;

namespace dte_local
{
    class Program
    {
        public static string @ruta = @"C:/Users/Administrador/Documents/bowa/";
       
        static public List<string> rut_produccion = new List<string>();
        static public List<string> rut_ejecutado = new List<string>();
        static List<string> AllFiles = new List<string>();
        static List<string> AllFiles2 = new List<string>();
        static List<string> AllFiles3 = new List<string>();
        static List<string> sucursal = new List<string>();
        static List<string> equipo = new List<string>();

        static string fecha = "";
        static public string envio;
        static public string[] lines = new string[100000];
        private static Autorizacion aut;
        static Handler handler = new Handler();
        static public Configuracion configuracion = new Configuracion();

        static public string rutempresa = "77193144-8";
        private List<string> listaClientes;
        static void Main(string[] args)
        {
            try
            {
                Thread t = new Thread(new ParameterizedThreadStart(ThreadLoop));
                t.Start((Action)CallBack);
            }
            catch (Exception ee)
            {
                using (StreamWriter sw = File.CreateText(@ruta+"\temp\\listo_2.txt"))
                {

                    sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                }
                Console.Write("Press <Enter> to exit... ");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
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
                    Thread.Sleep(500);
                }
                catch (Exception ee)
                {
                    using (StreamWriter sw = File.CreateText(@ruta+"\temp\\listo_2.txt"))
                    {

                        sw.WriteLine(rutempresa + "" + DateTime.Now.ToString() + " Error: " + ee);

                    }
                  
                }
            }
        }

        static void principal()
        {
            try
            {
                rut_ejecutado.Clear();
                rut_produccion.Clear();
                Console.WriteLine("Creando tabla rut declarar");
                Handler.guardar_tabla();
                Handler.HasRows();

                int cuenta_ruts = rut_produccion.Count();
                if (cuenta_ruts > 0)
                {
                    for (int pasar = 0; pasar < cuenta_ruts; pasar++)
                    {
                        rutempresa = rut_produccion[pasar];
                        char c = rutempresa[0];


                        if (c != '7' || c == '7')
                        {
                            Console.WriteLine("Ejecutando DTE_LOCAL para rut: " + rutempresa);
                            try
                            {/*
                        Process[] instancia = Process.GetProcessesByName("dte_local1");
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
                        }
                        
                            string rut_aviso = rutempresa;
                            using (StreamWriter sw = File.CreateText(@ruta+"\\temp\\aviso.txt"))
                            {



                                sw.WriteLine(rut_aviso);


                            }*/

                                AllFiles.Clear();
                                AllFiles2.Clear();
                                AllFiles3.Clear();
                                int buscar = 0;
                                /*                string[] lines = new string[2];
                                                string path = (@ruta+"\temp\aviso.txt");
                                                if (File.Exists(path))
                                                {
                                                    // string path = (@"C:\Users\admin\Documents\bowa\temp\aviso.txt");
                                                    lines = File.ReadAllLines(path);
                                                    Console.WriteLine(String.Join(Environment.NewLine, lines));
                                                    if (!lines[0].Equals(null))
                                                    {
                                                        rutempresa = lines[0];
                                                        //                fecha = lines[1];
                                                        Console.WriteLine("\nInicio de Firma de Boletas para RUT:[ " + rutempresa + " ]");
                                                    }*/

                                string path = @ruta + "\\temp\\" + rutempresa + "\\";
                                try
                                {
                                    String[] filePaths = Directory.GetFiles(@ruta + "\\temp\\" + rutempresa + "\\");
                                    string[] folderPaths = Directory.GetDirectories(path);
                                    AllFiles.Clear();
                                    foreach (string s in folderPaths)
                                    {
                                        string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                                        AllFiles.Add(t);

                                    }

                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine("\nNo se encontraron nuevos xml a firmar");
                                    using (StreamWriter sw = File.CreateText(@ruta + "\\temp\\listo_2.txt"))
                                    {

                                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                                    }
                                }

                                int cantidad = AllFiles.Count();

                                for (int i = 0; i < cantidad; i++)
                                {

                                    path = @ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\";
                                    if (Directory.Exists(path))
                                    {
                                        try
                                        {
                                            string[] folderPaths = Directory.GetDirectories(path);

                                            foreach (string s in folderPaths)
                                            {
                                                string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                                                AllFiles2.Add(t);

                                            }
                                        }

                                        catch (Exception ee)
                                        {
                                            Console.WriteLine("\nNo se encontraron nuevos xml a firmar");
                                            using (StreamWriter sw = File.CreateText(@ruta + "\\temp\\listo_2.txt"))
                                            {

                                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                                            }
                                        }
                                    }
                                }


                                cantidad = AllFiles.Count();
                                for (int i = 0; i < cantidad; i++)
                                {
                                    int cantidad2 = AllFiles2.Count();
                                    for (int i2 = 0; i2 < cantidad2; i2++)
                                    {


                                        path = @ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\";
                                        try
                                        {
                                            if (Directory.Exists(path))
                                            {
                                                string[] folderPaths = Directory.GetDirectories(path);

                                                foreach (string s in folderPaths)
                                                {
                                                    string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                                                    AllFiles3.Add(t);

                                                }
                                            }
                                        }

                                        catch (Exception ee)
                                        {
                                            Console.WriteLine("\nNo se encontraron nuevos xml a firmar");
                                            using (StreamWriter sw = File.CreateText(@ruta + "\temp\\listo_2.txt"))
                                            {

                                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                                            }
                                        }
                                    }

                                }


                                path = (@ruta + "\\" + rutempresa + "\\caf\\");
                                try
                                {
                                    //si no existe la carpeta temporal la creamos
                                    if (!(Directory.Exists(path)))
                                    {

                                        Directory.CreateDirectory(path);

                                    }


                                }
                                catch (Exception ex)
                                {
                                    using (StreamWriter sw = File.CreateText(@ruta + "\temp\\listo_2.txt"))
                                    {

                                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                                    }
                                }
                                // 



                                /*     Handler.buscar_viejo(rutempresa);
                                     if (Handler.existe_viejo == 1)
                                     {
                                         Console.WriteLine("\nEncontrado en servidor viejo");
                                         Bajar_caf(rutempresa);

                                     }

                                     else
                                     {
                                         Bajar_caf2(rutempresa);
                                     }*/

                                //               Bajar_caf3(rutempresa);
                                Console.WriteLine("Firmar XML");

                                firmar_boletas();
                                /*
                                                        Thread.Sleep(3000);

                                                        Process.Start(@"C:\Users\one\Documents\dte\dte_local1\dte_local1\bin\Debug\dte_local1.exe");
                                                        instancia = Process.GetProcessesByName("dte_local1");
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
                                                        }
                                */
                                Handler.grabar_estatus(rutempresa, 0);
                                Handler.grabar_sobre(rutempresa, 1);
                                //DeleteDirectory(@ruta+"\\temp\\" + rutempresa + "\\");
                                DeleteDirectory_fantasma(rutempresa);


                                Console.Clear();

                            }
                            catch (Exception ee)
                            {
                                Console.Write("error:" + ee);
                                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                            }
                        }
                    }
                }
                else
                {

                    using (StreamWriter sw = File.CreateText(@ruta + "\\temp\\listo_2.txt"))
                    {
                        //  lines[0] = "No se encontro aviso.txt";
                        Console.Write("\n" + lines[0]);
                        foreach (string line in lines)
                            if (line != "")
                            {
                                sw.WriteLine();
                            }
                    }

                    Console.Clear();
                    // Console.Write("Press <Enter> to exit... ");
                    //   while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                    //Thread.Sleep(1000);

                    //                Environment.Exit(0);

                }

            }
            catch (Exception e) { }

        }

        public static void borrar_directorio_fantasma(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                try
                {

                    Directory.Delete(dirPath, recursive: true);    //throws if directory doesn't exist. 
                }
                catch
                {
                  //  Thread.Sleep(2000);  //wait 2 seconds 
                    Directory.Delete(dirPath, recursive: true);
                }

            }
        }

        public static void DeleteDirectory(string dirPath)
        {
            int borrar = 0;
            int cantidad = AllFiles.Count();
            for (int i = 0; i < cantidad; i++)
            {
                int cantidad2 = AllFiles2.Count();

                for (int i2 = 0; i2 < cantidad2; i2++)
                {
                    int cantidad3 = AllFiles3.Count();

                    for (int i3 = 0; i3 < cantidad3; i3++)
                    {
                        string xpath = @ruta+"\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\";
                        if (Directory.Exists(xpath))
                        {
                            if (Directory.GetFiles(xpath).Count() > 0) { }
                            else
                            {
                                borrar = 1;
                                if (Directory.Exists(xpath))
                                {
                                    try
                                    {

                                        Directory.Delete(xpath, recursive: true);    //throws if directory doesn't exist. 
                                    }
                                    catch
                                    {
                                      //  Thread.Sleep(2000);  //wait 2 seconds 
                                        Directory.Delete(xpath, recursive: true);
                                    }

                                }
                            }
                        }
                    }
                    if (borrar == 1)
                    {
                        String xxpath = @ruta+"\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\";
                        if (Directory.Exists(xxpath))
                        {
                            if (Directory.GetFiles(xxpath).Count() > 0) { }
                            else
                            {
                                if (Directory.Exists(xxpath))
                                {
                                    try
                                    {
                                        Directory.Delete(xxpath, recursive: true);    //throws if directory doesn't exist. 
                                    }
                                    catch
                                    {
                                     //   Thread.Sleep(2000);  //wait 2 seconds 
                                        Directory.Delete(xxpath, recursive: true);
                                    }

                                }
                            }
                        }
                    }
                }
                if (borrar == 1)
                {
                    String zpath = @ruta+"\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\";
                    if (Directory.GetFiles(zpath).Count() > 0) { }
                    else
                    {
                        if (Directory.Exists(zpath))
                        {
                            try
                            {

                                Directory.Delete(zpath, recursive: true);    //throws if directory doesn't exist. 
                            }
                            catch
                            {
                               // Thread.Sleep(2000);  //wait 2 seconds 
                                Directory.Delete(zpath, recursive: true);
                            }

                        }
                    }
                }
            }
            if (borrar == 1)
            {
                String path = @ruta+"\\temp\\" + rutempresa + "\\";
                if (Directory.GetFiles(path).Count() > 1) { }
                else
                {
                    if (Directory.Exists(path))
                    {
                        try
                        {

                            Directory.Delete(path, recursive: true);    //throws if directory doesn't exist. 
                        }
                        catch
                        {
                        //    Thread.Sleep(2000);  //wait 2 seconds 
                            Directory.Delete(path, recursive: true);
                        }

                    }
                }
            }
        }


        public static void DeleteDirectory_fantasma(string rutempresa)
        {

            string path = @ruta+"\\temp\\" + rutempresa + "\\";
            try
            {
                String[] filePaths = Directory.GetFiles(@ruta+"\\temp\\" + rutempresa + "\\");
                string[] folderPaths = Directory.GetDirectories(path);
                AllFiles.Clear();
                foreach (string s in folderPaths)
                {
                    string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                    if (t != "pdf" && t != "caf" && t != "rcof" && t != "vacio")
                    {
                        AllFiles.Add(t);
                    }

                }

            }
            catch (Exception ee)
            {
            }

            int cantidad = AllFiles.Count();

            for (int i = 0; i < cantidad; i++)
            {

                path = @ruta+"\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\";
                if (Directory.Exists(path))
                {
                    try
                    {
                        string[] folderPaths = Directory.GetDirectories(path);

                        foreach (string s in folderPaths)
                        {
                            string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                            AllFiles2.Add(t);

                        }
                    }

                    catch (Exception ee)
                    {
                    }
                }
            }


            cantidad = AllFiles.Count();
            for (int i = 0; i < cantidad; i++)
            {
                int salir = 0;
                int cantidad2 = AllFiles2.Count();
                for (int i2 = 0; i2 < cantidad2; i2++)
                {


                    path = @ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\";
                    try
                    {
                        if (Directory.Exists(path))
                        {
                            string[] folderPaths = Directory.GetDirectories(path);

                            foreach (string s in folderPaths)
                            {
                                string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                                string path1 = @ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + t;
                                if (Directory.GetFiles(path1).Count() > 1){
                salir = 1;
            }   
                            }
                        }
                    }catch(Exception e) { }
                    if (Directory.Exists(path) && salir==0)
                    {
                        DeleteDirectory(path);

                    }
                }

            }

           
        }               

                        
                  

        
        public static void firmar_boletas()
        {
            try
            {
                Handler.vencido = "0";
                Console.WriteLine("\nBuscando xml para firmar");
                int cantidad = AllFiles.Count();
                for (int i = 0; i < cantidad; i++)
                {

                    string pathx = @ruta+"\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\";

                    int y = 0;

                    int cantidad1 = AllFiles2.Count();
                    try
                    {
                        for (int i2 = 0; i2 < cantidad1; i2++)
                        {
                            int cantidad2 = AllFiles3.Count();
                            for (int i3 = 0; i3 < cantidad2; i3++)
                            {
                                if (AllFiles3[i3].Length > 2)
                                {
                                }
                                else
                                {
                                    AllFiles3[i3] = "20" + AllFiles3[i3];
                                }
                                string path_ini = @ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\";

                                DirectoryInfo di = new DirectoryInfo(@ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\");

                                if (Directory.Exists(@ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\"))
                                {

                                    grabar_fechas(AllFiles3[i3], AllFiles3[i3], AllFiles[i], AllFiles[i], rutempresa);

                                    string path = (@ruta + "\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\xml_firmado\\");
                                    try
                                    {
                                        //si no existe la carpeta temporal la creamos
                                        if (!(Directory.Exists(path)))
                                        {
                                            if (AllFiles3[i3].Length > 2)
                                            {
                                            }
                                            else
                                            {
                                                AllFiles3[i3] = "20" + AllFiles3[i3];
                                            }
                                            path = (@ruta + "\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\xml_firmado\\");
                                            Directory.CreateDirectory(path);
                                            grabar_fechas(AllFiles3[i3], AllFiles3[i3], AllFiles[i], AllFiles[i], rutempresa);
                                        }


                                    }
                                    catch (Exception ex)
                                    {
                                        using (StreamWriter sw = File.CreateText(@ruta + "\temp\\listo_2.txt"))
                                        {

                                            sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                                        }
                                    }
                                    path = (@ruta + "\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\xml_empaquetado\\");

                                    try
                                    {
                                        //si no existe la carpeta temporal la creamos
                                        if (!(Directory.Exists(path)))
                                        {
                                            if (AllFiles3[i3].Length > 2)
                                            {
                                            }
                                            else
                                            {
                                                AllFiles3[i3] = "20" + AllFiles3[i3];
                                            }
                                            path = (@ruta + "\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\xml_empaquetado\\");

                                            Directory.CreateDirectory(path);

                                        }


                                    }
                                    catch (Exception ex)
                                    {
                                        using (StreamWriter sw = File.CreateText(@ruta + "\temp\\listo_2.txt"))
                                        {

                                            sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                                        }
                                    }


                                    path = (@ruta + "\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\sobre_preparado\\");

                                    try
                                    {
                                        //si no existe la carpeta temporal la creamos
                                        if (!(Directory.Exists(path)))
                                        {
                                            if (AllFiles3[i3].Length > 2)
                                            {
                                            }
                                            else
                                            {
                                                AllFiles3[i3] = "20" + AllFiles3[i3];
                                            }
                                            path = (@ruta + "\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\sobre_preparado\\");

                                            Directory.CreateDirectory(path);

                                        }


                                    }
                                    catch (Exception ex)
                                    {
                                        using (StreamWriter sw = File.CreateText(@ruta + "\temp\\listo_2.txt"))
                                        {

                                            sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                                        }
                                    }

                                    path = (@ruta + "\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\sobre_enviado\\");

                                    try
                                    {
                                        //si no existe la carpeta temporal la creamos
                                        if (!(Directory.Exists(path)))

                                        {
                                            if (AllFiles3[i3].Length > 2)
                                            {
                                            }
                                            else
                                            {
                                                AllFiles3[i3] = "20" + AllFiles3[i3];
                                            }
                                            path = (@ruta + "\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\sobre_enviado\\");

                                            Directory.CreateDirectory(path);
                                          
                                        }


                                    }
                                    catch (Exception ex)
                                    {
                                        using (StreamWriter sw = File.CreateText(@ruta + "\temp\\listo_2.txt"))
                                        {

                                            sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                                        }
                                    }


                                }
                                if ((Directory.Exists(path_ini)))
                                {
                                    foreach (var fi in di.GetFiles("*.xml") )
                                    {
                                        if (Handler.vencido == "0")
                                        {
                                            string pathFile = fi.Name;
                                        // System.IO.File.Copy(fi.FullName, "out\\temp\\" + rutempresa + "\\" + fi.Name, true);

                                        string xml1 = File.ReadAllText(@ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + pathFile, Encoding.GetEncoding("ISO-8859-1"));

                                        XmlDocument xmlDoc = new XmlDocument();
                                        XmlElement doc = xmlDoc.DocumentElement;
                                        Console.WriteLine("Leyendo " + @ruta + "temp/" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + pathFile);

                                        if (File.Exists(@ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + pathFile))
                                        {
                                            xmlDoc.Load(@ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + pathFile);
                                        }


                                       XmlNodeList contenido = xmlDoc.GetElementsByTagName("PrcItem");
                                  /*      string[] monto = new string[300];
                                        int numero = 0;
                                        foreach (XmlNode node in contenido)
                                        {
                                            monto[numero] = node.InnerText;
                                            numero = numero + 1;
                                        }
                                        contenido = xmlDoc.GetElementsByTagName("QtyItem");
                                        int cantidadx = 0;
                                        string[] cant = new string[1000];
                                        foreach (XmlNode node in contenido)
                                        {
                                            cant[cantidadx] = node.InnerText;
                                            cantidadx = cantidadx + 1;

                                        }
                                        int numero1 = 0;
                                        contenido = xmlDoc.GetElementsByTagName("MontoItem");
                                        foreach (XmlNode node in contenido)
                                        {
                                            cant[numero1] = cant[numero1].Replace(".", ",");
                                            //    Console.WriteLine("cant" + cant[numero1] + " monto" + monto[numero1]);
                                            double total = Convert.ToDouble(cant[numero1]) * Convert.ToInt32(monto[numero1]);
                                            int ggi = Convert.ToInt32(total);
                                            node.InnerText = ggi.ToString();
                                            numero1 = numero1 + 1;

                                        }*/
                                        xmlDoc.Save(@ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + pathFile);
                                       xmlDoc.Load(@ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + pathFile);
                                            contenido = xmlDoc.GetElementsByTagName("TED");
                                        string TED_inserto = null;
                                       foreach (XmlNode node in contenido)
                                        {
                                            TED_inserto = node.InnerText;

                                        }

                                        contenido = xmlDoc.GetElementsByTagName("TSTED");
                                        string time_stamp = null;
                                        foreach (XmlNode node in contenido)
                                        {
                                            time_stamp = node.InnerText;

                                        }
                                        contenido = xmlDoc.GetElementsByTagName("RR");
                                        string RR = null;
                                        foreach (XmlNode node in contenido)
                                        {
                                            RR = node.InnerText;
                                            if (RR == "6666666-6")
                                            {
                                                node.InnerText = "66666666-6";
                                            }
                                        }
                                        string path1 = "";
                                        if (TED_inserto == null)
                                        {
                                            time_stamp = DateTime.Now.ToString();
                                        }
                                        else
                                        {

                                        }
                                        XmlNodeList contenido1 = xmlDoc.GetElementsByTagName("MntTotal");
                                        string montox = "";

                                        foreach (XmlNode node in contenido1)
                                        {
                                   //         montox = node.InnerText;

                                        }

                                        if (montox == "" || Convert.ToInt32(montox) == 0)
                                        {
                                      //      error1(rutempresa + "_" + pathFile);
                                        }
             
                                        try
                                        {
                                            contenido = xmlDoc.GetElementsByTagName("TED");
                                            TED_inserto = null;
                                            foreach (XmlNode node in contenido)
                                            {
                                                TED_inserto = node.InnerText;

                                            }

                                            if (TED_inserto != null)
                                            {
                                                XmlNode node1 = xmlDoc.SelectSingleNode("/DTE/Documento/TED");
                                                node1.ParentNode.RemoveChild(node1);
                                                xmlDoc.Save(@ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + pathFile);
                                                    //  xmlDoc.Save(@ruta+"\\temp\\" + pathFile);
                                                    Console.Write("\nXXXXXXXXX-BORRO TED-XXXXXXXXXX");
                                                }
                                                contenido = xmlDoc.GetElementsByTagName("Folio");
                                                string numero_folio = "";
                                                string fecha_emision = "";
                                                string neto = "0";
                                                string exento = "0";
                                                string iva = "0";
                                                string total = "0";
                                                string tipo = "39";
                                                List<int> numbers = new List<int>();
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

                                                Handler.grabar_boleta(pathFile, rutempresa, tipo, AllFiles[i], AllFiles2[i2], numero_folio, fecha_emision, neto, exento, iva, total, pathFile);
                                                
                                        }
                                        catch (Exception ee) { }
                                        xml1 = File.ReadAllText(@ruta + "\\temp\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + pathFile, Encoding.GetEncoding("ISO-8859-1"));

                                        var dte = ItaSystem.DTE.Engine.XML.XmlHandler.DeserializeFromString<ItaSystem.DTE.Engine.Documento.DTE>(xml1);
                                        path1 = handler.TimbrarYFirmarXMLDTE(time_stamp, dte, @ruta + "\\temp\\" + rutempresa + "\\", @ruta + "" + rutempresa + "//caf//", fi.Name, @ruta + "\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + "xml_firmado\\");
                                        if (Handler.vencido=="0") { 
                                        File.Delete(di + fi.Name);

                                        if (!path1.Equals(""))
                                        {

                                            handler.Validate(path1, ITA_CHILE.Security.Firma.Firma.TipoXML.DTE, ItaSystem.DTE.Engine.XML.Schemas.DTE);
                                            //   Clave.SubirArchivo( path, handler.configuracion.Empresa.RutCuerpo.ToString()+"-"+ handler.configuracion.Empresa.DV);
                                            lines[y] = DateTime.Now.ToString() + " " + fi.Name + " " + "Firmado";
                                            System.IO.File.Copy(path1, @ruta + "\\" + rutempresa + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + "xml_firmado\\" + Path.GetFileName(path1), true);
                                            Console.WriteLine("\n" + lines[y]);
                                            y++;
                                            File.Delete(@ruta + "\\temp\\" + rutempresa + "\\" + Path.GetFileName(path1));


                                                }

                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                           
                                            try
                                            {
                                                string connectionString = @"server=localhost;userid=root1;password=12345678;database=bowa";
                                                // string connectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
                                                MySqlConnection connection = null;
                                                try
                                                {
                                                    connection = new MySqlConnection(connectionString);
                                                    connection.Open();
                                                    MySqlCommand cmd1 = new MySqlCommand();
                                                    cmd1.Connection = connection;
                                                    cmd1.CommandText = "Insert into revision_copiar_boletas (rut, fecha, sucursal,certificado, clave) VALUES " +
                                    "( @rut, @fecha, @sucursal,@certificado,@clave) ON DUPLICATE KEY UPDATE certificado=@certificado";
                                                    cmd1.Prepare();
                                                    cmd1.Parameters.AddWithValue("@rut", rutempresa);
                                                    cmd1.Parameters.AddWithValue("@fecha", AllFiles3[i3]);
                                                    cmd1.Parameters.AddWithValue("@sucursal", AllFiles[i]);
                                                    cmd1.Parameters.AddWithValue("@certificado", "vencido");
                                                    cmd1.Parameters.AddWithValue("@clave", rutempresa + "_" + AllFiles[i] + "_" + AllFiles3[i3]);
                                                    cmd1.ExecuteNonQuery();
                                                    if (connection != null)
                                                        connection.Close();
                                                    //  Thread.Sleep(1000);
                                                    if (connection != null)
                                                        connection.Close();


                                                }
                                                catch (Exception e) { }
                                            
                                                Handler.Initialize2();
                                                MySqlCommand cmd = new MySqlCommand();
                                                cmd.Connection = Handler.conexion;
                                                cmd.CommandText = "UPDATE  cliente_gerson SET vencido='1' WHERE rut like'" +rutempresa  + "'";
                                                cmd.Parameters.AddWithValue("@rut_empresa", rutempresa);
                                                cmd.Prepare();
                                                cmd.ExecuteNonQuery();

                                                Handler.conexion.Close();
                                                break;
                                            }
                                            catch (Exception ee)
                                            { }
                                        }


                                    }








                                    using (StreamWriter sw = File.CreateText(@ruta + "\\temp\\listo_2.txt"))
                                    {


                                        foreach (string line in lines)
                                            if (line != "")
                                            {
                                                sw.WriteLine(line);
                                            }

                                    }
                                    //  string path = (@ruta+"\\temp\\aviso.txt");
                                    //   File.Delete(path);


                                }
                                

                        }
                        }
                    }catch(Exception e)
                    {

                    }
                }

            }
            catch (Exception e)
            {
              
                using (StreamWriter sw = File.CreateText(@ruta+"temp/listo_2.txt"))
                {

                    foreach (string line in lines)
                        if (line != "")
                        {
                            sw.WriteLine(line);
                        }
                    sw.WriteLine("ERROR EN EJECUCION...");



                }
                using (StreamWriter sw = File.CreateText(@ruta+"/temp/listo_2.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + e);

                }
               // string path = (@ruta+"\temp\aviso.txt");
                //File.Delete(path);

                Console.WriteLine("\nError " + e);
                Console.Write("Press <Enter> to exit... ");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

            }

        }



        public static void Bajar_caf(string rut)
        {
            sucursal.Clear();
            equipo.Clear();
            string p1 = @ruta+"\\" + rut + "//caf//";
            Console.WriteLine("Verificando consumo de folios");
            try
            {
                string ftpServerIP = "sh-pro12.hostgator.cl";
                string ftpUserName = "posfacto";
                string ftpPassword = "eD1[P4q4kZw+0M";
                string filename = "CAF.xml";
                FileInfo objFile = new FileInfo(filename);
                string pp = ftpServerIP + "/public_html/bowa/php/upload/" + rut + "/";
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/public_html/bowa/php/upload/" + rut + "/"));
                //                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/" + rut + "/" + objFile.Name));

                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                request.KeepAlive = false;
                request.UsePassive = false;
                FtpWebResponse response1 = (FtpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response1.GetResponseStream());

                string line = reader.ReadLine();
                equipo.Clear();
                sucursal.Clear();
                int numero = 0;
                while (line != null)
                {
                    if (line != null & line != ".." & line != "")
                    {
                        if (line != null & line != ".." & line != "." & line != "" & int.TryParse(line, out numero))
                        {
                            sucursal.Add(line);

                        }
                    }
                    line = reader.ReadLine();
                }
                int cuenta = sucursal.Count;
                for (int i = 0; i < cuenta; i++)
                {
                    Console.WriteLine("Leyendo CAF en" + rut + "/" + sucursal[i] + "/" + sucursal[i] + "/" + objFile.Name);

                    request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/public_html/bowa/php/upload/" + rut + "/" + sucursal[i] + "/" + sucursal[i] + "/" + objFile.Name));


                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                    System.IO.Stream responseStream = response.GetResponseStream();
                    FileStream file = File.Create(p1 + "CAF_" + sucursal[i] + "_" + sucursal[i] + ".xml");
                    byte[] buffer = new byte[32 * 1024];
                    int read;
                    Console.WriteLine("\nBuscando en el servidor");
                    while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        file.Write(buffer, 0, read);
                    }
                    file.Close();
                    responseStream.Close();
                    response.Close();

                }
            }
            catch (Exception ee)
            {

            }


            try
            {
                Console.WriteLine("\nVERIFICANDO CAF");

                DirectoryInfo di = new DirectoryInfo(@ruta+"\\" + rut + "\\caf\\");
                foreach (var fi in di.GetFiles("*.xml"))
                {
                    {
                        aut = XmlHandler.DeserializeRaw<Autorizacion>(p1 + fi);
                        aut.CAF.IdCAF = 1;
                        string tipo = string.Empty;
                        try
                        {
                            string filePath = @ruta+"\\" + rut + "\\caf\\" + string.Format("{0}_{1}_{2}.dat", (int)aut.CAF.Datos.TipoDTE, aut.CAF.Datos.RangoAutorizado.Desde.ToString(), aut.CAF.Datos.RangoAutorizado.Hasta.ToString());
                            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                var xml = File.ReadAllBytes(p1 + fi);
                                fs.Write(xml, 0, xml.Length);
                                fs.Flush();
                                fs.Close();
                            }
                            Console.WriteLine("\nCAF Verificado");
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine("\nError " + ex);
                            using (StreamWriter sw = File.CreateText(@ruta+"\temp\\listo_2.txt"))
                            {

                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex);
                using (StreamWriter sw = File.CreateText(@ruta+"\temp\\listo_2.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
            }


        }
     
        public static void Bajar_caf2(string rut)
        {
            sucursal.Clear();
            equipo.Clear();
            string p1 = @ruta + "\\" + rut + "\\caf\\";
            Console.WriteLine("Verificando consumo de folios");
            try
            {
                string ftpServerIP = "162.215.213.231";
                string ftpUserName = "bowa@duncanmotors.cl";
                string ftpPassword = "]UnZ$MqO8?@H";
                string filename = "CAF.xml";
                FileInfo objFile = new FileInfo(filename);
                string pp = ftpServerIP + "/public_html/bowa/php/upload/" + rut + "/";
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/" + rut + "/"));
                //                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/" + rut + "/" + objFile.Name));

                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                request.KeepAlive = false;
                request.UsePassive = false;
                FtpWebResponse response1 = (FtpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response1.GetResponseStream());

                string line = reader.ReadLine();
                equipo.Clear();
                sucursal.Clear();
                int numero = 0;
                while (line != null)
                {
                    if (line != null & line != ".." & line != "")
                    {
                        if (line != null & line != ".." & line != "." & line != "" & int.TryParse(line, out numero))
                        {
                            sucursal.Add(line);

                        }
                    }
                    line = reader.ReadLine();
                }
                int cuenta = sucursal.Count;
                for (int i = 0; i < cuenta; i++)
                {
                    Console.WriteLine("Leyendo CAF en" + rut + "/" + sucursal[i] + "/" + sucursal[i] + "/" + objFile.Name);

                    request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/" + rut + "/" + sucursal[i] + "/" + sucursal[i] + "/" + objFile.Name));


                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                    System.IO.Stream responseStream = response.GetResponseStream();
                    FileStream file = File.Create(p1 + "CAF_" + sucursal[i] + "_" + sucursal[i] + ".xml");
                    byte[] buffer = new byte[32 * 1024];
                    int read;
                    Console.WriteLine("\nBuscando en el servidor");
                    while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        file.Write(buffer, 0, read);
                    }
                    file.Close();
                    responseStream.Close();
                    response.Close();

                }
            }
            catch (Exception ee)
            {

            }


            try
            {
                Console.WriteLine("\nVERIFICANDO CAF");

                DirectoryInfo di = new DirectoryInfo(@ruta + "\\" + rut + "\\caf\\");
                foreach (var fi in di.GetFiles("*.xml"))
                {
                    {
                        aut = XmlHandler.DeserializeRaw<Autorizacion>(p1 + fi);
                        aut.CAF.IdCAF = 1;
                        string tipo = string.Empty;
                        try
                        {
                            string filePath = @ruta + "\\" + rut + "\\caf\\" + string.Format("{0}_{1}_{2}.dat", (int)aut.CAF.Datos.TipoDTE, aut.CAF.Datos.RangoAutorizado.Desde.ToString(), aut.CAF.Datos.RangoAutorizado.Hasta.ToString());
                            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                var xml = File.ReadAllBytes(p1 + fi);
                                fs.Write(xml, 0, xml.Length);
                                fs.Flush();
                                fs.Close();
                            }
                            Console.WriteLine("\nCAF Verificado");
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine("\nError " + ex);
                            using (StreamWriter sw = File.CreateText(@ruta + "\temp\\listo_2.txt"))
                            {

                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex);
                using (StreamWriter sw = File.CreateText(@ruta + "\temp\\listo_2.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
            }


        }

        public static void Bajar_caf3(string rut)
        {
           
            string p1 = @ruta + "\\" + rut + "\\caf\\";

            Handler.Initialize3();
            string clave="" ;
            string query = "SELECT * FROM caf_descargados WHERE rut ='" + rut + "' and descargado<>'"+"1"+"'";
            if (Handler.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, Handler.conexion);
                MySqlDataReader reader = cmd.ExecuteReader();
                int numero_caf = 1;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.Write("Nuevo caf encontrado en: " + rut+"/"+ reader.GetString(2) + "/"+ reader.GetString(3));
                        clave = reader.GetString(6);
                        string path = p1+"caf_"+numero_caf+".xml";
                        string caf = reader.GetString(5);
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(path))
                            {
                                writer.WriteLine(caf);
                            }
                            string connectionString = @"server=sh-pro12.hostgator.cl;userid=posfacto;password=eD1[P4q4kZw+0M;database=posfacto_bowa";
                            MySqlConnection connection = null;
                            connection = new MySqlConnection(connectionString);
                            connection.Open();
                            cmd = new MySqlCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = "UPDATE caf_descargados SET descargado='" + "1" + "' WHERE rut =@rut  and clave=@clave";
                            cmd.Prepare();

                            cmd.Parameters.AddWithValue("@rut", rut);
                            cmd.Parameters.AddWithValue("@clave", clave);
                            cmd.ExecuteNonQuery();
                            connection.Close();
                            numero_caf++;
                        }
                        catch (Exception ee) { }
                    }
                }
                reader.Close();
                

            }
            Handler.conexion.Close();
            try
            {
                Console.WriteLine("\nVERIFICANDO CAF");

                DirectoryInfo di = new DirectoryInfo(@ruta + "\\" + rut + "\\caf\\");
                foreach (var fi in di.GetFiles("*.xml"))
                {
                    {
                        aut = XmlHandler.DeserializeRaw<Autorizacion>(p1 + fi);
                        aut.CAF.IdCAF = 1;
                        string tipo = string.Empty;
                        try
                        {
                            string filePath = @ruta + "\\" + rut + "\\caf\\" + string.Format("{0}_{1}_{2}.dat", (int)aut.CAF.Datos.TipoDTE, aut.CAF.Datos.RangoAutorizado.Desde.ToString(), aut.CAF.Datos.RangoAutorizado.Hasta.ToString());
                            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                var xml = File.ReadAllBytes(p1 + fi);
                                fs.Write(xml, 0, xml.Length);
                                fs.Flush();
                                fs.Close();
                            }
                            Console.WriteLine("\nCAF Verificado");
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine("\nError " + ex);
                            using (StreamWriter sw = File.CreateText(@ruta + "\temp\\listo_2.txt"))
                            {

                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex);
                using (StreamWriter sw = File.CreateText(@ruta + "\temp\\listo_2.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                }
            }


        }
        public static void Bajar_caf1(string rut)
        {


            sucursal.Clear();
            equipo.Clear();
            string pp = "";
            string p1 = @ruta+"\\" + rut + "\\caf\\";

            //string p1 = @ruta+"\" + rutempresa + "//caf//";
            Console.WriteLine("Verificando consumo de folios");
            try
            {
                string ftpServerIP = "162.215.213.231";
                string ftpUserName = "bowa@duncanmotors.cl";
                string ftpPassword = "]UnZ$MqO8?@H";
                string filename = "CAF.xml";
                FileInfo objFile = new FileInfo(filename);

                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + pp));
                pp = ftpServerIP + "/php/upload/" + rut + "/" + "1" + "/" + "1" + "/" + objFile.Name;

                FtpWebRequest objFTPRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/" + rut + "/" + "1" + "/" + "1" + "/" + objFile.Name));
                objFTPRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                // By default KeepAlive is true, where the control connection is 
                // not closed after a command is executed.
                objFTPRequest.KeepAlive = false;

                // Set the data transfer type.
                objFTPRequest.UseBinary = true;

                // Set content length
                objFTPRequest.ContentLength = objFile.Length;

                // Set request method
                objFTPRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                //                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/" + rut + "/" + objFile.Name));

                objFTPRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                FtpWebResponse response = (FtpWebResponse)objFTPRequest.GetResponse();

                System.IO.Stream responseStream = response.GetResponseStream();
                FileStream file = File.Create(p1 + "CAF_" + "1" + "_" + "1" + ".xml");

                byte[] buffer = new byte[32 * 1024];
                int read;
                while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    file.Write(buffer, 0, read);
                }
                file.Close();
                responseStream.Close();
                response.Close();



            }
            catch (Exception ee)
            {
                Console.WriteLine("\nerror" + ee + "pp " + pp);
                //    Console.Write("Press <Enter> to exit... ");
                //    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

            }
            try
            {
                string ftpServerIP = "162.215.213.231";
                string ftpUserName = "bowa@duncanmotors.cl";
                string ftpPassword = "]UnZ$MqO8?@H";
                string filename = "CAF.xml";
                FileInfo objFile = new FileInfo(filename);

                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + pp));
                pp = ftpServerIP + "/php/upload/" + rut + "/" + "2" + "/" + "2" + "/" + objFile.Name;

                FtpWebRequest objFTPRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/" + rut + "/" + "2" + "/" + "2" + "/" + objFile.Name));
                objFTPRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                objFTPRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                using (FtpWebResponse response1 = (FtpWebResponse)objFTPRequest.GetResponse())
                {
                    objFTPRequest.KeepAlive = false;

                    // Set the data transfer type.
                    objFTPRequest.UseBinary = true;

                    // Set content length
                    objFTPRequest.ContentLength = objFile.Length;

                    // Set request method
                    objFTPRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                    //                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/" + rut + "/" + objFile.Name));

                    objFTPRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                    FtpWebResponse response = (FtpWebResponse)objFTPRequest.GetResponse();

                    System.IO.Stream responseStream = response.GetResponseStream();
                    FileStream file = File.Create(p1 + "CAF_" + "2" + "_" + "2" + ".xml");

                    byte[] buffer = new byte[32 * 1024];
                    int read;
                    while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        file.Write(buffer, 0, read);
                    }
                    file.Close();
                    responseStream.Close();
                    response.Close();
                }




            }
            catch (Exception ee)
            {
                // Console.WriteLine("\nerror" + ee + "pp " + pp);

            }



            try
            {
                string ftpServerIP = "162.215.213.231";
                string ftpUserName = "bowa@duncanmotors.cl";
                string ftpPassword = "]UnZ$MqO8?@H";
                string filename = "CAF.xml";
                FileInfo objFile = new FileInfo(filename);

                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + pp));
                pp = ftpServerIP + "/php/upload/" + rut + "/" + "3" + "/" + "3" + "/" + objFile.Name;

                FtpWebRequest objFTPRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/" + rut + "/" + "3" + "/" + "3" + "/" + objFile.Name));
                objFTPRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                objFTPRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                using (FtpWebResponse response1 = (FtpWebResponse)objFTPRequest.GetResponse())
                {
                    objFTPRequest.KeepAlive = false;

                    // Set the data transfer type.
                    objFTPRequest.UseBinary = true;

                    // Set content length
                    objFTPRequest.ContentLength = objFile.Length;

                    // Set request method
                    objFTPRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                    //                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/" + rut + "/" + objFile.Name));

                    objFTPRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                    FtpWebResponse response = (FtpWebResponse)objFTPRequest.GetResponse();

                    System.IO.Stream responseStream = response.GetResponseStream();
                    FileStream file = File.Create(p1 + "CAF_" + "3" + "_" + "3" + ".xml");

                    byte[] buffer = new byte[32 * 1024];
                    int read;
                    while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        file.Write(buffer, 0, read);
                    }
                    file.Close();
                    responseStream.Close();
                    response.Close();
                }




            }
            catch (Exception ee)
            {
                // Console.WriteLine("\nerror" + ee + "pp " + pp);

            }

            try
            {
                string ftpServerIP = "162.215.213.231";
                string ftpUserName = "bowa@duncanmotors.cl";
                string ftpPassword = "]UnZ$MqO8?@H";
                string filename = "CAF.xml";
                FileInfo objFile = new FileInfo(filename);

                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + pp));
                pp = ftpServerIP + "/php/upload/" + rut + "/" + "4" + "/" + "4" + "/" + objFile.Name;

                FtpWebRequest objFTPRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/" + rut + "/" + "4" + "/" + "4" + "/" + objFile.Name));
                objFTPRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                objFTPRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                using (FtpWebResponse response1 = (FtpWebResponse)objFTPRequest.GetResponse())
                {
                    objFTPRequest.KeepAlive = false;

                    // Set the data transfer type.
                    objFTPRequest.UseBinary = true;

                    // Set content length
                    objFTPRequest.ContentLength = objFile.Length;

                    // Set request method
                    objFTPRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                    //                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/" + rut + "/" + objFile.Name));

                    objFTPRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                    FtpWebResponse response = (FtpWebResponse)objFTPRequest.GetResponse();

                    System.IO.Stream responseStream = response.GetResponseStream();
                    FileStream file = File.Create(p1 + "CAF_" + "4" + "_" + "4" + ".xml");

                    byte[] buffer = new byte[32 * 1024];
                    int read;
                    while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        file.Write(buffer, 0, read);
                    }
                    file.Close();
                    responseStream.Close();
                    response.Close();
                }




            }
            catch (Exception ee)
            {
                // Console.WriteLine("\nerror" + ee + "pp " + pp);

            }




            try
            {
                Console.WriteLine("\nVERIFICANDO CAF");


                DirectoryInfo di = new DirectoryInfo(@ruta+"\\" + rut + "//caf//");
                foreach (var fi in di.GetFiles("*.xml"))
                {
                    {
                        aut = XmlHandler.DeserializeRaw<Autorizacion>(p1 + fi);
                        aut.CAF.IdCAF = 1;
                        string tipo = string.Empty;
                        try
                        {
                            string filePath = @ruta+"\\" + rut + "\\caf\\" + string.Format("{0}_{1}_{2}.dat", (int)aut.CAF.Datos.TipoDTE, aut.CAF.Datos.RangoAutorizado.Desde.ToString(), aut.CAF.Datos.RangoAutorizado.Hasta.ToString());
                            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                var xml = File.ReadAllBytes(p1 + fi);
                                fs.Write(xml, 0, xml.Length);
                                fs.Flush();
                                fs.Close();
                            }
                            Console.WriteLine("\nCAF Verificado");
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine("\nError " + ex);
                            using (StreamWriter sw = File.CreateText(@ruta+"\temp\\listo_2.txt"))
                            {

                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);
                                // Console.Write("Press <Enter> to exit... ");
                                // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex);
                using (StreamWriter sw = File.CreateText(@ruta+"\temp\\listo_2.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);
                    //    Console.Write("Press <Enter> to exit... ");
                    //   while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                }
            }


        }

        public static void error1(string error)
        {
            string dia = DateTime.Now.ToString("dd/MM/yyyy");
            string hora = DateTime.Now.ToString("hh:mm:ss");
            string docPath = @ruta+"\\temp\\";
            TextWriter tw = new StreamWriter(docPath + "log_error_boletas_totoal_cero.txt", true);
            tw.WriteLine(dia + "_" + hora + ": " + error); tw.Close();
            tw.Close();
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
                string connectionString = @"server=localhost;userid=root1;password=12345678;database=bowa";
               // string connectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";
                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    string dis = "srx10k";
                    cmd.CommandText = "SELECT distribuidor FROM certificado WHERE rut_empresa= '" + rut + "'";
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        dis = (reader.GetString(0));
                    }
                    if (dis == "0")
                    {
                        dis = "srx10k";
                    }
                    reader.Close();

                    cmd.CommandText = "Insert into fechas_activas (rut, fecha_inicial, fecha_final, sucursal, equipo, clave_fecha, distribuidor, observaciones) VALUES " +
    "(@rut, @fecha_inicial, @fecha_final, @sucursal,@equipo, @clave_fecha, @distribuidor,@observaciones) ON DUPLICATE KEY UPDATE observaciones=@observaciones, fecha_inicial=@fecha_inicial," +
    "fecha_final= @fecha_final, distribuidor=@distribuidor";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@rut", rut);
                    cmd.Parameters.AddWithValue("@fecha_inicial", fecha_inicial);
                    cmd.Parameters.AddWithValue("@fecha_final", fecha_final);
                    cmd.Parameters.AddWithValue("@sucursal", sucursal);
                    cmd.Parameters.AddWithValue("@equipo", equipo);
                    cmd.Parameters.AddWithValue("@clave_fecha", rut + "_" + sucursal + "_" + equipo);
                    cmd.Parameters.AddWithValue("@distribuidor", dis);
                    cmd.Parameters.AddWithValue("@observaciones", Handler.vencido);
                    cmd.ExecuteNonQuery();
                    if (connection != null)
                        connection.Close();
                  //  Thread.Sleep(1000);
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
                using (StreamWriter sw = File.CreateText(@ruta + "bowa\\temp\\listo_2.txt"))
                {

                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                }
            }
        }
    }
}