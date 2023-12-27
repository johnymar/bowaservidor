using System;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    public class DocumentoCOCERT
    {
        /// <summary>
        /// Id
        /// </summary>
        [XmlElement("ID")]
        public string Id { get; set; }
        public bool ShouldSerializeId() { return true; }

        /// <summary>
        /// Rut contribuyente
        /// </summary>
        [XmlElement("RutContribuyente")]
        public string RutContribuyente { get; set; }
        public bool ShouldSerializeRutContribuyente() { return true; }

        /// <summary>
        /// TimeStamp de generación del firma.
        /// </summary>
        [XmlIgnore]
        public DateTime FechaEmision { get { return DateTime.Parse(FechaEmisionString); } set { this.FechaEmisionString = value.ToString(Config.Resources.DateFormat); } }

        /// <summary>
        /// TimeStamp de generación del firma. (AAAA-MM-DD)
        /// Do not set this property, set FechaTimbre instead.
        /// </summary>
        [XmlElement("FchEmision")]
        public string FechaEmisionString { get; set; }
        public bool ShouldSerializeFechaEmisionString() { return true; }

        /// <summary>
        /// Comprobante de autorización
        /// </summary>
        [XmlElement("LceCal")]
        public CertificadoAutorizacionLibro ComprobanteAutorizacionLibro { get; set; }
        public bool ShouldSerializeComprobanteAutorizacionLibro() { return true; }

        /// <summary>
        /// Rut firmante distribuidor.
        /// </summary>
        [XmlElement("RutFirmanteDistribuidor")]
        public string RutFirmanteDistribuidor { get; set; }
        public bool ShouldSerializeRutFirmanteDistribuidor() { return true; }

        /// <summary>
        /// TimeStamp de generación del firma.
        /// </summary>
        [XmlIgnore]
        public DateTime FechaFirma { get { return DateTime.Parse(FechaFirmaString); } set { this.FechaFirmaString = value.ToString(Config.Resources.DateTimeFormat); } }

        /// <summary>
        /// TimeStamp de generación del firma.
        /// Do not set this property, set FechaTimbre instead.
        /// </summary>
        [XmlElement("TmstFirma")]
        public string FechaFirmaString { get; set; }
        public bool ShouldSerializeFechaFirmaString() { return true; }
    }
}