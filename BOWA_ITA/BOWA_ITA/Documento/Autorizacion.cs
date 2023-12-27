using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Documento
{
    [XmlRoot("AUTORIZACION")]
    public class Autorizacion
    {
        /// <summary>
        /// Código de autorización de folios.
        /// </summary>
        [XmlElement("CAF")]
        public CAF CAF { get; set; }
        
        /// <summary>
        /// Clave privada RSA
        /// </summary>
        [XmlElement("RSASK")]
        public string ClavePrivadaRSA { get; set; }
        
        /// <summary>
        /// Clave pública RSA
        /// </summary>
        [XmlElement("RSAPUBK")]
        public string ClavePublicaRSA { get; set; }
    }
}
