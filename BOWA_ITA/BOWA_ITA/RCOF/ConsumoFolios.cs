using ItaSystem.DTE.Engine.Documento;
using ItaSystem.DTE.Engine.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.RCOF
{
    [XmlRoot("ConsumoFolios")]
    public class ConsumoFolios
    {
        [XmlAttribute("version")]
        public string Version { get { return Engine.Config.Resources.versionRCOF; } set { } }

        [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = "http://www.sii.cl/SiiDte ConsumoFolio_v10.xsd";

        [XmlElement("DocumentoConsumoFolios")]
        public DocumentoConsumoFolios DocumentoConsumoFolios { get; set; }

        public ConsumoFolios()
        {
            DocumentoConsumoFolios = new DocumentoConsumoFolios();
        }

        public string Firmar(string nombreCertificado, out string xml, string outputDirectory = "out\\temp\\", string customName = "", string password = "")
        {
            string filePath = "";
            List<string> namespaces = new List<string>();
            namespaces.Add("xsi&http://www.w3.org/2001/XMLSchema-instance");
            string xmlContent = XmlHandler.Serialize<ConsumoFolios>(this, SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false, namespaces, outputDirectory, true, "", customName);
            xml = ITA_CHILE.Security.Firma.Firma.FirmarDocumentoPath(filePath, DocumentoConsumoFolios.Id, nombreCertificado, password);
            return filePath;
        }

    }
}
