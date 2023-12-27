using ItaSystem.DTE.Engine.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.ReciboMercaderia
{
    /// <summary>
    /// Envio de Recibos de Recepcion de Mercaderias o Servicios Prestados.
    /// </summary>
    [XmlRoot("EnvioRecibos")]
    public class EnvioRecibos
    {
        [XmlAttribute("version")]
        public string Version { get { return Config.Resources.versionEnvioRecibos; } set { } }

        /// <summary>
        /// Localización del schema del XML.
        /// </summary>
        [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = "http://www.sii.cl/SiiDte EnvioRecibos_v10.xsd";

        /// <summary>
        /// Conjunto de Recibos Enviados
        /// </summary>
        [XmlElement("SetRecibos")]
        public SetRecibos SetRecibos { get; set; }

        public string Firmar(string nombreCertificado)
        {
            this.SetRecibos.Caratula.FechaHoraFirma = DateTime.Now;

            string filePath = "";
            List<string> namespaces = new List<string>();

            namespaces.Add("xsi&http://www.w3.org/2001/XMLSchema-instance");

            string xmlContent = XmlHandler.Serialize(this, SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false, namespaces);

            var xml = FormarXML(filePath);

            ITA_CHILE.Security.Firma.Firma.FirmarDocumentoPath(filePath, this.SetRecibos.Id, nombreCertificado);

            return filePath;
        }

        public string FormarXML(string filePath)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(filePath);

            foreach (var signedDoc in SetRecibos.signedXmls)
            {
                System.Xml.XmlDocument d = new System.Xml.XmlDocument();
                d.PreserveWhitespace = true;
                d.Load(signedDoc);

                doc.ChildNodes[2].ChildNodes[1].AppendChild(doc.ImportNode(d.DocumentElement, true));
            }

            doc.InnerXml = doc.InnerXml.Replace(@"xmlns=""""", "").Replace("iso-8859-1", "ISO-8859-1");

            doc.Save(filePath);

            return doc.InnerXml;
        }        
    }
}
