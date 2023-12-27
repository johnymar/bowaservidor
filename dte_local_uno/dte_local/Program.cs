using bowuaSystem.DTE.Engine.Documento;
using bowuaSystem.DTE.Engine.Envio;
using bowuaSystem.DTE.Engine.XML;
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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dte_local
{
    class Program
    {
        static List<string> AllFiles = new List<string>();
        static string fecha = "";
        static public string envio;
        private static Autorizacion aut;
        static Handler handler = new Handler();
        static public Configuracion configuracion = new Configuracion();

        static public string rutempresa = "77193144-8";
        static void Main(string[] args)
        {

            int buscar = 0;
            string[] lines = new string[2];
            string path = (@"C:\Users\Usuario\Documents\bowa\temp\aviso.txt");

            // string path = (@"C:\Users\admin\Documents\bowa\temp\aviso.txt");
            lines = File.ReadAllLines(path);
            Console.WriteLine(String.Join(Environment.NewLine, lines));
            if (!lines[0].Equals(null))
            {
                rutempresa = lines[0];
//                fecha = lines[1];
                Console.WriteLine("Inicio de Firma de Boletas para RUT:[ " + rutempresa + " ]");
            }

            path = @"C:\\Users\\Usuario\Documents\\bowa\\temp\\" + rutempresa + "\\";
            try
            {
                String[] filePaths = Directory.GetFiles(@"C:\\Users\\Usuario\Documents\\bowa\\temp\\" + rutempresa + "\\");
                string[] folderPaths = Directory.GetDirectories(path);
                AllFiles.Clear();
                foreach (string s in folderPaths)
                {
                    string t = s.Remove(0, s.LastIndexOf('\\') + 1);
                    AllFiles.Add(t);

                }
            }
            catch (Exception ee) {
                Console.WriteLine("No se encontraron nuevos xml a firmar");
            }


            
            path = (@"C:\\Users\\Usuario\Documents\\bowa\\" + rutempresa + "\\caf\\");
            try
            {
                //si no existe la carpeta temporal la creamos
                if (!(Directory.Exists(path)))
                {
                    Directory.CreateDirectory(path);

                }


            }
            catch (Exception errorC)
            {
               
            }
            // 




            
                Bajar_caf(rutempresa);
                firmar_boletas();
            
            Console.Write("Press <Enter> to exit... ");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }

        public static void DeleteDirectory(string dirPath)
        {
            if (Directory.GetFiles(dirPath).Count()!=0) { }
            else
            {
                if (Directory.Exists(dirPath))
                {
                    try
                    {
                        Directory.Delete(dirPath, recursive: true);    //throws if directory doesn't exist. 
                    }
                    catch
                    {
                        Thread.Sleep(2000);  //wait 2 seconds 
                        Directory.Delete(dirPath, recursive: true);
                    }

                }
            }

        }


        public static void firmar_boletas() {
            try
            {
                Console.WriteLine("Buscando xml para firmar");
                int cantidad = AllFiles.Count();
                for (int i = 0; i < cantidad; i++)
                {
                    fecha = AllFiles[i];
                    string pathx = @"C:\Users\Usuario\Documents\bowa\temp\" + rutempresa + "\\" + AllFiles[i] + "\\";
                    DirectoryInfo di = new DirectoryInfo(@"C:\Users\Usuario\Documents\bowa\temp\" + rutempresa + "\\" + AllFiles[i] + "\\");
                    int y = 0;

                    string path = (@"C:\\Users\\Usuario\Documents\\bowa\\" + rutempresa + "\\" + AllFiles[i] + "\\xml_firmado\\");

                    try
                    {
                        //si no existe la carpeta temporal la creamos
                        if (!(Directory.Exists(path)))
                        {
                            Directory.CreateDirectory(path);

                        }


                    }
                    catch (Exception errorC)
                    {
                    }
                    path = (@"C:\\Users\\Usuario\Documents\\bowa\\" + rutempresa + "\\" + AllFiles[i] + "\\xml_empaquetado\\");

                    try
                    {
                        //si no existe la carpeta temporal la creamos
                        if (!(Directory.Exists(path)))
                        {
                            Directory.CreateDirectory(path);

                        }


                    }
                    catch (Exception errorC)
                    {
                    }

                    
                    path = (@"C:\\Users\\Usuario\Documents\\bowa\\" + rutempresa + "\\" + AllFiles[i] + "\\sobre_preparado\\");

                    try
                    {
                        //si no existe la carpeta temporal la creamos
                        if (!(Directory.Exists(path)))
                        {
                            Directory.CreateDirectory(path);

                        }


                    }
                    catch (Exception errorC)
                    {
                    }

                    path = (@"C:\\Users\\Usuario\Documents\\bowa\\" + rutempresa + "\\" + AllFiles[i] + "\\sobre_enviado\\");

                    try
                    {
                        //si no existe la carpeta temporal la creamos
                        if (!(Directory.Exists(path)))
                        {
                            Directory.CreateDirectory(path);

                        }


                    }
                    catch (Exception errorC)
                    {
                    }

                    path = (@"C:\\Users\\Usuario\Documents\\bowa\\" + rutempresa + "\\" + AllFiles[i] + "\\rcof_preparado\\");

                    try
                    {
                        //si no existe la carpeta temporal la creamos
                        if (!(Directory.Exists(path)))
                        {
                            Directory.CreateDirectory(path);

                        }


                    }
                    catch (Exception errorC)
                    {
                    }

                    path = (@"C:\\Users\\Usuario\Documents\\bowa\\" + rutempresa + "\\" + AllFiles[i] + "\\rcof_enviado\\");

                    try
                    {
                        //si no existe la carpeta temporal la creamos
                        if (!(Directory.Exists(path)))
                        {
                            Directory.CreateDirectory(path);

                        }


                    }
                    catch (Exception errorC)
                    {
                    }

                    string[] lines = new string[1000];
                    foreach (var fi in di.GetFiles("*.xml"))
                    {

                        string pathFile = fi.Name;

                        // System.IO.File.Copy(fi.FullName, "out\\temp\\" + rutempresa + "\\" + fi.Name, true);

                        string xml1 = File.ReadAllText(pathx + pathFile, Encoding.GetEncoding("ISO-8859-1"));
                        var dte = bowuaSystem.DTE.Engine.XML.XmlHandler.DeserializeFromString<bowuaSystem.DTE.Engine.Documento.DTE>(xml1);
                        var path1 = handler.TimbrarYFirmarXMLDTE(dte, @"C:\Users\Usuario\Documents\bowa\temp\" + rutempresa + "\\", @"C:\Users\Usuario\Documents\bowa\" + rutempresa + "//caf//");


                        File.Delete(di + fi.Name);
                        if (!path1.Equals(""))
                        {
                            handler.Validate(path1, BOWUA_CHILE.Security.Firma.Firma.TipoXML.DTE, bowuaSystem.DTE.Engine.XML.Schemas.DTE);
                            //    Clave.SubirArchivo( path, handler.configuracion.Empresa.RutCuerpo.ToString()+"-"+ handler.configuracion.Empresa.DV);
                            lines[y] = DateTime.Now.ToString() + " " + fi.Name + " " + "Firmado";
                            Console.WriteLine(lines[y]);
                            System.IO.File.Copy(path1, @"C:\Users\Usuario\Documents\bowa\" + rutempresa + "\\" + fecha + "\\" + "\\xml_firmado\\"+ Path.GetFileName(path1) , true); y++;
                            File.Delete(path1);
                            y++;
                        }





                    }



                    




                    using (StreamWriter sw = File.CreateText(@"C:\Users\Usuario\Documents\bowa\temp\listo.txt"))
                    {

                        foreach (string line in lines)
                            if (line != "")
                            {
                                sw.WriteLine(line);
                            }

                    }

                    DeleteDirectory(@"C:\Users\Usuario\Documents\bowa\temp\" + rutempresa+ "\\");
                }
            }
            catch (Exception e) { Console.WriteLine("Error "+ e); }

           
        }
        public static void Bajar_caf(string rut)
        {
            string p1 = @"C:\Users\Usuario\Documents\bowa\" + rutempresa + "//caf//";
            Console.WriteLine("Verificando consumo de folios");
            try
            {

                string ftpServerIP = "ftp.pos-factory.cl";
                string ftpUserName = "test@pos-factory.cl";
                string ftpPassword = "n#fWwOH@lE3Q";
                string filename = "CAF.xml";

                FileInfo objFile = new FileInfo(filename);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/" + rut + "/" + objFile.Name));
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                System.IO.Stream responseStream = response.GetResponseStream();
                FileStream file = File.Create(p1 + objFile.Name);
                byte[] buffer = new byte[32 * 1024];
                int read;
                Console.WriteLine("Buscando en el servidor");
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
                Console.WriteLine("Error "+ ee);
            }
            try
            {

                if (File.Exists(p1 + "CAF.xml"))
                {
                    Console.WriteLine("DESCARGANDO CAF");
                    aut = XmlHandler.DeserializeRaw<Autorizacion>(p1 + "CAF.xml");
                    aut.CAF.IdCAF = 1;
                    string tipo = string.Empty;
                    try
                    {
                        string filePath = @"C:\Users\Usuario\Documents\bowa\" + rutempresa + "//caf//" + string.Format("{0}_{1}_{2}.dat", (int)aut.CAF.Datos.TipoDTE, aut.CAF.Datos.RangoAutorizado.Desde.ToString(), aut.CAF.Datos.RangoAutorizado.Hasta.ToString());
                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            var xml = File.ReadAllBytes(p1 + "CAF.xml");
                            fs.Write(xml, 0, xml.Length);
                            fs.Flush();
                            fs.Close();
                        }

                    }
                    catch (Exception ex) {

                        Console.WriteLine("Error " + ex);
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Error " + ex);
            }
           
        }
    }
}