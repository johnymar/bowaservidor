using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Documento
{
    public class ClavePublicaDSA
    {
        [XmlIgnore]
        private string _moduloPrimo;
        /// <summary>
        /// Módulo primo.
        /// </summary>
        [XmlElement("M")]
        public string ModuloPrimo { get { return Utilidades.BreakTextEveryNCharacters(_moduloPrimo); } set { _moduloPrimo = value; } }

        [XmlIgnore]
        private string _divisor;
        /// <summary>
        /// Entero divisor de P - 1.
        /// </summary>
        [XmlElement("Q")]
        public string Divisor { get { return Utilidades.BreakTextEveryNCharacters(_divisor); } set { _divisor = value; } }

        [XmlIgnore]
        private string _entero;
        /// <summary>
        /// Entero f(P,Q).
        /// </summary>
        [XmlElement("G")]
        public string Entero { get { return Utilidades.BreakTextEveryNCharacters(_entero); } set { _entero = value; } }

        [XmlIgnore]
        private string _y;
        /// <summary>
        /// G**X mod P
        /// </summary>
        [XmlElement("Y")]
        public string Y { get { return Utilidades.BreakTextEveryNCharacters(_y); } set { _y = value; } }
    }
}
