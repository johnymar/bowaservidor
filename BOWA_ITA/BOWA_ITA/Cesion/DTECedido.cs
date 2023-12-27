using ItaSystem.DTE.Engine.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ITA_CHILE.Cesion
{
    [XmlRoot("DTECedido")]
    public class DTECedido
    {
        [XmlAttribute("version")]
        public string Version { get { return "1.0"; } set { } }
 
        [XmlElement("DocumentoDTECedido")]
        public DocumentoDTECedido DocumentoDTECedido { get; set; }


        public DTECedido(string xmlDTE)
        {
            DocumentoDTECedido = new DocumentoDTECedido();
            var dte = ItaSystem.DTE.Engine.XML.XmlHandler.DeserializeFromString<ItaSystem.DTE.Engine.Documento.DTE>(xmlDTE);
            DocumentoDTECedido.ID = "DTE_CEDIDO_" + dte.Documento.Id;
            DocumentoDTECedido.XML_DTE = xmlDTE;
        }

        public DTECedido()
        {
            DocumentoDTECedido = new DocumentoDTECedido();
        }

        public string Firmar(string nombreCertificado, out string message, string outputDirectory = "out\\temp\\")
        {
            message = "";
            DocumentoDTECedido.FechaHoraFirma = DateTime.Now;
            string filePath = "";
            try
            {
                string xmlContent = XmlHandler.Serialize<DTECedido>(this, SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false, null, outputDirectory, true, "http://www.sii.cl/SiiDte");

                AppendXML(filePath);

                var content = ITA_CHILE.Security.Firma.Firma.FirmarDocumentoPath(filePath, this.DocumentoDTECedido.ID, nombreCertificado);
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
            d.LoadXml(DocumentoDTECedido.XML_DTE);
            doc.ChildNodes[2].ChildNodes[1].AppendChild(doc.ImportNode(d.DocumentElement, true));

            doc.InnerXml = doc.InnerXml.Replace(@"xmlns=""""", "").Replace("iso-8859-1", "ISO-8859-1");
            doc.Save(filePath);
            return doc.InnerXml;
        }

    }
}
