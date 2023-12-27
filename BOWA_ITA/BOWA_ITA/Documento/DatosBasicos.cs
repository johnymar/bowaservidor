using ItaSystem.DTE.Engine.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ItaSystem.DTE.Engine.Helpers;
using System.EnterpriseServices.Internal;

namespace ItaSystem.DTE.Engine.Documento
{
    [XmlRoot("DD")]
    public class DatosBasicos
    {
        /// <summary>
        /// Rut Emisor
        /// </summary>
        [XmlElement("RE")]
        public string RutEmisor { get; set; }

        /// <summary>
        /// Tipo DTE
        /// </summary>
        [XmlElement("TD")]
        public TipoDTE.DTEType TipoDTE { get; set; }

        /// <summary>
        /// Folio del DTE
        /// </summary>
        [XmlElement("F")]
        public int FolioDTE { get; set; }
        public bool ShouldSerializeFolioDTE() { return true; }

        /// <summary>
        /// Fecha Emisión Contable del DTE.
        /// </summary>
        [XmlIgnore]
        public DateTime FechaEmision { get { return DateTime.Parse(FechaEmisionString); } set { this.FechaEmisionString = value.ToString(Config.Resources.DateFormat); } }

        /// <summary>
        /// Fecha emisión del DTE en formato AAAAMMDD
        /// Do not set this property, set FechaEmision instead.
        /// </summary>
        [XmlElement("FE")]
        public string FechaEmisionString { get; set; }

        /// <summary>
        /// RutReceptor
        /// </summary>
        [XmlElement("RR")]
        public string RutReceptor { get; set; }

        [XmlIgnore]
        private string _razonSocialReceptor;
        /// <summary>
        /// Razón social del receptor
        /// </summary>
        [XmlElement("RSR")]
        public string RazonSocialReceptor { get { return _razonSocialReceptor.Truncate(40); } set { _razonSocialReceptor = value; } }

        /// <summary>
        /// Monto total del DTE
        /// </summary>
        [XmlElement("MNT")]
        public int MontoTotalDTE { get; set; }

        [XmlIgnore]
        private string _descripcionPrimerDetalle;
        /// <summary>
        /// Descripción del primer detalle del DTE
        /// </summary>
        [XmlElement("IT1")]
        public string DescripcionPrimerDetalle { get { return _descripcionPrimerDetalle.Truncate(40); } set { _descripcionPrimerDetalle = value; } }

        /// <summary>
        /// Código de autorización de Folios
        /// </summary>
        [XmlElement("CAF")]
        public CAF CAF { get; set; }

        [XmlIgnore]
        public string CAF_string { get; set; }

        /// <summary>
        /// TimeStamp de generación del timbre.
        /// </summary>
        [XmlIgnore]
        public DateTime FechaTimbre { get { return DateTime.Parse(FechaTimbreString); } set { this.FechaTimbreString = value.ToString(Config.Resources.DateTimeFormat); } }

        /// <summary>
        /// TimeStamp de generación del timbre. (AAAA-MM-DD)
        /// Do not set this property, set FechaTimbre instead.
        /// </summary>
        [XmlElement("TSTED")]
        public string FechaTimbreString { get; set; }

        public override string ToString()
        {
            string xml = XML.XmlHandler.Serialize<DatosBasicos>(this, XML.SerializationType.SerializationTypes.Inline, false, null, "", false);
            return xml;
        }
    }

}
