using ItaSystem.DTE.Engine.Documento;
using ItaSystem.DTE.Engine.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Envio
{
    [XmlRoot("EnvioDTE")]
    public class EnvioDTE
    {
        /// <summary>
        /// Versión de envío DTE.
        /// </summary>
        [XmlAttribute("version")]
        public string Version { get { return Config.Resources.versionEnvio; } set { } }

        /// <summary>
        /// Localización del schema del XML.
        /// </summary>
        [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = "http://www.sii.cl/SiiDte EnvioDTE_v10.xsd";

        /// <summary>
        /// Conjunto de DTE enviados.
        /// </summary>
        [XmlElement("SetDTE")]
        public SetDTE SetDTE { get; set; }

        public EnvioDTE()
        {
            SetDTE = new SetDTE();           
        }

        /* public string GetUnsignedXml(bool EnvioAutomatico = false)
         {
             this.SetDTE.Caratula.FechaEnvio = DateTime.Now;

             string filePath = "";
             List<string> namespaces = new List<string>();

             namespaces.Add("xsi&http://www.w3.org/2001/XMLSchema-instance");

             string xmlContent = XmlHandler.Serialize<EnvioDTE>(this, SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false, namespaces);
             if (EnvioAutomatico) return FormarXMLFromDTE(filePath);
             else return FormarXML(filePath);
         }*/

        public string Firmar(string nombreCertificado, string outputDirectory = "out\\temp\\", string customName = "", string password = "")
        {
            this.SetDTE.Caratula.FechaEnvio = DateTime.Now;

            string filePath = "";
            List<string> namespaces = new List<string>();
            namespaces.Add("xsi&http://www.w3.org/2001/XMLSchema-instance");
            string xmlContent = XmlHandler.Serialize<EnvioDTE>(this, SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false, namespaces, outputDirectory, true, "", customName);
            var xml = FormarXMLFromDTE(filePath);
            //xml = FormarXML(filePath);
            ITA_CHILE.Security.Firma.Firma.FirmarDocumentoPath(filePath, this.SetDTE.Id, nombreCertificado, password);

            return filePath;
        }

        private string FormarXMLFromDTE(string filePath)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(filePath);
            foreach (var a in SetDTE.dteXmls)
            {
                System.Xml.XmlDocument d = new System.Xml.XmlDocument();
                d.PreserveWhitespace = true;
                d.LoadXml(a);
                doc.ChildNodes[2].ChildNodes[1].AppendChild(doc.ImportNode(d.DocumentElement, true));
            }
            doc.InnerXml = doc.InnerXml.Replace(@"xmlns=""""", "").Replace("iso-8859-1", "ISO-8859-1");
            doc.Save(filePath);
            return doc.InnerXml;
        }
    }
}
