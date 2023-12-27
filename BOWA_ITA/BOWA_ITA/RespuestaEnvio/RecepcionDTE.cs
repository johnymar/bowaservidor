using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.RespuestaEnvio
{
    public class RecepcionDTE
    {
        /// <summary>
        /// Tipo de DTE.
        /// </summary>
        [XmlElement("TipoDTE")]
        public Enum.TipoDTE.DTEType TipoDTE { get; set; }
        
        /// <summary>
        /// Folio del DTE.
        /// </summary>
        [XmlElement("Folio")]
        public int Folio { get; set; }
        
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

        /// <summary>
        /// RUT emisor del DTE.
        /// </summary>
        [XmlElement("RUTEmisor")]
        public string RutEmisor { get; set; }

        /// <summary>
        /// RUT receptor del DTE.
        /// </summary>
        [XmlElement("RUTRecep")]
        public string RutReceptor { get; set; }

        /// <summary>
        /// Monto total del DTE.
        /// </summary>
        [XmlElement("MntTotal")]
        public int MontoTotal { get; set; }

        /// <summary>
        /// Estado de recepción del DTE.
        /// </summary>
        [XmlElement("EstadoRecepDTE")]
        public Enum.EstadoRecepcionDTE.EstadoRecepcionDTEEnum EstadoRecepcionDTE { get; set; }
        //[XmlElement("EstadoRecepDTE")]
        //public string EstadoRecepcionDTE { get; set; }

        /// <summary>
        /// Información adicional para el estado de recepción del DTE.
        /// </summary>
        [XmlElement("RecepDTEGlosa")]
        public string GlosaEstadoRecepcionDTE { get; set; }
    }
}
