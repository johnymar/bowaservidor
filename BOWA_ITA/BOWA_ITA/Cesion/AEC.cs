using ItaSystem.DTE.Engine.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ITA_CHILE.Cesion
{
    [XmlRoot("AEC")]
    public class AEC
    {
        [XmlAttribute("version")]
        public string Version { get { return ItaSystem.DTE.Engine.Config.Resources.versionAEC; } set { } }

        /// <summary>
        /// Localización del schema del XML.
        /// </summary>
        [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = "http://www.sii.cl/SiiDte AEC_v10.xsd";


        [XmlElement("DocumentoAEC")]
        public DocumentoAEC DocumentoAEC { get; set; }

        [XmlIgnore]
        public string signedXMLCedido { get; set; }

        [XmlIgnore]
        public List<string> signedXMLCesion { get; set; }

        public AEC()
        {
            DocumentoAEC = new DocumentoAEC();
            signedXMLCesion = new List<string>();
        }

        public string Firmar(string nombreCertificado, out string message, string outputDirectory = "out\\temp\\")
        {
            message = "";
            string filePath = "";
            try
            {
                List<string> namespaces = new List<string>();
                namespaces.Add("xsi&http://www.w3.org/2001/XMLSchema-instance");
                string xmlContent = XmlHandler.Serialize<AEC>(this, SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false, namespaces, outputDirectory, true);
                AppendXML(filePath);
                var content = ITA_CHILE.Security.Firma.Firma.FirmarDocumentoPath(filePath, this.DocumentoAEC.ID, nombreCertificado);
            }
            catch (Exception ex)
            {
                filePath = ex.Message;
                message = ex.StackTrace;
            }
            return filePath;
        }

        private string AppendXML(string filePath)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(filePath);


            System.Xml.XmlDocument d = new System.Xml.XmlDocument();
            d.PreserveWhitespace = true;
            d.LoadXml(signedXMLCedido);
            doc.ChildNodes[2].ChildNodes[1].ChildNodes[3].AppendChild(doc.ImportNode(d.DocumentElement, true));


            foreach (var a in signedXMLCesion)
            {
                System.Xml.XmlDocument temp = new System.Xml.XmlDocument();
                temp.PreserveWhitespace = true;
                temp.LoadXml(a);
                doc.ChildNodes[2].ChildNodes[1].ChildNodes[3].AppendChild(doc.ImportNode(temp.DocumentElement, true));
            }

            doc.InnerXml = doc.InnerXml.Replace(@"xmlns=""""", "").Replace("iso-8859-1", "ISO-8859-1");
            doc.Save(filePath);
            return doc.InnerXml;
        }

    }
}
