using ItaSystem.DTE.Engine.Helpers;
using System;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.ReciboMercaderia
{
    public class DocumentoRecibo
    {
        [XmlAttribute("ID")]
        public string Id { get; set; }

        /// <summary>
        /// Tipo de documento
        /// </summary>
        [XmlElement("TipoDoc")]
        public Enum.TipoDTE.DTEType TipoDocumento { get; set; }
        public bool ShouldSerializeTipoDocumento() { return true; }

        /// <summary>
        /// Folio del documento
        /// </summary>
        [XmlElement("Folio")]
        public int Folio { get; set; }
        public bool ShouldSerializeFolio() { return true; }

        /// <summary>
        /// Fecha emision contable del DTE. (AAAA-MM-DD).
        /// </summary>
        [XmlIgnore]
        public DateTime FechaEmision { get { return DateTime.Parse(FechaEmisionString); } set { this.FechaEmisionString = value.ToString(Config.Resources.DateFormat); } }

        /// <summary>
        /// Fecha emision contable del DTE. (AAAA-MM-DD).
        /// Do not set this property, set FechaEmision instead.
        /// </summary>
        [XmlElement("FchEmis")]
        public string FechaEmisionString { get; set; }
        public bool ShouldSerializeFechaEmisionString() { return true; }

        /// <summary>
        /// Rut emisor del documento
        /// </summary>
        [XmlElement("RUTEmisor")]
        public string RutEmisor { get; set; }
        public bool ShouldSerializeRutEmisor() { return true; }

        /// <summary>
        /// Rut receptor del documento
        /// </summary>
        [XmlElement("RUTRecep")]
        public string RutReceptor { get; set; }
        public bool ShouldSerializeRutReceptor() { return true; }

        /// <summary>
        /// Monto total del documento
        /// </summary>
        [XmlElement("MntTotal")]
        public int MontoTotal { get; set; }
        public bool ShouldSerializeMontoTotal() { return true; }

        [XmlIgnore]
        private string _recinto;
        /// <summary>
        /// Lugar donde se realiza la recepción conforme
        /// </summary>
        [XmlElement("Recinto")]
        public string Recinto { get { return _recinto.Truncate(80); } set { this._recinto = value; } }
        public bool ShouldSerializeRecinto() { return true; }

        /// <summary>
        /// Rut de quien firma el recibo
        /// </summary>
        [XmlElement("RutFirma")]
        public string RutFirma { get; set; }
        public bool ShouldSerializeRutFirma() { return true; }
        
        /// <summary>
        /// Texto Ley 19.983, acredita la recepcion mercaderías o servicio.
        /// </summary>
        [XmlElement("Declaracion")]
        public string Declaracion { get { return Config.Resources.DeclaracionLey19983.Truncate(256); } set { } }
        public bool ShouldSerializeDeclaracion() { return true; }

        /// <summary>
        /// Fecha y Hora de la Firma del Recibo.
        /// (AAAA-MM-DDTHH:MM:SS
        /// No not set this property, set FechaHoraFirma instead.
        /// </summary>
        [XmlElement("TmstFirmaRecibo")]
        public string FechaHoraFirmaString { get; set; }
        public bool ShouldSerializeFechaHoraFirmaString() { return true; }

        /// <summary>
        /// Fecha y Hora de la Firma del Recibo.
        /// (AAAA-MM-DDTHH:MM:SS
        /// </summary>
        [XmlIgnore]
        public DateTime FechaHoraFirma { get { return DateTime.Parse(FechaHoraFirmaString); } set { FechaHoraFirmaString = value.ToString(Config.Resources.DateTimeFormat); } }

    }
}