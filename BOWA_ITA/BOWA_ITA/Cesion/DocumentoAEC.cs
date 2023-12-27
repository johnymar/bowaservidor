using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ITA_CHILE.Cesion
{
    public class DocumentoAEC
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlElement("Caratula")]
        public Caratula Caratula { get; set; }

        [XmlElement("Cesiones")]
        public Cesiones Cesiones { get; set; }

        public DocumentoAEC()
        {
            Caratula = new Caratula();
            Cesiones = new Cesiones();
        }
    }
}
