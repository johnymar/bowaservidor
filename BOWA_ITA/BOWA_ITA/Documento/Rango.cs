using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Documento
{
    public class Rango
    {
        /// <summary>
        /// Folio inicial (desde).
        /// </summary>
        [XmlElement("D")]
        public int Desde { get; set; }
        
        /// <summary>
        /// Folio final (hasta).
        /// </summary>
        [XmlElement("H")]
        public int Hasta { get; set; }
    }
}
