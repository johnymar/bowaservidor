using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.RCOF
{
    public class Caratula
    {
        [XmlAttribute("version")]
        public string Version { get { return Engine.Config.Resources.versionCaratulaRCOF; } set { } }

        [XmlElement("RutEmisor")]
        public string RutEmisor { get; set; }

        [XmlElement("RutEnvia")]
        public string RutEnvia { get; set; }

        [XmlElement("FchResol")]
        public string FechaResolucionString { get; set; }

        [XmlElement("NroResol")]
        public int NroResol { get; set; }

        [XmlElement("FchInicio")]
        public string FechaInicioString { get; set; }

        [XmlElement("FchFinal")]
        public string FechaFinalString { get; set; }

        [XmlElement("SecEnvio")]
        public string SecEnvio { get; set; }

        [XmlElement("TmstFirmaEnv")]
        public string FechaEnvioString { get; set; }

        [XmlIgnore]
        public DateTime FechaEnvio { get { return DateTime.Parse(FechaEnvioString); } set { this.FechaEnvioString = value.ToString(Config.Resources.DateTimeFormat); } }

       
        [XmlIgnore]
        public DateTime FechaResolucion { get { return DateTime.Parse(FechaResolucionString); } set { this.FechaResolucionString = value.ToString(Config.Resources.DateFormat); } }

        

        [XmlIgnore]
        public DateTime FechaInicio { get { return DateTime.Parse(FechaInicioString); } set { this.FechaInicioString = value.ToString(Config.Resources.DateFormat); } }

        

        [XmlIgnore]
        public DateTime FechaFinal { get { return DateTime.Parse(FechaFinalString); } set { this.FechaFinalString = value.ToString(Config.Resources.DateFormat); } }

       

        public Caratula()
        {
        }
    }
}
