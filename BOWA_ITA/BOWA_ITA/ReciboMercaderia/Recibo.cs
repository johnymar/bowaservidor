using ItaSystem.DTE.Engine.XML;
using System;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.ReciboMercaderia
{
    /// <summary>
    /// Comprobante de Recepcion de Mercaderias o Servicios Prestados Recibos de Recepcion de Mercaderias o Servicios Prestados
    /// </summary>
    public class Recibo
    {
        [XmlAttribute("version")]
        public string Version { get { return Config.Resources.versionRecibo; } set { } }

        /// <summary>
        /// Identificacion del Documento Recibido
        /// </summary>
        [XmlElement("DocumentoRecibo")]
        public DocumentoRecibo DocumentoRecibo { get; set; }

        public string Firmar(string nombreCertificado)
        {
            this.DocumentoRecibo.FechaHoraFirma = DateTime.Now;
            string filePath = "";
            string xmlContent = XmlHandler.Serialize(this, SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false);
            var content = ITA_CHILE.Security.Firma.Firma.FirmarDocumentoPath(filePath, this.DocumentoRecibo.Id, nombreCertificado);
            return filePath;
        }
    }
}