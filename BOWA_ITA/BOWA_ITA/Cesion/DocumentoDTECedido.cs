using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ITA_CHILE.Cesion
{
    public class DocumentoDTECedido
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlIgnore]
        public string XML_DTE { get; set; }

        [XmlElement("TmstFirma")]
        private string FechaHoraFirmaString { get; set; }

        [XmlIgnore]
        public DateTime FechaHoraFirma { get { return DateTime.Parse(FechaHoraFirmaString); } set { FechaHoraFirmaString = value.ToString(ItaSystem.DTE.Engine.Config.Resources.DateTimeFormat); } }

    }
}
