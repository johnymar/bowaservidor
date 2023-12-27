using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Documento
{
    public class FirmaDigital
    {
        /// <summary>
        /// Algoritmo de la firma digital. "SHA1withRSA".
        /// </summary>
        [XmlAttribute("algoritmo")]
        public string Algoritmo { get { return "SHA1withRSA"; } set { } }

        [XmlIgnore]
        private string _firma;
        /// <summary>
        /// Firma
        /// </summary>
        [XmlText()]
        public string Firma { get { return Utilidades.BreakTextEveryNCharacters(_firma); } set { _firma = value; } }
    }
}
