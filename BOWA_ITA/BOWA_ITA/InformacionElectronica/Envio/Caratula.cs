using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Envio
{
    public class Caratula
    {
        [XmlAttribute("version")]
        public string Version { get { return Engine.Config.Resources.versionCaratula; } set { } }

        /// <summary>
        /// Rut emisor de los DTE.
        /// </summary>
        [XmlElement("RutEmisor")]
        public string RutEmisor { get; set; }
        
        /// <summary>
        /// Rut que envía los DTE.
        /// </summary>
        [XmlElement("RutEnvia")]
        public string RutEnvia { get; set; }
        
        /// <summary>
        /// Rut al que se le envían los DTE.
        /// </summary>
        [XmlElement("RutReceptor")]
        public string RutReceptor { get; set; }
        
        /// <summary>
        /// Fecha de resolución que autoriza el envío del DTE. (AAAA-MM-DD).
        /// </summary>
        [XmlIgnore]
        public DateTime FechaResolucion { get { return DateTime.Parse(FechaResolucionString); } set { this.FechaResolucionString = value.ToString(Config.Resources.DateFormat); } }

        /// <summary>
        /// Fecha de resolución que autoriza el envío del DTE. (AAAA-MM-DD).
        /// Do not set this property. Set FechaResolución instead.
        /// </summary>
        [XmlElement("FchResol")]
        public string FechaResolucionString { get; set; }
        
        /// <summary>
        /// Número de resolución que autoriza el envío del DTE.
        /// </summary>
        [XmlElement("NroResol")]
        public int NumeroResolucion { get; set; }
        
        /// <summary>
        /// Fecha y hora de la firma del archivo de envío.
        /// </summary>
        [XmlIgnore]
        public DateTime FechaEnvio { get { return DateTime.Parse(FechaEnvioString); } set { this.FechaEnvioString = value.ToString(Config.Resources.DateTimeFormat); } }
        
        /// <summary>
        /// Fecha y hora de la firma del archivo de envío.
        /// Do not set this property, set FechaEnvio instead.
        /// </summary>
        [XmlElement("TmstFirmaEnv")]
        public string FechaEnvioString { get; set; }
        
        /// <summary>
        /// Subtotales de DTE enviados.
        /// </summary>
        [XmlElement("SubTotDTE")]
        public List<SubTotalesDTE> SubTotalesDTE { get; set; }

        public Caratula()
        {
            SubTotalesDTE = new List<SubTotalesDTE>();
        }
    }
}
