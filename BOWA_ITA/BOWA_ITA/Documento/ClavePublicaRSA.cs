using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Documento
{
    public class ClavePublicaRSA
    {
        private string _modulo;
        /// <summary>
        /// Módulo RSA
        /// </summary>
        [XmlElement("M")]
        public string Modulo { get { return Utilidades.BreakTextEveryNCharacters(_modulo); } set { _modulo = value; } }

        private string _exponente;
        /// <summary>
        /// Exponente RSA
        /// </summary>
        [XmlElement("E")]
        public string Exponente { get { return Utilidades.BreakTextEveryNCharacters(_exponente); } set { _exponente = value; } }

        
    }
}
