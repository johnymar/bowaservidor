using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ItaSystem.DTE.Engine.Helpers;

namespace ItaSystem.DTE.Engine.Documento
{
    public class Chofer
    {
        /// <summary>
        /// Rut del chofer.
        /// Rut Chofer que realiza el transporte de mercaderías. 
        /// Con guión y dígito verificador.
        /// </summary>
        [XmlElement("RUTChofer")]
        public string Rut { get; set; }

        [XmlIgnore]
        private string _nombre;
        /// <summary>
        /// Nombre del chofer.
        /// </summary>
        [XmlElement("NombreChofer")]
        public string Nombre { get { return _nombre.Truncate(30); } set { _nombre = value; } }

        public Chofer()
        {
            Rut = string.Empty;
            Nombre = string.Empty;
        }
    }
}
