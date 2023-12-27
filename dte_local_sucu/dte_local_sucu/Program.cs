using ItaSystem.DTE.Engine.Documento;
using ItaSystem.DTE.Engine.Envio;
using ItaSystem.DTE.Engine.XML;
using dte_local_sucu.clases;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using static ITA_CHILE.Enum.Ambiente;

namespace dte_local_sucu
{
        class Program

        {
        static public string sucursal = "0";
        static public string equipo = "0";
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
            static Handler handler = new Handler();
            static public List<string> fecha_vacio = new List<string>();
            static public string fecha_vacio_actual = "";
            static public Configuracion configuracion = new Configuracion();
            static void Main(string[] args)
            {
                AllFiles.Clear();
                AllFiles1.Clear();
                AllFiles2.Clear();
                AllFiles3.Clear();
                fecha_vacio.Clear();
                rut = "0";
                string[] lines = new string[2];
                string path = (@"C:\Users\Administrador\Documents\bowa\temp\rut_rcof_sucu.txt");
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
                //   grabar_tracking(0, @"C:\\Users\\usuario\\Documents\\bowa\\15835677-5\\2021-1-5\\rcof\\ConsumoFolios_15835677-5_05012021.xml");


                // leer_estado_rcof(5196993742, "");
                /* path = @"C:\\Users\\usuario\Documents\\bowa\\";
                try
                {
                    String[] filePaths = Directory.GetFiles(@"C:\\Users\\usuario\Documents\\bowa\\");
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
                        path = @"C:\\Users\\Administrador\Documents\\bowa\\" + AllFiles[i] + "\\";
                        try
                        {
                            string[] folderPaths = Directory.GetDirectories(path);
                            foreach (string s in folderPaths)
                            {
                                string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                                if (t != "rcof")
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
                            using (StreamWriter sw = File.CreateText(@"C:\Users\Administrador\Documents\bowa\temp\listo1.txt"))
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
                            if (AllFiles2[i] != "caf" && AllFiles2[i] != "rcof")
                            {
                                path = @"C:\\Users\\Administrador\Documents\\bowa\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\";
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
                                    using (StreamWriter sw = File.CreateText(@"C:\Users\Administrador\Documents\bowa\temp\listo1.txt"))
                                    {

                                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

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
                            if (AllFiles2[i2] != "caf" && AllFiles2[i2] != "rcof")
                            {
                                int cantidad3 = AllFiles3.Count();
                                for (int i3 = 0; i3 < cantidad3; i3++)
                                {

                                    string hhh1 = AllFiles[i];
                                    string hhh2 = AllFiles2[i2];
                                    string hhh = AllFiles3[i3];

                                    path = @"C:\\Users\\Administrador\Documents\\bowa\\" + AllFiles[i] + "\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\";
                                    try
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
                                    catch (Exception ee)
                                    {
                                        using (StreamWriter sw = File.CreateText(@"C:\Users\Administrador\Documents\bowa\temp\listo1.txt"))
                                        {

                                            sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

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
                enviar_sobre();
                //   enviar_sobre();
           //     Console.Write("Press <Enter> to exit... ");
           //    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

            }



       

        public static string rutempresa = "";
            public static string fecha_rcof = "";
        public static string rutaz = "";
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
                catch (Exception errorC)
                {

                }

             
                int encontro_xml_enviar = 0;
                int cantidad = AllFiles1.Count();
                int cantidadx = AllFiles3.Count();
                for (int i1x = 0; i1x < cantidadx; i1x++)
                {
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

                                        aqui = 0;
                                        for (int i3 = 0; i3 < cantidad1; i3++)
                                        {
                                            fecha = AllFiles1[i1];
                                            entro = 1;
                                            rutempresa = AllFiles[i];



                                            string tt = @"C:\\Users\\Administrador\Documents\\bowa\\" + AllFiles[i] + "\\" +
                                              AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\";
                                            if (Directory.Exists(@"C:\\Users\\Administrador\Documents\\bowa\\" + AllFiles[i] + "\\" +
                                             AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\"))
                                            {

                                                rutempresa = AllFiles[i];
                                                string[] folderPaths = Directory.GetDirectories(@"C:\\Users\\Administrador\Documents\\bowa\\" + AllFiles[i] + "\\" +
                                                    AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\");

                                                string f = AllFiles1[i1];
                                                string[] lines = new string[100000];


                                                if (AllFiles1.Count() > 0)

                                                {
                                                    fecha = f;
                                                    try
                                                    {

                                                        string dpath = (@"C:\\Users\\Administrador\Documents\\bowa\\" + rutempresa + "\\rcof\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + fecha + "\\");

                                                        try
                                                        {
                                                            //si no existe la carpeta temporal la creamos
                                                            if (!(Directory.Exists(dpath)))
                                                            {
                                                                Directory.CreateDirectory(dpath);

                                                            }


                                                        }
                                                        catch (Exception ex)
                                                        {

                                                        }


                                                        DirectoryInfo di = new DirectoryInfo(@"C:\Users\Administrador\Documents\bowa\" + AllFiles[i] + "\\"
                                         + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_empaquetado\\");

                                                        DateTime fecha_busqueda = DateTime.Parse("01-05-2023");
                                                        DateTime fecha_busqueda1 = (DateTime.Parse(AllFiles1[i1]));
                                                        if (Directory.Exists(dpath) && fecha_busqueda<= fecha_busqueda1)
                                                        {
                                                            string[] pathFilesss = System.IO.Directory.GetFiles(dpath, "*.xml");
                                                            if (pathFilesss.Length != 0)
                                                            {

                                                            }
                                                            else
                                                            {
                                                                if (AllFiles[i] != "temp" && AllFiles[i] != "respaldo")
                                                                {
                                                                    if (AllFiles2[i2] != "caf" && AllFiles2[i2] != "rcof")
                                                                    {


                                                                        string[] pathFilesx = System.IO.Directory.GetFiles(@"C:\Users\Administrador\Documents\bowa\" + rutempresa + "\\rcof\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + fecha, "*.xml");

                                                                        foreach (string pathFilex1 in pathFilesx)
                                                                        {
                                                                            entro = 0;
                                                                        }
                                                                        
                                                                        if (Directory.Exists(@"C:\Users\Administrador\Documents\bowa\" + AllFiles[i] + "\\"
                                               + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_empaquetado\\"))
                                                                        {
                                                                            string[] pathFiles = System.IO.Directory.GetFiles(@"C:\Users\Administrador\Documents\bowa\" + AllFiles[i] + "\\"
                                                   + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_empaquetado\\", "*.xml");

                                                                            string fs = @"C:\Users\Administrador\Documents\bowa\" + AllFiles[i] + "\\"
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
                                                                                    string[] pathFiles1x = System.IO.Directory.GetFiles(@"C:\Users\Administrador\Documents\bowa\" + rutempresa + "\\rcof\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + fecha_rcof + "//", "*.xml");

                                                                                    foreach (string pathFilex1 in pathFiles1x)
                                                                                    {
                                                                                        rcof_existe = 1;
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
                                                                                string[] fh = DateTime.Today.ToString().Split(' ');
                                                                                hh = (fh[0].Split('-'));
                                                                                if (esteequipo == 1)
                                                                                {
                                                                                    hh = (fh[0].Split('/'));
                                                                                }
                                                                                else
                                                                                {
                                                                                    hh = (fh[0].Split('-'));
                                                                                }
                                                                                int[] ggg1 = new int[3];
                                                                                ggg1[0] = Convert.ToInt32(hh[2])+2000;
                                                                                ggg1[1] = Convert.ToInt32(hh[1]);
                                                                                ggg1[2] = Convert.ToInt32(hh[0]);
                                                                                
                                                                                Console.WriteLine("FECHA " + ggg1[0] + " " + ggg1[1] + " " + ggg1[2] + "-" + ggg[0] + " " + ggg[1] + " " + ggg[2]);
                                                                                 
                                                                                if (rcof_existe == 0 && !(ggg1[0] == ggg[0] && ggg1[1] == ggg[1] && ggg1[2] == ggg[2]))
                                                                                {
                                                                                    generar_rcof = 1;
                                                                                    Console.WriteLine("Buscando xml firmados en rut: [" + AllFiles[i] + "] en Sucursal [" + AllFiles2[i2] + "] Equipo [" + AllFiles3[i3] + "] fecha " + AllFiles1[i1]);
                                                                                    fff = fecha;
                                                                                    List<int> listint = new List<int>();
                                                                                    foreach (string pathFile1 in pathFiles)
                                                                                    {
                                                                                        try
                                                                                        {
                                                                                            fecha_rcof = AllFiles1[i1];
                                                                                            vaciox = 0;
                                                                                            string xml = File.ReadAllText(pathFile1, Encoding.GetEncoding("ISO-8859-1"));
                                                                                            var dte = ItaSystem.DTE.Engine.XML.XmlHandler.DeserializeFromString<ItaSystem.DTE.Engine.Documento.DTE>(xml);
                                                                                            dtes.Add(dte);
                                                                                            String filename = Path.GetFileName(pathFile1).Replace(".xml","");
                                                                                            string[] gf = filename.Split('_');
                                                                                            listint.Add(Convert.ToInt32(gf[3]));
                                                                                            xmlDtes.Add(xml);
                                                                                            lines[y] = DateTime.Now.ToString() + " " + Path.GetFileName(pathFile1) + " " + "incluida_en RCOF";
                                                                                            Console.WriteLine(lines[y]);
                                                                                            rutaz = @"C:\Users\Administrador\Documents\bowa\" + rutempresa + "\\rcof\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + fecha + "\\";
                                                                                            string docPath1 = @"C:\Users\Administrador\Documents\bowa\" + rutempresa + "\\rcof\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + fecha + "\\";
                                                                                            TextWriter xtw = new StreamWriter(docPath1 + "log_" + fecha_rcof + ".txt", true);
                                                                                            xtw.WriteLine(lines[y]);
                                                                                            xtw.Close();
                                                                                        }
                                                                                        catch (Exception ee) { error(@"C: \Users\Administrador\Documents\bowa\" + rutempresa + "\\rcof\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + fecha + "\\", "" + ee); }
                        


                                                                                        //  System.IO.File.Copy(@"C:\Users\usuario\Documents\bowa\" + AllFiles[i] + "\\"+ AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + AllFiles1[i1] + "\\xml_empaquetado\\" + pathFile1, @"C:\Users\usuario\Documents\bowa\" + AllFiles1[i1] + "\\rcof\\" + pathFile1, true); y++;
                                                                                        //File.Delete(pathFile1);
                                                                                        //DeleteDirectory(pathx);
                                                                                        y++;
                                                                                        sucursal = AllFiles2[i2];
                                                                                        equipo = AllFiles3[i3];
                                                                                    }
                                                                                    string docPath1x = @"C:\Users\Administrador\Documents\bowa\" + rutempresa + "\\rcof\\" + AllFiles2[i2] + "\\" + AllFiles3[i3] + "\\" + fecha + "\\";
                                                                                    int_min = listint.Min();
                                                                                    int_max = listint.Max();
                                                                                    TextWriter tw = new StreamWriter(docPath1x + "log_" + fecha_rcof + ".txt", true);
                                                                                    tw.WriteLine("\n" + int_min + "_" + int_max);
                                                                                    tw.Close();
                                                                                
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

                                        if (generar_rcof == 1)
                                        {
                                            var rcof = handler.GenerarRCOF(dtes, rutaz);
                                            rcof.DocumentoConsumoFolios.Id = "RCOF_" + fecha.Replace("-", "");

                                            Handler.Initialize2();
                                            Handler.leer(rutempresa);
                                            try
                                            {
                                                filePathArchivo = rcof.Firmar(Handler.represen_n, out xmlString, rutaz, "");
                                            }
                                            catch (Exception ee) { }
                                            Console.WriteLine("RCOF de fecha:  " + fecha_rcof + " rut:" + rutempresa + " Generado " + filePathArchivo);
                                            string docPath = rutaz;
                                            TextWriter tw = new StreamWriter(docPath + "log_" + fecha_rcof + ".txt", true);
                                            tw.WriteLine("RCOF de fecha:  " + fecha_rcof + " rut:" + rutempresa + " Generado " + filePathArchivo);
                                            tw.Close();
                                            fecha_rcof = "";
                                            generar_rcof = 0;
                                            dtes.Clear();
                                            string[]pathFiles = System.IO.Directory.GetFiles(rutaz,"*.xml");
                                            foreach (string pathFile1 in pathFiles)
                                            {
                                                nombreconsumo = fecha+"_"+sucursal+"_"+equipo;
                                                Console.WriteLine("aaa " + pathFile1);
                                               
                                                grabar_venta(pathFile1, int_min, int_max);
                                            }
                                            }

                                    }
                                }
                            }
                        }
                        try
                        {
                            int ixx = 0;
                            nombreconsumo = filePathArchivo;
                            Console.WriteLine("Busqueda de Sobres por enviar Finalizada");
                            using (StreamWriter sw = File.CreateText(@"C:\Users\Administrador\Documents\bowa\temp\listo2.txt"))
                            {
                                linesx[1] = "Barrido terminado";
                                Console.Write(linesx[1]);
                                foreach (string line in linesx)
                                    if (line != "")
                                    {
                                        sw.WriteLine(line);
                                    }
                            }

                            string path1xx = (@"C:\Users\Administrador\Documents\bowa\temp\aviso3.txt");
                            if (File.Exists(path1xx))
                            {
                                //    File.Delete(path1xx);
                            }
                        }
                        catch (Exception ee)
                        {

                        }

                           

                    }
                }
            }
            catch (Exception ee) { }

        }
        public static int int_min = 0;
        public static int int_max  = 0;

        public static void error(string pathh, string error)
            {
            string docPath = pathh;
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
                     using (StreamWriter sw = File.CreateText(@"C:\Users\Administrador\Documents\bowa\temp\listo1.txt"))
                    {

                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                    }
                }
            }

            public static void grabar_venta(string archivo, int int_min, int int_max)
            {

                Handler.Initialize2();
                Handler.leer(rutempresa);
                configuracion.LeerArchivo();
            Console.WriteLine("Grabando venta en localhost");
            Thread.Sleep(2000);
            try
                {
                    string fecha_consumo = "";
                    string neto = "0";
                    string iva = "0";
                string total = "0";
                string Exento = "0";
                string numero_folios = "0";
                    string rango1 = "0";
                    string rangof1 = "0";
                 
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
                contenido = xmlDoc.GetElementsByTagName("MntExento");

                foreach (XmlNode node in contenido)
                {
                    Exento = node.InnerText;

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
                        f = f + 1;
                    }
                rango1 = "" + int_min;
                f = 0;
                    contenido = xmlDoc.GetElementsByTagName("Final");

                    foreach (XmlNode node in contenido)
                    {
                        if (f == 0)
                        {
                            rangof1 = node.InnerText;
                        }
                        f = f + 1;
                    }

                rangof1 = "" + int_max;
                Handler.Initialize2();
                    Handler.leer(Program.rutempresa);
                    configuracion.LeerArchivo();

                    Handler.Cliente = Program.rutempresa;
                    Handler.dte = archivo;


                    if (rango1 == "")
                    {
                        rango1 = "0";
                    }

                    if (rangof1 == "")
                    {
                        rangof1 = "0";
                    }
                string consumo_clave = rutempresa +"_"+fecha_consumo+ "_" + sucursal + "_" + equipo;
                    string connectionString = @"server=45.7.230.91;userid=root1;password=12345678;database=bowa";

                string[] dias = DateTime.Now.ToString().Split(' ');
                    string dia = dias[0];
                    MySqlConnection connection = null;
                    try
                    {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into envioconsumo_sucu (Cliente, sucursal, equipo, fecha_consumo, consumo_clave ,NumeroFolios, Neto, Iva, Total,Exento," +
                        "rango_inicial1,rango_final1) VALUES (@Cliente, @sucursal, @equipo, @fecha_consumo, @consumo_clave,@NumeroFolios, @Neto, @Iva, @Total,@Exento," +
                        "@rango_inicial1,@rango_final1) ON DUPLICATE KEY UPDATE fecha_consumo=@fecha_consumo, sucursal=@sucursal, equipo=@equipo, NumeroFolios = @NumeroFolios, Neto = @Neto,Total=@total, Iva=@iva, Exento=@Exento," +
                        "rango_inicial1 = @rango_inicial1, rango_final1 = @rango_final1";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@Cliente", Handler.Cliente);

                    cmd.Parameters.AddWithValue("@sucursal", sucursal);
                    cmd.Parameters.AddWithValue("@equipo", equipo);
                    cmd.Parameters.AddWithValue("@NumeroFolios", numero_folios);
                    cmd.Parameters.AddWithValue("@Neto", neto);
                    cmd.Parameters.AddWithValue("@Iva", iva);
                    cmd.Parameters.AddWithValue("@Exento", Exento);
                    cmd.Parameters.AddWithValue("@Total", total);
                    cmd.Parameters.AddWithValue("@consumo_clave", consumo_clave);
                    cmd.Parameters.AddWithValue("@fecha_consumo", fecha_consumo);
                    cmd.Parameters.AddWithValue("@rango_inicial1", rango1);
                    cmd.Parameters.AddWithValue("@rango_final1", rangof1);
                    cmd.ExecuteNonQuery();

                    connectionString = @"server=sh-pro12.hostgator.cl;userid=posfacto;password=eD1[P4q4kZw+0M;database=posfacto_bowa";
                    Console.WriteLine("Grabando venta en servidor");
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into envioconsumo_sucu (Cliente, sucursal, equipo, fecha_consumo, consumo_clave ,NumeroFolios, Neto, Iva, Total,Exento," +
                        "rango_inicial1,rango_final1) VALUES (@Cliente, @sucursal, @equipo, @fecha_consumo, @consumo_clave,@NumeroFolios, @Neto, @Iva, @Total,@Exento," +
                        "@rango_inicial1,@rango_final1) ON DUPLICATE KEY UPDATE fecha_consumo=@fecha_consumo, sucursal=@sucursal, equipo=@equipo, NumeroFolios = @NumeroFolios, Neto = @Neto,Total=@total, Iva=@iva, Exento=@Exento," +
                        "rango_inicial1 = @rango_inicial1, rango_final1 = @rango_final1";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@Cliente", Handler.Cliente);

                    cmd.Parameters.AddWithValue("@sucursal", sucursal);
                    cmd.Parameters.AddWithValue("@equipo", equipo);
                    cmd.Parameters.AddWithValue("@NumeroFolios", numero_folios);
                    cmd.Parameters.AddWithValue("@Neto", neto);
                    cmd.Parameters.AddWithValue("@Iva", iva);
                    cmd.Parameters.AddWithValue("@Exento", Exento);
                    cmd.Parameters.AddWithValue("@Total", total);
                    cmd.Parameters.AddWithValue("@fecha_consumo", fecha_consumo);
                    cmd.Parameters.AddWithValue("@consumo_clave", consumo_clave);
                    cmd.Parameters.AddWithValue("@rango_inicial1", rango1);
                    cmd.Parameters.AddWithValue("@rango_final1", rangof1);
                    cmd.ExecuteNonQuery();
                }
                finally
                    {
                        if (connection != null)
                            connection.Close();
                    }

                }
                catch (Exception ee)
                {
                Console.WriteLine("ERROR "+ee);
                Thread.Sleep(10000);                    //   Console.WriteLine("Ha ocurrido un error:" + ex);
                                                       //  error("Ha ocurrido un error:" + ex);
                using (StreamWriter sw = File.CreateText(@"C:\Users\Administrador\Documents\bowa\temp\listo1.txt"))
                    {

                        sw.WriteLine(DateTime.Now.ToString() + " Error: " + ee);

                    }
                }
            }

            public static int encontro_xml_enviar = 0;
            public static int espera = 100;
        
        }



    }
