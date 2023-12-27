using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    [XmlRoot("LibroCompraVenta")]
    public class LibroCompraVenta
    {
        /// <summary>
        /// Version del envío del libro.
        /// Fixed to 1.0
        /// </summary>
        [XmlAttribute("version")]
        public string Version { get { return Config.Resources.versionLCV; } set { } }

        /// <summary>
        /// Localización del schema del XML.
        /// </summary>
        [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = "http://www.sii.cl/SiiDte LibroCV_v10.xsd";

        /// <summary>
        /// Envio libro.
        /// </summary>
        [XmlElement("EnvioLibro")]
        public EnvioLibro EnvioLibro { get; set; }

        public string Firmar(string nombreCertificado, string outputDirectory, string password = "")
        {
            string filePath = "";
            List<string> namespaces = new List<string>();
            namespaces.Add("xsi&http://www.w3.org/2001/XMLSchema-instance");
            EnvioLibro.FechaFirma = DateTime.Now;
            XML.XmlHandler.Serialize<LibroCompraVenta>(this, XML.SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false, namespaces, outputDirectory);
            ITA_CHILE.Security.Firma.Firma.FirmarDocumentoLibro(filePath, this.EnvioLibro.Id, nombreCertificado, password);
            return filePath;
        }
    }
}
