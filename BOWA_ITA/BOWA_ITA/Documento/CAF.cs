using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Documento
{
    public class CAF
    {
        /// <summary>
        /// Id CAF usado para identificar el CAF que se está usando.
        /// Dato relevante para el cliente, más no para DTE.
        /// </summary>
        [XmlIgnore]
        public int IdCAF { get; set; }

        [XmlIgnore]
        public string cafString { get; set; }
        /// <summary>
        /// Versión del CAF
        /// </summary>
        [XmlAttribute("version")]
        public string Version { get;set; }
        
        /// <summary>
        /// Datos de autorización de folios
        /// </summary>
        [XmlElement("DA")]
        public DatosAutorizacionFolio Datos { get; set; }
        
        /// <summary>
        /// Firma digital RSA del SII sobre DA
        /// </summary>
        [XmlElement("FRMA")]
        public FirmaDigital Firma { get; set; }
    }
}
