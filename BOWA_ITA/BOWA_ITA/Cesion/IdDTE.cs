using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ITA_CHILE.Cesion
{
    [XmlRoot("IdDTE")]
    public class IdDTE
    {
        /// <summary>
        /// Tipo de DTE
        /// </summary>
        [XmlElement("TipoDTE")]
        public ItaSystem.DTE.Engine.Enum.TipoDTE.DTEType TipoDTE { get; set; }

        [XmlElement("Folio")]
        public int Folio { get; set; }

        [XmlElement("RUTEmisor")]
        public string RUTEmisor { get; set; }

        [XmlElement("RUTReceptor")]
        public string RUTReceptor { get; set; }
        /// <summary>
        /// Fecha Emisión Contable del DTE (AAAA-MM-DD)
        /// </summary>
        [XmlElement("FchEmis")]
        public string FechaEmisionString { get; set; }

        /// <summary>
        /// Fecha Emisión Contable del DTE.
        /// </summary>
        [XmlIgnore]
        public DateTime FechaEmision { get { return DateTime.Parse(FechaEmisionString); } set { this.FechaEmisionString = value.ToString(ItaSystem.DTE.Engine.Config.Resources.DateFormat); } }

        [XmlElement("MntTotal")]
        public int MontoTotal { get; set; }

        public IdDTE()
        {
        }
    }
}
