
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Clave_manager
{
        public class  Clave
    {
        private string url;

        public static string Encriptar(string texto)
        {
            try
            {

                string key = "boguaSystem"; //llave para encriptar datos

                byte[] keyArray;

                byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(texto);

                //Se utilizan las clases de encriptación MD5f

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();

                //Algoritmo TripleDES
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();

                byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);

                tdes.Clear();

                //se regresa el resultado en forma de una cadena
                texto = Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);

            }
            catch (Exception)
            {

            }
            return texto;
        }
        public static string Desencriptar(string textoEncriptado)
        {
            try
            {
                string key = "boguaSystem";
                byte[] keyArray;
                byte[] Array_a_Descifrar = Convert.FromBase64String(textoEncriptado);

                //algoritmo MD5
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);

                tdes.Clear();
                textoEncriptado = UTF8Encoding.UTF8.GetString(resultArray);

            }
            catch (Exception)
            {

            }
            return textoEncriptado;
        }

        public static void SubirArchivo(string url, string rut, string sucursal, string equipo)
        {
            try
            {
                string ftpServerIP = "162.215.213.231";
                string ftpUserName = "bowa@duncanmotors.cl";
                string ftpPassword = "]UnZ$MqO8?@H";


                string path = (@"C:\bowa\configuracion_servidor.txt");
                string[] lines1 = new string[3];

                if (File.Exists(path))
                {
                    // string path = (@"C:\Users\admin\Documents\desla\temp\aviso.txt");
                    lines1 = File.ReadAllLines(path);
                    if (!lines1[0].Equals(null))
                    {
                        string[] u = new string[2];
                        u = lines1[0].Split('=');
                        ftpServerIP = u[1];
                        //  Console.WriteLine("\nUsar como ruta  de Entrada:[ " + ruta + " ]");
                    }
                    if (!lines1[1].Equals(null))
                    {
                        string[] u = new string[2];
                        u = lines1[1].Split('=');
                        ftpUserName = (u[1]);
                        //   Console.WriteLine("\nUsar como ruta  de salida:[ " + ruta1 + " ]");
                    }

                    if (!lines1[2].Equals(null))
                    {
                        string[] u = new string[2];
                        u = lines1[2].Split('=');
                        ftpPassword = (u[1]);
                        //   Console.WriteLine("\nUsar como ruta  de salida:[ " + ruta1 + " ]");
                    }
                }
                string filename = url;

                FileInfo objFile = new FileInfo(filename);

                FtpWebRequest objFTPRequest;

                // Create FtpWebRequest object 
                objFTPRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/public_html/bowa/php/upload/dis/" + rut + "/" + sucursal + "/" + equipo + "/" + objFile.Name));

                // Set Credintials
                objFTPRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                // By default KeepAlive is true, where the control connection is 
                // not closed after a command is executed.
                objFTPRequest.KeepAlive = false;

                // Set the data transfer type.
                objFTPRequest.UseBinary = true;

                // Set content length
                objFTPRequest.ContentLength = objFile.Length;

                // Set request method
                objFTPRequest.Method = WebRequestMethods.Ftp.UploadFile;

                // Set buffer size
                int intBufferLength = 16 * 1024;
                byte[] objBuffer = new byte[intBufferLength];

                // Opens a file to read
                FileStream objFileStream = objFile.OpenRead();

                try
                {
                    // Get Stream of the file
                    System.IO.Stream objStream = objFTPRequest.GetRequestStream();

                    int len = 0;

                    while ((len = objFileStream.Read(objBuffer, 0, intBufferLength)) != 0)
                    {
                        // Write file Content 
                        objStream.Write(objBuffer, 0, len);

                    }

                    objStream.Close();
                    objFileStream.Close();

                }
                catch (Exception ex)
                {

                    throw ex;

                }
            }
            catch (Exception)
            {

            }
            }

        public static string ftpServerIP ;
        public static string ftpUserName ;
        public static string ftpPassword ;

        public static string Bajar_key(string rut, string sucursal, string equipo)
        {
                try
                {
                string p1 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "//";

                string macAddresses = string.Empty;

                    foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        if (nic.OperationalStatus == OperationalStatus.Up)
                        {
                            macAddresses += nic.GetPhysicalAddress().ToString();
                            break;
                        }
                    }
                    string mac1 = "";
                    string path = p1 + "mac.txt";
                    if (!File.Exists(path))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine(macAddresses);

                        }
                    }


                string path1= (@"C:\bowa\configuracion_servidor.txt");
                string[] lines1 = new string[3];

                if (File.Exists(path1))
                {
                    // string path = (@"C:\Users\admin\Documents\desla\temp\aviso.txt");
                    lines1 = File.ReadAllLines(path1);
                    if (!lines1[0].Equals(null))
                    {
                        string[] u = new string[2];
                        u = lines1[0].Split('=');
                        ftpServerIP = u[1];
                        //  Console.WriteLine("\nUsar como ruta  de Entrada:[ " + ruta + " ]");
                    }
                    if (!lines1[1].Equals(null))
                    {
                        string[] u = new string[2];
                        u = lines1[1].Split('=');
                        ftpUserName = (u[1]);
                        //   Console.WriteLine("\nUsar como ruta  de salida:[ " + ruta1 + " ]");
                    }

                    if (!lines1[2].Equals(null))
                    {
                        string[] u = new string[2];
                        u = lines1[2].Split('=');
                        ftpPassword = (u[1]);
                        //   Console.WriteLine("\nUsar como ruta  de salida:[ " + ruta1 + " ]");
                    }
                }
                string filename = "clave.txt";
                    string mac = "";
                    int Error = 0;
                    FileInfo objFile = new FileInfo(filename);

                    FtpWebRequest request4 = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/dis/" + rut + "/" + sucursal + "/" + equipo + "/" + "mac.txt"));
                    request4.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                    request4.Method = WebRequestMethods.Ftp.GetFileSize;
                    try
                    {
                        FtpWebResponse response4 = (FtpWebResponse)request4.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        FtpWebResponse response4 = (FtpWebResponse)ex.Response;

                        if (response4.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                        {
                            SubirArchivo(p1 + "mac.txt", rut, sucursal, equipo);
                            mac1 = macAddresses;
                            Error = 1;
                        }
                        else
                        {
                            Error = 0;

                        }

                    }

                    if (Error == 0)
                    {

                        Bajar("mac.txt", rut, sucursal, equipo);

                        mac1 = System.IO.File.ReadAllText(p1 + "mac.txt");
                        File.Delete(p1 + "mac.txt");
                    }
                    if (mac1.Contains(macAddresses))
                    {
                        FtpWebRequest request3 = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/dis/" + rut + "/" + sucursal + "/" + equipo + "/" + "clave.txt"));
                        request3.Method = WebRequestMethods.Ftp.DownloadFile;

                        request3.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                        FtpWebResponse response6 = (FtpWebResponse)request3.GetResponse();

                        System.IO.Stream responseStream = response6.GetResponseStream();


                        FileStream file = File.Create(p1 + "read.txt");


                        byte[] buffer = new byte[32 * 1024];
                        int read;

                        while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            file.Write(buffer, 0, read);
                        }
                        file.Close();
                        responseStream.Close();
                        response6.Close();


                        string text = System.IO.File.ReadAllText(p1 + "read.txt");


                        DateTime date1 = DateTime.Today;

                        string[] clave = text.Split('_');

                        int result = DateTime.Compare(date1, Convert.ToDateTime(clave[1]));
                        if (result <= 0)
                        {
                            return "Activada";

                        }
                        else
                        {
                            return "Desactivada";
                        }
                    }
                    else
                    {
                        return "Desactivada";
                    }
                }catch (Exception)
                {
                return "Desactivado";
                }
            }

        public static string validar_fecha()
        {
            try
            {
                string p1 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "//";

                if (!File.Exists(p1))
                {
                    string[] lines = new string[1];
                    using (StreamWriter sw = File.CreateText(p1 + "read.txt"))
                    {
                        lines[0] = DateTime.Today.ToString();
                        Console.Write("\n" + lines[0]);
                        foreach (string line in lines)
                            if (line != "")
                            {
                                sw.WriteLine("prueba_"+line);
                            }
                    }
                }
         
                    string text = System.IO.File.ReadAllText(p1 + "read.txt");

                    DateTime date1 = DateTime.Today;
                    string[] clave = text.Split('_');
                string tt= clave[1];
                    int result = DateTime.Compare(date1, Convert.ToDateTime(clave[1]));
                    if (result <= 0)
                    {

                        return "ok";


                    }
                    else
                    {
                        return "no_ok";
                    }

                }
         

            catch (Exception ee)
            {
                return "no_ok";
            }
        
            
            }
    public static string verificar_internet() {
                try
                {
                    string Estado = "";
                    Uri Url = new Uri("https://www.google.com/");

                    WebRequest WebRequest;
                    WebRequest = System.Net.WebRequest.Create(Url);
                    WebResponse objetoResp;

                    try
                    {
                        objetoResp = WebRequest.GetResponse();
                        Estado = "Si";
                        objetoResp.Close();
                        return Estado;
                    }
                    catch (Exception e)
                    {
                        Estado = "No";
                        return Estado;
                    }
                }
                catch (Exception)
                {
                return "No";
                }


                }
        public static void Bajar(string url, string rut, string sucursal, string equipo)
        {
                try
                {
                    string p1 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "//";
                string ftpServerIP = "162.215.213.231";
                string ftpUserName = "bowa@duncanmotors.cl";
                string ftpPassword = "]UnZ$MqO8?@H";


                string path = (@"C:\bowa\configuracion_servidor.txt");
                string[] lines1 = new string[3];
                string filename = url;

                if (File.Exists(path))
                {
                    // string path = (@"C:\Users\admin\Documents\desla\temp\aviso.txt");
                    lines1 = File.ReadAllLines(path);
                    if (!lines1[0].Equals(null))
                    {
                        string[] u = new string[2];
                        u = lines1[0].Split('=');
                        ftpServerIP = u[1];
                        //  Console.WriteLine("\nUsar como ruta  de Entrada:[ " + ruta + " ]");
                    }
                    if (!lines1[1].Equals(null))
                    {
                        string[] u = new string[2];
                        u = lines1[1].Split('=');
                        ftpUserName = (u[1]);
                        //   Console.WriteLine("\nUsar como ruta  de salida:[ " + ruta1 + " ]");
                    }

                    if (!lines1[2].Equals(null))
                    {
                        string[] u = new string[2];
                        u = lines1[2].Split('=');
                        ftpPassword = (u[1]);
                        //   Console.WriteLine("\nUsar como ruta  de salida:[ " + ruta1 + " ]");
                    }
                }
               
                    FileInfo objFile = new FileInfo(filename);

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/php/upload/dis/" + rut + "/" + sucursal + "/" + equipo + "/" + objFile.Name));
                    request.Method = WebRequestMethods.Ftp.DownloadFile;

                    request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                    System.IO.Stream responseStream = response.GetResponseStream();
                    FileStream file = File.Create(p1 + objFile.Name);
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
                catch (Exception)
                {

                }
            }
    }
    

}
