using ItaSystem.DTE.Engine.Helpers;
using System;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    public class Caratula
    {
        /// <summary>
        /// Corresponde al rut del contribuyente emisor.
        /// Con guión y dígito verificador.
        /// </summary>
        [XmlElement("RutEmisorLibro")]
        public string RutEmisor { get; set; }
        public bool ShouldSerializeRutEmisor() { return true; }

        /// <summary>
        /// Corresponde al rut del usuario autorizado que hace el envío al SII.
        /// </summary>
        [XmlElement("RutEnvia")]
        public string RutEnvia { get; set; }
        public bool ShouldSerializeRutEnvia() { return true; }

        /// <summary>
        /// Periodo tributario, en formato AAAA-MM.
        /// Si TipoLibro = 'Especial', y abarca más de un periodo, debe indicarse el primer periodo.
        /// Para la DJ 3328: debe indicarse el periodo del libro enviado. Se debe enviar un libro por cada periodo.
        /// </summary>
        [XmlElement("PeriodoTributario")]
        public string PeriodoTributario { get; set; }
        public bool ShouldSerializePeriodoTributario() { return true; }

        /// <summary>
        /// Fecha resolución en fomarto AAAA-MM-DD.
        /// Para la DJ 3328: Debe ser valor fijo: 2016-01-20
        /// </summary>
        [XmlIgnore]
        public DateTime FechaResolucion { get { return DateTime.Parse(FechaResolucionString); } set { this.FechaResolucionString = value.ToString(Config.Resources.DateFormat); } }

        /// <summary>
        /// Fecha resolución en fomarto AAAA-MM-DD.
        /// Para la DJ 3328: Debe ser valor fijo: 2016-01-20
        /// Do not set this property, set FechaTimbre instead.
        /// </summary>
        [XmlElement("FchResol")]
        public string FechaResolucionString { get; set; }
        public bool ShouldSerializeFechaResolucionString() { return true; }

        /// <summary>
        /// Número de resolución que autoriza al contribuyente como emisor electrónico.
        /// Para la DJ 3328: Debe ser valor fijo: 102006.
        /// </summary>
        [XmlElement("NroResol")]
        public int NumeroResolucion { get; set; }
        public bool ShouldSerializeNumeroResolucion() { return true; }

        /// <summary>
        /// Para IEV y F3328, valor fijo: VENTA.
        /// </summary>
        [XmlElement("TipoOperacion")]
        public Enum.TipoOperacionLibro.TipoOperacionLibroEnum TipoOperacion { get; set; }
        public bool ShouldSerializeTipoOperacion() { return TipoOperacion != Enum.TipoOperacionLibro.TipoOperacionLibroEnum.NotSet; }

        /// <summary>
        /// Mensual: Corresponde a los libros regulares.
        /// Especial: Corresponde a un libro solicitado vía una notificación.
        /// Rectifica: Corresponde a un libro que reemplaza a uno ya recibido por el SII. Requiere un código de autorización de reemplazo de libro electrónico en 'CodigoAutorizacionRectificacion'
        /// </summary>
        [XmlElement("TipoLibro")]
        public Enum.TipoLibro.TipoLibroEnum TipoLibro { get; set; }
        public bool ShouldSerializeTipoLibro() { return TipoLibro != Enum.TipoLibro.TipoLibroEnum.NotSet; }

        /// <summary>
        /// Tipo de envío del libro.
        /// Si no se incluye, se asume TOTAL.
        /// </summary>
        [XmlElement("TipoEnvio")]
        public Enum.TipoEnvioLibro.TipoEnvioLibroEnum TipoEnvio { get; set; }
        public bool ShouldSerializeTipoEnvio() { return TipoEnvio != Enum.TipoEnvioLibro.TipoEnvioLibroEnum.NotSet; }

        /// <summary>
        /// Número del segmento.
        /// Sólo si TipoEnvio = 'PARCIAL'
        /// </summary>
        [XmlElement("NroSegmento")]
        public int NumeroSegmento { get; set; }
        public bool ShouldSerializeNumeroSegmento() { return NumeroSegmento != 0; }

        /// <summary>
        /// Folio de notificación. Obligatorio cuando el TipoEnvio = 'ESPECIAL'.
        /// Para la DJ3328: Debe ser valor fijo: 102006
        /// </summary>
        [XmlElement("FolioNotificacion")]
        public int FolioNotificacion { get; set; }
        public bool ShouldSerializeFolioNotificacion() { return FolioNotificacion != 0; }

        [XmlIgnore]
        private string _codigoAutorizacionRectificacionLibro;
        /// <summary>
        /// Código de autorización de reemplazo de libro electrónico: obtenido por un representante legal de la empresa, para permitir el reemplazo de un libro recibido OK por SII para un periodo y tipo de libro específico.
        /// Para la DJ 3328: No aplica.
        /// </summary>
        [XmlElement("CodAutRec")]
        public string CodigoAutorizacionRectificacionLibro { get { return _codigoAutorizacionRectificacionLibro.Truncate(10); } set { _codigoAutorizacionRectificacionLibro = value; } }
        public bool ShouldSerializeCodigoAutorizacionRectificacionLibro() { return !string.IsNullOrEmpty(CodigoAutorizacionRectificacionLibro); }

        public Caratula()
        {
            TipoEnvio = Enum.TipoEnvioLibro.TipoEnvioLibroEnum.Total;
        }
    }
}