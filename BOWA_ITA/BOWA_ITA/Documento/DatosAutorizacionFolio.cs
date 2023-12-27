using ItaSystem.DTE.Engine.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ItaSystem.DTE.Engine.Helpers;

namespace ItaSystem.DTE.Engine.Documento
{
    public class DatosAutorizacionFolio
    {
        /// <summary>
        /// Rut del emisor del DTE.
        /// </summary>
        [XmlElement("RE")]
        public string RutEmisor { get; set; }

        [XmlIgnore]
        private string _razonSocialEmisor;
        /// <summary>
        /// Razón social del emisor del DTE.
        /// </summary>
        [XmlElement("RS")]
        public string RazonSocialEmisor { get { return _razonSocialEmisor.Truncate(40); } set { _razonSocialEmisor = value; } }

        /// <summary>
        /// Tipo de DTE que se está autorizando a emitir.
        /// </summary>
        [XmlElement("TD")]
        public TipoDTE.DTEType TipoDTE { get; set; }

        /// <summary>
        /// Rango autorizado de folios.
        /// </summary>
        [XmlElement("RNG")]
        public Rango RangoAutorizado { get; set; }

        /// <summary>
        /// Fecha de autorización.
        /// </summary>
        [XmlIgnore]
        public DateTime FechaAutorizacion { get { return DateTime.Parse(FechaAutorizacionString); } set { this.FechaAutorizacionString = value.ToString(Config.Resources.DateFormat); } }

        /// <summary>
        /// Fecha de autorización en formato AAAA-MM-DD.
        /// Do not set this property, set FechaAutorizacion instead.
        /// </summary>
        [XmlElement("FA")]
        public string FechaAutorizacionString { get; set; }


        /// <summary>
        /// Clave pública RSA del solicitante
        /// </summary>
        [XmlElement("RSAPK")]
        public ClavePublicaRSA ClavePublicaRSA { get; set; }

        /// <summary>
        /// Clave pública DSA del solicitante.
        /// </summary>
        [XmlElement("DSAPK")]
        public ClavePublicaDSA ClavePublicaDSA { get; set; }

        /// <summary>
        /// Identificador de llave,
        /// </summary>
        [XmlElement("IDK")]
        public long IdentificadorLlave { get; set; }
    }
}
