using System;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    public class DocumentoCAL
    {
        /// <summary>
        /// Id
        /// </summary>
        [XmlElement("ID")]
        public string Id { get; set; }
        public bool ShouldSerializeId() { return true; }

        /// <summary>
        /// RUT del distribuidor.
        /// </summary>
        [XmlElement("RutDistribuidor")]
        public string RutDistribuidor { get; set; }
        public bool ShouldSerializeRutDistribuidor() { return true; }

        /// <summary>
        /// Tipo de Certificado.
        /// C = Certificación.
        /// P = Producción.
        /// </summary>
        public Enum.TipoCertificado.TipoCertificadoEnum TipoCertificado { get; set; }
        public bool ShouldSerializeTipoCertificado() { return true; }

        public Enum.ClaseCAL.ClaseCALEnum Clase { get; set; }
        public bool ShouldSerializeClase() { return true; }

        /// <summary>
        /// Tipo de Libro Contable
        /// </summary>
        [XmlElement("TipoLCE")]
        public Enum.TipoLCE.TipoLCEEnum TipoLCE { get; set; }
        public bool ShouldSerializeTipoLCE() { return true; }

        /// <summary>
        /// Fecha de emisión del CAL. (AAAA-MM-DD)
        /// </summary>
        [XmlIgnore]
        public DateTime FechaEmision { get { return DateTime.Parse(FechaEmisionString); } set { this.FechaEmisionString = value.ToString(Config.Resources.DateFormat); } }

        /// <summary>
        /// Fecha de emisión del CAL. (AAAA-MM-DD)
        /// Do not set this property, set FechaEmision instead.
        /// </summary>
        [XmlElement("FchEmision")]
        public string FechaEmisionString { get; set; }
        public bool ShouldSerializeFechaEmisionString() { return true; }

        /// <summary>
        /// En caso que Clase = 3; corresponde al año en que es válido hacer envíos con este CAL, de lo contrario es año de inicio para realizar envíos. (Formato AAAA)
        /// </summary>
        [XmlElement("PeriodoVigencia")]
        public int Año { get; set; }
        public bool ShouldSerializeAño() { return true; }

        /// <summary>
        /// TimeStamp de generación del firma.
        /// </summary>
        [XmlIgnore]
        public DateTime FechaFirma { get { return DateTime.Parse(FechaFirmaString); } set { this.FechaFirmaString = value.ToString(Config.Resources.DateTimeFormat); } }

        /// <summary>
        /// TimeStamp de generación del firma. (AAAA-MM-DD)
        /// Do not set this property, set FechaTimbre instead.
        /// </summary>
        [XmlElement("TmstFirma")]
        public string FechaFirmaString { get; set; }
        public bool ShouldSerializeFechaFirmaString() { return true; }
    }
}