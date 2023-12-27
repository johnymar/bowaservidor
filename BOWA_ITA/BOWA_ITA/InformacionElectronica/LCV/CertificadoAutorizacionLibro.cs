using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    public class CertificadoAutorizacionLibro
    {
        /// <summary>
        /// Versión del Certificado de Autorización de Libro Contable Electrónico.
        /// </summary>
        [XmlAttribute("version")]
        public string Version { get { return Config.Resources.versionCAL; } set { } }

        /// <summary>
        /// Documento CAL
        /// </summary>
        [XmlElement("DocumentoCal")]
        public DocumentoCAL CAL { get; set; }
        public bool ShouldSerializeCAL() { return true; }
    }
}