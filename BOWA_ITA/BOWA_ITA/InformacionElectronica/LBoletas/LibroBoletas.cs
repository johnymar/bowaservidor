using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LBoletas
{
    [XmlRoot("LibroBoleta")]
    public class LibroBoletas
    {
        [XmlAttribute("version")]
        public string Version { get { return Config.Resources.versionLibroBoleta; } set { } }

        [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = "http://www.sii.cl/SiiDte LibroBOLETA_v10.xsd";

        [XmlElement("EnvioLibro")]
        public EnvioLibro EnvioLibro { get; set; }

        public string Firmar(string nombreCertificado, string password = "")
        {
            string filePath = "";
            List<string> namespaces = new List<string>();
            namespaces.Add("xsi&http://www.w3.org/2001/XMLSchema-instance");
            EnvioLibro.FechaFirma = DateTime.Now;
            XML.XmlHandler.Serialize(this, XML.SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false, namespaces);
            ITA_CHILE.Security.Firma.Firma.FirmarDocumentoLibro(filePath, this.EnvioLibro.Id, nombreCertificado, password);

            //doc.InnerXml = doc.InnerXml.Replace("xmlns=\"http://www.w3.org/2000/09/xmldsig#\"", "xmlns = \"http://www.sii.cl/SiiDte\"");
            string contenido = File.ReadAllText(filePath, Encoding.GetEncoding("ISO-8859-1"));

            //LUEGO REEMPLAZO LOS NAMESPACE
            contenido = contenido.Replace("xmlns=\"http://www.w3.org/2000/09/xmldsig#\"", "xmlns=\"http://www.sii.cl/SiiDte\"");

            //LO VUELVO A GUARDAR CON LA MISMA CODIFICACIÓN
            File.WriteAllText(filePath, contenido, Encoding.GetEncoding("ISO-8859-1"));

            return filePath;
        }
    }
}
