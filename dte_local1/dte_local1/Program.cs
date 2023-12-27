using ItaSystem.DTE.Engine.Documento;
using ItaSystem.DTE.Engine.Envio;
using ItaSystem.DTE.Engine.XML;
using dte_local1.clases;
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
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static ITA_CHILE.Enum.Ambiente;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;
using System.Diagnostics;

namespace dte_local1
{
    class Program
    {
        static string track = "";
        static string fecha_rec = "";
        static string estado = "";
        static string informados = "";
        static string aceptados = "";
        static string rechazados = "";
        static string reparos = "";
        static string tipo = "";
        public static string ruta = @"C:/Users/Administrador/Documents/bowa/"; 

        static readonly string[][] emptyArray = new string[0][];
        static List<string> AllFiles = new List<string>();
        static List<string> AllFiles1 = new List<string>();
        static List<string> AllFiles2 = new List<string>();
        static List<string> AllFiles3 = new List<string>();
        static public string docPath = "";
        static public string nombre_file = "";
        static public string fecha = "";
        static public string pathtoken = "";
        static public string envio;
        static public string enviox;
        private static Autorizacion aut;
        static Handler handler = new Handler();
        static public Configuracion configuracion = new Configuracion();
        static public List<string> rut_produccion = new List<string>();

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
            grabar_consulta(track, fecha_rec, tipo, informados, aceptados, rechazados, reparos);

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


        static void Main(string[] args)
        {
            Handler.Initialize2();
            /*    Process[] instancia = Process.GetProcessesByName("dte_local");
              if (instancia.Length == 1)
              {
                  Console.Write("Esperando cierre proceso dte_local.exe");
                  while (instancia.Length == 1)
                  {
                      instancia = Process.GetProcessesByName("dte_local");
                  }
                  Console.Write("Proceso dte_local.exe finalizado");
              }
              else
              {
              }
          */
            while (true)
            {
                Handler.guardar_tabla();
                Handler.HasRows();
                int cuenta_ruts = rut_produccion.Count();
                if (cuenta_ruts > 0)
                {
                    for (int pasar = 0; pasar < cuenta_ruts; pasar++)
                    {
                        rutempresa = rut_produccion[pasar];
                        char c = rutempresa[0];
                      
                        if (c == '7' || c!='7')
                        {
                          
                            Console.WriteLine("Rut: " + rutempresa);
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
                        */
                                string rut_aviso = rutempresa;
                                using (StreamWriter sw = File.CreateText(@ruta+"\\temp\\aviso1_2.txt"))
                                {



                                    sw.WriteLine(rut_aviso);


                                }

                                AllFiles.Clear();
                                AllFiles1.Clear();
                                AllFiles2.Clear();
                                AllFiles3.Clear();

                                string[] lines = new string[2];
                                string path = (@ruta+"\\temp\aviso1_2.txt");
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
                                    }
                                }



                                path = @ruta;
                                /* try
                                 {
                                     String[] filePaths = Directory.GetFiles(@ruta+);
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
                                    if (AllFiles[i] != "temp" && AllFiles[i] != "respaldo" && AllFiles[i] != "caf")
                                    {
                                        path = ruta+"\\" + AllFiles[i] + "\\";

                                        try
                                        {
                                            if (Directory.Exists(path))
                                            {

                                                string[] folderPaths = Directory.GetDirectories(path);
                                                foreach (string s in folderPaths)
                                                {
                                                    string t = s.Remove(0, s.LastIndexOf('\\') + 1);
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
                                            error1("" + ee);
                                            using (StreamWriter sw = File.CreateText(@ruta+"\\temp\\error_dte1.txt"))
                                            {

                                                sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                                            }
                                            //                  Console.WriteLine("No se encontraron nuevos xml a enviar");
                                        }
                                    }
                                }

                                cantidad = AllFiles2.Count();
                                for (int i = 0; i < cantidad; i++)
                                {
                                    string ver = AllFiles2[i];
                                }

                                cantidad = AllFiles.Count();
                                for (int i = 0; i < cantidad; i++)
                                {

                                    if (AllFiles[i] != "temp")
                                    {
                                        int cantidad2 = AllFiles2.Count();
                                        for (int i2 = 0; i2 < cantidad2; i2++)
                                        {
                                            if (AllFiles2[i] != "caf" && AllFiles2[i] != "rcof")
                                            {
                                                path = @ruta+ "\\"+AllFiles[i] + "\\" + AllFiles2[i2] + "\\";
                                                try
                                                {
                                                    if (Directory.Exists(path))
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
                                                }

                                                catch (Exception ee)
                                                {
                                                    error1("" + ee);
                                                    using (StreamWriter sw = File.CreateText(@ruta+"\\temp\\error_dte1.txt"))
                                                    {

                                                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                                                    }
                                                    //                        Console.WriteLine("No se encontraron nuevos xml a enviar");
                                                }
                                            }
                                        }
                                    }
                                }

                                cantidad = AllFiles3.Count();
                                for (int i = 0; i < cantidad; i++)
                                {
                                    string ver = AllFiles3[i];
                                }
                                cantidad = AllFiles.Count();
                                for (int i = 0; i < cantidad; i++)
                                {
                                    if (AllFiles[i] != "temp" && AllFiles[i] != "respaldo")
                                    {

                                        int cantidad2 = AllFiles2.Count();
                                        for (int i2 = 0; i2 < cantidad2; i2++)
                                        {
                                            if (AllFiles2[i2] != "caf" && AllFiles2[i2] != "rcof")
                                            {
                                                int cantidad3 = AllFiles3.Count();
                                                for (int i3 = 0; i3 < cantidad3; i3++)
                                                {

                                                    string hhh1 = AllFiles[i];
                                                    string hhh2 = AllFiles2[i2];
                                                    string hhh = AllFiles3[i3];

                                                    path = @ruta+"\\"+ AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\";
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
                                                        error1("" + ee);
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
                                }




                                enviar_sobre();
                                Handler.grabar_estatus(rutempresa, 0);

                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine("error " + ee);
                                // Console.Write("Press <Enter> to exit... ");
                                // while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                            }
                        }
                    }
                }
            }
        }





        public static string rutempresa = "";
        public static void enviar_sobre()
        {
            try
            {
                int finalizar = 0;
                int y = 0;
                int encontro_xml_enviar = 0;
                int cantidad = AllFiles.Count();
                for (int i = 0; i < cantidad; i++)
                {
                    if (finalizar == 1)
                    {
                        break;
                        finalizar = 0;
                    }
                    if (AllFiles[i] != "temp" && AllFiles[i] != "respaldo")
                    {

                        int cantidad2 = AllFiles2.Count();
                        for (int i2 = 0; i2 < cantidad2; i2++)
                        {

                            if (AllFiles2[i2] != "caf" && AllFiles2[i2] != "rcof")
                            {

                                int cantidad3 = AllFiles3.Count();
                                for (int i3 = 0; i3 < cantidad3; i3++)
                                {

                                    int cantidad1 = AllFiles1.Count();
                                    for (int i1 = 0; i1 < cantidad1; i1++)
                                    {
                                        rutempresa = AllFiles[i];
                                        if (Directory.Exists(@ruta + "\\" + AllFiles[i] + "\\" +
                                            AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\"))
                                        {
                                            string[] folderPaths = Directory.GetDirectories(@ruta + "\\" + AllFiles[i] + "\\" +
                                                AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\");

                                            string f = AllFiles1[i1];
                                            string[] lines = new string[100000];


                                            if (AllFiles1.Count() > 0)
                                            {
                                                DirectoryInfo di = new DirectoryInfo(@ruta + "\\" + AllFiles[i] + "\\"
                                                    + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_firmado\\");

                                                string[] pathFilesss = System.IO.Directory.GetFiles(@ruta + "\\" + AllFiles[i] + "\\"
                                                    + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_firmado\\", "*.xml");

                                               // Console.WriteLine("Buscando xml Directorio: " + @ruta + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_firmado\\");

                                                if (pathFilesss.Length == 0)
                                                {

                                                }
                                                else
                                                {
                                                    Console.WriteLine("Nuevos xml firmados sin enviar en fecha: " + AllFiles1[i1] + " sucursal[" + AllFiles2[i2] + "] equipo[" + AllFiles3[i3] + "]");
                                                    if (AllFiles1[i1] != "temp")
                                                    {
                                                        if (AllFiles2[i2] != "caf" && AllFiles2[i2] != "rcof")
                                                        {
                                                            try
                                                            {
                                                                if (!File.Exists(@ruta + "\\" + AllFiles[i] + "\\"
                               + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\envio.txt"))
                                                                {

                                                                    System.IO.StreamWriter ficheroTemporal =
                                                                new System.IO.StreamWriter(@ruta + "\\" + AllFiles[i] + "\\"
                               + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\envio.txt");
                                                                    ficheroTemporal.WriteLine("1");
                                                                    ficheroTemporal.Close();
                                                                    enviox = "1";
                                                                }
                                                                else
                                                                {
                                                                    string line = "";
                                                                    System.IO.StreamReader file =
                                                                       new System.IO.StreamReader(@ruta + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\"
                                                                                + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\envio.txt");

                                                                    while ((line = file.ReadLine()) != null)
                                                                    {

                                                                        enviox = line;
                                                                    }
                                                                    file.Close();
                                                                    int u = Convert.ToInt32(enviox) + 1;
                                                                    enviox = "" + u;
                                                                    System.IO.StreamWriter ficheroTemporal =
                                                                new System.IO.StreamWriter(@ruta + "\\" + AllFiles[i] + "\\"
                               + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\envio.txt");
                                                                    ficheroTemporal.WriteLine(enviox);
                                                                    ficheroTemporal.Close();

                                                                }
                                                                //si no existe la carpeta temporal la creamos

                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                //  error1("" + ex);
                                                                using (StreamWriter sw = File.CreateText(@ruta + "\\temp\\error_dte1.txt"))
                                                                {

                                                                    sw.WriteLine(DateTime.Now.ToString() + " Error: " + ex);

                                                                }
                                                                Console.Write("Press <Enter> to exit... ");
                                                                //  while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                                                            }
                                                        }
                                                        fecha = AllFiles1[i1];
                                                        string[] pathFiles = System.IO.Directory.GetFiles(@ruta + "\\" + AllFiles[i] + "\\"
                               + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_firmado\\", "*.xml");
                                                        encontro_xml_enviar = 0;
                                                        List<int> numbers = new List<int>();
                                                        List<DTE> dtes = new List<DTE>();
                                                        List<string> xmlDtes = new List<string>();
                                                        int termino = 0;
                                                        int cuentas = 0;
                                                        DateTime fecha_busqueda = DateTime.Parse("01-05-2023");
                                                        DateTime fecha_busqueda1 = (DateTime.Parse(AllFiles1[i1]));
                                                        while (termino == 0 && fecha_busqueda <= fecha_busqueda1)
                                                        {
                                                           
                                                                foreach (string pathFile1 in pathFiles)
                                                            {
                                                                if (cuentas <= 300)
                                                                {
                                                                    cuentas = cuentas + 1;
                                                                    string xml = File.ReadAllText(pathFile1, Encoding.GetEncoding("ISO-8859-1"));
                                                                    var dte = ItaSystem.DTE.Engine.XML.XmlHandler.DeserializeFromString<ItaSystem.DTE.Engine.Documento.DTE>(xml);
                                                                    dtes.Add(dte);
                                                                    xmlDtes.Add(xml);
                                                                    string rutax = ruta + AllFiles[i] + "/" + AllFiles2[i2] + "/" + AllFiles3[i3] + "/" + AllFiles1[i1] + "/xml_empaquetado/" + Path.GetFileName(pathFile1);

                                                                    string rutax1 = ruta + AllFiles[i] + "/" + AllFiles2[i2] + "/" + AllFiles3[i3] + "/" + AllFiles1[i1] + "/sobre_enviado/";
                                                                    System.IO.File.Copy(pathFile1, rutax, true);
                                                                    File.Delete(pathFile1);

                                                                    string docPath = @ruta + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\sobre_enviado\\";


                                                                    lines[y] = DateTime.Now.ToString() + " " + Path.GetFileName(pathFile1) + " " + "incluida_envio " + "EnvioBOLETA_" + AllFiles1[i1] + "_" + enviox;
                                                                    TextWriter tw = new StreamWriter(rutax1 + "log.txt", true);
                                                                    tw.WriteLine(lines[y]);
                                                                    tw.Close();

                                                                    encontro_xml_enviar = 1;// enviar de una en una



                                                                }
                                                                else {
                                                                    termino = 1;
                                                                }
                                                                termino = 1;
                                                            }
                                                            
                                                        }
                                                        /*   XmlDocument xmlDoc = new XmlDocument();
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




                                                         grabar_boleta(pathFile1, AllFiles[i], tipo, AllFiles2[i2], AllFiles3[i3], numero_folio, fecha_emision, neto, exento, iva, total, pathFile1);
                                                        */

                                                        Console.WriteLine("Generando secuencia de envio : " + enviox);

                                                        /*           Handler.boleta_inicial = Convert.ToString(numbers.Min());
                                                        Handler.boletas_final = Convert.ToString(numbers.Max());
                                                        if (Handler.boleta_inicial == "0")
                                                        {
                                                            Handler.boleta_inicial = Handler.boletas_final;

                                                        }*/

                                                        var EnvioSII = handler.GenerarEnvioBoletaDTEToSII(dtes, xmlDtes);
                                                        Handler.leer(rutempresa);
                                                        configuracion.LeerArchivo(rutempresa);


                                                    
                                                            var filePath = EnvioSII.Firmar(configuracion.Certificado.Nombre, @ruta + "\\"+AllFiles[i] + "\\"
                               + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\sobre_preparado\\");
                                                             // Write the string array to a new file named "WriteLines.txt".
                                                        
                                                            long trackId = 0;

                                                            if (Handler.produccion == true && encontro_xml_enviar == 1)
                                                            {
                                                                docPath = @ruta + "\\" + AllFiles[i] + "\\"
                                    + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\sobre_enviado\\";
                                                                DirectoryInfo dii = new DirectoryInfo(ruta+"\\" + AllFiles[i] + "\\"
                       + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\sobre_preparado\\");
                                                               
                                                                trackId = handler.EnviarEnvioDTEToSII(dii + "EnvioBoleta_"+Handler.sobre + ".xml", AmbienteEnum.Produccion, @"C:\Users\one\Documents\bowa\" + AllFiles[i] + "\\"
                                                    + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\sobre_enviado\\", true);
                                                                if (Convert.ToInt64(trackId.ToString()) <= 0)
                                                                {
                                                                    lines[y] = Handler.sobre + ".xml" + " error_envio " + trackId.ToString();
                                                                    Console.WriteLine(lines[y]);
                                                                    y++;
                                                                    
                  SaveToMysql(trackId, AllFiles2[i2], AllFiles2[i2], @ruta + "\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\sobre_enviado\\" + Handler.sobre + ".xml");


                                                                    //DeleteDirectory(pathx);

                                                                    y++;


                                                                    
                                                                }
                                                                else
                                                                {
                                                                    
                                                                    Console.WriteLine("ENVIO exitoso del dia: " + fecha + " rut:" + rutempresa +  " Tracking:  " + trackId.ToString() + " fecha:" + DateTime.Now.ToString());

                                                                    // leer_estado_sobre(trackId, fi.ToString());
                                                                    docPath = @ruta + "\\" + AllFiles[i] + "\\"
                                        + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\sobre_enviado\\";

                                                                // Write the string array to a new file named "WriteLines.txt".
                                                                StreamWriter tw = new StreamWriter(docPath + "log.txt", true);
                                                                    tw.WriteLine("ENVIO exitoso del dia: " + fecha + " rut:" + rutempresa + " " + " Tracking:  " + trackId.ToString() + " fecha:" + DateTime.Now.ToString());
                                                                    tw.Close();



                                                                    string verifica_rcof = @ruta + "\\" + AllFiles[i] + "\\rcof\\" + AllFiles1[i1] + "\\";
                                                                    if (Directory.Exists(verifica_rcof))
                                                                    {
                                                                        string[] pathFilesss1 = System.IO.Directory.GetFiles(verifica_rcof, "*.xml");
                                                                        if (pathFilesss1.Length > 0)
                                                                        {
                                                                            File.Delete(pathFilesss1[0]);
                                                                        }
                                                                        if (File.Exists(verifica_rcof + "log_" + AllFiles[i] + ".txt"))
                                                                        {
                                                                            File.Delete(verifica_rcof + "log_" + AllFiles[i] + ".txt");
                                                                        }
                                                                        if (File.Exists(verifica_rcof + "rcof_enviado.txt"))
                                                                        {
                                                                            File.Delete(verifica_rcof + "rcof_enviado.txt");
                                                                        }
                                                                        Console.WriteLine("Borrando rcof de fecha " + AllFiles[i] + "nuevos xml encontrados");
                                                                    }

                                                                    string verifica_rcof_sucu = @ruta + "\\" + AllFiles[i] + "\\rcof\\" + AllFiles2[i2] + "\\" + AllFiles2[i2] + "\\" + AllFiles1[i1] + "\\";
                                                                 /*   if (Directory.Exists(verifica_rcof))
                                                                    {
                                                                        string[] pathFilesss1 = System.IO.Directory.GetFiles(verifica_rcof_sucu, "*.xml");
                                                                        if (pathFilesss1.Length > 0)
                                                                        {
                                                                            File.Delete(pathFilesss1[0]);
                                                                        }
                                                                        if (File.Exists(verifica_rcof + "log_" + AllFiles[i] + ".txt"))
                                                                        {
                                                                            File.Delete(verifica_rcof + "log_" + AllFiles[i] + ".txt");
                                                                        }

                                                                        Console.WriteLine("Borrando rcof de fecha " + AllFiles[i] + "nuevos xml encontrados sucu");
                                                                     }*/

                                                                SaveToMysql(trackId, AllFiles3[i3], AllFiles3[i3],  @ruta + "\\" + AllFiles[i] + "\\"+ AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\sobre_enviado\\" + Handler.sobre + ".xml");
                                                                dtes.Clear();

                                                                }


                                                            }

                                                         encontro_xml_enviar = 0;
                                                         }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                   
                                }
                            }

                        }

                        Console.WriteLine("Busqueda de Sobres por enviar Finalizada");
                    }
              }
            catch (Exception ee)
            {
                error1("" + ee);
                Console.WriteLine("Error " + ee);
                //  Console.Write("Press <Enter> to exit... ");
                //  while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

            }

            using (StreamWriter sw = File.CreateText(@ruta+"\\temp\\listo1_2_1.txt"))
            {
                string[] lines = new string[1];
                foreach (string line in lines)
                    if (line != "")
                    {
                        sw.WriteLine(line);
                    }

            }
            String path = (@ruta+"\\temp\\aviso1_2.txt");
            File.Delete(path);
            //  Console.Write("Press <Enter> to exit... ");
            //    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            Thread.Sleep(500);
            //Environment.Exit(0);
        }


        public static void DeleteDirectory(string ruta)
          {
            Console.Write("\nBuscando para borrar " + ruta);
          
            if (Directory.Exists(ruta))
            {
                if (Directory.GetFiles(ruta).Count() > 0) { }
                else
                {
                    if (Directory.Exists(ruta))
                    {
                        try
                        {

                            Directory.Delete(ruta, recursive: true);    //throws if directory doesn't exist. 
                        }
                        catch
                        {
                            Thread.Sleep(2000);  //wait 2 seconds 
                            Directory.Delete(ruta, recursive: true);
                        }

                    }
                }
            }
        }

        public static void DeleteDirectory_1(string ruta)
        {
            Console.Write("\nBuscando para borrar " + ruta);
           
            if (Directory.Exists(ruta))
            {
                if (Directory.GetFiles(ruta).Count() > 0) { }
                else
                {
                    if (Directory.Exists(ruta))
                    {
                        try
                        {
                            string[] pathFilesss = System.IO.Directory.GetFiles(ruta, "*.xml");


                            foreach (string pathFile1 in pathFilesss)
                            {
                                File.Delete(pathFile1);
                            }

                             pathFilesss = System.IO.Directory.GetFiles(ruta, "*.txt");


                            foreach (string pathFile1 in pathFilesss)
                            {
                                if (!Path.GetFileName(pathFile1).Contains("envio1"))
                                {
                                    File.Delete(pathFile1);
                                }
                            }

                        }
                        catch
                        {
                            Thread.Sleep(2000);  //wait 2 seconds 
                            Directory.Delete(ruta, recursive: true);
                        }

                    }
                }
            }
        }

        public static void leer_estado_sobre(long trackid, string archivo)
        {
            try
            {
                Handler.Initialize2();
                Handler.leer(Program.rutempresa);
                configuracion.LeerArchivo(rutempresa);
                var responseEstadoDTE = handler.ConsultarEstadoEnvioBoleta(AmbienteEnum.Produccion, trackid);
                string respuesta = responseEstadoDTE.Estado;
                respuesta = responseEstadoDTE.FechaRecepcion;
                string respuesta1 = JsonConvert.SerializeObject(responseEstadoDTE, Formatting.Indented);
                string[] rr = respuesta1.Split(',');
                string variable = "";
                for (int r = 0; r < rr.Length - 1; r++)
                {
                    variable = rr[r];
                    if (variable.Contains("TrackId"))
                    {
                        string[] ee = rr[r].Split(':');
                        Handler.trackid = ee[1].Replace('"', ' ').Trim();

                    }
                    if (variable.Contains("Estado"))
                    {
                        string[] ee = rr[r].Split(':');
                        Handler.Estado = ee[1].Replace('"', ' ');
                        Handler.Estado = ee[1].Replace('/', ' ').Trim();

                    }
                    if (variable.Contains("Informados"))
                    {
                        string[] ee = rr[r].Split(':');
                        Handler.Informados = ee[1].Replace('"', ' ').Trim();
                        ;

                    }
                    if (variable.Contains("Aceptados"))
                    {
                        string[] ee = rr[r].Split(':');
                        Handler.aceptados = ee[1].Replace('"', ' ').Trim(); ;

                    }
                    if (variable.Contains("Reparos"))
                    {
                        string[] ee = rr[r].Split(':');
                        Handler.reparos = ee[1].Replace('"', ' ').Trim(); ;
                    }
                    if (variable.Contains("fecha_recepcion"))
                    {
                        string[] ee = rr[r].Split(':');
                        Handler.Fecha = ee[1].Replace('"', ' ').Trim(); ;
                        Handler.hora = ee[1].Replace('"', ' ').Trim(); ;
                    }

                }


                Handler.Cliente = Program.rutempresa;
                Handler.dte = archivo;
                Handler.Initialize2();

                string connectionString = @"server=localhost;userid=root;password=12345678;database=bowa";

                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                 /*   cmd.CommandText = "Insert into respuestasii (Cliente, dte, trackid, Estado, Glosa, NumAtencion, Fecha, hora, tipoDoc, Informados, aceptados, rechazados, reparos ) VALUES " +
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

                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into respuestasii (Cliente, dte, trackid, Estado, Glosa, NumAtencion, Fecha, hora, tipoDoc, Informados, aceptados, rechazados, reparos ) VALUES " +
    "(@Cliente, @dte, @trackid, @Estado, @Glosa, @NumAtencion, @Fecha, @hora, @tipoDoc, @Informados, @aceptados, @rechazados, @reparos)";
                    cmd.Prepare();
                 */

                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
            }

            catch (Exception ex)
            {
                error1("" + ex);
                //   Console.WriteLine("Ha ocurrido un error:" + ex);
                //  error("Ha ocurrido un error:" + ex);
            }
        }


            
        
    



public static void grabar_tracking(long trackid,string sucursal, string equipo, string archivo)
        {
            try
            {
                Handler.Initialize2();
                Handler.leer(Program.rutempresa);
                configuracion.LeerArchivo(Program.rutempresa);

                Handler.Cliente = Program.rutempresa;
                Handler.dte = archivo;


                string[] ee = Handler.fecha_envio.Split(':');
                Handler.Fecha = ee[0].Replace('"', ' ').Trim(); ;
                Handler.hora = ee[1].Replace('"', ' ').Trim(); ;
              
                string connectionString = @"server=localhost;userid=root;password=12345678;database=bowa";

                MySqlConnection connection = null;
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into envioboletas (Cliente, dte, trackid, NumeroBoletas, BoletaInicial,BoletaFinal,fecha_emision,sucursal,equipo) VALUES " +
    "(@Cliente, @dte, @trackid, @NumeroBoletas, @BoletaInicial, @BoletaFinal,@Fecha_emision,@sucursal,@equipo)";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@Cliente", Handler.Cliente);
                    cmd.Parameters.AddWithValue("@dte", Handler.dte);
                    cmd.Parameters.AddWithValue("@trackid", trackid);
                    cmd.Parameters.AddWithValue("@NumeroBoletas", Handler.boletas);
                    cmd.Parameters.AddWithValue("@BoletaInicial", Handler.boleta_inicial);
                    cmd.Parameters.AddWithValue("@BoletaFinal", Handler.boletas_final);
                    cmd.Parameters.AddWithValue("@Fecha_emision", Handler.Fecha);
                    cmd.Parameters.AddWithValue("@sucursal", sucursal);
                    cmd.Parameters.AddWithValue("@equipo", equipo);
                    cmd.ExecuteNonQuery(); 
                    
                    /* connection = null;
                    connectionString = @"server=sh-pro12.hostgator.cl;userid=posfacto;password=eD1[P4q4kZw+0M;database=posfacto_bowa";
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into envioboletas (Cliente, dte, trackid, NumeroBoletas, BoletaInicial,BoletaFinal,fecha_emision, sucursal, equipo) VALUES " +
    "(@Cliente, @dte, @trackid, @NumeroBoletas, @BoletaInicial, @BoletaFinal,@Fecha_emision, @sucursal, @equipo)";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@Cliente", Handler.Cliente);
                    cmd.Parameters.AddWithValue("@dte", Handler.dte);
                    cmd.Parameters.AddWithValue("@trackid", trackid);
                    cmd.Parameters.AddWithValue("@NumeroBoletas", Handler.boletas);
                    cmd.Parameters.AddWithValue("@BoletaInicial", Handler.boleta_inicial);
                    cmd.Parameters.AddWithValue("@BoletaFinal", Handler.boletas_final);
                    cmd.Parameters.AddWithValue("@Fecha_emision", Handler.Fecha);
                    cmd.Parameters.AddWithValue("@sucursal", sucursal);
                    cmd.Parameters.AddWithValue("@equipo",equipo);
               */
               cmd.ExecuteNonQuery();
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
                error1("" + ex);
                //   Console.WriteLine("Ha ocurrido un error:" + ex);
                //  error("Ha ocurrido un error:" + ex);
            }
        }




        public static void grabar_consulta(string track, string fecha_rec, string tipo, string informados, string acpetados, string rechazados, string reparos)
        {
            try
            {
                Handler.Initialize2();
                Handler.leer(Program.rutempresa);
                configuracion.LeerArchivo(Program.rutempresa);

                Handler.Cliente = Program.rutempresa;

                
                string connectionString = @"server=localhost;userid=root;password=12345678;database=bowa";

                MySqlConnection connection = null;
                try
                {
                /*    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into respuestasii (Trackid, Estado, Fecha, tipoDoc, Informados," +
                        "aceptados, rechazados, reparos,cliente) VALUES " + "(@Trackid, @Estado, @Fecha, @tipoDoc," +
                        "@Informados, @aceptados,@rechazados,@reparos,@cliente)";
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
                    cmd.ExecuteNonQuery();
                */}
                finally
                {
                    if (connection != null)
                        connection.Close();
                }






            }
            catch (Exception ex)
            {
                error1("" + ex);
                //   Console.WriteLine("Ha ocurrido un error:" + ex);
                //  error("Ha ocurrido un error:" + ex);
            }
                
            
        }

        public static void grabar_boleta(string ruta,string rut,string tipo,string sucu, string equipo, string folio, string fecha, string neto, string exento, string iva, string total, string documento)
        {
            try
            {
                Handler.Initialize2();
                Handler.leer(Program.rutempresa);
                configuracion.LeerArchivo(Program.rutempresa);
                string boleta = File.ReadAllText(ruta, Encoding.GetEncoding("ISO-8859-1"));

                // string aa= @"C:/Users/one/Documents/bowa/" + rut + "/pdf/"+folio+".pdf";
                //   byte[] pdf_boleta = File.ReadAllBytes(aa);
                //FileInfo info = new FileInfo(aa);

                Handler.Cliente = Program.rutempresa;
                byte[] rawData = File.ReadAllBytes(documento);
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
                    cmd.Parameters.AddWithValue("@boleta", boleta);
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

                    cmd.CommandText = "Insert into boleta_archivo(clave_boleta,boleta) VALUES " + "(@claveboleta,@boleta) ON DUPLICATE KEY UPDATE boleta=@boleta";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@boleta", boleta);
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
                error1("" + ex);
                   Console.WriteLine("Ha ocurrido un error:" + ex);
                  
            }
        }

        public static void SaveToMysql(long trackid, string sucu, string equi,string ruta)
        {
            FileInfo info = new FileInfo(ruta);

            Handler.Initialize2();

            Handler.leer(Program.rutempresa);
            configuracion.LeerArchivo(Program.rutempresa);

            Handler.Cliente = Program.rutempresa;
            Handler.dte = ruta;

           
            string connectionString = @"server=localhost;userid=root;password=12345678;database=bowa";

            MySqlConnection connection = null;
            try
            {

                connection = new MySqlConnection(connectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "Insert into envioboletas (Cliente, dte, trackid, NumeroBoletas, BoletaInicial,BoletaFinal,fecha_emision,fecha_envio,sucursal, equipo) VALUES " +
"(@Cliente, @dte, @trackid, @NumeroBoletas, @BoletaInicial, @BoletaFinal,@Fecha_emision,@fecha_envio,@sucursal, @equipo)";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@Cliente", Handler.Cliente);
                cmd.Parameters.AddWithValue("@dte", Handler.dte);
                cmd.Parameters.AddWithValue("@trackid", trackid);
                cmd.Parameters.AddWithValue("@NumeroBoletas", Handler.boletas);
                cmd.Parameters.AddWithValue("@BoletaInicial", Handler.boleta_inicial);
                cmd.Parameters.AddWithValue("@BoletaFinal", Handler.boletas_final);
                cmd.Parameters.AddWithValue("@Fecha_emision",fecha);
                cmd.Parameters.AddWithValue("@fecha_envio", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@sucursal", sucu);
                cmd.Parameters.AddWithValue("@equipo", equi);

                cmd.ExecuteNonQuery();
                connection.Close();

            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }

        }



                
        public static void error(string path,string error)
        {
            string docPath = path;
            TextWriter tw = new StreamWriter(docPath + "log.txt", true);
            tw.WriteLine(error); tw.Close();
            tw.Close();
        }
        public static void error1( string error)
        {
            string dia = DateTime.Now.ToString("dd/MM/yyyy");
            string hora = DateTime.Now.ToString("hh:mm:ss");
            string docPath = @ruta+"\\temp\\";
            TextWriter tw = new StreamWriter(docPath + "log_error_dtelocal1.txt", true);
            tw.WriteLine(dia+"_"+hora+": "+error); tw.Close();
            tw.Close();
        }
    }
}