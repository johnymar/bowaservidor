using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    public class ComprobanteCertificado
    {
        /// <summary>
        /// Versión del Comprobante de Certificado 
        /// </summary>
        [XmlAttribute("version")]
        public string Version { get { return Config.Resources.versionCOCERT; } set { } }

        /// <summary>
        /// Documento de comprobante de certificado.
        /// </summary>
        [XmlElement("DocumentoCoCertif")]
        public DocumentoCOCERT DocumentoCOCERT { get; set; }
        public bool ShouldSerializeDocumentoCOCERT() { return true; }

    }
}