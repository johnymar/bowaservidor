using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ITA_CHILE.Cesion
{
    public class Caratula
    {
        [XmlAttribute("version")]
        public string Version { get { return "1.0"; } set { } }

        [XmlElement("RutCedente")]
        public string RutCedente { get; set; }
        public bool ShouldSerializeRutCedente() { return true; }

        [XmlElement("RutCesionario")]
        public string RutCesionario { get; set; }
        public bool ShouldSerializeRutCesionario() { return true; }

        [XmlElement("NmbContacto")]
        public string NombreContacto { get; set; }
        public bool ShouldSerializeNombreContacto() { return !string.IsNullOrEmpty(NombreContacto); }

        [XmlElement("FonoContacto")]
        public string FonoContacto { get; set; }
        public bool ShouldSerializeFonoContacto() { return !string.IsNullOrEmpty(FonoContacto); }

        [XmlElement("MailContacto")]
        public string MailContacto { get; set; }
        public bool ShouldSerializeMailContacto() { return !string.IsNullOrEmpty(MailContacto); }

        [XmlElement("TmstFirmaEnvio")]
        public string TmstFirmaEnvioString { get; set; }

        [XmlIgnore]
        public DateTime TmstFirmaEnvio { get { return DateTime.Parse(TmstFirmaEnvioString); } set { TmstFirmaEnvioString = value.ToString(ItaSystem.DTE.Engine.Config.Resources.DateTimeFormat); } }

       

        public Caratula()
        {
        }
    }
}
