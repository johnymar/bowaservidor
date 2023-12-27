using ITA_CHILE.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.XML
{
    public class XmlHandler
    {
        public static string Serialize<T>(T obj, SerializationType.SerializationTypes serializationType, bool writeXmlDeclaration = true, List<string> namespaces = null, string outputDirectory = "out\\temp\\", bool escribeArchivo = true)
        {
            string aux = string.Empty;
            return XmlHandler.Serialize<T>(obj, serializationType, out aux, writeXmlDeclaration, true, namespaces, outputDirectory, escribeArchivo);
        }
        public static string Serialize<T>(T obj, SerializationType.SerializationTypes serializationType, out string filePath, bool writeXmlDeclaration = true, bool deleteTempFile = true, List<string> namespaces = null, string outputDirectoryPath = "out\\temp\\", bool escribeArchivo = true, string defaultNamespace = "", string customName = "") 
        {
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                if (!string.IsNullOrEmpty(defaultNamespace))
                    ns.Add("", defaultNamespace);
                else if (namespaces == null || namespaces.Count == 0)
                    ns.Add("", "");
                else
                {
                    string[] list;
                    foreach (var n in namespaces)
                    {
                        list = n.Split('&');
                        ns.Add(list[0], list[1]);
                    }
                }

                XmlWriterSettings settings = new XmlWriterSettings();

                settings.Encoding = Encoding.GetEncoding("ISO-8859-1");
                switch(serializationType)
                {
                    case SerializationType.SerializationTypes.Inline:
                        settings.Indent = false;
                        settings.IndentChars = "";
                        settings.OmitXmlDeclaration = !writeXmlDeclaration;
                        settings.NewLineHandling = NewLineHandling.None;
                        break;
                    case SerializationType.SerializationTypes.LineBreakNoIndent:
                        settings.Indent = true;
                        settings.IndentChars = "";
                        settings.OmitXmlDeclaration = !writeXmlDeclaration;
                        break;
                    case SerializationType.SerializationTypes.PrettyPrint: 
                        settings.Indent = true;
                        settings.OmitXmlDeclaration = !writeXmlDeclaration;
                        break;
                    default: break;
                }

                try
                {
                    string prefix = string.Empty;
                    var tipo = obj.GetType();
                    if (obj.GetType() == typeof(Engine.Documento.DTE))
                    {
                        var temp = obj as Engine.Documento.DTE;
                        if (temp.Documento.Id != null)
                        {
                            if (string.IsNullOrEmpty(customName))
                                prefix = "DTE_" + (int)temp.Documento.Encabezado.IdentificacionDTE.TipoDTE + "_" + temp.Documento.Encabezado.Emisor.Rut + "_" + temp.Documento.Encabezado.IdentificacionDTE.Folio;
                            else prefix = customName;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(customName))
                                prefix = "DTE_" + (int)temp.Exportaciones.Encabezado.IdentificacionDTE.TipoDTE + "_" + temp.Exportaciones.Encabezado.Emisor.Rut + "_" + temp.Exportaciones.Encabezado.IdentificacionDTE.Folio;
                            else prefix = customName;
                        }
                    }
                    else if (obj.GetType() == typeof(Engine.Envio.EnvioDTE))
                    {
                        var temp = obj as Engine.Envio.EnvioDTE;
                        prefix = string.IsNullOrEmpty(customName) ? "Envio_" + temp.SetDTE.Id : customName;
                    }
                    else if (obj.GetType() == typeof(ItaSystem.DTE.Engine.Envio.EnvioBoleta))
                    {
                        var temp = obj as Engine.Envio.EnvioBoleta;
                        prefix = string.IsNullOrEmpty(customName) ?  "EnvioBOLETA_" + temp.SetDTE.Id : customName;
                    }
                    else if (obj.GetType() == typeof(Engine.InformacionElectronica.LBoletas.LibroBoletas))
                    {
                        var temp = obj as Engine.InformacionElectronica.LBoletas.LibroBoletas;
                        prefix = string.IsNullOrEmpty(customName) ? "LibroBoletas_" + temp.EnvioLibro.Caratula.PeriodoTributario : customName;
                    }
                    else if (obj.GetType() == typeof(Engine.InformacionElectronica.LCV.LibroCompraVenta))
                    {
                        var temp = obj as Engine.InformacionElectronica.LCV.LibroCompraVenta;
                        prefix = string.IsNullOrEmpty(customName) ? "LibroCompraVenta_" + temp.EnvioLibro.Caratula.PeriodoTributario + temp.EnvioLibro.Caratula.TipoOperacion.ToString() : customName;
                    }
                    else if (obj.GetType() == typeof(Engine.InformacionElectronica.LCV.LibroGuia))
                    {
                        var temp = obj as Engine.InformacionElectronica.LCV.LibroGuia;
                        prefix = string.IsNullOrEmpty(customName) ? "LibroGuia_" + temp.EnvioLibro.Caratula.PeriodoTributario : customName;
                    }
                    else if (obj.GetType() == typeof(ItaSystem.DTE.Engine.RespuestaEnvio.RespuestaDTE))
                    {
                        var temp = obj as ItaSystem.DTE.Engine.RespuestaEnvio.RespuestaDTE;
                        prefix = string.IsNullOrEmpty(customName) ? "RespuestaIntercambio_" + temp.Resultado.Id : customName;
                    }
                    else if (obj.GetType() == typeof(ItaSystem.DTE.Engine.ReciboMercaderia.EnvioRecibos))
                    {
                        var temp = obj as ItaSystem.DTE.Engine.ReciboMercaderia.EnvioRecibos;
                        prefix = string.IsNullOrEmpty(customName) ? "RespuestaIntercambio_RECIBO_MERCADERIAS" : customName;
                    }
                    else if (obj.GetType() == typeof(ItaSystem.DTE.Engine.RCOF.ConsumoFolios))
                    {
                        var temp = obj as ItaSystem.DTE.Engine.RCOF.ConsumoFolios;
                        prefix = string.IsNullOrEmpty(customName) ? "ConsumoFolios_" + temp.DocumentoConsumoFolios.Caratula.RutEmisor + "_" + temp.DocumentoConsumoFolios.Caratula.FechaInicio.ToString("ddMMyyyy") : customName;
                    }

                    else prefix = "_" + DateTime.Now.Ticks;
                    string tempFilePath = outputDirectoryPath + prefix + ".xml";
                    if (escribeArchivo)
                    {
                        XmlSerializer s;
                        if (!string.IsNullOrEmpty(defaultNamespace))
                            s = new XmlSerializer(obj.GetType(), defaultNamespace);
                        else if (namespaces == null || namespaces.Count == 0)
                            s = new XmlSerializer(obj.GetType());
                        else
                            s = new XmlSerializer(obj.GetType(), "http://www.sii.cl/SiiDte");

                        using (XmlWriter xmlWriter = XmlWriter.Create(tempFilePath, settings))
                        {
                            s.Serialize(xmlWriter, obj, ns);
                        }
                        //using (EscapeQuotesXmlWriter exw = new EscapeQuotesXmlWriter(XmlWriter.Create(tempFilePath, settings)))
                        //{
                        //    s.Serialize(exw, obj, ns);
                        //}
                        string r = File.ReadAllText(tempFilePath, Encoding.GetEncoding("ISO-8859-1"));
                        if (deleteTempFile)
                            File.Delete(tempFilePath);
                        filePath = tempFilePath;
                        return r;
                    }
                    else
                    {
                        XmlSerializer s;
                        if (namespaces == null)
                            s = new XmlSerializer(obj.GetType());
                        else if (namespaces.Count == 0)
                            s = new XmlSerializer(obj.GetType());
                        else
                            s = new XmlSerializer(obj.GetType(), "http://www.sii.cl/SiiDte");
                        StringWriter outStream = new StringWriter();

                        using (var writer = XmlWriter.Create(outStream, settings))
                        {
                            //serializer.Serialize(writer, value, emptyNamespaces);
                            s.Serialize(writer, obj, ns);
                            filePath = "";
                            return outStream.ToString();
                        }

                        //using (EscapeQuotesXmlWriter exw = new EscapeQuotesXmlWriter(XmlWriter.Create(outStream, settings)))
                        //{
                        //    //XmlSerializer s = new XmlSerializer(obj.GetType());
                        //    s.Serialize(exw, obj, ns);
                        //    filePath = "";
                        //    return outStream.ToString();
                        //}

                    }
                }
                catch (Exception ex)
                {
                    File.WriteAllText("Error_" + DateTime.Now.Ticks.ToString() + ".txt", ex.Message + "\n\n" + ex.StackTrace);
                    filePath = "";
                    throw new Exception(ex.Message);
                }
            }
            catch(Exception ex) 
            {
                filePath = "";
                return ex.Message;
                throw new Exception(ex.Message);
            }
        }       
        
        public static T DeserializeFromString<T>(string text)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T obj;
            //using (StreamReader reader = new StreamReader(text))
            using(StringReader reader = new StringReader(text))
            {
                obj = (T)serializer.Deserialize(reader);
                reader.Close();
            }
            return obj;
        }

        public static T TryDeserializeFromString<T>(string text)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T obj;

            using (StringReader reader = new StringReader(text.Replace("xmlns=\"http://www.sii.cl/SiiDte\"", "")))
            {
                obj = (T)serializer.Deserialize(reader);
                reader.Close();
            }
            return obj;
        }

        /// <summary>
        /// No usa defaultNameSpace = "http://www.siii.cl/SiiDte, se usa para deserializar objetos crudos.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto a deserializar</typeparam>
        /// <param name="filePath">Ruta al archivo que se va a leer</param>
        /// <returns>Objeto deserializado</returns>
        public static T DeserializeRaw<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T obj;
            using (StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding("ISO-8859-1")))
            {
                obj = (T)serializer.Deserialize(reader);
                reader.Close();
            }
            return obj;
        }

        public static T DeserializeRawWithoutEncoding<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T obj;
            using (StreamReader reader = new StreamReader(filePath))
            {
                obj = (T)serializer.Deserialize(reader);
                reader.Close();
            }
            return obj;
        }

        /// <summary>
        /// Deserializa un objeto desde un archivo usando defaultNameSpace = "http://www.siii.cl/SiiDte.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto a deserializar</typeparam>
        /// <param name="filePath">Ruta al archivo que se va a leer</param>
        /// <returns>Objeto deserializado</returns>
        public static T Deserialize<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), "http://www.sii.cl/SiiDte");            
            T obj = default(T);

            using (StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding("ISO-8859-1")))
            {
                obj = (T)serializer.Deserialize(reader);
                reader.Close();
            }
            return obj;
        }

        public static T Deserialize<T>(byte[] dte)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), "http://www.sii.cl/SiiDte");
            T obj = default(T);
            Stream stream = new MemoryStream(dte);
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("ISO-8859-1")))
            {
                obj = (T)serializer.Deserialize(reader);
                reader.Close();
            }
            return obj;
        }

        public static T TryDeserialize<T>(string filePath)
        {
            T obj;
            var xml = XDocument.Parse(File.ReadAllText(filePath));

            //var dte = (((xml.Document.FirstNode as XElement).FirstNode as XElement)).LastNode;
            //var dteString = dte.ToString().Replace("xmlns=\"http://www.sii.cl/SiiDte\"", "");

            var ser = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(xml.ToString().Replace("xmlns=\"http://www.sii.cl/SiiDte\"", "")))
            {
                obj = (T)ser.Deserialize(sr);
                sr.Close();
            }
            return obj;
        }



        private static string _errors;

        /// <summary>
        /// Attemps to validate xml file loadede from "xmlFilePath" with schema loaded from schemaFilePath.
        /// </summary>
        /// <param name="xmlFilePath">Filepath from wich xml will be loaded.</param>
        /// <param name="schemaFilePath">Filepath from wich schema will be loaded.</param>
        /// <param name="message">Operation result state message.</param>
        /// <returns>True for valid xml, otherwise false.</returns>                    
        public static bool ValidateXmlWithSchema(string xmlFilePath, string relativeSchemaFilePath, out string message)
        {
            if (String.IsNullOrEmpty(xmlFilePath) || String.IsNullOrEmpty(relativeSchemaFilePath))
            {
                message = "Both filepats are required.";
                return false;
            }

            string schemaFilePath = AppDomain.CurrentDomain.BaseDirectory + relativeSchemaFilePath;

            if(!File.Exists(xmlFilePath) || !File.Exists(schemaFilePath))
            {
                message ="File not found.";
                return false;
            }
            try
            {
                _errors = "";

                XmlDocument x = new XmlDocument();
                x.PreserveWhitespace = true;
                x.Load(xmlFilePath);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.CloseInput = true;
                settings.ValidationEventHandler += Handler;

                settings.ValidationType = ValidationType.Schema;

                settings.Schemas.Add("http://www.sii.cl/SiiDte", schemaFilePath);
                settings.ValidationFlags =
                     XmlSchemaValidationFlags.ReportValidationWarnings |
                     XmlSchemaValidationFlags.ProcessIdentityConstraints |
                     XmlSchemaValidationFlags.ProcessInlineSchema |
                     XmlSchemaValidationFlags.ProcessSchemaLocation;

                //StringReader r = new StringReader(xmlFilePath);

                using(StreamReader stream = new StreamReader(xmlFilePath))
                using (XmlReader validatingReader = XmlReader.Create(stream, settings))
                {
                    while (validatingReader.Read()) { /* just loop through document */ }
                }

                message = _errors;
                return true;
            }
            catch(Exception ex) 
            {
                message = ex.Message;
                return false;
            }
        }

        private static void Handler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error || e.Severity == XmlSeverityType.Warning)
                _errors +=
                  String.Format("Line: {0}, Position: {1} \"{2}\"",
                      e.Exception.LineNumber, e.Exception.LinePosition, e.Exception.Message) + Environment.NewLine;
        }

        public static bool ValidateWithSchema(string xmlFilePath, out string message, string schema = null)
        {
            bool match = false;
            return ValidateWithSchema(xmlFilePath, out message, out match, schema);
        }
        public static bool ValidateWithSchema(string xmlFilePath, out string message, out bool xmlMatchesSchema, string schema = null) 
        {
            xmlMatchesSchema = false;

            if (String.IsNullOrEmpty(xmlFilePath))
            {
                message = "Both filepats are required.";
                return false;
            }

            if (!File.Exists(xmlFilePath))
            {
                message = "File not found.";
                return false;
            }

            bool containErrors = false;

            XmlSchemaSet schemas = new XmlSchemaSet();

            schemas.Add(XmlSchema.Read(XmlReader.Create(new FileStream(AppDomain.CurrentDomain.BaseDirectory + (string.IsNullOrEmpty(schema) ? Schemas.EnvioDTE : schema), FileMode.Open, FileAccess.Read), null, AppDomain.CurrentDomain.BaseDirectory + (string.IsNullOrEmpty(schema) ? Schemas.EnvioDTE : schema)), null));

            schemas.Compile();

            XDocument x = XDocument.Parse(File.ReadAllText(xmlFilePath, Encoding.GetEncoding("ISO-8859-1")), LoadOptions.PreserveWhitespace);

            bool matches = false;
            string n = x.Root.Name.LocalName;
            XmlSchemaElement element;

            foreach (XmlSchema s in schemas.Schemas())
            {
                try
                {
                    element = s.Items[0] as XmlSchemaElement;
                }
                catch { break; }

                if (n != null)
                    if (n == element.Name)
                    {
                        xmlMatchesSchema = true;
                        matches = true;
                        break;
                    }
            }
            if (!matches)
            {
                message = "Ningún esquema proporcionado puede validar el xml.";
                return false;
            }

            x.Validate(schemas, (o, e) =>
            {
                _errors +=
                    String.Format("Line: {0}, Position: {1} \"{2}\"",
                      e.Exception.LineNumber, e.Exception.LinePosition, e.Exception.Message) + Environment.NewLine;
                
                if (e.Severity == XmlSeverityType.Error)
                    containErrors = true;
            });

            message = _errors;
            return !containErrors;
        }

        public static bool ValidateXMLWithSchema(string xmlContent, out string message, string schema = null)
        {
            bool xmlMatchesSchema = false;

            bool containErrors = false;

            XmlSchemaSet schemas = new XmlSchemaSet();

            schemas.Add(XmlSchema.Read(XmlReader.Create(new FileStream(AppDomain.CurrentDomain.BaseDirectory + (string.IsNullOrEmpty(schema) ? Schemas.EnvioDTE : schema), FileMode.Open, FileAccess.Read), null, AppDomain.CurrentDomain.BaseDirectory + (string.IsNullOrEmpty(schema) ? Schemas.EnvioDTE : schema)), null));

            schemas.Compile();

            XDocument x = XDocument.Parse(xmlContent, LoadOptions.PreserveWhitespace);

            bool matches = false;
            string n = x.Root.Name.LocalName;
            XmlSchemaElement element;

            foreach (XmlSchema s in schemas.Schemas())
            {
                try
                {
                    element = s.Items[0] as XmlSchemaElement;
                }
                catch { break; }

                if (n != null)
                    if (n == element.Name)
                    {
                        xmlMatchesSchema = true;
                        matches = true;
                        break;
                    }
            }
            if (!matches)
            {
                message = "Ningún esquema proporcionado puede validar el xml.";
                return false;
            }

            x.Validate(schemas, (o, e) =>
            {
                _errors +=
                    String.Format("Line: {0}, Position: {1} \"{2}\"",
                      e.Exception.LineNumber, e.Exception.LinePosition, e.Exception.Message) + Environment.NewLine;

                if (e.Severity == XmlSeverityType.Error)
                    containErrors = true;
            });

            message = _errors;
            return !containErrors;
        }
    }
}
