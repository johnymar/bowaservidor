using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ITA_CHILE.Cesion
{
    public class RUTAutorizado
    {
        [XmlElement("RUT")]
        public string RUT { get; set; }

        [XmlElement("Nombre")]
        public string Nombre { get; set; }
    }
}
