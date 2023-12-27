using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ItaSystem.DTE.Engine.Helpers;

namespace ItaSystem.DTE.Engine.Documento
{
    public class SubCantidad
    {
        /// <summary>
        /// Cantidad distribuida.
        /// </summary>
        [XmlElement("SubQty")]
        public double Cantidad { get; set; }

        [XmlIgnore]
        public string _codigo;
        /// <summary>
        /// Codigo descriptivo de la subcantidad.
        /// </summary>
        [XmlElement("SubCod")]
        public string Codigo { get { return _codigo.Truncate(35); } set { _codigo = value; } }

        public SubCantidad()
        {
            Cantidad = 0;
            Codigo = string.Empty;
        }
    }
}
